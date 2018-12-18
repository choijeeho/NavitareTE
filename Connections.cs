namespace NavitaireTE
{
    using System;
    using System.Windows.Forms;

    internal class Connections
    {
        private int _connectionPort = 0x17;
        public string ConnectionHost = "localhost";
        public string ConnectionName = "localhost";

        public Connections(string str)
        {
            try
            {
                string[] strArray = str.Split(";".ToCharArray(), 4);
                this.ConnectionName = strArray[0];
                this.ConnectionHost = strArray[1];
                this.ConnectionPort = strArray[2];
            }
            catch (Exception)
            {
                MessageBox.Show("Please use the following format;\nHostName;HostDNS;Port#\nExample:\nMyComputer;LocalHost;23", "Connection Error");
            }
        }

        public string ConnectionPort
        {
            set
            {
                try
                {
                    this._connectionPort = Convert.ToInt32(value);
                }
                catch
                {
                    MessageBox.Show("Invalid port number in connections:" + value);
                    this._connectionPort = 0x17;
                }
            }
        }

        public int Port
        {
            get
            {
                return this._connectionPort;
            }
        }
    }
}

