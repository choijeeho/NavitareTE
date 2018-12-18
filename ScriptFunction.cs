namespace NavitaireTE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Forms;

    public class ScriptFunction
    {
        private List<ScriptCommand> _commands;
        private Hashtable _htInputs;
        private Random _rnd;
        public string Inputs;
        public string Name;

        public ScriptFunction()
        {
            this.Inputs = null;
            this._commands = new List<ScriptCommand>();
        }

        public ScriptFunction(Random rnd) : this()
        {
            this._rnd = rnd;
        }

        public void Add(ScriptCommand cmd)
        {
            this._commands.Add(cmd);
        }

        public void AddRange(List<ScriptCommand> cmds)
        {
            this._commands.AddRange(cmds);
        }

        private static string getMonthString(int theMonth)
        {
            DateTimeFormatInfo info = new DateTimeFormatInfo();
            return info.AbbreviatedMonthNames[theMonth - 1];
        }

        private int randomIndex(int count)
        {
            if (this._rnd == null)
            {
                MessageBox.Show("Rnd was null");
                this._rnd = new Random((int) DateTime.Now.Ticks);
            }
            return this._rnd.Next(count);
        }

        private string replaceInputs(string inputs, string commandText)
        {
            string str = inputs;
            string[] strArray = inputs.Split(";".ToCharArray());
            string str2 = commandText;
            int length = 0;
            int num2 = 0;
            foreach (string str3 in strArray)
            {
                try
                {
                    if (((str3.IndexOf("RandomNumber", 0) != -1) || (str3.IndexOf("ALPHA", 0) != -1)) || (str3.IndexOf("alpha", 0) != -1))
                    {
                        if (str3.IndexOf("RandomNumber", 0) != -1)
                        {
                            string[] strArray2 = str3.Split("~".ToCharArray(), 4);
                            int minValue = Convert.ToInt32(strArray2[1]);
                            int maxValue = Convert.ToInt32(strArray2[2]);
                            int num5 = this._rnd.Next(minValue, maxValue);
                            length = str2.IndexOf(str3);
                            num2 = str3.Length;
                            str2 = str2.Substring(0, length) + num5.ToString() + str2.Substring(length + num2, (str2.Length - length) - num2);
                        }
                        else if (str3.IndexOf("ALPHA", 0) != -1)
                        {
                            int num6 = this._rnd.Next(0x41, 90);
                            string str4 = string.Format(CultureInfo.CurrentCulture, "{0}", new object[] { (char) num6 });
                            length = str2.IndexOf(str3);
                            num2 = str3.Length;
                            str2 = str2.Substring(0, length) + str4 + str2.Substring(length + num2, (str2.Length - length) - num2);
                        }
                        else if (str3.IndexOf("alpha", 0) != -1)
                        {
                            int num7 = this._rnd.Next(0x61, 0x7a);
                            string str5 = string.Format(CultureInfo.CurrentCulture, "{0}", new object[] { (char) num7 });
                            length = str2.IndexOf(str3);
                            num2 = str3.Length;
                            str2 = str2.Substring(0, length) + str5 + str2.Substring(length + num2, (str2.Length - length) - num2);
                        }
                        length = str.IndexOf(str3);
                        num2 = str3.Length;
                        str = str.Substring(0, length) + str.Substring(length + num2, (str.Length - length) - num2);
                        if ((str.Length > 0) && (str.Substring(0, 1) == ";"))
                        {
                            str = str.Substring(1, str.Length - 1);
                        }
                        if (str.EndsWith(";"))
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                        str = str.Replace(";;", ";");
                    }
                }
                catch
                {
                    throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, "Macro replace error:{0}:{1}", new object[] { inputs, commandText }));
                }
            }
            if (str != string.Empty)
            {
                foreach (string str6 in str.Split(";".ToCharArray()))
                {
                    Input input = (Input) this._htInputs[str6];
                    if (input == null)
                    {
                        return null;
                    }
                    int num8 = this.randomIndex(input.inputs.Count);
                    string newValue = (string) input.inputs[num8];
                    switch (str6)
                    {
                        case "DateRange":
                        case "FullDateRange":
                            try
                            {
                                string[] strArray3 = newValue.Split(";".ToCharArray());
                                DateTime time = Convert.ToDateTime(strArray3[0]);
                                TimeSpan span = (TimeSpan) (Convert.ToDateTime(strArray3[1]) - time);
                                DateTime time3 = time.AddDays((double) this._rnd.Next(span.Days));
                                if (str6 == "DateRange")
                                {
                                    newValue = time3.Day.ToString() + getMonthString(time3.Month);
                                }
                                else
                                {
                                    newValue = time3.Day.ToString() + getMonthString(time3.Month) + time3.Year.ToString().Substring(2, 2);
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
                    str2 = str2.Replace(input.Mask, newValue);
                }
            }
            return str2;
        }

        public List<ScriptCommand> Commands
        {
            get
            {
                if (this.Inputs != null)
                {
                    return this._commands;
                }
                List<ScriptCommand> list = new List<ScriptCommand>(this._commands.Count);
                ScriptCommand item = null;
                foreach (ScriptCommand command2 in this._commands)
                {
                    item = new ScriptCommand();
                    item.Inputs = command2.Inputs;
                    item.CommandType = command2.CommandType;
                    if (item.Inputs == null)
                    {
                        item.CommandText = command2.CommandText;
                    }
                    else
                    {
                        item.CommandText = this.replaceInputs(item.Inputs, command2.CommandText);
                    }
                    list.Add(item);
                }
                return list;
            }
        }

        public Hashtable InputTable
        {
            set
            {
                this._htInputs = value;
            }
        }
    }
}

