namespace NavitaireTE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Schema;

    public class ScriptParser
    {
        private Hashtable _htFunctions = new Hashtable();
        private Hashtable _htInputs = new Hashtable();
        private Random _rnd = new Random((int) DateTime.Now.Ticks);
        private ArrayList _validationErrors = new ArrayList();
        private bool scriptIsValid;

        private string getMonthString(int theMonth)
        {
            DateTimeFormatInfo info = new DateTimeFormatInfo();
            return info.AbbreviatedMonthNames[theMonth - 1];
        }

        public List<ScriptCommand> Parse(string xmlFile)
        {
            List<ScriptCommand> list = new List<ScriptCommand>();
            if (this.validateScript(xmlFile))
            {
                XmlTextReader script = new XmlTextReader(xmlFile);
                ScriptCommand item = new ScriptCommand();
                Input input = new Input();
                ScriptFunction function = new ScriptFunction(this._rnd);
                try
                {
                    while (script.Read())
                    {
                        if (script.NodeType == XmlNodeType.Element)
                        {
                            if (script.Name == "Call")
                            {
                                List<ScriptCommand> collection = this.parseCall(script.GetAttribute("name"), script.GetAttribute("params"));
                                list.AddRange(collection);
                            }
                            if (script.Name == "Command")
                            {
                                item = this.parseCommands(script);
                                list.Add(item);
                            }
                            if (script.Name == "Input")
                            {
                                input = this.parseInputs(script);
                                if (input.inputs.Count > 0)
                                {
                                    this._htInputs.Add(input.Name, input);
                                }
                            }
                            if (script.Name == "Function")
                            {
                                function = this.parseFunctions(script);
                                this._htFunctions.Add(function.Name, function);
                            }
                            if (script.Name == "Loop")
                            {
                                list.AddRange(this.parseLoop(script));
                            }
                        }
                    }
                    script.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "XML parse error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return null;
                }
                if (this._validationErrors.Count > 0)
                {
                    return new List<ScriptCommand>();
                }
            }
            return list;
        }

        private List<ScriptCommand> parseCall(string callee, string fxnParams)
        {
            ScriptCommand item = new ScriptCommand();
            List<ScriptCommand> list = new List<ScriptCommand>();
            ScriptFunction function = (ScriptFunction) this._htFunctions[callee];
            if (function == null)
            {
                this._validationErrors.Add(string.Format(CultureInfo.CurrentCulture, "Function not found:{0}", new object[] { callee }));
                return null;
            }
            try
            {
                if (fxnParams != null)
                {
                    ScriptCommand command2 = function.Commands[0];
                    item.CommandText = command2.CommandText;
                    item.CommandText = item.CommandText.Replace("PARAMS", fxnParams);
                    item.CommandType = command2.CommandType;
                    list.Add(item);
                    return list;
                }
                list.AddRange(function.Commands);
                return list;
            }
            catch (SystemException exception)
            {
                MessageBox.Show(exception.Message);
            }
            return null;
        }

        private ScriptCommand parseCommands(XmlTextReader script)
        {
            return this.parseCommands(script, true);
        }

        private ScriptCommand parseCommands(XmlTextReader script, bool expandInputs)
        {
            ScriptCommand command = new ScriptCommand();
            try
            {
                command.CommandType = script.GetAttribute("CommandType");
                command.Inputs = script.GetAttribute("inputs");
                script.Read();
                string commandText = script.Value;
                if ((command.Inputs != null) && expandInputs)
                {
                    command.CommandText = this.replaceInputsSP(command.Inputs, commandText);
                    return command;
                }
                command.CommandText = commandText;
                return command;
            }
            catch (SystemException exception)
            {
                MessageBox.Show(exception.Message);
            }
            return new ScriptCommand();
        }

        private ScriptFunction parseFunctions(XmlTextReader functions)
        {
            ScriptFunction function = new ScriptFunction(this._rnd);
            ScriptCommand cmd = new ScriptCommand();
            try
            {
                function.Name = functions.GetAttribute("name");
                function.Inputs = functions.GetAttribute("inputs");
                function.InputTable = this._htInputs;
                functions.MoveToElement();
                while (functions.Read())
                {
                    if ((functions.NodeType == XmlNodeType.EndElement) && (functions.Name == "Function"))
                    {
                        return function;
                    }
                    if (functions.NodeType == XmlNodeType.Element)
                    {
                        if (!(functions.Name == "Command") && !(functions.Name == "Call"))
                        {
                            return function;
                        }
                        if (functions.Name == "Command")
                        {
                            cmd = new ScriptCommand();
                            cmd = this.parseCommands(functions, false);
                            function.Add(cmd);
                        }
                        if (functions.Name == "Call")
                        {
                            string attribute = functions.GetAttribute("name");
                            string fxnParams = functions.GetAttribute("params");
                            List<ScriptCommand> cmds = this.parseCall(attribute, fxnParams);
                            function.AddRange(cmds);
                        }
                    }
                }
                return function;
            }
            catch (SystemException exception)
            {
                MessageBox.Show(exception.Message);
            }
            return null;
        }

        private Input parseInputs(XmlTextReader inputs)
        {
            Input input = new Input();
            input.inputs = new ArrayList();
            input.Name = inputs.GetAttribute("name");
            input.Mask = inputs.GetAttribute("mask");
            string attribute = inputs.GetAttribute("filename");
            if (attribute != null)
            {
                XmlTextReader reader = new XmlTextReader(attribute);
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "Input"))
                    {
                        Input input2 = this.parseInputs(reader);
                        inputs.Skip();
                        return input2;
                    }
                }
            }
            inputs.MoveToElement();
            while (inputs.Read())
            {
                if (inputs.NodeType == XmlNodeType.Element)
                {
                    if (inputs.Name != input.Name)
                    {
                        this._validationErrors.Add(string.Format(CultureInfo.CurrentCulture, "XML Error:Element name {0} <> {1}", new object[] { inputs.Name, input.Name }));
                        return input;
                    }
                    string str2 = inputs.GetAttribute("value");
                    if (str2.IndexOf("File=", 0) != -1)
                    {
                        try
                        {
                            string str4;
                            StreamReader reader2 = File.OpenText(str2.Substring(5, str2.Length - 5));
                            while ((str4 = reader2.ReadLine()) != null)
                            {
                                input.inputs.Add(str4);
                            }
                            reader2.Close();
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(string.Format(CultureInfo.CurrentCulture, "Read error in script parser {0}", new object[] { exception.Message }));
                        }
                    }
                    else
                    {
                        input.inputs.Add(str2);
                    }
                }
                if (inputs.NodeType == XmlNodeType.EndElement)
                {
                    return input;
                }
            }
            return input;
        }

        private List<ScriptCommand> parseLoop(XmlTextReader loop)
        {
            int num = int.Parse(loop.GetAttribute("count"));
            if (num < 0)
            {
                num = this._rnd.Next(0, num * -1);
            }
            List<ScriptCommand> collection = new List<ScriptCommand>();
            List<ScriptCommand> list2 = new List<ScriptCommand>();
            string xmlFragment = loop.ReadOuterXml();
            XmlParserContext context = new XmlParserContext(loop.NameTable, null, "", XmlSpace.None);
            for (int i = 0; i < num; i++)
            {
                XmlTextReader script = new XmlTextReader(xmlFragment, XmlNodeType.Element, context);
                while (script.Read())
                {
                    if ((script.NodeType == XmlNodeType.EndElement) && (script.Name == "Loop"))
                    {
                        break;
                    }
                    if ((script.Name == "Call") && (script.NodeType != XmlNodeType.EndElement))
                    {
                        List<ScriptCommand> list3 = this.parseCall(script.GetAttribute("name"), script.GetAttribute("params"));
                        if (list3 != null)
                        {
                            collection.AddRange(list3);
                        }
                    }
                    if ((script.Name == "Command") && (script.NodeType != XmlNodeType.EndElement))
                    {
                        ScriptCommand item = this.parseCommands(script);
                        collection.Add(item);
                    }
                }
            }
            list2.AddRange(collection);
            return list2;
        }

        private int randomIndex(int count)
        {
            return this._rnd.Next(count);
        }

        private string replaceInputsSP(string inputs, string commandText)
        {
            string[] strArray = inputs.Split(";".ToCharArray());
            string str = commandText;
            foreach (string str2 in strArray)
            {
                Input input = (Input) this._htInputs[str2];
                if (input == null)
                {
                    this._validationErrors.Add(string.Format(CultureInfo.CurrentCulture, "Invalid mask {0} <> {1}", new object[] { inputs, commandText }));
                    return null;
                }
                int num = this.randomIndex(input.inputs.Count);
                string newValue = (string) input.inputs[num];
                switch (str2)
                {
                    case "DateRange":
                    case "FullDateRange":
                        try
                        {
                            string[] strArray2 = newValue.Split(";".ToCharArray());
                            DateTime time = Convert.ToDateTime(strArray2[0]);
                            TimeSpan span = (TimeSpan) (Convert.ToDateTime(strArray2[1]) - time);
                            DateTime time3 = time.AddDays((double) this._rnd.Next(span.Days));
                            if (str2 == "DateRange")
                            {
                                newValue = time3.Day.ToString() + this.getMonthString(time3.Month);
                            }
                            else
                            {
                                newValue = time3.Day.ToString() + this.getMonthString(time3.Month) + time3.Year.ToString().Substring(2, 2);
                                if (time3.Day < 10)
                                {
                                    newValue = "0" + newValue;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            throw new ApplicationException("DateRange Config Error:" + exception.Message);
                        }
                        break;
                }
                str = str.Replace(input.Mask, newValue);
            }
            return str;
        }

        private bool validateScript(string xmlFile)
        {
            this.scriptIsValid = true;
            XmlReader reader = null;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;
                XmlTextReader reader2 = new XmlTextReader(xmlFile);
                reader = XmlReader.Create(reader2, settings);
                while (reader.Read())
                {
                }
            }
            catch (Exception exception)
            {
                string str = string.Format(CultureInfo.CurrentCulture, "XML Validation Error:{0}", new object[] { exception.Message });
                this._validationErrors.Add(str);
                return false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return this.scriptIsValid;
        }

        private void validationEventHandler(object sender, ValidationEventArgs args)
        {
            this._validationErrors.Add(args.Message);
            this.scriptIsValid = false;
        }

        public ArrayList ValidationErrors
        {
            get
            {
                return this._validationErrors;
            }
        }
    }
}

