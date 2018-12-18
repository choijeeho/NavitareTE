namespace NavitaireTE.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"), CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("False")]
        public bool AllowConnectionChanges
        {
            get
            {
                return (bool) this["AllowConnectionChanges"];
            }
            set
            {
                this["AllowConnectionChanges"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
        public bool AllowOnlyPrinterChanges
        {
            get
            {
                return (bool) this["AllowOnlyPrinterChanges"];
            }
            set
            {
                this["AllowOnlyPrinterChanges"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("")]
        public string BagTagPrinter
        {
            get
            {
                return (string) this["BagTagPrinter"];
            }
            set
            {
                this["BagTagPrinter"] = value;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("Free 3 of 9"), UserScopedSetting]
        public string BarCodeFont
        {
            get
            {
                return (string) this["BarCodeFont"];
            }
            set
            {
                this["BarCodeFont"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue(""), DebuggerNonUserCode]
        public string BoardingPassPrinter
        {
            get
            {
                return (string) this["BoardingPassPrinter"];
            }
            set
            {
                this["BoardingPassPrinter"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("")]
        public string CharacterSize
        {
            get
            {
                return (string) this["CharacterSize"];
            }
            set
            {
                this["CharacterSize"] = value;
            }
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("utf-8"), UserScopedSetting]
        public string Encoding
        {
            get
            {
                return (string) this["Encoding"];
            }
            set
            {
                this["Encoding"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>10,7</string>\r\n  <string>12,8</string>\r\n  <string>16,9</string>\r\n  <string>20,10</string>\r\n  <string>30,11</string>\r\n  <string>41,12</string>\r\n  <string>45,13</string>\r\n  <string>50,15</string>\r\n  <string>60,16</string>\r\n  <string>61,17</string>\r\n  <string>67,18</string>\r\n</ArrayOfString>"), DebuggerNonUserCode]
        public StringCollection FontSizeMappings
        {
            get
            {
                return (StringCollection) this["FontSizeMappings"];
            }
            set
            {
                this["FontSizeMappings"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>Localhost;localhost;23</string>\r\n  <string>ROD;RZPHEAP500-FR;23</string>\r\n  <string>UAT;149.122.160.22;23</string>\r\n  <string>Localhost;localhost;23</string>\r\n</ArrayOfString>")]
        public StringCollection Hosts
        {
            get
            {
                return (StringCollection) this["Hosts"];
            }
            set
            {
                this["Hosts"] = value;
            }
        }

        [DefaultSettingValue(""), DebuggerNonUserCode, UserScopedSetting]
        public string ReportPrinter
        {
            get
            {
                return (string) this["ReportPrinter"];
            }
            set
            {
                this["ReportPrinter"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("")]
        public string ReportsDirectory
        {
            get
            {
                return (string) this["ReportsDirectory"];
            }
            set
            {
                this["ReportsDirectory"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool RunScript
        {
            get
            {
                return (bool) this["RunScript"];
            }
            set
            {
                this["RunScript"] = value;
            }
        }

        [DefaultSettingValue("False"), UserScopedSetting, DebuggerNonUserCode]
        public bool WindowsPrinting
        {
            get
            {
                return (bool) this["WindowsPrinting"];
            }
            set
            {
                this["WindowsPrinting"] = value;
            }
        }

        [DefaultSettingValue("Lucida Console, 9pt"), UserScopedSetting, DebuggerNonUserCode]
        public Font WindowsPrintingFont
        {
            get
            {
                return (Font) this["WindowsPrintingFont"];
            }
            set
            {
                this["WindowsPrintingFont"] = value;
            }
        }

        [DefaultSettingValue("DavesScript.xml"), DebuggerNonUserCode, UserScopedSetting]
        public string XMLScript
        {
            get
            {
                return (string) this["XMLScript"];
            }
            set
            {
                this["XMLScript"] = value;
            }
        }
    }
}

