namespace NavitaireTE
{
    using Dart.PowerTCP.Emulation;
    using Dart.PowerTCP.Telnet;
    using NavitaireTE.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    using System.Runtime.InteropServices;
    using System.Drawing.Drawing2D;
    using System.Net.Sockets;
    using System.Configuration;
    using System.Net;
    using System.Text.RegularExpressions;

    public class FrmMain : Form
    {
        private string _bagMessage = string.Empty;
        private bool _bagTagOn;
        private string _bagTagPrinterOff = string.Format("{0}", '\x0003');
        private string _bagTagPrinterOn = string.Format("{0}", '\x0002');
        private byte[] _buffer = new byte[0x400];
        private char[] _charsToTrim = new char[1];
        private List<ScriptCommand> _commands = new List<ScriptCommand>();
        private string _dc1 = string.Format("{0}", '\x0011');
        private string _dc2 = string.Format("{0}", '\x0012');
        private string _dc3 = string.Format("{0}", '\x0013');
        private string _dce = string.Format("{0}", '\x0014');
        private ListBox _displayBox;
        private string _dPrinterOff = string.Format("{0}{1}", '\x001b', "[4i");
        private string _dPrinterOn = string.Format("{0}{1}", '\x001b', "[5i");
        private byte[] _entireBuffer = new byte[0x7d01];
        private List<Filters> _filters = new List<Filters>();
        private Dictionary<string, FontMapping> _fontMappings = new Dictionary<string, FontMapping>(20);
        private FrmConnect _frmConnect;
        private string _fullCut = string.Format("{0}{1}{2}", new object[] { '\x001b', "i", "".PadRight(40), ' ' });
        private Panel _functionKeysPanel;
        private string _host = string.Empty;
        private string _hostName = string.Empty;
        private int _pagesPrinted;
        private string _partialCut = string.Format("{0}{1}{2}", new object[] { '\x001b', "m", "".PadRight(20), ' ' });
        private int _port;
        private int _printedLines;
        private string _printerOff = string.Format("{0}{1}", '\x001b', "[4i");
        private string _printerOn = string.Format("{0}{1}", '\x001b', "[5i");
        private Dictionary<string, string> _printers = new Dictionary<string, string>(3);
        private bool _printingOn;
        private string _printMessage = string.Empty;
        private string _printToFileOff = string.Format("{0}{1}{2}", '\x0004', '\x001b', "[4i");
        private string _printToFileOn = string.Format("{0}{1}{2}", '\x001b', "[5i", '\x0004');
        private string _reportPrinterOff = string.Format("{0}{1}{2}", '\x0001', '\x001b', "[4i");
        private string _reportPrinterOn = string.Format("{0}{1}{2}", '\x001b', "[5i", '\x0001');
        private string _reportsDirectory = string.Empty;
        private string _reportToSave = string.Empty;
        private float _saveFontSize;
        private bool _savingOn;
        private StringBuilder _sb = new StringBuilder();
        private int _scriptCnt;
        private bool _showExceptionInMsgBox;
        private bool _showFunctionKeys;
        private bool _showListBox;
        private int _startPosition;
        private Settings _userSettings;
        private bool _windowsPrinting;
        private string _xmlScript = string.Empty;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private string APP_NAME = null;
        // private const string APP_NAME = MSISKYPORTVERSION;
        private string AT_PROMPT = string.Format(CultureInfo.CurrentCulture, "{0}", new object[] { '\x0004' });
        private const string BAGTAGS = "BagTags";
        public string barCodeData = string.Format("{0}{1}{2}{3}{4}", new object[] { '\x001d', 'k', 'E', '\v', "Q" });
        public string barCodeData2= string.Format("{0}{1}{2}{3}{4}", new object[] { '\x001d', 'k', 'E', '\u000F', "Q" });
      //  public string barCodeData3 = string.Format("{0}{1}{2}{3}{4}{5}{4}{4}{4}{4}", new object[] { '\x001d', 'k', 'E', '\u000F', "Q" });
        public string barCodeHeader = string.Format("{0}{1}", '\x001d', "h9");
        public string barCodeHeight = string.Format("{0}{1}", '\x001d', "hd");
        public string barCodeWidth = string.Format("{0}{1}{2}", '\x001d', 'w', '\x0002');
        public string ascii = string.Format("{0}",'\x001d');
        private string BEING_PROMPTED = string.Format(CultureInfo.CurrentCulture, "{0}", new object[] { '\x0017' });
        private const string BOARDINGPASS = "BoardingPass";
        private Button btnExportMemory;
        private Button btnExportScreen;
        private Button btnHelp;
        private Button btnPrtScreen;
        private IContainer components;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem disconnectToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private const short MAXBUFFER = 0x7d00;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private const string REPORTS = "Reports";
        private ToolStripMenuItem settingsToolStripMenuItem;
        private string size10 = string.Format("{0}{1}{2}", '\x001b', '!', '\n');
        private string size12 = string.Format("{0}{1}{2}", '\x001b', '!', '\f');
        private string size16 = string.Format("{0}{1}{2}", '\x001b', "!", '\x000e');
        private string size20 = string.Format("{0}{1}{2}", '\x001b', "!", '\x0010');
        private string size30 = string.Format("{0}{1}{2}", '\x001b', "!", '\x0018');
        private string size41 = string.Format("{0}{1}{2}", '\x001b', "!", '!');
        private string size45 = string.Format("{0}{1}{2}", '\x001b', "!", '%');
        private string size50 = string.Format("{0}{1}{2}", '\x001b', "!", '(');
        private string size60 = string.Format("{0}{1}{2}", '\x001b', "!", '0');
        private string size61 = string.Format("{0}{1}{2}", '\x001b', "!", '1');
        private string size67 = string.Format("{0}{1}{2}", '\x001b', "!", '7');
        private StatusStrip statusStrip1;
        internal Dart.PowerTCP.Telnet.Telnet telnet1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private Vt vt1;

        //////// 변경되는 부분 //////
        string[] INTairport;
        string[] RemoveINTairport;
        int barcodetype = 1;
        string MSISKYPORTVERSION;
        string domesticBoardingTerm = "15";
        string internationalBoardingTerm = "30";
        string[] forbidCommand;

        string[] months =
        new string[] { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        //string sPattern = "^[ID]{1}[ZE]{2}[0-9]{5}[0-9]{4}[0-9]{8}[CHMK]{1}[0-4]{1}[A-Z]{3}\r\n$";   //2d 바코드 대문자
                      //정규식D     ZE     00201   0011    20141008 H      1       GMP
        string sPattern = "^[ID|id]{1}[ZE|ze]{2}[0-9]{5}[0-9]{4}[0-9]{8}[CHMK|chmk]{1}[0-4]{1}[A-Z|a-z]{3}\r\n$";   //2d 바코드 소문자
                      //정규식D     ZE     00201   0011    20141008 H      1       GMP

        //c
        string recvData = null; // 화면 출력 메세지
        string[] payment = new string[20]; //ct환불 금지
        int printtype = 0;  //국제 성인 유아, 국내 성인 유아 
        int CTlengths = 0;  //CT결재 내역 갯수
        // wav파일 실행을 위해서 
        [System.Flags]
        public enum PlaySoundFlags : int
        {
            SND_SYNC = 0x0000,
            SND_ASYNC = 0x0001,
            SND_NODEFAULT = 0x0002,
            SND_LOOP = 0x0008,
            SND_NOSTOP = 0x0010,
            SND_NOWAIT = 0x00002000,
            SND_FILENAME = 0x00020000,
            SND_RESOURCE = 0x00040004
        }
        [System.Runtime.InteropServices.DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode, ThrowOnUnmappableChar = true)]
        private static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);

        public FrmMain()
        {
            this.InitializeComponent();

            try
            {
                if (System.IO.Directory.Exists(@"C:\Program Files\Navitaire\Navitaire TE"))
                {
                    try
                    {
                        // System.IO.Directory.Delete(@"C:\Program Files\Navitaire\Navitaire TE", true);
                        MessageBox.Show("C:\\Program Files\\Navitaire\\Navitaire TE 위치한 나비테어 스카이포트는 삭제를 해주시기 바랍니다");
                    }

                    catch (System.IO.IOException e)
                    {

                    }
                }
                string InternationalAirPortSetup = ConfigurationSettings.AppSettings["InternationalAirPortSetup"].ToString();
                string RemoveINTairportSetup = "제주,청주," + ConfigurationSettings.AppSettings["RemoveINTairportSetup"].ToString();
                string barcodetypeSetup = ConfigurationSettings.AppSettings["PrinttypeSetup"].ToString();
                string forbidcommand = ConfigurationSettings.AppSettings["forbidCommand"].ToString();

                MSISKYPORTVERSION = ConfigurationSettings.AppSettings["MSISKYPORTVERSION"].ToString();
                domesticBoardingTerm = ConfigurationSettings.AppSettings["DomesticBoardingTerm"].ToString();
                internationalBoardingTerm = ConfigurationSettings.AppSettings["InternationalBoardingTerm"].ToString();

                INTairport = InternationalAirPortSetup.Split(',');
                RemoveINTairport = RemoveINTairportSetup.Split(',');
                forbidCommand = forbidcommand.Split(',');

                for (int i = 0; i < forbidCommand.Length; i++)
                    forbidCommand[i] = forbidCommand[i] + "\r\n";

                for (int removeairpoftcount = 0; removeairpoftcount < RemoveINTairport.Length; removeairpoftcount++)
                {
                    RemoveINTairport[removeairpoftcount] = "!( " + RemoveINTairport[removeairpoftcount];
                }

                barcodetype = Int32.Parse(barcodetypeSetup);
                APP_NAME = MSISKYPORTVERSION;
            }
            catch (Exception ex)
            {
                MessageBox.Show("MSISETUP.config 파일 로드 실패");
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        private void btnExportMemory_Click(object sender, EventArgs e)
        {
            this.doPrtMemoryNotepad();
        }

        private void btnExportScreen_Click(object sender, EventArgs e)
        {
            this.doPrtScreenNotepad();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            this.doHelp();
        }

        private void btnPrtScreen_Click(object sender, EventArgs e)
        {
            this.vt1.PrintScreen();
        }

        private bool checkConnected()
        {
            if (this.telnet1.Connected)
            {
                this.toolStripStatusLabel1.Text = string.Format("Connected to  {0}  Type:{1}  Printer:{2}  {3}", new object[] { this._hostName, this.telnet1.TerminalType, this.BoardingPassPrinter, this.WindowsPrinting ? "Windows Printing Enabled" : "Print Pass-Through Enabled" });
                this.connectToolStripMenuItem.Enabled = false;
                this.disconnectToolStripMenuItem.Enabled = true;
                return true;
            }
            this.toolStripStatusLabel1.Text = "Not Connected";
            this.connectToolStripMenuItem.Enabled = true;
            this.disconnectToolStripMenuItem.Enabled = false;
            return false;
        }

        private string checkEscapeCodes(string message)
        {
            string str = string.Format(CultureInfo.CurrentCulture, "{0}{1}", new object[] { '\x001b', "[?3l" });
            string str2 = string.Format(CultureInfo.CurrentCulture, "{0}{1}", new object[] { '\x001b', "[?3h" });
            if (message.IndexOf(str, 0) != -1)
            {
                if (this._showListBox)
                {
                    this.ShowListBox = true;
                }
                message = message.Replace(str, string.Empty);
                Dart.PowerTCP.Emulation.Font font = new Dart.PowerTCP.Emulation.Font(this.vt1.FontEx.Name, this._saveFontSize, this.GetFontStyle(false, false));
                this.vt1.FontEx = font;
                this.DoResize(80, false);
            }
            else if (message.IndexOf(str2, 0) != -1)
            {
                this._displayBox.Enabled = false;
                this._displayBox.Visible = false;
                message = message.Replace(str2, string.Empty);
                float emSize = this._saveFontSize * 0.85f;
                Dart.PowerTCP.Emulation.Font font2 = new Dart.PowerTCP.Emulation.Font(this.vt1.FontEx.Name, emSize, this.GetFontStyle(false, false));
                this.vt1.FontEx = font2;
                this.DoResize(0x84, false);
            }
            if ((message.IndexOf(this._bagTagPrinterOn) != -1) && ((message.IndexOf("BTT") != -1) || (message.IndexOf("BTP") != -1)))
            {
                message = message.Replace(this._bagTagPrinterOn, this._printerOn + this._bagTagPrinterOn);
                this.SelectedPrinter = this.BagTagPrinter;
                this._bagTagOn = true;
            }
            if (this._bagTagOn && (message.IndexOf(this._bagTagPrinterOff) != -1))
            {
                message = message.Replace(this._bagTagPrinterOff, this._bagTagPrinterOff + this._printerOff);
                this.SelectedPrinter = this.BagTagPrinter;
            }
            else if ((message.IndexOf(this._reportPrinterOn) != -1) || (message.IndexOf(this._reportPrinterOff) != -1))
            {
                this.SelectedPrinter = this.ReportPrinter;
                this._bagTagOn = false;
            }
            else if ((message.IndexOf(this._bagTagPrinterOn) == -1) && (message.IndexOf(this._reportPrinterOn) == -1))
            {
                this.SelectedPrinter = this.BoardingPassPrinter;
                this._bagTagOn = false;
            }
            if (!this._bagTagOn)
            {
                message = this.doPrintToFile(message);
            }
            if (this.WindowsPrinting && (this.SelectedPrinter == this.BoardingPassPrinter))
            {
                message = this.doWindowsPrinting(message);
            }
            return message;
        }

        private string checkException(string message)
        {
            if (this._showExceptionInMsgBox)
            {
                string text = string.Empty;
                int index = message.IndexOf(this._dc1, 0);
                int startIndex = message.IndexOf(this._dc2, 0);
                int num3 = message.IndexOf(this._dc3, 0);
                int num4 = message.IndexOf(this._dce, 0);
             
                if ((message.IndexOf("GeneralException", 0) != -1) || (index != -1))
                {
                    boardingerrorMessage(message);//추가-jeeho
                    if (index != -1)
                    {
                        message = message.Remove(index, 1);
                    }
                    if (num4 != -1)
                    {
                        num4--;
                        message = message.Remove(num4, 1);
                    }
                    if ((index != -1) && (num4 != -1))
                    {
                        try
                        {
                            text = message.Substring(index, num4 - index);
                            message = message.Substring(num4, message.Length - num4);
                            MessageBox.Show(text, "General User Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(message, "General User Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        MessageBox.Show(message, "General User Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        message = this.AT_PROMPT;
                    }
                }
                if ((message.IndexOf("WarningException", 0) != -1) || (startIndex != -1))
                {
                    boardingerrorMessage(message);//추가-jeeho
                    if (startIndex != -1)
                    {
                        message = message.Remove(startIndex, 1);
                    }
                    if (num4 != -1)
                    {
                        num4--;
                        message = message.Remove(num4, 1);
                    }
                    if ((startIndex != -1) && (num4 != -1))
                    {
                        text = message.Substring(startIndex, num4 - startIndex);
                        message = message.Substring(num4, message.Length - num4);
                        MessageBox.Show(text, "Critical Error Occured!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show(message, "Critical Error Occured!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        message = this.AT_PROMPT;
                    }
                }
                if ((message.IndexOf("CriticalException", 0) != -1) || (num3 != -1))
                {
                    boardingerrorMessage(message);//추가-jeeho
                    if (num3 != -1)
                    {
                        message = message.Remove(num3, 1);
                    }
                    if (num4 != -1)
                    {
                        num4--;
                        message = message.Remove(num4, 1);
                    }
                    if ((num3 != -1) && (num4 != -1))
                    {
                        text = message.Substring(num3, num4 - num3);
                        message = message.Substring(num4, message.Length - num4);
                        MessageBox.Show(text, "Critical Error Occured Cannot Continue - Notify Navitaire", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        message = this.AT_PROMPT;
                        return message;
                    }
                    MessageBox.Show(message, "Critical Error Occured Cannot Continue - Notify Navitaire", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    message = this.AT_PROMPT;
                }
                text = text + "";
            }
            return message;
        }

        private string checkForDisplayListBox(string message, ListBox box)
        {
            if (box.Enabled)
            {
                foreach (Filters filters in this._filters)
                {
                    if ((filters.FilterType == FilterType.DisplayList) && (message.IndexOf(filters.FilterValue, 0) != -1))
                    {
                        message = this.formatMessage(message, box);
                    }
                }
            }
            return message;
        }

        private string checkForLabels(string message)
        {
            int index = message.IndexOf(":", 0);
            int num2 = message.IndexOf(")", 0);
            int num3 = message.IndexOf(">", 0);
            int num4 = message.IndexOf(string.Format(">{0}", '\x0004'), 0);
            int num5 = message.IndexOf(string.Format(">{0}{1}{2}", '\x001b', "[0m", '\x0004'), 0);
            try
            {
                if ((((message.Length < 0x5f) && (index != -1)) && ((num2 != -1) && (num3 != -1))) && ((num4 > 0x27) || (num5 > 0x27)))
                {
                    string str = message.Substring(index + 3, num3 - (index + 2)).Replace('\n', ' ').Replace('\r', ' ');
                    int totalWidth = 80 - (str.Length / 2);
                    totalWidth -= MSISKYPORTVERSION.Length;
                    if (totalWidth < 0)
                    {
                        totalWidth = 1;
                    }
                    this.Text = string.Format("{0}{1}* {2} *", MSISKYPORTVERSION, " ".PadRight(totalWidth), str);
                }
            }
            catch (Exception)
            {
            }
            if (message.IndexOf("No Flight Manifest Loaded", 0) != -1)
            {
                this.Text = string.Format("{0}", MSISKYPORTVERSION);
            }
            index = message.IndexOf("Total Bag Weight");
            if (index != -1)
            {
                num2 = message.IndexOf("-", index);
                if (num2 != -1)
                {
                    message = message.Remove(num2 + 1, 2);
                }
            }
            return message;
        }

        private string checkPopUps(string message)
        {
            foreach (Filters filters in this._filters)
            {
                if ((filters.FilterType == FilterType.MessageBox) && (message.IndexOf(filters.FilterValue, 0) != -1))
                {
                    MessageBox.Show(message, filters.FilterName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    message = this.AT_PROMPT;
                }
            }
            return message;
        }

        private void closeUpShop()
        {
            if (this._commands.Count > 0)
            {
                Thread.Sleep(0x1388);
            }
            if (this.telnet1.Connected)
            {
                this.telnet1.Dispose();
            }
            base.Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.telnet1.Connected)
            {
                this.telnet1.Close();
                this.checkConnected();
            }
            this.vt1.Clear();
            this._frmConnect = new FrmConnect(MSISKYPORTVERSION);
            if (this._frmConnect.ShowDialog() == DialogResult.OK)
            {
                this.makeConnection();
                this.vt1.Focus();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.vt1.SelectionLength < 1)
            {
                MessageBox.Show("Please select some text to copy first.");
            }
            else
            {
                Clipboard.SetDataObject("");
                Clipboard.SetDataObject(this.vt1.ScrapeText(this.vt1.SelectionStart, this.vt1.SelectionLength));
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.telnet1.Connected)
            {
                this.vt1.Clear();
                this.telnet1.Close();
                this.checkConnected();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoConnect()
        {
            try
            {
                this.telnet1.Connect(this._host, this._port);
            }
            catch (Exception exception)
            {
                this.Text = MSISKYPORTVERSION;
                this.ShowError(string.Format(CultureInfo.CurrentCulture, "{0}:{1} {2}", new object[] { this._host, this._port, exception.Message }));
            }
        }

        private void doHelp()
        {
            if (this.checkConnected())
            {
                this.vt1.Write("Help");
                this.telnet1.Send("Help\r");
            }
        }

        private string doPrintToFile(string message)
        {
            int index = message.IndexOf(this._printToFileOn, 0);
            int length = message.IndexOf(this._printToFileOff, 0);
            int startIndex = index + this._printToFileOn.Length;
            int num4 = (length - startIndex) - 1;
            if (!this._savingOn && (index != -1))
            {
                if (length != -1)
                {
                    try
                    {
                        this._reportToSave = message.Substring(startIndex, num4);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Substring error(0):" + message);
                    }
                    message = message.Substring(0, index);
                    this.doPrtReportNotepad();
                }
                else
                {
                    try
                    {
                        this._reportToSave = message.Substring(startIndex, message.Length - startIndex);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(string.Format("Substring error(1) \n{0}\n:{1}\n:{2}", this._reportToSave, message, startIndex));
                    }
                    this._savingOn = true;
                    message = message.Substring(0, index);
                }
            }
            if ((this._savingOn && (index == -1)) && (length != -1))
            {
                try
                {
                    this._reportToSave = this._reportToSave + message.Substring(0, length);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Substring error(2) \n[printmessage]:{0}\n[message]:{1}\n[start]:{2}\n[printoffpos]:{3}", new object[] { this._reportToSave, message, startIndex, length }));
                }
                message = message.Substring(length, message.Length - length);
                this._savingOn = false;
                this.doPrtReportNotepad();
            }
            if ((this._savingOn && (index == -1)) && (length == -1))
            {
                this._reportToSave = this._reportToSave + message;
                message = string.Empty;
            }
            if ((this._savingOn && (index != -1)) && (length != -1))
            {
                try
                {
                    this._reportToSave = this._reportToSave + message.Substring(0, length);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Substring error(3) \n{0}\n:{1}\n:{2}", this._reportToSave, message, message.Substring(0, length)));
                }
                this.doPrtReportNotepad();
                try
                {
                    message = message.Substring(length + this._printToFileOff.Length, message.Length - (length + this._printToFileOff.Length));
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Substring error(4) \n{0}", message));
                }
                this._savingOn = true;
                int num5 = message.IndexOf(this._printToFileOn, 0) + this._printToFileOn.Length;
                this._reportToSave = message.Substring(num5, message.Length - num5);
                message = string.Empty;
            }
            return message;
        }

        private void doPrtMemoryNotepad()
        {
            string path = "NavitaireTEOutPut.txt";
            try
            {
                StreamWriter writer = File.CreateText(path);
                string message = this.vt1.ScrapeText(0, 0, this.vt1.TextLength);
                string[] strArray = this.unStringMessage(message);
                for (int i = 0; i < strArray.Length; i++)
                {
                    writer.WriteLine(strArray[i]);
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                this.ShowError(string.Format(CultureInfo.CurrentCulture, "Writing to file :{0}:{1}", new object[] { path, exception.Message }));
            }
            this.showTextFile(path);
        }

        private void doPrtReportNotepad()
        {
            string path = string.Format(@"{0}\{1}{2}{3}", new object[] { this._reportsDirectory, "NavitaireTEReportOutPut_", DateTime.Now.ToString("yyyyMMdd_HHmmss_fffffff"), ".txt" });
            try
            {
                StreamWriter writer = File.CreateText(path);
                string[] strArray = this.unStringMessage(this._reportToSave);
                for (int i = 0; i < strArray.Length; i++)
                {
                    writer.WriteLine(strArray[i]);
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                this.ShowError(string.Format(CultureInfo.CurrentCulture, "Writing to file :{0}:{1}", new object[] { path, exception.Message }));
                return;
            }
            this.showTextFile(path);
        }

        private void doPrtScreenNotepad()
        {
            string path = "NavitaireTEScreenOutPut.txt";
            this.vt1.SelectionStart = this.vt1.BufferRows * (this.vt1.ScreenSize.Width + 2);
            this.vt1.SelectionLength = this.vt1.ScreenSize.Height * (this.vt1.ScreenSize.Width + 2);
            try
            {
                StreamWriter writer = File.CreateText(path);
                string message = this.vt1.ScrapeText(this.vt1.SelectionStart, this.vt1.SelectionLength);
                string[] strArray = this.unStringMessage(message);
                for (int i = 0; i < strArray.Length; i++)
                {
                    writer.WriteLine(strArray[i]);
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                this.ShowError(string.Format(CultureInfo.CurrentCulture, "Writing to file :{0}:{1}", new object[] { path, exception.Message }));
            }
            this.vt1.SelectionStart = 0;
            this.vt1.SelectionLength = 0;
            this.showTextFile(path);
        }

        private void DoResize(bool sendToServer)
        {
            base.Height = ((this.vt1.ScreenSize.Height * this.vt1.CharacterSize.Height) + this.vt1.Margin.Height) + (base.Height - this.vt1.Height);
            base.Width = (((this.vt1.ScreenSize.Width * this.vt1.CharacterSize.Width) + this.vt1.Margin.Width) + SystemInformation.VerticalScrollBarWidth) + (base.Width - this.vt1.Width);
            if (this.telnet1.Connected && sendToServer)
            {
                this.telnet1.Send(this.vt1.Encoding.GetBytes(string.Format("Set DisplayLines={0}{1}", this.vt1.ScreenSize.Height, '\r')));
                this.telnet1.Send(this.vt1.Encoding.GetBytes(string.Format("Set DisplayColumns={0}{1}", this.vt1.ScreenSize.Width, '\r')));
            }
        }

        private void DoResize(int width, bool sendToServer)
        {
            this.vt1.Telnet.WindowSize = new Size(width, this.vt1.ScreenSize.Height);
            this.vt1.ScreenSize = new Size(width, this.vt1.ScreenSize.Height);
            this.DoResize(sendToServer);
        }

        private string doWindowsPrinting(string message)
        {
            int index = message.IndexOf(this._printerOn, 0);
            int length = message.IndexOf(this._printerOff, 0);
            int startIndex = index + this._printerOn.Length;
            int num4 = (length - startIndex) - 1;

            if (!this._printingOn && (index != -1))
            {
                if (length != -1)
                {
                    try
                    {
                        this._printMessage = message.Substring(startIndex, num4);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Substring error(0):" + message);
                    }
                    this.printDocument();
                    message = this.doWindowsPrinting(message.Remove(index, (length + this._printerOff.Length) - index));
                    return message;
                }
                try
                {
                    this._printMessage = message.Substring(startIndex, message.Length - startIndex);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Substring error(1) \n{0}\n:{1}\n:{2}", this._printMessage, message, startIndex));
                }
                this._printingOn = true;
                message = message.Substring(0, index);
            }
            if ((this._printingOn && (index == -1)) && (length != -1))
            {
                try
                {
                    this._printMessage = this._printMessage + message.Substring(0, length);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Substring error(2) \n[printmessage]:{0}\n[message]:{1}\n[start]:{2}\n[printoffpos]:{3}", new object[] { this._printMessage, message, startIndex, length }));
                }
                message = message.Substring(length + this._printerOff.Length, message.Length - (length + this._printerOff.Length));
                this._printingOn = false;
                this.printDocument();
            }
            if ((this._printingOn && (index == -1)) && (length == -1))
            {
                this._printMessage = this._printMessage + message;
                message = string.Empty;
            }
            if ((this._printingOn && (index != -1)) && (length != -1))
            {
                try
                {
                    this._printMessage = this._printMessage + message.Substring(0, length);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Substring error(3) \n{0}\n:{1}\n:{2}", this._printMessage, message, message.Substring(0, length)));
                }
                this.printDocument();
                try
                {
                    message = message.Substring(length + this._printerOff.Length, message.Length - (length + this._printerOff.Length));
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Substring error(4) \n{0}", message));
                }
                this._printingOn = true;
                int num5 = message.IndexOf(this._printerOn, 0) + this._printerOn.Length;
                this._printMessage = message.Substring(num5, message.Length - num5);
                message = string.Empty;
            }
            return message;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.closeUpShop();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.checkConnected();
        }

        private float fontSize(string size)
        {
            if (this._fontMappings.ContainsKey(size))
            {
                try
                {
                    return this._fontMappings[size].TEFontSize;
                }
                catch (Exception)
                {
                    MessageBox.Show("Font size conversion error, please check app config for size:" + size, "Mapping Error");
                    goto Label_0051;
                }
            }
            MessageBox.Show("Font size conversion not mapped, please check app config for size:" + size, "Mapping Error");
        Label_0051:
            return 9f;
        }

        private FontStyle fontStyle(string size)
        {
            if (this._fontMappings.ContainsKey(size))
            {
                try
                {
                    return this._fontMappings[size].FontStyle;
                }
                catch (Exception)
                {
                    MessageBox.Show("Font size conversion error, please check app config for size:" + size, "Mapping Error");
                    goto Label_0051;
                }
            }
            MessageBox.Show("Font size conversion not mapped, please check app config for size:" + size, "Mapping Error");
        Label_0051:
            return FontStyle.Regular;
        }

        private string formatMessage(string message, ListBox box)
        {
            string[] strArray = this.unStringMessage(message);
            StringBuilder builder = new StringBuilder();
            try
            {
                box.Items.Clear();
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].IndexOf('\x001b') != -1)
                    {
                        builder.Append(strArray[i]);
                    }
                    else
                    {
                        strArray[i].IndexOf(':');
                        box.Items.Add(strArray[i]);
                    }
                }
            }
            catch (Exception exception)
            {
                this.ShowError(string.Format(CultureInfo.CurrentCulture, "{0}:{1}", new object[] { "formatMessage", exception.Message }));
            }
            return builder.ToString();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this._displayBox.Enabled = false;
            this.vt1.Encoding = Encoding.UTF8;
            this.getSettings();
            this.telnet1.SynchronizingObject = this;
            this.vt1.Telnet.WindowSize = this.vt1.ScreenSize;
            this.vt1.AutoWrap = true;
            this.DoResize(true);
            this.vt1.Focus();
            this._commands = this.loadScript();
            if (this._commands.Count > 0)
            {
                this.startScript();
            }
            this.Text = MSISKYPORTVERSION;
        }

        private NationalCharSet GetCharSet(string charSet)
        {
            switch (charSet.ToLower())
            {
                case "british":
                    return NationalCharSet.British;

                case "dutch":
                    return NationalCharSet.Dutch;

                case "finish":
                    return NationalCharSet.Finnish;

                case "french":
                    return NationalCharSet.French;

                case "frenchcanadian":
                    return NationalCharSet.FrenchCanadian;

                case "german":
                    return NationalCharSet.German;

                case "italian":
                    return NationalCharSet.Italian;

                case "norwegiandanish":
                    return NationalCharSet.NorwegianDanish;

                case "portuguese":
                    return NationalCharSet.Portuguese;

                case "spanish":
                    return NationalCharSet.Spanish;

                case "swedish":
                    return NationalCharSet.Swedish;

                case "swiss":
                    return NationalCharSet.Swiss;
            }
            return NationalCharSet.Ascii;
        }

        private FontStyle GetFontStyle(bool bold, bool italic)
        {
            FontStyle regular = FontStyle.Regular;
            if (bold)
            {
                regular |= FontStyle.Bold;
            }
            if (italic)
            {
                regular |= FontStyle.Italic;
            }
            return regular;
        }

        private void getSettings()
        {
            string familyName = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "FontName", "Courier New").ToString();
            float emSize = (float)Convert.ToDecimal(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "FontSize", 12.75));
            bool bold = Convert.ToBoolean(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "FontBold", false));
            bool italic = Convert.ToBoolean(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "FontItalic", false));
            Dart.PowerTCP.Emulation.Font font = new Dart.PowerTCP.Emulation.Font(familyName, emSize, this.GetFontStyle(bold, italic));
            this.vt1.FontEx = font;
            this._saveFontSize = this.vt1.FontEx.Size;
            this.vt1.ForeColor = ColorTranslator.FromHtml(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "ForeColor", "Lime").ToString());
            this.vt1.BoldColor = ColorTranslator.FromHtml(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "BoldColor", "White").ToString());
            this.vt1.BackColor = ColorTranslator.FromHtml(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "BackColor", "Black").ToString());
            this.vt1.NationalCharSet = this.GetCharSet(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "CharSet", "Ascii").ToString());
            this.vt1.NormalIntensity = Convert.ToInt32(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "NormalIntensity", 100));
            this.vt1.BlinkIntensity = Convert.ToInt32(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "BlinkIntensity", 50));
            int height = Convert.ToInt32(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "Height", 0x1a));
            int width = Convert.ToInt32(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "Width", 80));
            this.vt1.ScreenSize = new Size(width, height);
            this.vt1.BufferRows = Convert.ToInt32(Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "BufferRows", 0x138));
            string str2 = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "ShowListBox", "false").ToString();
            this.ShowListBox = str2 == "true";
            string str3 = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "ShowFunctionKeys", "true").ToString();
            this.ShowFunctionKeys = str3 == "true";
            string str4 = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "ExceptionInMsgBox", "true").ToString();
            this.ShowExceptionInMsgBox = str4 == "true";
            this.BoardingPassPrinter = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "BoardingPassPrinter", string.Empty).ToString();
            this.ReportPrinter = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "ReportPrinter", string.Empty).ToString();
            this.BagTagPrinter = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "BagTagPrinter", string.Empty).ToString();
            this.ReportsDirectory = Utilities.GetUtilitiesSettingsObject(MSISKYPORTVERSION, "ReportsDirectory", ".").ToString();
            try
            {
                this._userSettings = new Settings();
                Connections connections = new Connections(this._userSettings.Hosts[0]);
                this._host = connections.ConnectionHost;
                this._port = connections.Port;
                this._hostName = connections.ConnectionName;
                if (string.IsNullOrEmpty(this.BoardingPassPrinter))
                {
                    this.BoardingPassPrinter = this._userSettings.BoardingPassPrinter;
                    if (this.BoardingPassPrinter == string.Empty)
                    {
                        PrintDocument document = new PrintDocument();
                        this.BoardingPassPrinter = document.PrinterSettings.PrinterName;
                    }
                }
                if (string.IsNullOrEmpty(this.ReportPrinter))
                {
                    if (this._userSettings.ReportPrinter == string.Empty)
                    {
                        this.ReportPrinter = this.BoardingPassPrinter;
                    }
                    else
                    {
                        this.ReportPrinter = this._userSettings.ReportPrinter;
                    }
                }
                if (string.IsNullOrEmpty(this.BagTagPrinter))
                {
                    if (this._userSettings.BagTagPrinter == string.Empty)
                    {
                        this.BagTagPrinter = this.BoardingPassPrinter;
                    }
                    else
                    {
                        this.BagTagPrinter = this._userSettings.BagTagPrinter;
                    }
                }
                if ((string.IsNullOrEmpty(this.ReportsDirectory) || (this.ReportsDirectory == @"\")) && (this._userSettings.ReportsDirectory != string.Empty))
                {
                    this.ReportsDirectory = this._userSettings.ReportsDirectory;
                }
                if (!this._userSettings.AllowConnectionChanges)
                {
                    this.connectToolStripMenuItem.Enabled = false;
                }
                if (this._userSettings.RunScript)
                {
                    this._xmlScript = this._userSettings.XMLScript;
                }
                this.WindowsPrinting = this.UserSettings.WindowsPrinting;
                if (!string.IsNullOrEmpty(this._userSettings.Encoding))
                {
                    this.vt1.Encoding = Encoding.GetEncoding(this._userSettings.Encoding);
                }
                if (!string.IsNullOrEmpty(this._userSettings.CharacterSize))
                {
                    string[] strArray = this._userSettings.CharacterSize.Split(",".ToCharArray(), 2);
                    int num4 = 0;
                    int num5 = 0;
                    if (strArray.Length == 2)
                    {
                        try
                        {
                            num4 = Convert.ToInt32(strArray[0]);
                            num5 = Convert.ToInt32(strArray[1]);
                        }
                        catch
                        {
                        }
                    }
                    if ((num4 != 0) && (num5 != 0))
                    {
                        this.vt1.CharacterSize = new Size(num4, num5);
                    }
                }
                // 원본 : using (StringEnumerator enumerator = this._userSettings.FontSizeMappings.GetEnumerator())
                StringEnumerator enumerator = this._userSettings.FontSizeMappings.GetEnumerator();
                {
                    while (enumerator.MoveNext())
                    {
                        string[] strArray2 = enumerator.Current.Split(", ".ToCharArray(), 4);
                        if (strArray2.Length > 2)
                        {
                            this._fontMappings.Add(strArray2[0], new FontMapping(strArray2[0], strArray2[1], strArray2[2]));
                        }
                        else
                        {
                            this._fontMappings.Add(strArray2[0], new FontMapping(strArray2[0], strArray2[1], " "));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.ShowError("Error reading app.confg:" + exception.Message);
            }
            this.readSettingTheOldWay();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmMain));
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.connectToolStripMenuItem = new ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.editToolStripMenuItem = new ToolStripMenuItem();
            this.copyToolStripMenuItem = new ToolStripMenuItem();
            this.pasteToolStripMenuItem = new ToolStripMenuItem();
            this.optionsToolStripMenuItem = new ToolStripMenuItem();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.statusStrip1 = new StatusStrip();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new ToolStripStatusLabel();
            this.telnet1 = new Dart.PowerTCP.Telnet.Telnet(this.components);
            this.vt1 = new Vt(this.components);
            this._displayBox = new ListBox();
            this._functionKeysPanel = new Panel();
            this.btnExportMemory = new Button();
            this.btnExportScreen = new Button();
            this.btnPrtScreen = new Button();
            this.btnHelp = new Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this._functionKeysPanel.SuspendLayout();
            base.SuspendLayout();
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.editToolStripMenuItem, this.optionsToolStripMenuItem, this.helpToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(0x36c, 0x18);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.connectToolStripMenuItem, this.disconnectToolStripMenuItem, this.exitToolStripMenuItem });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(0x23, 20);
            this.fileToolStripMenuItem.Text = "&File";
            this.fileToolStripMenuItem.Click += new EventHandler(this.fileToolStripMenuItem_Click);
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new Size(0x7e, 0x16);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new EventHandler(this.connectToolStripMenuItem_Click);
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new Size(0x7e, 0x16);
            this.disconnectToolStripMenuItem.Text = "&Disconnect";
            this.disconnectToolStripMenuItem.Click += new EventHandler(this.disconnectToolStripMenuItem_Click);
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(0x7e, 0x16);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);
            this.editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.copyToolStripMenuItem, this.pasteToolStripMenuItem });
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new Size(0x25, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            this.copyToolStripMenuItem.Size = new Size(0x8b, 0x16);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.ToolTipText = "Select text that you wish to copy then press <copy>\r\nThis will place your selection into the Windows Clip Board";
            this.copyToolStripMenuItem.Click += new EventHandler(this.copyToolStripMenuItem_Click);
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            this.pasteToolStripMenuItem.Size = new Size(0x8b, 0x16);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.ToolTipText = "This will take the text from the Windows Clip Board and place them in the window";
            this.pasteToolStripMenuItem.Click += new EventHandler(this.pasteToolStripMenuItem_Click);
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.settingsToolStripMenuItem });
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new Size(0x38, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            this.settingsToolStripMenuItem.Size = new Size(0x97, 0x16);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new EventHandler(this.settingsToolStripMenuItem_Click);
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.aboutToolStripMenuItem });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(0x67, 0x16);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripStatusLabel1, this.toolStripStatusLabel2 });
            this.statusStrip1.Location = new Point(0, 0x233);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x36c, 0x16);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "Not Connected";
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(0x5b, 0x11);
            this.toolStripStatusLabel1.Text = "Not Connected";
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new Size(0, 0x11);
            this.telnet1.Charset = "UTF-8";
            Option[] items = new Option[3];
            items[0] = new Option(OptionCode.SuppressGoAheads, null, OptionState.RequestOn);
            byte[] subOption = new byte[4];
            subOption[1] = 80;
            subOption[3] = 0x1a;
            items[1] = new Option(OptionCode.WindowSize, subOption, OptionState.RequestOn);
            items[2] = new Option(OptionCode.TerminalType, new byte[] { 0, 0x76, 0x74, 0x33, 50, 0x30 }, OptionState.RequestOn);
            this.telnet1.ClientOptions.AddRange(items);
            this.telnet1.ConnectTimeout = 0x3a98;
            this.telnet1.ServerOptions.AddRange(new Option[] { new Option(OptionCode.SuppressGoAheads, null, OptionState.RequestOn), new Option(OptionCode.Echo, null, OptionState.RequestOn) });
            this.telnet1.TerminalType = "vt320";
            this.telnet1.WindowSize = new Size(80, 0x1a);
            this.telnet1.EndReceive += new SegmentEventHandler(this.telnet1_EndReceive);
            this.vt1.AutoRepeat = false;
            this.vt1.BackColor = SystemColors.Window;
            this.vt1.BoldColor = SystemColors.WindowText;
            this.vt1.Dock = DockStyle.Fill;
            this.vt1.Encoding = (Encoding)manager.GetObject("vt1.Encoding");
            this.vt1.FontEx = (Dart.PowerTCP.Emulation.Font)manager.GetObject("vt1.FontEx");
            this.vt1.ForeColor = SystemColors.WindowText;
            this.vt1.LocalEcho = true;
            this.vt1.Location = new Point(0x108, 0x18);
            this.vt1.Name = "vt1";
            this.vt1.NewLine = NewLine.CrLf;
            this.vt1.PrintPassthrough = true;
            this.vt1.ScreenSize = new Size(80, 0x1a);
            this.vt1.Size = new Size(0x264, 0x1c3);
            this.vt1.TabIndex = 0;
            this.vt1.Tabs = "8,16,24,32,40,48,56,64,72,80,88,96,104,112,120,128";
            this.vt1.Telnet.AutoReceive = true;
            Option[] optionArray3 = new Option[3];
            optionArray3[0] = new Option(OptionCode.SuppressGoAheads, null, OptionState.RequestOn);
            byte[] buffer2 = new byte[4];
            buffer2[1] = 80;
            buffer2[3] = 0x18;
            optionArray3[1] = new Option(OptionCode.WindowSize, buffer2, OptionState.RequestOn);
            optionArray3[2] = new Option(OptionCode.TerminalType, new byte[] { 0, 0x74, 0x74, 0x79 }, OptionState.RequestOn);
            this.vt1.Telnet.ClientOptions.AddRange(optionArray3);
            this.vt1.Telnet.ServerOptions.AddRange(new Option[] { new Option(OptionCode.SuppressGoAheads, null, OptionState.RequestOn), new Option(OptionCode.Echo, null, OptionState.RequestOn) });
            this.vt1.Telnet.SynchronizingObject = this.vt1;
            this.vt1.Telnet.WindowSize = new Size(80, 0x18);
            this.vt1.UseDefaultPrinter = false;
            this.vt1.UseWaitCursor = true;
            this.vt1.DoubleClick += new EventHandler(this.vt1_DoubleClick);
            this.vt1.KeyDown += new VtKeyEventHandler(this.vt1_KeyDown);
            this.vt1.KeyPress += new DataEventHandler(this.vt1_KeyPress);
            this.vt1.HelpRequested += new HelpEventHandler(this.vt1_HelpRequested);
            this._displayBox.BackColor = SystemColors.Window;
            this._displayBox.Dock = DockStyle.Left;
            this._displayBox.Font = new System.Drawing.Font("Lucida Console", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._displayBox.FormattingEnabled = true;
            this._displayBox.ItemHeight = 11;
            this._displayBox.Location = new Point(0, 0x18);
            this._displayBox.Name = "_displayBox";
            this._displayBox.Size = new Size(0x108, 0x214);
            this._displayBox.TabIndex = 5;
            this._functionKeysPanel.BackColor = SystemColors.ControlDark;
            this._functionKeysPanel.BorderStyle = BorderStyle.Fixed3D;
            this._functionKeysPanel.Controls.Add(this.btnExportMemory);
            this._functionKeysPanel.Controls.Add(this.btnExportScreen);
            this._functionKeysPanel.Controls.Add(this.btnPrtScreen);
            this._functionKeysPanel.Controls.Add(this.btnHelp);
            this._functionKeysPanel.Dock = DockStyle.Bottom;
            this._functionKeysPanel.Enabled = false;
            this._functionKeysPanel.ForeColor = SystemColors.ControlText;
            this._functionKeysPanel.Location = new Point(0x108, 0x1db);
            this._functionKeysPanel.Name = "_functionKeysPanel";
            this._functionKeysPanel.Size = new Size(0x264, 0x58);
            this._functionKeysPanel.TabIndex = 6;
            this._functionKeysPanel.Visible = false;
            this.btnExportMemory.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnExportMemory.BackColor = SystemColors.Window;
            this.btnExportMemory.FlatAppearance.BorderColor = Color.White;
            this.btnExportMemory.Font = new System.Drawing.Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnExportMemory.ForeColor = SystemColors.ControlText;
            this.btnExportMemory.Location = new Point(0x141, 0x1b);
            this.btnExportMemory.Name = "btnExportMemory";
            this.btnExportMemory.Size = new Size(90, 0x35);
            this.btnExportMemory.TabIndex = 3;
            this.btnExportMemory.Text = "(F4)\r\nExp Buffer\r\nNotepad";
            this.btnExportMemory.TextAlign = ContentAlignment.TopCenter;
            this.btnExportMemory.UseVisualStyleBackColor = false;
            this.btnExportMemory.Click += new EventHandler(this.btnExportMemory_Click);
            this.btnExportScreen.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnExportScreen.BackColor = SystemColors.Window;
            this.btnExportScreen.Font = new System.Drawing.Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnExportScreen.ForeColor = SystemColors.ControlText;
            this.btnExportScreen.Location = new Point(0xda, 0x1c);
            this.btnExportScreen.Name = "btnExportScreen";
            this.btnExportScreen.Size = new Size(90, 0x34);
            this.btnExportScreen.TabIndex = 2;
            this.btnExportScreen.Text = "(F3)\r\nExp Screen\r\nNotepad";
            this.btnExportScreen.TextAlign = ContentAlignment.TopCenter;
            this.btnExportScreen.UseVisualStyleBackColor = false;
            this.btnExportScreen.Click += new EventHandler(this.btnExportScreen_Click);
            this.btnPrtScreen.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnPrtScreen.BackColor = SystemColors.Window;
            this.btnPrtScreen.Font = new System.Drawing.Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnPrtScreen.ForeColor = SystemColors.ControlText;
            this.btnPrtScreen.Location = new Point(0x73, 0x1b);
            this.btnPrtScreen.Name = "btnPrtScreen";
            this.btnPrtScreen.Size = new Size(90, 0x34);
            this.btnPrtScreen.TabIndex = 1;
            this.btnPrtScreen.Text = "(F2)\r\nPrt Screen";
            this.btnPrtScreen.TextAlign = ContentAlignment.TopCenter;
            this.btnPrtScreen.UseVisualStyleBackColor = false;
            this.btnPrtScreen.Click += new EventHandler(this.btnPrtScreen_Click);
            this.btnHelp.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnHelp.BackColor = SystemColors.Window;
            this.btnHelp.FlatAppearance.BorderColor = Color.FromArgb(0, 0xc0, 0);
            this.btnHelp.FlatAppearance.BorderSize = 2;
            this.btnHelp.Font = new System.Drawing.Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnHelp.ForeColor = SystemColors.ControlText;
            this.btnHelp.Location = new Point(12, 0x1c);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(90, 0x34);
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "(F1)\r\nHelp";
            this.btnHelp.TextAlign = ContentAlignment.TopCenter;
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            base.AutoScaleMode = AutoScaleMode.Inherit;
            this.AutoSize = true;
            base.ClientSize = new Size(0x36c, 0x249);
            base.Controls.Add(this.vt1);
            base.Controls.Add(this._functionKeysPanel);
            base.Controls.Add(this._displayBox);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.menuStrip1);
            base.Icon = (Icon)manager.GetObject("$this.Icon");
            base.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new Size(400, 200);
            base.Name = "FrmMain";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Navitaire TE";
            base.Load += new EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this._functionKeysPanel.ResumeLayout(false);
            this._functionKeysPanel.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public string InitializePrinter()
        {
            return string.Format("{0}{1}{2}", '\x001b', "@", '\n');
        }

        private List<ScriptCommand> loadScript()
        {
            if (!string.IsNullOrEmpty(this._xmlScript))
            {
                string xmlFile = this._xmlScript;
                ScriptParser parser = new ScriptParser();
                List<ScriptCommand> list = parser.Parse(xmlFile);
                ArrayList validationErrors = parser.ValidationErrors;
                if (validationErrors.Count > 0)
                {
                    foreach (string str2 in validationErrors)
                    {
                        this.ShowError(str2);
                    }
                }
                if ((list != null) && ((list == null) || (list.Count != 0)))
                {
                    return list;
                }
            }
            return new List<ScriptCommand>();
        }

        private void makeConnection()
        {
            if (this._frmConnect == null)
            {
                this._frmConnect = new FrmConnect(MSISKYPORTVERSION);
            }
            if (!this.telnet1.Connected)
            {
                this.Text = "Navitaire TE V3.0.9  [Connecting ... " + this._frmConnect.ConnectName + "]";
                this._host = this._frmConnect.ConnectHost;
                this._hostName = this._frmConnect.ConnectName;
                this._port = this._frmConnect.ConnectPort;
                this.DoConnect();
                if (this.telnet1.Connected)
                {
                    this.checkConnected();
                    this.telnet1.ReceiveTimeout = 0;
                    this.telnet1.BeginReceive(this._buffer);
                    this.Text = MSISKYPORTVERSION;
                }
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                string message = dataObject.GetData(DataFormats.Text).ToString();
                string[] strArray = this.unStringMessage(message);
                if ((message.Length > 0) && this.telnet1.Connected)
                {
                    foreach (string str2 in strArray)
                    {
                        string data = str2.TrimEnd(new char[] { ' ' });
                        this.vt1.Write(data);
                        this.telnet1.Send(data + "\r");
                    }
                }
            }
            else
            {
                MessageBox.Show("I'm sorry but the data must be text only.");
            }
        }
        private string Create2DBarcodeData(string[] strArray)
        {
            string customerdata = "";
            string passengername = strArray[9];
            bool intState = passengername.Contains("유아");
            if (intState)
            {
                customerdata = "null";
            }
            else 
            {
                string Format_code = "D";
                string OperationCarrierDesignator = strArray[18].Substring(4, 2);
                string FlightNumber = "00" + strArray[18].Substring(7, 3);
                string tempCheckInSequenceNumber = strArray[23].Substring(22, strArray[23].Length - 22);
                string CheckInSequenceNumber = string.Format("{0:0000}", int.Parse(tempCheckInSequenceNumber));
                string[] ArrayMonth = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                string month = strArray[18].Substring(17, 3);
                int monthcount;
                for (monthcount = 0; monthcount < 12; monthcount++)
                {
                    if (String.Compare(ArrayMonth[monthcount], month) == 0)
                        break;
                }
                monthcount++;
                string.Format("{0:00}", monthcount);
                string dateofflight = "20" + strArray[18].Substring(20, 2) + string.Format("{0:00}", monthcount) + strArray[18].Substring(15, 2);
                string SourceofBoardingPassIssuance = "C";
                string FromCityAirportCode = strArray[18].Substring(25, 3);


                customerdata = Format_code + OperationCarrierDesignator + FlightNumber + CheckInSequenceNumber + dateofflight
                                      + SourceofBoardingPassIssuance + printtype.ToString() + FromCityAirportCode;
            }               
                return customerdata;
        }
        
        private string CharErrorCount(string str)
        {
            int startindex = 0;
            int errorcharcount = 0;
            char[] buf = str.ToCharArray();
            int k = 0;
            for (int j = 0; j < str.Length; j++)
            {
                if (buf[j] == '�')
                {
                    startindex = j;
                    k = j;
                    while (buf[k] == '�')
                    {
                        errorcharcount++;
                        k++;
                    }
                }
                if (startindex != 0)
                    break;
            }
            if (errorcharcount != 0)
                return str.Remove(startindex, errorcharcount);
            else
                return str;
        }
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            string[] strArray = this.unStringMessage(this._printMessage);

            printtype = 0;

            if (strArray[5].CompareTo("    !7 탑       승       권") == 0)
            {
                string passengername = strArray[9];
                bool intState = passengername.Contains("유아");

                for (int i = 0; i < INTairport.Length; i++)
                {
                    if (strArray[13].Contains(INTairport[i]) == true)
                    {
                        if (intState)
                            printtype = 4;
                        else
                            printtype = 3;
                        break;
                    }
                    else
                    {
                        if (intState)
                        {
                            printtype = 2;
                        }
                        else
                            printtype = 1;
                    }
                }
            }
            else if (strArray[1].CompareTo("!7        EASTAR JET") == 0)
            {
                string passengername = strArray[5];
                bool intState = passengername.Contains("(유아)");

                if (!intState)
                {
                    printtype = 5;
                }
                else
                    printtype = 6;
            }

            switch (printtype)
            {
                case 1:  //국내선 성인
                    PrintBoardingPass(sender, ev);
                    break;

                case 2:  //국내선 유아
                    PrintInfantBoardingPass(sender, ev);
                    break;

                case 3:   //청주,군산,제주 출발하는 국제선 노선 성인
                    PrintFromAdultBoardingPass(sender, ev);
                    break;

                case 4:   //청주,군산,제주 출발하는 국제선 노선 성인
                    PrintFromInfantBoardingPass(sender, ev);
                    break;

                case 5:   //해외에서 출발하는 국제선 노선 성인
                    PrintToBoardingPass(sender, ev);
                    break;

                case 6:   //해외에서 출발하는 국제선 노선 유아
                    PrintToBoardingPass(sender, ev);
                    break;

                default:
                    Source_pd_PrintPage(sender, ev);
                    break;
            }
        }
        private void PrintATB(int printstartpoint)
        {/*
            try
            {
                TcpClient socket = new TcpClient(ATBIP, int.Parse(ATBPORT));
                NetworkStream stream = socket.GetStream();

                try
                {
                    ASCIIEncoding asc = new ASCIIEncoding();
                    string[] strArray = this.unStringMessage(this._printMessage);

                    string stx = "CP|A|01E";
                    string fullname = GetName(strArray[printstartpoint + 3]);
                    string from = GetDeparture(strArray[printstartpoint + 12]);
                    string to = GetDestination(strArray[printstartpoint + 12]);

                    string flight =  "ZE";
                    string flightnumber = GetFlight(strArray[printstartpoint + 12]);
                    strArray[13] = strArray[13].ToUpper();
                    string departuredate = GetFlightDate(strArray[printstartpoint + 12]);

                    string departuretime = GetDeptTime(strArray[printstartpoint + 9]);
                    string gate = GetGate(strArray[printstartpoint + 15]);
                    string boardingtime =  GetBrdgTime(departuretime);
                    string seat;
                  
                    if(passengerdiscript != 4)
                        seat = GetSeat(strArray[printstartpoint + 15]);
                    else
                        seat =  "INF";

                    string sequence = string.Format("{0:0000}", int.Parse(GetSequence(strArray[printstartpoint + 17])));
                    string barcodeflightnumber = string.Format("{0:00000}", int.Parse(flightnumber));
                    string barcodedeparturedate = GetBarcodeDepartureDate(departuredate);
                  // string etx = "CP|1C01|";
                    string endpoint = "CP|1C01";
                    string barcode = "I" + "ZE" + barcodeflightnumber + sequence + barcodedeparturedate + "C" + passengerdiscript + from + "|";
                    string etx = "";
               
                    
                    if (fullname.Length > 14)
                    {
                        fullname = fullname.Substring(0, 14);
                    }

                    string bp = stx + "|" + "06" + fullname + "|" + "12" + from + "|" + "1C" + to + "|" + "1F" + flight + "|" + "21" + flightnumber + "|"
                                               + "25" + departuredate + "|" + "35" + departuretime + "|" + "3A" + gate + "|" + "3B" + boardingtime + "|" + "3C" + seat
                                               + "|" + endpoint + "|" + "EE" + barcode + etx;

                     byte[] buf = asc.GetBytes(bp);
                     WriteBinary(stream, buf);
                     Thread.Sleep(2000);
                    
                }
                catch
                {
                }
                
                if (stream != null)
                 {
                   stream.Close();
                }
                  
            }
            catch
            {
                MessageBox.Show("IER560 기기에 네트워크 연결이 되 않습니다");
            }
           */
        }
        public void WriteBinary(NetworkStream n, byte[] b)
        {
            n.Write(b, 0, b.Length);
            n.Flush();
        }

        private string GetName(string str)
        {
            string[] result = str.Split(':');
            result[1] = result[1].Replace(" ", "");
            return result[1];
        }
        private string GetBarcodeDepartureDate(string str)
        {
            string month = str.Substring(2, 3);
            int monthcount;
            for (monthcount = 0; monthcount < 12; monthcount++)
            {
                if (String.Compare(months[monthcount], month) == 0)
                    break;
            }
            monthcount++;
            string dateofflight = "20" + str.Substring(5, 2) + string.Format("{0:00}", monthcount) + str.Substring(0, 2);
            return dateofflight;
        }
        private string GetSequence(string str)
        {
            string[] result = str.Split(':');
            result[1] = result[1].Replace(" ", "");
            return result[1];
        }
        private string GetSeat(string str)
        {

            string[] result = str.Split(' ');
            return result[2];
        }
        private string GetGate(string str)
        {
            string result = str.Substring(str.Length - 3);
            result = result.Replace(" ", "");
            return result;
        }
        private string GetBrdgTime(string str)
        {
            string[] brdgtime = str.Split(':');
            DateTime datebrdg = new DateTime(1982, 12, 27, Convert.ToInt32(brdgtime[0]), Convert.ToInt32(brdgtime[1]), 0, 0);
            datebrdg = datebrdg.AddMinutes(-30);
            string hour = String.Format("{0:00}", datebrdg.Hour);
            string minute = String.Format("{0:00}", datebrdg.Minute);

            brdgtime[0] = hour + ":" + minute;
            return brdgtime[0];
        }
        private string GetDeptTime(string str)
        {
            string[] depttime = str.Split(':');
            depttime[0] = depttime[0].Substring(depttime[0].Length - 2, 2);
            depttime[1] = depttime[1].Substring(0, 2);
            return depttime[0] + ':' + depttime[1];
        }
        private string GetFlight(string str)
        {
            string result = "";
            int flightstartindex = str.IndexOf("ZE");

            if (flightstartindex != -1)
                result = str.Substring(flightstartindex + 2, 4);

            return result;
        }
        private string GetFlightDate(string str)
        {
            string result = "";
            int temp = -1;
            int flightstartindex = -1;

            for (int i = 0; i < months.Length; i++)
            {
                temp = str.IndexOf(months[i]);

                if (temp != -1)
                {
                    flightstartindex = temp - 2;    //두 칸 앞으로
                    result = str.Substring(flightstartindex, 7);
                    break;
                }
            }
            return result;
        }
        private string GetDestination(string str)
        {
            string result = "";
            result = str.Substring(str.Length - 3, 3);
            return result;
        }
        private string GetDeparture(string str)
        {
            string result = "";
            result = str.Substring(str.Length - 6, 3);
            return result;
        }
        private void pd_Print2DBarcode(PrintPageEventArgs ev, int x, int y)
        {
            string[] strArray = this.unStringMessage(this._printMessage);
            string customerdata = Create2DBarcodeData(strArray);
            Image byteimage = ImageGenerator.CreatePdf417Barcode(customerdata);
            Point ulCorner = new Point(x, y);
            ev.Graphics.DrawImage(byteimage, ulCorner);
            byteimage.Dispose();
        }
        private void PrintBoardingPass(object sender, PrintPageEventArgs ev)
        {
            System.Drawing.Font font = null;
            string familyName = "Free 3 of 9";
            // font = new System.Drawing.Font("HY헤드라인M", 9, FontStyle.Regular);
            font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
            string name = font.Name;
            float num = 0f;
            float x = 15f;
            if (!ev.PageSettings.Landscape)
            {
                x = 0f;
            }
            float top = ev.MarginBounds.Top;
            top = 0f;
            float y = top;
            string s = null;
            num = ((float)ev.MarginBounds.Height) / font.GetHeight(ev.Graphics);
            string[] strArray = this.unStringMessage(this._printMessage);

            strArray = DivideAirportName(strArray);

            strArray[4] = strArray[4].ToUpper();
        


            int num5 = 0;
           
            if (barcodetype == 2)
                pd_Print2DBarcode(ev, 0, 520);

            for (int i = this._printedLines; i < strArray.Length; i++)
            {
                string str4 = strArray[i];
                s = str4;
                y += font.GetHeight(ev.Graphics);
                if (font.Name == familyName)
                {
                    font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
                }
                if (str4.IndexOf(this.size10) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("10"), this.fontStyle("10"));
                    s = str4.Replace(this.size10, string.Empty);
                }
                else if (str4.IndexOf(this.size12) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("12"), this.fontStyle("12"));
                    s = str4.Replace(this.size12, string.Empty);
                }
                else if (str4.IndexOf(this.size16) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("16"), this.fontStyle("16"));
                    s = str4.Replace(this.size16, string.Empty);
                }
                else if (str4.IndexOf(this.size20) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("20"), this.fontStyle("20"));
                    s = str4.Replace(this.size20, string.Empty);
                }
                else if (str4.IndexOf(this.size30) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("30"), this.fontStyle("30"));
                    s = str4.Replace(this.size30, string.Empty);
                }
                else if (str4.IndexOf(this.size41) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("41"), this.fontStyle("41"));
                    s = str4.Replace(this.size41, string.Empty);
                }
                else if (str4.IndexOf(this.size45) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("45"), this.fontStyle("45"));
                    s = str4.Replace(this.size45, string.Empty);
                }
                else if (str4.IndexOf(this.size50) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("50"), this.fontStyle("50"));
                    s = str4.Replace(this.size50, string.Empty);
                }
                else if (str4.IndexOf(this.size60) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("60"), this.fontStyle("60"));
                    s = str4.Replace(this.size60, string.Empty);
                }
                else if (str4.IndexOf(this.size61) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("61"), this.fontStyle("61"));
                    s = str4.Replace(this.size61, string.Empty);
                }
                else if (str4.IndexOf(this.size67) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("67"), this.fontStyle("67"));
                    s = str4.Replace(this.size67, string.Empty);
                }
                else if (str4.IndexOf(this.barCodeHeader) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeHeight) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeData) != -1)
                {
                    font = new System.Drawing.Font(familyName, 36f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                }
                else if (str4.IndexOf(this.barCodeData2) != -1)
                {
                    font = new System.Drawing.Font(familyName, 29.2f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData2, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                }
                else if (str4.IndexOf(this.barCodeWidth) != -1)
                {
                    s = string.Empty;
                }
                if (font.Name == familyName)
                {
                    if (barcodetype == 1)
                       // ev.Graphics.DrawString(s, font, new SolidBrush(Color.Black), x, y);
                       //pd_Print2DBarcode(ev, 0, 520);
                       pd_Print2DBarcode(ev, 52, 20);
                }
                else
                {
                    ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                }
                this._printedLines++;
                num5++;
                if (num5 > num)
                {
                    ev.HasMorePages = true;
                    this._pagesPrinted++;
                    return;
                }
                ev.HasMorePages = false;
            }
        }
        private void PrintInfantBoardingPass(object sender, PrintPageEventArgs ev)
        {
            System.Drawing.Font font = null;
            string familyName = "Free 3 of 9";
            //   font = new System.Drawing.Font("HY헤드라인M", 9, FontStyle.Regular);
            font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
            string name = font.Name;
            float num = 0f;
            float x = 15f;
            if (!ev.PageSettings.Landscape)
            {
                x = 0f;
            }
            float top = ev.MarginBounds.Top;
            top = 0f;
            float y = top;
            string s = null;
            num = ((float)ev.MarginBounds.Height) / font.GetHeight(ev.Graphics);
            string[] strArray = this.unStringMessage(this._printMessage);
            strArray = DivideAirportName(strArray);
            strArray[4] = strArray[4].ToUpper();
            int num5 = 0;
            //    strArray[22] = "사업자등록번호(Reg_No):    401_81_32460";
            if (barcodetype == 2)
                pd_Print2DBarcode(ev, 0, 500);
            for (int i = this._printedLines; i < strArray.Length; i++)
            {

                string str4 = strArray[i];
                s = str4;
                y += font.GetHeight(ev.Graphics);
                if (font.Name == familyName)
                {
                    font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
                }
                if (str4.IndexOf(this.size10) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("10"), this.fontStyle("10"));
                    s = str4.Replace(this.size10, string.Empty);
                }
                else if (str4.IndexOf(this.size12) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("12"), this.fontStyle("12"));
                    s = str4.Replace(this.size12, string.Empty);
                }
                else if (str4.IndexOf(this.size16) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("16"), this.fontStyle("16"));
                    s = str4.Replace(this.size16, string.Empty);
                }
                else if (str4.IndexOf(this.size20) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("20"), this.fontStyle("20"));
                    s = str4.Replace(this.size20, string.Empty);
                }
                else if (str4.IndexOf(this.size30) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("30"), this.fontStyle("30"));
                    s = str4.Replace(this.size30, string.Empty);
                }
                else if (str4.IndexOf(this.size41) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("41"), this.fontStyle("41"));
                    s = str4.Replace(this.size41, string.Empty);
                }
                else if (str4.IndexOf(this.size45) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("45"), this.fontStyle("45"));
                    s = str4.Replace(this.size45, string.Empty);
                }
                else if (str4.IndexOf(this.size50) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("50"), this.fontStyle("50"));
                    s = str4.Replace(this.size50, string.Empty);
                }
                else if (str4.IndexOf(this.size60) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("60"), this.fontStyle("60"));
                    s = str4.Replace(this.size60, string.Empty);
                }
                else if (str4.IndexOf(this.size61) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("61"), this.fontStyle("61"));
                    s = str4.Replace(this.size61, string.Empty);
                }
                else if (str4.IndexOf(this.size67) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("67"), this.fontStyle("67"));
                    s = str4.Replace(this.size67, string.Empty);
                }
                else if (str4.IndexOf(this.barCodeHeader) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeHeight) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeData) != -1)
                {
                    font = new System.Drawing.Font(familyName, 36f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                }
                else if (str4.IndexOf(this.barCodeData2) != -1)
                {
                    font = new System.Drawing.Font(familyName, 29.2f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData2, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                }
                else if (str4.IndexOf(this.barCodeWidth) != -1)
                {
                    s = string.Empty;
                }
                if (font.Name == familyName)
                {
                    if (barcodetype == 1)
                       // ev.Graphics.DrawString(s, font, new SolidBrush(Color.Black), x, y);
                       //pd_Print2DBarcode(ev, 0, 480);
                        pd_Print2DBarcode(ev, 52, 20);
                }
                else
                {
                    ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                }
                this._printedLines++;
                num5++;
                if (num5 > num)
                {
                    ev.HasMorePages = true;
                    this._pagesPrinted++;
                    return;
                }
                ev.HasMorePages = false;
            }
        }
        private void PrintToBoardingPass(object sender, PrintPageEventArgs ev)
        {
            System.Drawing.Font font = null;
            string familyName = "Free 3 of 9";
            font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
            string name = font.Name;
            float num = 0f;
            float x = 15f;
            if (!ev.PageSettings.Landscape)
            {
                x = 0f;
            }
            float top = ev.MarginBounds.Top;
            top = 0f;
            float y = top;
            string s = null;
            num = ((float)ev.MarginBounds.Height) / font.GetHeight(ev.Graphics);
            string[] strArray = this.unStringMessage(this._printMessage);
            int num5 = 0;

            if (internationalBoardingTerm != "00")
            {
                try
                {
                    string brdtime = strArray[12].Substring(23, 5);
                    string[] brdg = brdtime.Split(':');
                    DateTime datebrdg = new DateTime(1982, 12, 27, Convert.ToInt32(brdg[0]), Convert.ToInt32(brdg[1]), 0, 0);

                    datebrdg = datebrdg.AddMinutes(-(int.Parse(internationalBoardingTerm)));

                    string hour = String.Format("{0:00}", datebrdg.Hour);
                    string minute = String.Format("{0:00}", datebrdg.Minute);

                    //brdtime[11] = String.Format("{0:00}:{0:00}", datebrdg.Hour, datebrdg.Minute);
                    brdtime = hour + ":" + minute;
                    char[] temps = strArray[16].ToCharArray();

                    for (int p = 0; p < 5; p++)
                        temps[5 + p] = brdtime[p];

                    strArray[16] = new String(temps);
                }
                catch (Exception e)
                {

                }
            }
            for (int i = this._printedLines; i < strArray.Length; i++)
            {
                string str4 = strArray[i];
                s = str4;
                y += font.GetHeight(ev.Graphics);
                if (font.Name == familyName)
                {
                    font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
                }
                if (str4.IndexOf(this.size10) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("10"), this.fontStyle("10"));
                    s = str4.Replace(this.size10, string.Empty);
                }
                else if (str4.IndexOf(this.size12) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("12"), this.fontStyle("12"));
                    s = str4.Replace(this.size12, string.Empty);
                }
                else if (str4.IndexOf(this.size16) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("16"), this.fontStyle("16"));
                    s = str4.Replace(this.size16, string.Empty);
                }
                else if (str4.IndexOf(this.size20) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("20"), this.fontStyle("20"));
                    s = str4.Replace(this.size20, string.Empty);
                }
                else if (str4.IndexOf(this.size30) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("30"), this.fontStyle("30"));
                    s = str4.Replace(this.size30, string.Empty);
                }
                else if (str4.IndexOf(this.size41) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("41"), this.fontStyle("41"));
                    s = str4.Replace(this.size41, string.Empty);
                }
                else if (str4.IndexOf(this.size45) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("45"), this.fontStyle("45"));
                    s = str4.Replace(this.size45, string.Empty);
                }
                else if (str4.IndexOf(this.size50) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("50"), this.fontStyle("50"));
                    s = str4.Replace(this.size50, string.Empty);
                }
                else if (str4.IndexOf(this.size60) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("60"), this.fontStyle("60"));
                    s = str4.Replace(this.size60, string.Empty);
                }
                else if (str4.IndexOf(this.size61) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("61"), this.fontStyle("61"));
                    s = str4.Replace(this.size61, string.Empty);
                }
                else if (str4.IndexOf(this.size67) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("67"), this.fontStyle("67"));
                    s = str4.Replace(this.size67, string.Empty);
                }
                else if (str4.IndexOf(this.barCodeHeader) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeHeight) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeData) != -1)
                {
                    font = new System.Drawing.Font(familyName, 36f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                }
                else if (str4.IndexOf(this.barCodeData2) != -1)
                {
                    font = new System.Drawing.Font(familyName, 29.2f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData2, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                }
                else if (str4.IndexOf(this.barCodeWidth) != -1)
                {
                    s = string.Empty;
                }
                if (font.Name == familyName)
                {
                   // ev.Graphics.DrawString(s, font, new SolidBrush(Color.Black), x, y);
                    pd_Print2DBarcode(ev, 0, 520);
                }
                else
                {
                    ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                }
                this._printedLines++;
                num5++;
                if (num5 > num)
                {
                    ev.HasMorePages = true;
                    this._pagesPrinted++;
                    return;
                }
                ev.HasMorePages = false;
            }
        }

        private void PrintFromAdultBoardingPass(object sender, PrintPageEventArgs ev)
        {
            System.Drawing.Font font = null;
            string familyName = "Free 3 of 9";
            //    font = new System.Drawing.Font("HY헤드라인M", 9, FontStyle.Regular);
            font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
            System.Drawing.Font subfont = null;
            string name = font.Name;
            float num = 0f;
            float x = 15f;

            if (!ev.PageSettings.Landscape)
            {
                x = 0f;
            }
            float top = ev.MarginBounds.Top;
            top = 0f;
            float y = top;
            string s = null;
            num = ((float)ev.MarginBounds.Height) / font.GetHeight(ev.Graphics);
            string[] strArray = this.unStringMessage(this._printMessage);
            int num5 = 0;

            if (barcodetype == 2)
                pd_Print2DBarcode(ev, 0, 590);

            for (int i = this._printedLines; i < strArray.Length; i++)
            {
                string str4 = strArray[i];

                s = str4;
                y += font.GetHeight(ev.Graphics);

                if (font.Name == familyName)
                {
                    font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
                }
                if (str4.IndexOf(this.size10) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("10"), this.fontStyle("10"));
                    s = str4.Replace(this.size10, string.Empty);
                }
                else if (str4.IndexOf(this.size12) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("12"), this.fontStyle("12"));
                    s = str4.Replace(this.size12, string.Empty);
                }
                else if (str4.IndexOf(this.size16) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("16"), this.fontStyle("16"));
                    s = str4.Replace(this.size16, string.Empty);
                }
                else if (str4.IndexOf(this.size20) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("20"), this.fontStyle("20"));
                    s = str4.Replace(this.size20, string.Empty);
                }
                else if (str4.IndexOf(this.size30) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("30"), this.fontStyle("30"));
                    s = str4.Replace(this.size30, string.Empty);
                }
                else if (str4.IndexOf(this.size41) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("41"), this.fontStyle("41"));
                    s = str4.Replace(this.size41, string.Empty);
                }
                else if (str4.IndexOf(this.size45) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("45"), this.fontStyle("45"));
                    s = str4.Replace(this.size45, string.Empty);
                }
                else if (str4.IndexOf(this.size50) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("50"), this.fontStyle("50"));
                    s = str4.Replace(this.size50, string.Empty);
                }
                else if (str4.IndexOf(this.size60) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("60"), this.fontStyle("60"));
                    s = str4.Replace(this.size60, string.Empty);
                }
                else if (str4.IndexOf(this.size61) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("61"), this.fontStyle("61"));
                    s = str4.Replace(this.size61, string.Empty);
                }
                else if (str4.IndexOf(this.size67) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("67"), this.fontStyle("67"));
                    s = str4.Replace(this.size67, string.Empty);
                }
                else if (str4.IndexOf(this.barCodeHeader) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeHeight) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeData) != -1)
                {
                    font = new System.Drawing.Font(familyName, 36f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                    y -= 30;
                }
                else if (str4.IndexOf(this.barCodeData2) != -1)
                {
                    font = new System.Drawing.Font(familyName, 29.2f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData2, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                    y -= 30;
                }
                
                else if (str4.IndexOf(this.barCodeWidth) != -1)
                {
                    s = string.Empty;
                }
                if (font.Name == familyName)
                {
                    if (barcodetype == 1)
                        //ev.Graphics.DrawString(s, font, new SolidBrush(Color.Black), x, y);
                        pd_Print2DBarcode(ev, 0, 520);
                }
                else
                {
                    switch (i)
                    {
                        case 4:
                            string[] NameData = str4.Split(':');
                            s = "성명 (Name)";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            y = 133;
                            string sdfds = NameData[1].Trim();
                            ev.Graphics.DrawString(NameData[1].Trim(), font, Brushes.Black, x, y, new StringFormat());
                            y = 112;
                            break;

                        case 6:
                            break;

                        case 7:
                            break;

                        case 9:
                            y += 4;
                            s = "출발지(From)                           도착지(To)";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            y += 10;
                            break;

                        case 10:
                            s = s.Replace("청주", "CheongJu");
                            s = s.Replace("제주", "   JeJu");
                            string[] fromto = s.Split(':');
                            fromto[0] = fromto[0].Remove(fromto[0].Length - 2);
                            fromto[1] = fromto[1].Remove(fromto[1].Length - 2);
                            fromto[1] = fromto[1].Remove(0, 2);
                            //     s = fromto[0] + fromto[1];
                            ev.Graphics.DrawString(fromto[0], font, Brushes.Black, x, y, new StringFormat());
                            ev.Graphics.DrawString(fromto[1], font, Brushes.Black, 130, y, new StringFormat());
                            break;

                        case 12:
                            s = s.Replace("구간(Segment)", "출발시각(Dept)");
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 13:
                            y += 6;
                            string[] removesegment = s.Split(' ');
                            string[] depttime = strArray[10].Split('▶');
                            //  depttime[0] = depttime[0].Replace("!( 제주", "");
                            //  depttime[0] = depttime[0].Replace("!( 청주", "");
                            //  s = removesegment[1] + "   " + removesegment[4] + "   " + depttime[0].Substring(depttime[0].Length-5 , 5);

                            for (int removeairportcount = 0; removeairportcount < RemoveINTairport.Length; removeairportcount++)
                            {
                                if (depttime[0].Contains(RemoveINTairport[removeairportcount]))
                                {
                                    depttime[0] = depttime[0].Replace(RemoveINTairport[removeairportcount], "");
                                    break;
                                }
                            }
                            s = removesegment[1] + "   " + removesegment[4] + "   " + depttime[0];
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 15:
                            y += 3;
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 16:
                            y += 8;
                            string brdtime = s.Substring(13, 5);
                            string[] brdg = brdtime.Split(':');
                            DateTime datebrdg = new DateTime(1982, 12, 27, Convert.ToInt32(brdg[0]), Convert.ToInt32(brdg[1]), 0, 0);

                            datebrdg = datebrdg.AddMinutes(-(int.Parse(internationalBoardingTerm)));

                            string hour = String.Format("{0:00}", datebrdg.Hour);
                            string minute = String.Format("{0:00}", datebrdg.Minute);

                            //brdtime[11] = String.Format("{0:00}:{0:00}", datebrdg.Hour, datebrdg.Minute);
                            brdtime = hour + ":" + minute;
                            char[] temps = s.ToCharArray();

                            for (int p = 0; p < 5; p++)
                                temps[13 + p] = brdtime[p];

                            s = new String(temps);
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 19:
                            y += 3;
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 20:
                            y += 5;
                            subfont = new System.Drawing.Font(name, 10, FontStyle.Regular);
                            ev.Graphics.DrawString(s, subfont, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 21:
                            y += 5;
                            ev.Graphics.DrawString(s, subfont, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 22:
                            y += 5;
                            string[] membernumber = strArray[22].Split(':');
                            membernumber[0] = "회원번호(Customer No):";
                            s = membernumber[0] + membernumber[1];
                            ev.Graphics.DrawString(s, subfont, Brushes.Black, x, y, new StringFormat());
                            y += 18;
                            break;

                        case 24:
                            break;

                        case 30:
                            subfont = new System.Drawing.Font(name, 9, FontStyle.Regular);
                            ev.Graphics.DrawString(s, subfont, Brushes.Black, x, y, new StringFormat());
                            break;
                        case 32:
                            s = "출발시각 10분전에 탑승이 종료되며, 승무원의";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;
                        case 33:
                            s = " 탑승권 재확인 시 협조하여 주시기 바랍니다.";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            y += 10;
                            break;

                        case 34:
                            s = " Gate closes 10 minutes before departure.";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        default:
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;
                    }
                }
                this._printedLines++;
                num5++;
                if (num5 > num)
                {
                    ev.HasMorePages = true;
                    this._pagesPrinted++;
                    return;
                }
                ev.HasMorePages = false;
            }
        }
        private void PrintFromInfantBoardingPass(object sender, PrintPageEventArgs ev)
        {
            System.Drawing.Font font = null;
            string familyName = "Free 3 of 9";
            //   font = new System.Drawing.Font("HY헤드라인M", 9, FontStyle.Regular);
            font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
            System.Drawing.Font subfont = null;
            string name = font.Name;
            float num = 0f;
            float x = 15f;

            if (!ev.PageSettings.Landscape)
            {
                x = 0f;
            }
            float top = ev.MarginBounds.Top;
            top = 0f;
            float y = top;
            string s = null;
            num = ((float)ev.MarginBounds.Height) / font.GetHeight(ev.Graphics);
            string[] strArray = this.unStringMessage(this._printMessage);
            int num5 = 0;

            if (barcodetype == 2)
                pd_Print2DBarcode(ev, 0, 510);

            for (int i = this._printedLines; i < strArray.Length; i++)
            {
                string str4 = strArray[i];

                s = str4;
                y += font.GetHeight(ev.Graphics);

                if (font.Name == familyName)
                {
                    font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
                }
                if (str4.IndexOf(this.size10) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("10"), this.fontStyle("10"));
                    s = str4.Replace(this.size10, string.Empty);
                }
                else if (str4.IndexOf(this.size12) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("12"), this.fontStyle("12"));
                    s = str4.Replace(this.size12, string.Empty);
                }
                else if (str4.IndexOf(this.size16) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("16"), this.fontStyle("16"));
                    s = str4.Replace(this.size16, string.Empty);
                }
                else if (str4.IndexOf(this.size20) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("20"), this.fontStyle("20"));
                    s = str4.Replace(this.size20, string.Empty);
                }
                else if (str4.IndexOf(this.size30) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("30"), this.fontStyle("30"));
                    s = str4.Replace(this.size30, string.Empty);
                }
                else if (str4.IndexOf(this.size41) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("41"), this.fontStyle("41"));
                    s = str4.Replace(this.size41, string.Empty);
                }
                else if (str4.IndexOf(this.size45) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("45"), this.fontStyle("45"));
                    s = str4.Replace(this.size45, string.Empty);
                }
                else if (str4.IndexOf(this.size50) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("50"), this.fontStyle("50"));
                    s = str4.Replace(this.size50, string.Empty);
                }
                else if (str4.IndexOf(this.size60) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("60"), this.fontStyle("60"));
                    s = str4.Replace(this.size60, string.Empty);
                }
                else if (str4.IndexOf(this.size61) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("61"), this.fontStyle("61"));
                    s = str4.Replace(this.size61, string.Empty);
                }
                else if (str4.IndexOf(this.size67) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("67"), this.fontStyle("67"));
                    s = str4.Replace(this.size67, string.Empty);
                }
                else if (str4.IndexOf(this.barCodeHeader) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeHeight) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeData) != -1)
                {
                    font = new System.Drawing.Font(familyName, 36f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                    y -= 50;
                }
                else if (str4.IndexOf(this.barCodeData2) != -1)
                {
                    font = new System.Drawing.Font(familyName, 29.2f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData2, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                    y -= 50;
                }
                else if (str4.IndexOf(this.barCodeWidth) != -1)
                {
                    s = string.Empty;
                }
                if (font.Name == familyName)
                {
                    if (barcodetype == 1)
                      //  ev.Graphics.DrawString(s, font, new SolidBrush(Color.Black), x, y);
                        pd_Print2DBarcode(ev, 0, 480);
                }
                else
                {
                    switch (i)
                    {
                        case 4:
                            string[] NameData = str4.Split(':');
                            s = "성명 (Name)";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            y = 133;
                            string sdfds = NameData[1].Trim();
                            ev.Graphics.DrawString(NameData[1].Trim(), font, Brushes.Black, x, y, new StringFormat());
                            y = 112;
                            break;

                        case 6:
                            break;

                        case 7:
                            break;

                        case 9:
                            y += 4;
                            s = "출발지(From)                           도착지(To)";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            y += 10;
                            break;

                        case 10:
                            s = s.Replace("청주", "CheongJu");
                            s = s.Replace("제주", "   JeJu");
                            string[] fromto = s.Split(':');
                            fromto[0] = fromto[0].Remove(fromto[0].Length - 2);
                            fromto[1] = fromto[1].Remove(fromto[1].Length - 2);
                            fromto[1] = fromto[1].Remove(0, 2);
                            //     s = fromto[0] + fromto[1];
                            ev.Graphics.DrawString(fromto[0], font, Brushes.Black, x, y, new StringFormat());
                            ev.Graphics.DrawString(fromto[1], font, Brushes.Black, 130, y, new StringFormat());
                            break;
                        case 12:
                            s = s.Replace("구간(Segment)", "출발시각(Dept)");
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;
                        case 13:
                            y += 6;
                            string[] removesegment = s.Split(' ');
                            string[] depttime = strArray[10].Split('▶');
                            //  depttime[0] = depttime[0].Replace("!( 제주", "");
                            //  depttime[0] = depttime[0].Replace("!( 청주", "");
                            //  s = removesegment[1] + "   " + removesegment[4] + "   " + depttime[0].Substring(depttime[0].Length-5 , 5);

                            for (int removeairportcount = 0; removeairportcount < RemoveINTairport.Length; removeairportcount++)
                            {
                                if (depttime[0].Contains(RemoveINTairport[removeairportcount]))
                                {
                                    depttime[0] = depttime[0].Replace(RemoveINTairport[removeairportcount], "");
                                    break;
                                }
                            }
                            s = removesegment[1] + "   " + removesegment[4] + "   " + depttime[0];
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 15:
                            y += 3;
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 16:
                            y += 8;
                            string brdtime = s.Substring(13, 5);
                            string[] brdg = brdtime.Split(':');
                            DateTime datebrdg = new DateTime(1982, 12, 27, Convert.ToInt32(brdg[0]), Convert.ToInt32(brdg[1]), 0, 0);

                            datebrdg = datebrdg.AddMinutes(-(int.Parse(internationalBoardingTerm)));

                            string hour = String.Format("{0:00}", datebrdg.Hour);
                            string minute = String.Format("{0:00}", datebrdg.Minute);

                            //brdtime[11] = String.Format("{0:00}:{0:00}", datebrdg.Hour, datebrdg.Minute);
                            brdtime = hour + ":" + minute;
                            char[] temps = s.ToCharArray();

                            for (int p = 0; p < 5; p++)
                                temps[13 + p] = brdtime[p];

                            s = new String(temps);
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 19:
                            y += 3;
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 20:
                            y += 5;
                            subfont = new System.Drawing.Font(name, 10, FontStyle.Regular);
                            ev.Graphics.DrawString(s, subfont, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 22:
                            break;

                        case 29:
                            subfont = new System.Drawing.Font(name, 9, FontStyle.Regular);
                            ev.Graphics.DrawString(s, subfont, Brushes.Black, x, y, new StringFormat());
                            break;

                        case 31:
                            s = "출발시각 10분 전에 탑승이 종료되며, 승무원의";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;
                        case 33:
                            y += 10;
                            s = " Gate closes 10 minutes before departure.";
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                        default:
                            ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                            break;

                    }
                }
                this._printedLines++;
                num5++;
                if (num5 > num)
                {
                    ev.HasMorePages = true;
                    this._pagesPrinted++;
                    return;
                }
                ev.HasMorePages = false;
            }
        }
        private void Source_pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            System.Drawing.Font font = null;
            string familyName = "Free 3 of 9";
            font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
            string name = font.Name;
            float num = 0f;
            float x = 15f;
            if (!ev.PageSettings.Landscape)
            {
                x = 0f;
            }
            float top = ev.MarginBounds.Top;
            top = 0f;
            float y = top;
            string s = null;
            num = ((float)ev.MarginBounds.Height) / font.GetHeight(ev.Graphics);
            string[] strArray = this.unStringMessage(this._printMessage);
            int num5 = 0;
            for (int i = this._printedLines; i < strArray.Length; i++)
            {
                string str4 = strArray[i];
                s = str4;
                y += font.GetHeight(ev.Graphics);
                if (font.Name == familyName)
                {
                    font = new System.Drawing.Font(this._userSettings.WindowsPrintingFont, FontStyle.Regular);
                }
                if (str4.IndexOf(this.size10) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("10"), this.fontStyle("10"));
                    s = str4.Replace(this.size10, string.Empty);
                }
                else if (str4.IndexOf(this.size12) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("12"), this.fontStyle("12"));
                    s = str4.Replace(this.size12, string.Empty);
                }
                else if (str4.IndexOf(this.size16) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("16"), this.fontStyle("16"));
                    s = str4.Replace(this.size16, string.Empty);
                }
                else if (str4.IndexOf(this.size20) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("20"), this.fontStyle("20"));
                    s = str4.Replace(this.size20, string.Empty);
                }
                else if (str4.IndexOf(this.size30) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("30"), this.fontStyle("30"));
                    s = str4.Replace(this.size30, string.Empty);
                }
                else if (str4.IndexOf(this.size41) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("41"), this.fontStyle("41"));
                    s = str4.Replace(this.size41, string.Empty);
                }
                else if (str4.IndexOf(this.size45) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("45"), this.fontStyle("45"));
                    s = str4.Replace(this.size45, string.Empty);
                }
                else if (str4.IndexOf(this.size50) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("50"), this.fontStyle("50"));
                    s = str4.Replace(this.size50, string.Empty);
                }
                else if (str4.IndexOf(this.size60) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("60"), this.fontStyle("60"));
                    s = str4.Replace(this.size60, string.Empty);
                }
                else if (str4.IndexOf(this.size61) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("61"), this.fontStyle("61"));
                    s = str4.Replace(this.size61, string.Empty);
                }
                else if (str4.IndexOf(this.size67) != -1)
                {
                    font = new System.Drawing.Font(name, this.fontSize("67"), this.fontStyle("67"));
                    s = str4.Replace(this.size67, string.Empty);
                }
                else if (str4.IndexOf(this.barCodeHeader) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeHeight) != -1)
                {
                    s = string.Empty;
                }
                else if (str4.IndexOf(this.barCodeData) != -1)
                {
                    font = new System.Drawing.Font(familyName, 36f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                    
                }
                else if (str4.IndexOf(this.barCodeData2) != -1)
                {
                    font = new System.Drawing.Font(familyName, 29.2f);
                    if (font.Name != familyName)
                    {
                        MessageBox.Show("Please install bar code True Type Font " + familyName);
                    }
                    s = str4.Replace(this.barCodeData2, string.Empty).Replace("\0", string.Empty);
                    s = "*Q" + s + "*";
                    
                }
                else if (str4.IndexOf(this.barCodeWidth) != -1)
                {
                    s = string.Empty;
                }
                if (font.Name == familyName)
                {
                    if (barcodetype == 1)
                    //ev.Graphics.DrawString(s, font, new SolidBrush(Color.Black), x, y);
                    //pd_Print2DBarcode(ev, 0, 520);
                    pd_Print2DBarcode(ev, 50, 20);
                }
                else
                {
                    ev.Graphics.DrawString(s, font, Brushes.Black, x, y, new StringFormat());
                }
                this._printedLines++;
                num5++;
                if (num5 > num)
                {
                    ev.HasMorePages = true;
                    this._pagesPrinted++;
                    return;
                }
                ev.HasMorePages = false;
            }
        }

        private void playCommands(string message)
        {
            if ((message.IndexOf(this.AT_PROMPT) != -1) || (message.IndexOf(this.BEING_PROMPTED) != -1))
            {
                this.vt1.Write(" \r\n");
                ScriptCommand command = this._commands[this._scriptCnt++];
                this.vt1.Write(command.ToString() + "\r\n");
                this.vt1.Update();
                this.telnet1.Send(command.CommandText + "\r");
            }
        }

        private void printDocument()
        {
            this.stripMessage();
            string[] separator = new string[] { this._partialCut, this._fullCut };
            string[] strArray2 = this._printMessage.Split(separator, 3, StringSplitOptions.None);
            int num = 0;

            foreach (string str in strArray2)
            {
                if (str.Length > 10)
                {
                    this._printMessage = str;
                    PrintDocument document = new PrintDocument();
                    document.PrinterSettings.PrinterName = this.SelectedPrinter;
                    if (this.vt1.ScreenSize.Width == 0x84)
                    {
                        document.DefaultPageSettings.Landscape = true;
                    }
                    this._printedLines = 0;
                    this._pagesPrinted = 0;
                    document.DocumentName = "Document:" + string.Format("{0}", ++num);
                    document.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    document.Print();
                }
            }
        }
        //boardong message test
        private void Form1_FormClosing(string bordingmsg)
        {
            DialogResult result;
            result = MessageBox.Show("Please check erorr message", "scan error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
            {
                //boardingerrorMessage(message);
                Form1_FormClosing(bordingmsg + "\r\n");
                // MessageBox.Show("Invalid input1", "test");
            }
        }

        private void boardingerrorMessage(string message)
        {/*
            if (message.Contains("Barcode error.\r\nSEQUENCE-NUMBER:") || message.Contains("Invalid input. \r\nSEQUENCE-NUMBER:")
                      || message.Contains("Passenger has already been boarded.") || message.Contains("CommandBoardPax: Value was either too large or too small for an Int32.")
                      || message.Contains("UnRecognized CheckIn command.") || message.Contains("Sequence number not found.") || message.Contains("CommandBoardPax: startIndex cannot be larger than length of string.")
                      || message.Contains("Scan Error: Invalid departure city. ") || message.Contains("Scan Error: Invalid flight number."))
            {
                try
                {
                    PlaySound("C:\\Program Files\\MasterSolution\\MSI TE\\BrdErrorSound.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
                catch
                {
                }
            }
          */

         /*   string bordingmsg = "Invalid input1";
            switch(message)
            {
                case "Barcode error":
                    return 

            }
            */


            try
            {
                if (message.Contains("Barcode error"))
                {
                    PlaySound("barcbrdodeerror.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    //MessageBox.Show("Invalid input1", "test", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2,0,true);
                   Form1_FormClosing(message);
                }
                else if (message.Contains("Invalid input")) 
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("Passenger has already been boarded."))
                {
                    PlaySound("alreadyboarded.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("CommandBoardPax: Value was either too large or too small for an Int32."))
                {
                    PlaySound("bperror.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("UnRecognized Checkin command."))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("Sequence number not found."))
                
                {
                    PlaySound("sequenceerror.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    //boardingerrorMessage(message);
                    Form1_FormClosing(message);
                    
                    //  DialogResult result;

                //     result = MessageBox.Show("test111", "test", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                     
                //      if (result == DialogResult.No)
                //      {
                //          PlaySound("C:\\Program Files\\MasterSolution\\MSI TE\\sequenceerror.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    //boardingerrorMessage(message);
               //           Form1_FormClosing(message);
                    
             //   }
                // MessageBox.Show("Invalid input1", "test");
                  }
                //  else
                //  {


                //    }


                else if (message.Contains("CommandBoardPax: startIndex cannot be larger than length of string."))
                {
                    PlaySound("bperror.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("Scan Error: Invalid departure city. "))
                {
                    PlaySound("invalidcity.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("Scan Error: Invalid flight number."))
                {
                    PlaySound("invalidflight.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("Scan Error: Invalid departure date."))
                {
                    PlaySound("invaliddate.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("Unrecognized CheckIn Command."))
                {
                    PlaySound("bperror.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                    Form1_FormClosing(message);
                }
                else if (message.Contains("GeneralException"))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
                else if (message.Contains("WarningException"))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
                else if (message.Contains("CriticalException"))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
                else if (message.Contains(string.Format("{0}", '\x0011')))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
                else if (message.Contains(string.Format("{0}", '\x0012')))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
                else if (message.Contains(string.Format("{0}", '\x0013')))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
                else if (message.Contains(string.Format("{0}", '\x0014')))
                {
                    PlaySound("invalidinput.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                }
            }
            catch
            {
            }

        }
        private void getCTData(string message)
        {
            if (message.Contains("Payments"))
            {
                for (int i = 0; i < CTlengths; i++)
                    payment[i] = null;
                CTlengths = 0;
            }

            string[] messageLine = message.Split('\r');
            for (int i = 0; i < messageLine.Length; i++)
            {
                if (messageLine[i].Contains("_CT"))
                {
                    payment[CTlengths] = messageLine[i];
                    CTlengths++;
                }
            }
        }
        private void processMessage(string message, byte[] myBuffer)
        {
            try
            {
                this.checkConnected();
                if (this.telnet1.Connected)
                {
                    if (myBuffer[0] == 6)
                    {
                        this.telnet1.Send(this.vt1.Encoding.GetBytes(string.Format(CultureInfo.CurrentCulture, "Set TermType={0}{1}", new object[] { this.telnet1.TerminalType, '\r' })));
                        this.telnet1.Send(this.vt1.Encoding.GetBytes(string.Format(CultureInfo.CurrentCulture, "Set DisplayLines={0}{1}", new object[] { this.vt1.ScreenSize.Height, '\r' })));
                        this.telnet1.Send(this.vt1.Encoding.GetBytes(string.Format(CultureInfo.CurrentCulture, "Set DisplayColumns={0}{1}", new object[] { this.vt1.ScreenSize.Width, '\r' })));
                    }
                    message = this.checkException(message);
                    if (message.IndexOf(this._dce, 0) != -1)
                    {
                        message = this.checkException(message);
                    }
                    message = this.checkEscapeCodes(message);
                    message = this.checkForDisplayListBox(message, this._displayBox);
                    message = this.checkPopUps(message);
                    message = this.checkForLabels(message);


                    /////변경 부분 ////
                    boardingerrorMessage(message);
                    getCTData(message);

                    this.vt1.Write(message);
                    if (this._scriptCnt < this._commands.Count)
                    {
                        this.playCommands(message);
                    }
                    else if (this._commands.Count > 0)
                    {
                        this.doPrtMemoryNotepad();
                        this.closeUpShop();
                    }
                }
            }
            catch (Exception exception)
            {
                this.ShowError(exception.Message);
            }
        }

        private void readSettingTheOldWay()
        {
            string url = Environment.CurrentDirectory + @"\" + base.GetType().Assembly.GetName().Name + ".exe.config";
            try
            {
                XmlTextReader reader = new XmlTextReader(url);
                while (reader.Read())
                {
                    if ((reader.MoveToContent() == XmlNodeType.Element) && (reader.Name == "DisplayList"))
                    {
                        reader.MoveToNextAttribute();
                        Filters item = new Filters(reader.Value, reader.ReadElementString(), FilterType.DisplayList);
                        this._filters.Add(item);
                    }
                    else if ((reader.MoveToContent() == XmlNodeType.Element) && (reader.Name == "PopUpMessage"))
                    {
                        reader.MoveToNextAttribute();
                        Filters filters2 = new Filters(reader.Value, reader.ReadElementString(), FilterType.MessageBox);
                        this._filters.Add(filters2);
                    }
                }
            }
            catch (Exception exception)
            {
                this.ShowError("Error reading app.confg:" + exception.Message);
            }
        }

        public string Rotate90()
        {
            return string.Format("{0}{1}{2}", '\x001b', "V", "1");
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSettings settings = new FrmSettings(this.vt1, MSISKYPORTVERSION, this);
            if (settings.ShowDialog() == DialogResult.OK)
            {
                if (settings.BoardingPassPrinter != string.Empty)
                {
                    //this.BoardingPassPrinter = settings.BoardingPassPrinter;
                }
                if (settings.ReportPrinter != string.Empty)
                {
                    this.ReportPrinter = settings.ReportPrinter;
                }
                if (settings.BagTagPrinter != string.Empty)
                {
                    //this.BagTagPrinter = settings.BagTagPrinter;
                }
                this.DoResize(true);
            }
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "NavitaireTE Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void showTextFile(string fileName)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "notepad";
                process.StartInfo.Arguments = fileName;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.Start();
            }
            catch (Exception exception)
            {
                this.ShowError(string.Format(CultureInfo.CurrentCulture, "Notepad:{0}:{1}", new object[] { fileName, exception.Message }));
            }
        }

        private void startScript()
        {
            if (this.telnet1.Connected)
            {
                this.telnet1.Send(" ");
            }
            else
            {
                this.makeConnection();
            }
        }

        private void stripMessage()
        {
            this._printMessage = this._printMessage.Replace(this.InitializePrinter(), string.Empty);
        }

        private void telnet1_EndReceive(object sender, SegmentEventArgs e)
        {
            if ((this._startPosition + e.Segment.Count) >= 0x7d00)
            {
                this.processMessage(this.vt1.Encoding.GetString(this._entireBuffer), this._entireBuffer);
                this._startPosition = 0;
                this._entireBuffer = new byte[0x7d00];
            }
            for (int i = 0; i < e.Segment.Count; i++)
            {
                this._entireBuffer[this._startPosition++] = e.Segment.Buffer[i];
            }
            if (((e.Segment.Count > 0) && (e.Segment.Buffer[e.Segment.Count - 1] < 0x7f)) || (this._startPosition >= 0x7d00))
            {
                this.processMessage(this.vt1.Encoding.GetString(this._entireBuffer).TrimEnd(this._charsToTrim), this._entireBuffer);
                this._startPosition = 0;
                this._entireBuffer = new byte[0x7d00];
            }
            if (this.telnet1.Connected)
            {
                this.telnet1.BeginReceive(this._buffer);
            }
        }
        private int CheckingErrorMessage(string errormessage)
        {
            try
            {
                string message = SplitString(errormessage, "\r");
                if (message.Contains("Passenger has already been boarded.") == true)
                    return 0;
                else if (message.Contains("CommandBoardPax: Value was either too large or too small for an Int32.") == true)
                    return 0;
                else if (message.Contains("UnRecognized CheckIn command.") == true)
                    return 0;
                else if (message.Contains("Sequence number not found.") == true)
                    return 0;
                else if (message.Contains("Invalid input. ") == true)
                    return 0;
                else if (message.Contains("CommandBoardPax:") == true)
                    return 0;
            }
            catch (Exception exception)
            {
            }
            return 1;
        }
        private string SplitString(string message, string splitmessage)
        {
            int index = message.IndexOf(splitmessage);
            return message.Substring(0, index);
        }
        private string[] unStringMessage(string message)
        {
            string[] strArray = new string[0x3e8];
            try
            {
                message = message.Replace("\r\n", "~");
                message = message.Replace(ascii + "h9", barCodeHeader + "~");
                message = message.Replace(ascii + "w", barCodeWidth + "~");
                message = message.Replace(ascii + "ke", barCodeData2);

                strArray = message.Split("~".ToCharArray(), 0x3e8);
            }
            catch (Exception exception)
            {
                this.ShowError(string.Format(CultureInfo.CurrentCulture, "{0}:{1}", new object[] { "unStringMessage", exception.Message }));
            }
            return strArray;
        }

        private void vt1_DoubleClick(object sender, EventArgs e)
        {
            this.doPrtMemoryNotepad();
        }

        private void vt1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
        }

        private void vt1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                e.Handled = true;
                this.vt1.Write("+");
                this._sb.Append("+");
            }
            else if (e.KeyCode == Keys.Divide)
            {
                e.Handled = true;
                this.vt1.Write("/");
                this._sb.Append("/");
            }
            else if (e.KeyCode == Keys.Multiply)
            {
                e.Handled = true;
                this.vt1.Write("*");
                this._sb.Append("*");
            }
            else if (e.KeyCode == Keys.Subtract)
            {
                e.Handled = true;
                this.vt1.Write("-");
                this._sb.Append("-");
            }
            else if ((e.KeyValue == 0x4e) && e.Control)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F1)
            {
                this.doHelp();
                e.Handled = true;
            }
            else if (e.KeyCode != Keys.F2)
            {
                if (e.KeyCode == Keys.F3)
                {
                    this.doPrtScreenNotepad();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F4)
                {
                    this.doPrtMemoryNotepad();
                    e.Handled = true;
                }
            }
        }
        private string replaceBackSpace(char[] contents)
        {
            char[] bytes = new char[contents.Length];
            int i = 0;
            int j = 0;

            try
            {
                while (i < contents.Length)
                {
                    if (contents[i].Equals('\b'))
                    {
                        i++;
                        if (0 < j)
                        {
                            j--;
                        }
                    }
                    else
                    {
                        bytes[j] = contents[i];
                        i++;
                        j++;
                    }
                }
                string contentsString = new string(bytes, 0, i);
                return contentsString.ToLower();
            }
            catch (Exception e)
            {
            }
            return null;
        }
        private void vt1_KeyPress(object sender, DataEventArgs e)
        {
            if (this.telnet1.Connected)
            {
                this._sb.Append(this.vt1.Encoding.GetString(e.Bytes, 0, e.Bytes.Length));
                if (e.Bytes[0] == 13)
                {
                    /* 2d 바코드 리딩을 코드로 푼 것
                    if (this._sb.ToString().Length == 27 && string.Compare("ZE", this._sb.ToString().Substring(1, 2)) == 0)
                    {
                        string tempbarcodedata = "Q" + string.Format("{0:000}", int.Parse(this._sb.ToString().Substring(9, 3))) + " " + this._sb.ToString().Substring(5, 3) + this._sb.ToString().Substring(22, 3) + '\r' + '\n';
                        StringBuilder bpbarcodedata = new StringBuilder(tempbarcodedata);
                        this._sb = bpbarcodedata;
                        this.telnet1.Send(this.vt1.Encoding.GetBytes(this._sb.ToString()));
                    }*/
                    // 2d 바코드 리딩을 정규식으로 푼 것 
                    /*
                    char[] inputCommand = this._sb.ToString().ToCharArray();
                    int backSpaceCount = 0;

                    for (int i = 0; i < inputCommand.Length; i++)
                        if (inputCommand[i].Equals('\b'))
                            backSpaceCount++;

                    for (int i = 0; i < backSpaceCount; i++)
                    {
                        if(inputCommand[i].Equals('\b'))
                        {
                            inputCommand[i] = inputCommand[i + 1];
                            i--;
                        }
                    }
                    string command = inputCommand.ToString().Substring(0, inputCommand.Length - backSpaceCount);
                    */
                    string command = replaceBackSpace(this._sb.ToString().ToCharArray());

                    if (this._sb.ToString().Length == 27 && System.Text.RegularExpressions.Regex.IsMatch(this._sb.ToString(), sPattern))
                    {
                        string tempbarcodedata = "Q" + string.Format("{0:000}", int.Parse(this._sb.ToString().Substring(9, 3))) + " " + this._sb.ToString().Substring(5, 3) + this._sb.ToString().Substring(22, 3) + this._sb.ToString().Substring(16,4)+ '\r' + '\n';
                        StringBuilder bpbarcodedata = new StringBuilder(tempbarcodedata);
                        this._sb = bpbarcodedata;
                        this.telnet1.Send(this.vt1.Encoding.GetBytes(this._sb.ToString()));
                    }

                        // 제주 국제선 게이트에서 사용하는 로직, 포맷이 잘못된 바코드 처리를 위해서
/*
                    else if (11 <= this._sb.ToString().Length && this._sb.ToString().Length <= 13 && this._sb.ToString().Substring(this._sb.ToString().Length - 5, 3).Equals("CJU"))
                    {

                        if (this._sb.ToString().ToCharArray()[0].Equals('Q'))
                        {
                            string Q = this._sb.ToString(0, 1);
                            string tempbarcodedata = this._sb.ToString().Substring(1);

                            int zerocount = 13 - this._sb.ToString().Length;

                            for (int i = 0; i < zerocount; i++)
                            {
                                Q += "0";
                            }

                            tempbarcodedata = Q + tempbarcodedata;
                            StringBuilder bpbarcodedata = new StringBuilder(tempbarcodedata);
                            this._sb = bpbarcodedata;
                            this.telnet1.Send(this.vt1.Encoding.GetBytes(this._sb.ToString()));
                        }

                        
                    }
                        */
                    else if (command.Contains("f--"))
                    {
                        if (CTlengths > 0)
                        {//CT 결재 내역이 있으면
                            if (isCT(command))
                            {
                                MessageBox.Show("DCS 상에서 CT 환불 불가합니다.");
                                this.telnet1.Send(this.vt1.Encoding.GetBytes("\r\n"));
                            }
                            else
                                this.telnet1.Send(this.vt1.Encoding.GetBytes(this._sb.ToString()));
                        }
                        else
                            this.telnet1.Send(this.vt1.Encoding.GetBytes(this._sb.ToString()));
                    }
                    /* else if (command.Contains("N*") || command.Contains("n*"))
                      {
                          MessageBox.Show("DCS 상에서 PAX TYPE 변경 불가합니다.");
                          this.telnet1.Send(this.vt1.Encoding.GetBytes("\r\n"));
                      }*/
                    else
                        this.telnet1.Send(this.vt1.Encoding.GetBytes(this._sb.ToString()));

                    this._sb.Remove(0, this._sb.Length);
                }
            }
            else
            {
                this.makeConnection();
            }
        }
        private Boolean isCT(string command)
        {
            Boolean result = false;

            if (payment != null)
            {
                string[] fee = command.Split('/');
                int index = Int32.Parse(fee[0].Substring(3, fee[0].Length - 3));

                if (index > -1)
                {
                    int i = 0;

                    while (payment[i] != null)
                    {
                        string tellme = payment[i].Substring(1, 2);
                        int sourceindex = Int32.Parse(tellme);
                        if (sourceindex == index)
                        {
                            if (payment[i].Contains("_CT"))
                                result = true;
                        }
                        i++;
                    }
                }
            }
            return result;
        }
        private string[] getPaymentDetail(string[] recvdata)
        {
            string[] result = null;
            int startindex = -1;
            int endindex = -1;
            int k = 0;

            for (int i = 0; i < recvdata.Length; i++)
            {
                if (recvdata[i].Contains("Payments"))
                    startindex = i + 1;
                else if (recvdata[i].Contains("Comments"))
                {
                    endindex = i;
                    i = recvdata.Length;
                }
            }
            for (int i = startindex; i < endindex; i++)
            {
                result[k] = recvdata[i];
                k++;
            }
            return result;
        }
        internal string BagTagPrinter
        {
            get
            {
                return this._printers["BagTags"];
            }
            set
            {
                this._printers["BagTags"] = value;
            }
        }

        internal string BoardingPassPrinter
        {
            get
            {
                return this._printers["BoardingPass"];
            }
            set
            {
                this.SelectedPrinter = value;
                this._printers["BoardingPass"] = value;
            }
        }

        public ListBox DisplayBox
        {
            get
            {
                return this._displayBox;
            }
        }

        public Panel FunctionKeys
        {
            get
            {
                return this._functionKeysPanel;
            }
        }

        internal string ReportPrinter
        {
            get
            {
                return this._printers["Reports"];
            }
            set
            {
                this._printers["Reports"] = value;
            }
        }

        internal string ReportsDirectory
        {
            get
            {
                return this._reportsDirectory;
            }
            set
            {
                this._reportsDirectory = value;
            }
        }

        internal string SelectedPrinter
        {
            get
            {
                return this.vt1.SelectedPrinter;
            }
            set
            {
                this.vt1.SelectedPrinter = value;
                this.toolStripStatusLabel2.Text = string.Format("  Active Printer:{0}", this.vt1.SelectedPrinter);
            }
        }

        internal bool ShowExceptionInMsgBox
        {
            get
            {
                return this._showExceptionInMsgBox;
            }
            set
            {
                this._showExceptionInMsgBox = value;
            }
        }

        public bool ShowFunctionKeys
        {
            get
            {
                return this._showFunctionKeys;
            }
            set
            {
                this._showFunctionKeys = value;
                if (this._showFunctionKeys)
                {
                    this._functionKeysPanel.Enabled = true;
                    this._functionKeysPanel.Visible = true;
                }
                else
                {
                    this._functionKeysPanel.Enabled = false;
                    this._functionKeysPanel.Visible = false;
                }
            }
        }

        public bool ShowListBox
        {
            get
            {
                return this._showListBox;
            }
            set
            {
                this._showListBox = value;
                if (this._showListBox)
                {
                    this._displayBox.Enabled = true;
                    this._displayBox.Visible = true;
                }
                else
                {
                    this._displayBox.Enabled = false;
                    this._displayBox.Visible = false;
                }
            }
        }

        internal Settings UserSettings
        {
            get
            {
                return this._userSettings;
            }
        }

        public bool WindowsPrinting
        {
            get
            {
                return this._windowsPrinting;
            }
            set
            {
                this._windowsPrinting = value;
            }
        }

        private String[] DivideAirportName(String[] arr)
        { 
        
            String Airportnames = "";
            String Departure = "";
            String DepartureE = "";
            String Arrival = "";
            String ArrivalE = "";
            String addEmptyString = "";

            Airportnames = Regex.Replace(arr[15], @"[^a-zA-Z0-9가-힣▶: ]", "", RegexOptions.Singleline).Trim();

            Departure = Airportnames.Split('▶')[0];
            DepartureE = Departure.Split(' ')[0];
            Arrival = Airportnames.Split('▶')[1];
            ArrivalE = Arrival.Split(' ')[0];

            arr[15] = arr[15].Replace(Departure, Departure.Split(' ')[1]);
            arr[15] = arr[15].Replace(Arrival, Arrival.Split(' ')[1]);
            arr[15] = arr[15].Replace("▶", "   ▶  ");

            switch(DepartureE)
            {
                case "Gimpo" : addEmptyString = "             ";
                                break;
                case "Jeju": addEmptyString = "                ";
                                break;
                case "Cheongju": addEmptyString = "         ";
                                break;
                case "Pusan": addEmptyString = "             ";
                                break;
                case "Gunsan": addEmptyString = "           ";
                                break;
                default: addEmptyString = "             ";
                                break;

            }

            //Encoding.Default.GetByteCount(ArrivalE);
            arr[16] = " " + DepartureE + addEmptyString + ArrivalE;
            

            return arr;
        }
    }
}

