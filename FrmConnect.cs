namespace NavitaireTE
{
    using NavitaireTE.Properties;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmConnect : Form
    {
        private string _connectHost;
        private Hashtable _connections = new Hashtable();
        private string _connectName;
        private int _connectPort;
        private string appName = "";
        internal Button cmdCancel;
        internal Button cmdOk;
        private ComboBox comboBoxConnections;
        private Container components;
        internal Label lblHost;

        public FrmConnect(string appName)
        {
            this.InitializeComponent();
            this.appName = appName;
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmConnect_Load(object sender, EventArgs e)
        {
            this.GetSettings();
        }

        private void GetSettings()
        {
            this._connections.Clear();
            Settings settings = new Settings();
            // 원본 : using (StringEnumerator enumerator = settings.Hosts.GetEnumerator())
            StringEnumerator enumerator = settings.Hosts.GetEnumerator();
            {
                while (enumerator.MoveNext())
                {
                    Connections connections = new Connections(enumerator.Current);
                    if (!this._connections.Contains(connections.ConnectionName))
                    {
                        this._connections.Add(connections.ConnectionName, connections);
                        this.comboBoxConnections.Items.Add(connections.ConnectionName);
                    }
                }
            }
            if (this._connections.Count > 0)
            {
                this.comboBoxConnections.SelectedIndex = 0;
                string str2 = (string) this.comboBoxConnections.Items[0];
                Connections connections2 = (Connections) this._connections[str2];
                this._connectPort = connections2.Port;
                this._connectHost = connections2.ConnectionHost;
                this._connectName = connections2.ConnectionName;
            }
            else
            {
                MessageBox.Show("You must have at least one host configured in the app.config file.");
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmConnect));
            this.lblHost = new Label();
            this.cmdCancel = new Button();
            this.cmdOk = new Button();
            this.comboBoxConnections = new ComboBox();
            base.SuspendLayout();
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new Point(8, 15);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new Size(0x20, 13);
            this.lblHost.TabIndex = 6;
            this.lblHost.Text = "Host:";
            this.lblHost.TextAlign = ContentAlignment.MiddleRight;
            this.cmdCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.cmdCancel.DialogResult = DialogResult.Cancel;
            this.cmdCancel.Location = new Point(0x95, 0x2d);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(80, 0x18);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Text = "Cancel";
            this.cmdOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.cmdOk.DialogResult = DialogResult.OK;
            this.cmdOk.Location = new Point(0x3f, 0x2d);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new Size(80, 0x18);
            this.cmdOk.TabIndex = 5;
            this.cmdOk.Text = "OK";
            this.cmdOk.Click += new EventHandler(this.cmdOk_Click);
            this.comboBoxConnections.FormattingEnabled = true;
            this.comboBoxConnections.Location = new Point(40, 11);
            this.comboBoxConnections.Name = "comboBoxConnections";
            this.comboBoxConnections.Size = new Size(0xcd, 0x15);
            this.comboBoxConnections.TabIndex = 0x29;
            base.AcceptButton = this.cmdOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cmdCancel;
            base.ClientSize = new Size(0x125, 0x61);
            base.Controls.Add(this.comboBoxConnections);
            base.Controls.Add(this.lblHost);
            base.Controls.Add(this.cmdCancel);
            base.Controls.Add(this.cmdOk);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FrmConnect";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Connect";
            base.Load += new EventHandler(this.frmConnect_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SaveSettings()
        {
            Connections connections = null;
            if (this.comboBoxConnections.SelectedIndex == -1)
            {
                Settings settings = new Settings();
                if (!settings.AllowConnectionChanges)
                {
                    MessageBox.Show("You must select an item from the list.", "Input Error");
                }
                else
                {
                    connections = new Connections(this.comboBoxConnections.Text);
                    this._connections.Add(connections.ConnectionHost, connections);
                }
                this.comboBoxConnections.SelectedIndex = 0;
            }
            if (connections == null)
            {
                try
                {
                    string str = (string) this.comboBoxConnections.Items[this.comboBoxConnections.SelectedIndex];
                    connections = (Connections) this._connections[str];
                }
                catch (Exception)
                {
                    connections = null;
                }
            }
            this._connectPort = connections.Port;
            this._connectHost = connections.ConnectionHost;
            this._connectName = connections.ConnectionName;
        }

        private void txtHost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.cmdOk.PerformClick();
            }
        }

        public string ConnectHost
        {
            get
            {
                if (this._connectHost == null)
                {
                    this.GetSettings();
                }
                return this._connectHost;
            }
        }

        public string ConnectName
        {
            get
            {
                if (this._connectName == null)
                {
                    this.GetSettings();
                }
                return this._connectName;
            }
        }

        public int ConnectPort
        {
            get
            {
                if (this._connectPort == 0)
                {
                    this.GetSettings();
                }
                return this._connectPort;
            }
        }
    }
}

