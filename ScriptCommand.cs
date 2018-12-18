namespace NavitaireTE
{
    using System;
    using System.Globalization;

    public class ScriptCommand
    {
        public string CommandText;
        public string CommandType;
        public string Inputs;

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "<{0}>{1}", new object[] { this.CommandType, this.CommandText });
        }
    }
}

