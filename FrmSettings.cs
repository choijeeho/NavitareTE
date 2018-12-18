namespace NavitaireTE
{
    using Dart.PowerTCP.Emulation;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FrmSettings : Form
    {
        private string _bagTagPrinter = string.Empty;
        private string _boardingPassPrinter = string.Empty;
        private Button _btnReportsDirectory;
        private Button _buttonBagTag;
        private Button _buttonBoardingPass;
        private Button _buttonReports;
        private CheckBox _checkBoxFunctionKeys;
        private CheckBox _checkBoxLeftColumn;
        private FrmMain _frmMain;
        private Label _labelBagTagPrinter;
        private Label _labelBoardingPassPrinter;
        private Label _labelReportPrinter;
        private string _reportPrinter = string.Empty;
        private string _reportsDirectory = string.Empty;
        private TextBox _textBoxReportsDirectory;
        private string appName = "";
        private bool bold;
        public ComboBox cboCharSet;
        public Button cmdBackColor;
        public Button cmdBoldColor;
        public Button cmdCancel;
        public Button cmdFont;
        public Button cmdForeColor;
        public Button cmdOK;
        private ColorDialog colorDialog;
        private Container components;
        private GroupBox exceptionHBox;
        private string fontname;
        private float fontsize;
        private FontType fonttype;
        private GroupBox groupBox1;
        public GroupBox groupCharSet;
        public GroupBox groupColors;
        public GroupBox groupFont;
        private GroupBox groupIntensity;
        private bool italic;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblBlinkIntensity;
        public Label lblBoldColor;
        private Label lblFont;
        public Label lblForeColor;
        private Label lblNormalIntensity;
        private RadioButton myDontShowExceptionBox;
        private NumericUpDown myScreenBufferRows;
        private RadioButton myShowExceptionBox;
        private RadioButton myWindowColumns80;
        private RadioButton myWindowsColumns132;
        private NumericUpDown myWindowsRows;
        private Panel panelBackColor;
        private PrintDialog printDialog1;
        private NumericUpDown updownBlink;
        private NumericUpDown updownNormal;
        private Vt vt;

        public FrmSettings(Vt vt, string appName, FrmMain myFmrMain)
        {
            this.InitializeComponent();
            this.vt = vt;
            this.appName = appName;
            this._frmMain = myFmrMain;
        }

        private void _btnReportsDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._textBoxReportsDirectory.Text = dialog.SelectedPath;
                this.ReportsDirectory = dialog.SelectedPath;
            }
        }

        private void _buttonBagTag_Click(object sender, EventArgs e)
        {
            FrmPrinters printers = new FrmPrinters();
            printers.ShowDialog();
            this._labelBagTagPrinter.Text = printers.SelectedPrinter;
            this._bagTagPrinter = printers.SelectedPrinter;
            base.Update();
        }

        private void _buttonBoardingPass_Click(object sender, EventArgs e)
        {
            FrmPrinters printers = new FrmPrinters();
            if (printers.ShowDialog() == DialogResult.OK)
            {
                this._labelBoardingPassPrinter.Text = printers.SelectedPrinter;
                this._boardingPassPrinter = printers.SelectedPrinter;
                base.Update();
            }
        }

        private void _buttonReports_Click(object sender, EventArgs e)
        {
            FrmPrinters printers = new FrmPrinters();
            if (printers.ShowDialog() == DialogResult.OK)
            {
                this._labelReportPrinter.Text = printers.SelectedPrinter;
                this._reportPrinter = printers.SelectedPrinter;
                base.Update();
            }
        }

        private void _textBoxReportsDirectory_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this._textBoxReportsDirectory.Text))
            {
                this._textBoxReportsDirectory.Text = string.Empty;
            }
            this.ReportsDirectory = this._textBoxReportsDirectory.Text;
        }

        private void AddCharSets()
        {
            this.cboCharSet.Items.Add(NationalCharSet.Ascii);
            this.cboCharSet.Items.Add(NationalCharSet.British);
            this.cboCharSet.Items.Add(NationalCharSet.Dutch);
            this.cboCharSet.Items.Add(NationalCharSet.Finnish);
            this.cboCharSet.Items.Add(NationalCharSet.French);
            this.cboCharSet.Items.Add(NationalCharSet.FrenchCanadian);
            this.cboCharSet.Items.Add(NationalCharSet.German);
            this.cboCharSet.Items.Add(NationalCharSet.Italian);
            this.cboCharSet.Items.Add(NationalCharSet.NorwegianDanish);
            this.cboCharSet.Items.Add(NationalCharSet.Portuguese);
            this.cboCharSet.Items.Add(NationalCharSet.Spanish);
            this.cboCharSet.Items.Add(NationalCharSet.Swedish);
            this.cboCharSet.Items.Add(NationalCharSet.Swiss);
        }

        private void checkShowListBox_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show("If you check this then you will have another display list \r on the left hand side of your main window.");
        }

        private void cmdBackColor_Click(object sender, EventArgs e)
        {
            this.colorDialog.Color = this.panelBackColor.BackColor;
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.panelBackColor.BackColor = this.colorDialog.Color;
                this.lblFont.BackColor = this.colorDialog.Color;
            }
        }

        private void cmdBoldColor_Click(object sender, EventArgs e)
        {
            this.colorDialog.Color = this.lblBoldColor.ForeColor;
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.lblBoldColor.ForeColor = this.colorDialog.Color;
            }
        }

        private void cmdFont_Click(object sender, EventArgs e)
        {
            Dart.PowerTCP.Emulation.FontDialog dialog = new Dart.PowerTCP.Emulation.FontDialog();
            dialog.Font = new Dart.PowerTCP.Emulation.Font(this.fontname, this.fontsize, this.GetFontStyle(this.bold, this.italic));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.fontname = dialog.Font.Name;
                this.fontsize = dialog.Font.Size;
                this.bold = dialog.Font.Bold;
                this.italic = dialog.Font.Italic;
                this.fonttype = dialog.Font.Type;
                this.ShowFont();
            }
        }

        private void cmdForeColor_Click(object sender, EventArgs e)
        {
            this.colorDialog.Color = this.lblForeColor.ForeColor;
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.lblForeColor.ForeColor = this.colorDialog.Color;
                this.lblFont.ForeColor = this.colorDialog.Color;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.vt.NationalCharSet = (NationalCharSet) this.cboCharSet.SelectedItem;
            this.vt.FontEx = new Dart.PowerTCP.Emulation.Font(this.fontname, this.fontsize, this.GetFontStyle(this.bold, this.italic));
            this.vt.ForeColor = this.lblForeColor.ForeColor;
            this.vt.BackColor = this.panelBackColor.BackColor;
            this.vt.BoldColor = this.lblBoldColor.ForeColor;
            this.vt.NormalIntensity = (int) this.updownNormal.Value;
            this.vt.BlinkIntensity = (int) this.updownBlink.Value;
            int width = this.myWindowsColumns132.Checked ? 0x84 : 80;
            this.vt.ScreenSize = new Size(width, (int) this.myWindowsRows.Value);
            this.vt.BufferRows = (int) this.myScreenBufferRows.Value;
            this._frmMain.ShowListBox = this._checkBoxLeftColumn.Checked;
            this._frmMain.ShowFunctionKeys = this._checkBoxFunctionKeys.Checked;
            this._frmMain.ShowExceptionInMsgBox = this.myShowExceptionBox.Checked;
            this.ReportsDirectory = this._textBoxReportsDirectory.Text;
            this._frmMain.ReportsDirectory = this.ReportsDirectory;
            this.SaveSettings();
        }

        private void disableSettings()
        {
            this.myDontShowExceptionBox.Enabled = false;
            this.myShowExceptionBox.Enabled = false;
            this.myWindowColumns80.Enabled = false;
            this.myWindowsColumns132.Enabled = false;
            this.myWindowsRows.Enabled = false;
            this.myScreenBufferRows.Enabled = false;
            this.cboCharSet.Enabled = false;
            this.cmdBackColor.Enabled = false;
            this.cmdBoldColor.Enabled = false;
            this.cmdFont.Enabled = false;
            this.cmdForeColor.Enabled = false;
            this.updownBlink.Enabled = false;
            this.updownNormal.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            this.fontname = this.vt.FontEx.Name;
            this.fontsize = this.vt.FontEx.Size;
            this.bold = this.vt.FontEx.Bold;
            this.italic = this.vt.FontEx.Italic;
            this.fonttype = this.vt.FontEx.Type;
            this.myWindowColumns80.Checked = true;
            if (this.vt.ScreenSize.Width == 0x84)
            {
                this.myWindowsColumns132.Checked = true;
            }
            this.myWindowsRows.Value = this.vt.ScreenSize.Height;
            this.myScreenBufferRows.Value = this.vt.BufferRows;
            if (this._frmMain.DisplayBox.Visible)
            {
                this._checkBoxLeftColumn.Checked = true;
            }
            else
            {
                this._checkBoxLeftColumn.Checked = false;
            }
            if (this._frmMain.FunctionKeys.Visible)
            {
                this._checkBoxFunctionKeys.Checked = true;
            }
            else
            {
                this._checkBoxFunctionKeys.Checked = false;
            }
            if (this._frmMain.ShowExceptionInMsgBox)
            {
                this.myShowExceptionBox.Checked = true;
            }
            else
            {
                this.myDontShowExceptionBox.Checked = true;
            }
            this._labelBoardingPassPrinter.Text = this._frmMain.BoardingPassPrinter;
            this._boardingPassPrinter = this._frmMain.BoardingPassPrinter;
            this._labelReportPrinter.Text = this._frmMain.ReportPrinter;
            this._reportPrinter = this._frmMain.ReportPrinter;
            this._labelBagTagPrinter.Text = this._frmMain.BagTagPrinter;
            this._bagTagPrinter = this._frmMain.BagTagPrinter;
            this._textBoxReportsDirectory.Text = this._frmMain.ReportsDirectory;
            this.ReportsDirectory = this._frmMain.ReportsDirectory;
            this.AddCharSets();
            this.UpdateInterface();
            if (this._frmMain.UserSettings.AllowOnlyPrinterChanges)
            {
                this.disableSettings();
            }
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

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmSettings));
            this.groupColors = new GroupBox();
            this.panelBackColor = new Panel();
            this.lblBoldColor = new Label();
            this.lblForeColor = new Label();
            this.cmdBoldColor = new Button();
            this.cmdBackColor = new Button();
            this.cmdForeColor = new Button();
            this.groupFont = new GroupBox();
            this.lblFont = new Label();
            this.cmdFont = new Button();
            this.groupCharSet = new GroupBox();
            this.cboCharSet = new ComboBox();
            this.cmdCancel = new Button();
            this.cmdOK = new Button();
            this.colorDialog = new ColorDialog();
            this.groupIntensity = new GroupBox();
            this.updownBlink = new NumericUpDown();
            this.updownNormal = new NumericUpDown();
            this.lblBlinkIntensity = new Label();
            this.lblNormalIntensity = new Label();
            this.groupBox1 = new GroupBox();
            this.myScreenBufferRows = new NumericUpDown();
            this.label3 = new Label();
            this.myWindowsColumns132 = new RadioButton();
            this.myWindowColumns80 = new RadioButton();
            this.myWindowsRows = new NumericUpDown();
            this.label1 = new Label();
            this.label2 = new Label();
            this.exceptionHBox = new GroupBox();
            this.myDontShowExceptionBox = new RadioButton();
            this.myShowExceptionBox = new RadioButton();
            this.printDialog1 = new PrintDialog();
            this._labelBoardingPassPrinter = new Label();
            this._checkBoxFunctionKeys = new CheckBox();
            this._labelReportPrinter = new Label();
            this._labelBagTagPrinter = new Label();
            this._buttonBoardingPass = new Button();
            this._buttonReports = new Button();
            this._buttonBagTag = new Button();
            this._btnReportsDirectory = new Button();
            this._textBoxReportsDirectory = new TextBox();
            this._checkBoxLeftColumn = new CheckBox();
            this.groupColors.SuspendLayout();
            this.panelBackColor.SuspendLayout();
            this.groupFont.SuspendLayout();
            this.groupCharSet.SuspendLayout();
            this.groupIntensity.SuspendLayout();
            this.updownBlink.BeginInit();
            this.updownNormal.BeginInit();
            this.groupBox1.SuspendLayout();
            this.myScreenBufferRows.BeginInit();
            this.myWindowsRows.BeginInit();
            this.exceptionHBox.SuspendLayout();
            base.SuspendLayout();
            this.groupColors.BackColor = SystemColors.Control;
            this.groupColors.Controls.Add(this.panelBackColor);
            this.groupColors.Controls.Add(this.cmdBoldColor);
            this.groupColors.Controls.Add(this.cmdBackColor);
            this.groupColors.Controls.Add(this.cmdForeColor);
            this.groupColors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupColors.ForeColor = SystemColors.ControlText;
            this.groupColors.Location = new Point(4, 0x100);
            this.groupColors.Name = "groupColors";
            this.groupColors.RightToLeft = RightToLeft.No;
            this.groupColors.Size = new Size(0xe0, 0x68);
            this.groupColors.TabIndex = 0;
            this.groupColors.TabStop = false;
            this.groupColors.Text = "Colors";
            this.panelBackColor.BackColor = Color.White;
            this.panelBackColor.BorderStyle = BorderStyle.FixedSingle;
            this.panelBackColor.Controls.Add(this.lblBoldColor);
            this.panelBackColor.Controls.Add(this.lblForeColor);
            this.panelBackColor.Location = new Point(8, 0x10);
            this.panelBackColor.Name = "panelBackColor";
            this.panelBackColor.Size = new Size(0x60, 80);
            this.panelBackColor.TabIndex = 9;
            this.lblBoldColor.AutoSize = true;
            this.lblBoldColor.BackColor = Color.Transparent;
            this.lblBoldColor.Cursor = Cursors.Default;
            this.lblBoldColor.ForeColor = SystemColors.ControlText;
            this.lblBoldColor.Location = new Point(0x13, 40);
            this.lblBoldColor.Name = "lblBoldColor";
            this.lblBoldColor.RightToLeft = RightToLeft.No;
            this.lblBoldColor.Size = new Size(0x37, 13);
            this.lblBoldColor.TabIndex = 11;
            this.lblBoldColor.Text = "Bold Color";
            this.lblForeColor.AutoSize = true;
            this.lblForeColor.BackColor = Color.Transparent;
            this.lblForeColor.Cursor = Cursors.Default;
            this.lblForeColor.ForeColor = SystemColors.ControlText;
            this.lblForeColor.Location = new Point(0x13, 0x10);
            this.lblForeColor.Name = "lblForeColor";
            this.lblForeColor.RightToLeft = RightToLeft.No;
            this.lblForeColor.Size = new Size(0x37, 13);
            this.lblForeColor.TabIndex = 9;
            this.lblForeColor.Text = "Text Color";
            this.cmdBoldColor.BackColor = SystemColors.Control;
            this.cmdBoldColor.Cursor = Cursors.Default;
            this.cmdBoldColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdBoldColor.Location = new Point(0x70, 0x48);
            this.cmdBoldColor.Name = "cmdBoldColor";
            this.cmdBoldColor.RightToLeft = RightToLeft.No;
            this.cmdBoldColor.Size = new Size(0x69, 0x19);
            this.cmdBoldColor.TabIndex = 2;
            this.cmdBoldColor.Text = "Bold Color";
            this.cmdBoldColor.UseVisualStyleBackColor = false;
            this.cmdBoldColor.Click += new EventHandler(this.cmdBoldColor_Click);
            this.cmdBackColor.BackColor = SystemColors.Control;
            this.cmdBackColor.Cursor = Cursors.Default;
            this.cmdBackColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdBackColor.Location = new Point(0x70, 0x2c);
            this.cmdBackColor.Name = "cmdBackColor";
            this.cmdBackColor.RightToLeft = RightToLeft.No;
            this.cmdBackColor.Size = new Size(0x69, 0x19);
            this.cmdBackColor.TabIndex = 1;
            this.cmdBackColor.Text = "Background Color";
            this.cmdBackColor.UseVisualStyleBackColor = false;
            this.cmdBackColor.Click += new EventHandler(this.cmdBackColor_Click);
            this.cmdForeColor.BackColor = SystemColors.Control;
            this.cmdForeColor.Cursor = Cursors.Default;
            this.cmdForeColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdForeColor.Location = new Point(0x70, 0x10);
            this.cmdForeColor.Name = "cmdForeColor";
            this.cmdForeColor.RightToLeft = RightToLeft.No;
            this.cmdForeColor.Size = new Size(0x69, 0x19);
            this.cmdForeColor.TabIndex = 0;
            this.cmdForeColor.Text = "Text Color";
            this.cmdForeColor.UseVisualStyleBackColor = false;
            this.cmdForeColor.Click += new EventHandler(this.cmdForeColor_Click);
            this.groupFont.Anchor = AnchorStyles.Top;
            this.groupFont.BackColor = SystemColors.Control;
            this.groupFont.Controls.Add(this.lblFont);
            this.groupFont.Controls.Add(this.cmdFont);
            this.groupFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupFont.ForeColor = SystemColors.ControlText;
            this.groupFont.Location = new Point(6, 0x170);
            this.groupFont.Name = "groupFont";
            this.groupFont.RightToLeft = RightToLeft.No;
            this.groupFont.Size = new Size(0x1ab, 0x74);
            this.groupFont.TabIndex = 3;
            this.groupFont.TabStop = false;
            this.groupFont.Text = "Font";
            this.lblFont.BorderStyle = BorderStyle.FixedSingle;
            this.lblFont.Location = new Point(8, 20);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new Size(0x198, 0x38);
            this.lblFont.TabIndex = 11;
            this.lblFont.Text = "Size Fontname";
            this.lblFont.TextAlign = ContentAlignment.MiddleCenter;
            this.cmdFont.BackColor = SystemColors.Control;
            this.cmdFont.Cursor = Cursors.Default;
            this.cmdFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdFont.Location = new Point(8, 0x54);
            this.cmdFont.Name = "cmdFont";
            this.cmdFont.RightToLeft = RightToLeft.No;
            this.cmdFont.Size = new Size(0x198, 0x19);
            this.cmdFont.TabIndex = 0;
            this.cmdFont.Text = "Display Font...";
            this.cmdFont.UseVisualStyleBackColor = false;
            this.cmdFont.Click += new EventHandler(this.cmdFont_Click);
            this.groupCharSet.Anchor = AnchorStyles.Top;
            this.groupCharSet.BackColor = SystemColors.Control;
            this.groupCharSet.Controls.Add(this.cboCharSet);
            this.groupCharSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupCharSet.ForeColor = SystemColors.ControlText;
            this.groupCharSet.Location = new Point(240, 0x100);
            this.groupCharSet.Name = "groupCharSet";
            this.groupCharSet.RightToLeft = RightToLeft.No;
            this.groupCharSet.Size = new Size(0xc1, 0x2c);
            this.groupCharSet.TabIndex = 1;
            this.groupCharSet.TabStop = false;
            this.groupCharSet.Text = "National Character Set";
            this.cboCharSet.BackColor = SystemColors.Window;
            this.cboCharSet.Cursor = Cursors.Default;
            this.cboCharSet.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboCharSet.DropDownWidth = 0xd1;
            this.cboCharSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cboCharSet.ForeColor = SystemColors.WindowText;
            this.cboCharSet.Location = new Point(8, 0x10);
            this.cboCharSet.Name = "cboCharSet";
            this.cboCharSet.RightToLeft = RightToLeft.No;
            this.cboCharSet.Size = new Size(0xac, 0x15);
            this.cboCharSet.TabIndex = 0;
            this.cmdCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.cmdCancel.BackColor = SystemColors.Control;
            this.cmdCancel.Cursor = Cursors.Default;
            this.cmdCancel.DialogResult = DialogResult.Cancel;
            this.cmdCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdCancel.Location = new Point(0x161, 0x1f5);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.RightToLeft = RightToLeft.No;
            this.cmdCancel.Size = new Size(80, 0x18);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.cmdOK.BackColor = SystemColors.Control;
            this.cmdOK.Cursor = Cursors.Default;
            this.cmdOK.DialogResult = DialogResult.OK;
            this.cmdOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdOK.Location = new Point(0x10a, 0x1f5);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.RightToLeft = RightToLeft.No;
            this.cmdOK.Size = new Size(80, 0x18);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
            this.groupIntensity.Controls.Add(this.updownBlink);
            this.groupIntensity.Controls.Add(this.updownNormal);
            this.groupIntensity.Controls.Add(this.lblBlinkIntensity);
            this.groupIntensity.Controls.Add(this.lblNormalIntensity);
            this.groupIntensity.Location = new Point(240, 0x134);
            this.groupIntensity.Name = "groupIntensity";
            this.groupIntensity.Size = new Size(0xc0, 0x30);
            this.groupIntensity.TabIndex = 2;
            this.groupIntensity.TabStop = false;
            this.groupIntensity.Text = "Intensity";
            this.updownBlink.Location = new Point(140, 20);
            this.updownBlink.Name = "updownBlink";
            this.updownBlink.Size = new Size(40, 20);
            this.updownBlink.TabIndex = 1;
            this.updownBlink.TextAlign = HorizontalAlignment.Right;
            this.updownNormal.Location = new Point(50, 20);
            this.updownNormal.Name = "updownNormal";
            this.updownNormal.Size = new Size(40, 20);
            this.updownNormal.TabIndex = 0;
            this.updownNormal.TextAlign = HorizontalAlignment.Right;
            int[] bits = new int[4];
            bits[0] = 100;
            this.updownNormal.Value = new decimal(bits);
            this.lblBlinkIntensity.AutoSize = true;
            this.lblBlinkIntensity.Location = new Point(0x6c, 0x16);
            this.lblBlinkIntensity.Name = "lblBlinkIntensity";
            this.lblBlinkIntensity.Size = new Size(0x21, 13);
            this.lblBlinkIntensity.TabIndex = 2;
            this.lblBlinkIntensity.Text = "Blink:";
            this.lblNormalIntensity.AutoSize = true;
            this.lblNormalIntensity.Location = new Point(8, 0x16);
            this.lblNormalIntensity.Name = "lblNormalIntensity";
            this.lblNormalIntensity.Size = new Size(0x2b, 13);
            this.lblNormalIntensity.TabIndex = 1;
            this.lblNormalIntensity.Text = "Normal:";
            this.groupBox1.Controls.Add(this.myScreenBufferRows);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.myWindowsColumns132);
            this.groupBox1.Controls.Add(this.myWindowColumns80);
            this.groupBox1.Controls.Add(this.myWindowsRows);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(6, 0xca);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1aa, 0x30);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Window Size and Buffer";
            this.myScreenBufferRows.Location = new Point(0x16a, 0x18);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x270f;
            this.myScreenBufferRows.Maximum = new decimal(numArray2);
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.myScreenBufferRows.Minimum = new decimal(numArray3);
            this.myScreenBufferRows.Name = "myScreenBufferRows";
            this.myScreenBufferRows.Size = new Size(0x34, 20);
            this.myScreenBufferRows.TabIndex = 5;
            this.myScreenBufferRows.TextAlign = HorizontalAlignment.Right;
            int[] numArray4 = new int[4];
            numArray4[0] = 0x48;
            this.myScreenBufferRows.Value = new decimal(numArray4);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xfb, 0x1a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x69, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Screen Buffer Rows:";
            this.myWindowsColumns132.AutoSize = true;
            this.myWindowsColumns132.Location = new Point(200, 0x18);
            this.myWindowsColumns132.Name = "myWindowsColumns132";
            this.myWindowsColumns132.Size = new Size(0x2b, 0x11);
            this.myWindowsColumns132.TabIndex = 4;
            this.myWindowsColumns132.Text = "132";
            this.myWindowsColumns132.UseVisualStyleBackColor = true;
            this.myWindowColumns80.AutoSize = true;
            this.myWindowColumns80.Checked = true;
            this.myWindowColumns80.Location = new Point(0x9d, 0x18);
            this.myWindowColumns80.Name = "myWindowColumns80";
            this.myWindowColumns80.Size = new Size(0x25, 0x11);
            this.myWindowColumns80.TabIndex = 3;
            this.myWindowColumns80.TabStop = true;
            this.myWindowColumns80.Text = "80";
            this.myWindowColumns80.UseVisualStyleBackColor = true;
            this.myWindowsRows.Location = new Point(0x2b, 0x18);
            this.myWindowsRows.Name = "myWindowsRows";
            this.myWindowsRows.Size = new Size(40, 20);
            this.myWindowsRows.TabIndex = 0;
            this.myWindowsRows.TextAlign = HorizontalAlignment.Right;
            int[] numArray5 = new int[4];
            numArray5[0] = 0x1a;
            this.myWindowsRows.Value = new decimal(numArray5);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x63, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Columns:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 0x1a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x25, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rows:";
            this.exceptionHBox.Controls.Add(this.myDontShowExceptionBox);
            this.exceptionHBox.Controls.Add(this.myShowExceptionBox);
            this.exceptionHBox.Location = new Point(6, 0x9a);
            this.exceptionHBox.Name = "exceptionHBox";
            this.exceptionHBox.Size = new Size(0x1aa, 0x2a);
            this.exceptionHBox.TabIndex = 8;
            this.exceptionHBox.TabStop = false;
            this.exceptionHBox.Text = "Exception Handling";
            this.myDontShowExceptionBox.AutoSize = true;
            this.myDontShowExceptionBox.Location = new Point(0xe2, 0x13);
            this.myDontShowExceptionBox.Name = "myDontShowExceptionBox";
            this.myDontShowExceptionBox.Size = new Size(0x89, 0x11);
            this.myDontShowExceptionBox.TabIndex = 2;
            this.myDontShowExceptionBox.Text = "Show Exception in Text";
            this.myDontShowExceptionBox.UseVisualStyleBackColor = true;
            this.myDontShowExceptionBox.HelpRequested += new HelpEventHandler(this.radioMsgBox2_HelpRequested);
            this.myShowExceptionBox.AutoSize = true;
            this.myShowExceptionBox.Checked = true;
            this.myShowExceptionBox.Location = new Point(9, 0x13);
            this.myShowExceptionBox.Name = "myShowExceptionBox";
            this.myShowExceptionBox.Size = new Size(0x9d, 0x11);
            this.myShowExceptionBox.TabIndex = 1;
            this.myShowExceptionBox.TabStop = true;
            this.myShowExceptionBox.Text = "Show Exception in Msg Box";
            this.myShowExceptionBox.UseVisualStyleBackColor = true;
            this.myShowExceptionBox.HelpRequested += new HelpEventHandler(this.radioMsgBox1_HelpRequested);
            this.printDialog1.UseEXDialog = true;
            this._labelBoardingPassPrinter.AutoSize = true;
            this._labelBoardingPassPrinter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._labelBoardingPassPrinter.Location = new Point(0x95, 0x11);
            this._labelBoardingPassPrinter.Name = "_labelBoardingPassPrinter";
            this._labelBoardingPassPrinter.Size = new Size(0x2e, 0x10);
            this._labelBoardingPassPrinter.TabIndex = 11;
            this._labelBoardingPassPrinter.Text = "Printer";
            this._checkBoxFunctionKeys.CheckAlign = ContentAlignment.MiddleRight;
            this._checkBoxFunctionKeys.Location = new Point(14, 0x81);
            this._checkBoxFunctionKeys.Name = "_checkBoxFunctionKeys";
            this._checkBoxFunctionKeys.Size = new Size(0xa8, 0x11);
            this._checkBoxFunctionKeys.TabIndex = 13;
            this._checkBoxFunctionKeys.Text = "Show User Function Keys";
            this._checkBoxFunctionKeys.UseVisualStyleBackColor = true;
            this._labelReportPrinter.AutoSize = true;
            this._labelReportPrinter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._labelReportPrinter.Location = new Point(0x95, 0x2c);
            this._labelReportPrinter.Name = "_labelReportPrinter";
            this._labelReportPrinter.Size = new Size(0x2e, 0x10);
            this._labelReportPrinter.TabIndex = 14;
            this._labelReportPrinter.Text = "Printer";
            this._labelBagTagPrinter.AutoSize = true;
            this._labelBagTagPrinter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._labelBagTagPrinter.Location = new Point(0x95, 0x49);
            this._labelBagTagPrinter.Name = "_labelBagTagPrinter";
            this._labelBagTagPrinter.Size = new Size(0x2e, 0x10);
            this._labelBagTagPrinter.TabIndex = 15;
            this._labelBagTagPrinter.Text = "Printer";
            this._buttonBoardingPass.Location = new Point(14, 15);
            this._buttonBoardingPass.Name = "_buttonBoardingPass";
            this._buttonBoardingPass.Size = new Size(0x81, 0x15);
            this._buttonBoardingPass.TabIndex = 0x10;
            this._buttonBoardingPass.Text = "Boarding Pass Printer";
            this._buttonBoardingPass.UseVisualStyleBackColor = true;
            this._buttonBoardingPass.Click += new EventHandler(this._buttonBoardingPass_Click);
            this._buttonReports.Location = new Point(14, 0x2a);
            this._buttonReports.Name = "_buttonReports";
            this._buttonReports.Size = new Size(0x81, 0x15);
            this._buttonReports.TabIndex = 0x11;
            this._buttonReports.Text = "Reports Printer";
            this._buttonReports.UseVisualStyleBackColor = true;
            this._buttonReports.Click += new EventHandler(this._buttonReports_Click);
            this._buttonBagTag.Location = new Point(14, 0x45);
            this._buttonBagTag.Name = "_buttonBagTag";
            this._buttonBagTag.Size = new Size(0x81, 0x15);
            this._buttonBagTag.TabIndex = 0x12;
            this._buttonBagTag.Text = "Bag Tag Printer";
            this._buttonBagTag.UseVisualStyleBackColor = true;
            this._buttonBagTag.Click += new EventHandler(this._buttonBagTag_Click);
            this._btnReportsDirectory.Location = new Point(14, 0x60);
            this._btnReportsDirectory.Name = "_btnReportsDirectory";
            this._btnReportsDirectory.Size = new Size(0x81, 0x15);
            this._btnReportsDirectory.TabIndex = 20;
            this._btnReportsDirectory.Text = "Reports Directory";
            this._btnReportsDirectory.UseVisualStyleBackColor = true;
            this._btnReportsDirectory.Click += new EventHandler(this._btnReportsDirectory_Click);
            this._textBoxReportsDirectory.Location = new Point(0x95, 0x60);
            this._textBoxReportsDirectory.Name = "_textBoxReportsDirectory";
            this._textBoxReportsDirectory.Size = new Size(0x11b, 20);
            this._textBoxReportsDirectory.TabIndex = 0x15;
            this._textBoxReportsDirectory.Leave += new EventHandler(this._textBoxReportsDirectory_Leave);
            this._checkBoxLeftColumn.CheckAlign = ContentAlignment.MiddleRight;
            this._checkBoxLeftColumn.Location = new Point(240, 0x83);
            this._checkBoxLeftColumn.Name = "_checkBoxLeftColumn";
            this._checkBoxLeftColumn.Size = new Size(0xc0, 0x11);
            this._checkBoxLeftColumn.TabIndex = 0x16;
            this._checkBoxLeftColumn.Text = "Show Left Column Display";
            this._checkBoxLeftColumn.UseVisualStyleBackColor = true;
            base.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cmdCancel;
            base.ClientSize = new Size(0x1bc, 0x214);
            base.Controls.Add(this._checkBoxLeftColumn);
            base.Controls.Add(this._textBoxReportsDirectory);
            base.Controls.Add(this._btnReportsDirectory);
            base.Controls.Add(this._buttonBagTag);
            base.Controls.Add(this._buttonReports);
            base.Controls.Add(this._buttonBoardingPass);
            base.Controls.Add(this._labelBagTagPrinter);
            base.Controls.Add(this._labelReportPrinter);
            base.Controls.Add(this._checkBoxFunctionKeys);
            base.Controls.Add(this._labelBoardingPassPrinter);
            base.Controls.Add(this.exceptionHBox);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.groupIntensity);
            base.Controls.Add(this.groupColors);
            base.Controls.Add(this.groupFont);
            base.Controls.Add(this.groupCharSet);
            base.Controls.Add(this.cmdCancel);
            base.Controls.Add(this.cmdOK);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FrmSettings";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Settings";
            base.Load += new EventHandler(this.frmSettings_Load);
            this.groupColors.ResumeLayout(false);
            this.panelBackColor.ResumeLayout(false);
            this.panelBackColor.PerformLayout();
            this.groupFont.ResumeLayout(false);
            this.groupCharSet.ResumeLayout(false);
            this.groupIntensity.ResumeLayout(false);
            this.groupIntensity.PerformLayout();
            this.updownBlink.EndInit();
            this.updownNormal.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.myScreenBufferRows.EndInit();
            this.myWindowsRows.EndInit();
            this.exceptionHBox.ResumeLayout(false);
            this.exceptionHBox.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void radioMsgBox1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show("If you choose this option all the exceptions, (warnings and information), \r sent from New Skies will show up in a Message Box simular to this one.");
        }

        private void radioMsgBox2_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show("If you choose this option all the exceptions, (warnings and information), \r sent from New Skies will continue to show up as text messages on your screen.");
        }

        private void SaveSettings()
        {
            Utilities.SaveUtilitiesSettingsObject(this.appName, "FontName", this.fontname);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "FontSize", this.fontsize);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "FontBold", this.bold);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "FontItalic", this.italic);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "ForeColor", Utilities.GetColorString(this.vt.ForeColor));
            Utilities.SaveUtilitiesSettingsObject(this.appName, "BoldColor", Utilities.GetColorString(this.vt.BoldColor));
            Utilities.SaveUtilitiesSettingsObject(this.appName, "BackColor", Utilities.GetColorString(this.vt.BackColor));
            Utilities.SaveUtilitiesSettingsObject(this.appName, "CharSet", this.vt.NationalCharSet);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "NormalIntensity", this.updownNormal.Value);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "BlinkIntensity", this.updownBlink.Value);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "Width", this.myWindowsColumns132.Checked ? 0x84 : 80);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "Height", this.myWindowsRows.Value);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "BufferRows", this.myScreenBufferRows.Value);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "ShowListBox", this._checkBoxLeftColumn.Checked ? "true" : "false");
            Utilities.SaveUtilitiesSettingsObject(this.appName, "ShowFunctionKeys", this._checkBoxFunctionKeys.Checked ? "true" : "false");
            Utilities.SaveUtilitiesSettingsObject(this.appName, "ExceptionInMsgBox", this.myShowExceptionBox.Checked ? "true" : "false");
            Utilities.SaveUtilitiesSettingsObject(this.appName, "BoardingPassPrinter", this.BoardingPassPrinter);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "ReportPrinter", this.ReportPrinter);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "BagTagPrinter", this.BagTagPrinter);
            Utilities.SaveUtilitiesSettingsObject(this.appName, "ReportsDirectory", this.ReportsDirectory);
        }

        private void ShowFont()
        {
            System.Drawing.Font font;
            if (this.fonttype == FontType.TrueType)
            {
                font = new System.Drawing.Font(this.fontname, this.fontsize, this.GetFontStyle(this.bold, this.italic));
            }
            else
            {
                font = new System.Drawing.Font("Microsoft Sans Serif", this.fontsize, this.GetFontStyle(this.bold, this.italic));
            }
            this.lblFont.Font = font;
            this.lblFont.Text = this.fontname + " Size " + this.fontsize.ToString();
        }

        private void UpdateInterface()
        {
            this.ShowFont();
            this.panelBackColor.BackColor = this.vt.BackColor;
            this.lblFont.BackColor = this.vt.BackColor;
            this.lblForeColor.ForeColor = this.vt.ForeColor;
            this.lblBoldColor.ForeColor = this.vt.BoldColor;
            this.lblFont.ForeColor = this.vt.ForeColor;
            this.lblFont.BackColor = this.vt.BackColor;
            this.updownNormal.Value = this.vt.NormalIntensity;
            this.updownBlink.Value = this.vt.BlinkIntensity;
            this.cboCharSet.Text = this.vt.NationalCharSet.ToString();
        }

        public string BagTagPrinter
        {
            get
            {
                return this._bagTagPrinter;
            }
        }

        public string BoardingPassPrinter
        {
            get
            {
                return this._boardingPassPrinter;
            }
        }

        public string ReportPrinter
        {
            get
            {
                return this._reportPrinter;
            }
        }

        public string ReportsDirectory
        {
            get
            {
                return this._reportsDirectory;
            }
            set
            {
                DirectoryInfo info = new DirectoryInfo(value);
                if (info.Exists)
                {
                    this._reportsDirectory = value;
                }
                else
                {
                    MessageBox.Show("Invalid Reports Directory Chosen", "Error");
                    this._textBoxReportsDirectory.Text = string.Empty;
                }
            }
        }
    }
}

