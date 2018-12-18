namespace NavitaireTE
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class FrmPrinters : Form
    {
        private string _selectedPrinter = string.Empty;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox comboBox1;
        private IContainer components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;

        public FrmPrinters()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.btnCancel.Enabled = false;
            this.btnOK.Enabled = false;
            base.DialogResult = DialogResult.OK;
            try
            {
                string str = this.comboBox1.Items[this.comboBox1.SelectedIndex].ToString();
                this._selectedPrinter = str;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error setting default printer:" + exception.Message);
                base.DialogResult = DialogResult.Ignore;
            }
            finally
            {
                base.Close();
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

        private void FrmPrinters_Load(object sender, EventArgs e)
        {
            PrintDocument document = new PrintDocument();
            string printerName = document.PrinterSettings.PrinterName;
            foreach (string str2 in PrinterSettings.InstalledPrinters)
            {
                this.comboBox1.Items.Add(str2);
                if (str2 == printerName)
                {
                    this.comboBox1.SelectedIndex = this.comboBox1.Items.IndexOf(str2);
                }
            }
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Dock = DockStyle.Bottom;
            this.groupBox1.Location = new Point(0, 0x26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(330, 0x37);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.btnCancel.Location = new Point(0xb8, 0x15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(0x47, 0x15);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(0x56, 0x12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(240, 0x15);
            this.comboBox1.TabIndex = 2;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xa2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Installed Printers";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(4, 0x16);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4c, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Choose Printer";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            base.ClientSize = new Size(330, 0x5d);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.groupBox1);
            base.Name = "FrmPrinters";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Choose Default Printer";
            base.Load += new EventHandler(this.FrmPrinters_Load);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public string SelectedPrinter
        {
            get
            {
                return this._selectedPrinter;
            }
        }
    }
}

