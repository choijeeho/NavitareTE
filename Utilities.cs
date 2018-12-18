namespace NavitaireTE
{
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class Utilities
    {
        public static string GetColorString(System.Drawing.Color c)
        {
            if (c == System.Drawing.Color.Empty)
            {
                return "Empty";
            }
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (c.IsNamedColor)
            {
                return c.Name;
            }
            str2 = c.R.ToString("X");
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            str3 = c.G.ToString("X");
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            str4 = c.B.ToString("X");
            if (str4.Length == 1)
            {
                str4 = "0" + str4;
            }
            return ("#" + str2 + str3 + str4);
        }

        public static string GetFilename(string path)
        {
            int num = path.LastIndexOf(@"\");
            int num2 = path.LastIndexOf("/");
            if (num2 > num)
            {
                num = num2;
            }
            return path.Substring(num + 1);
        }

        public static string GetPath(string filename)
        {
            int num = filename.LastIndexOf(@"\");
            int num2 = filename.LastIndexOf("/");
            if (num2 > num)
            {
                num = num2;
            }
            return filename.Substring(0, num + 1);
        }

        public static string GetSafeFileName(string filename)
        {
            string str = filename;
            string newValue = "_";
            return str.Replace("<", newValue).Replace(">", newValue).Replace("\"", newValue).Replace("?", newValue).Replace("*", newValue).Replace(@"\", newValue).Replace("/", newValue).Replace("|", newValue).Replace(":", newValue);
        }

        public static string GetTransferRate(long transferRate)
        {
            decimal d = transferRate;
            if (d <= 0M)
            {
                return "";
            }
            if (d > 1048576M)
            {
                return (Math.Round((decimal) (d / 1048576M), 1).ToString() + " MBps");
            }
            if (d > 1000M)
            {
                return (Math.Round((decimal) (d / 1024M), 1).ToString() + " KBps");
            }
            return (Math.Round(d, 2).ToString() + " Bps");
        }

        public static object GetUtilitiesSettingsObject(string applicationName, string keyname)
        {
            object defaultValue = null;
            try
            {
                return Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Navitaire\" + applicationName).GetValue(keyname, defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static object GetUtilitiesSettingsObject(string applicationName, string keyname, object defaultValue)
        {
            try
            {
                return Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Navitaire\" + applicationName).GetValue(keyname, defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void SaveUtilitiesSettingsObject(string applicationName, string keyname, object value)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Navitaire\" + applicationName).SetValue(keyname, value);
            }
            catch
            {
            }
        }

        public static Exception ViewFile(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = filename;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    if (MessageBox.Show("The application could not be started. Do you want to open it with Notepad?", "Cannot Start Application") == DialogResult.OK)
                    {
                        try
                        {
                            Process process2 = new Process();
                            process2.StartInfo.FileName = "Notepad.exe";
                            process2.StartInfo.Arguments = filename;
                            process2.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            process2.Start();
                        }
                        catch (Exception exception)
                        {
                            return exception;
                        }
                    }
                }
            }
            return null;
        }
    }
}

