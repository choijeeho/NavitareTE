namespace NavitaireTE
{
    using System;
    using System.Drawing;

    public class FontMapping
    {
        private System.Drawing.FontStyle _fontStyle;
        private float _NSFontSize;
        private float _TEFontSize;

        public FontMapping(string nsFontSize, string teFontSize, string fontStyle)
        {
            this.NSFontSize = (float) Convert.ToDouble(nsFontSize);
            this.TEFontSize = (float) Convert.ToDouble(teFontSize);
            switch (fontStyle.ToUpper())
            {
                case "B":
                    this.FontStyle = System.Drawing.FontStyle.Bold;
                    return;

                case "R":
                    this.FontStyle = System.Drawing.FontStyle.Regular;
                    return;

                case " ":
                    this.FontStyle = System.Drawing.FontStyle.Regular;
                    return;

                case "I":
                    this.FontStyle = System.Drawing.FontStyle.Italic;
                    return;

                case "S":
                    this.FontStyle = System.Drawing.FontStyle.Strikeout;
                    return;

                case "U":
                    this.FontStyle = System.Drawing.FontStyle.Underline;
                    return;
            }
            this.FontStyle = System.Drawing.FontStyle.Regular;
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this._fontStyle;
            }
            set
            {
                this._fontStyle = value;
            }
        }

        public float NSFontSize
        {
            get
            {
                return this._NSFontSize;
            }
            set
            {
                this._NSFontSize = value;
            }
        }

        public float TEFontSize
        {
            get
            {
                return this._TEFontSize;
            }
            set
            {
                this._TEFontSize = value;
            }
        }
    }
}

