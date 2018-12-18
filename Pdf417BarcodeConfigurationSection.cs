
    using System;

    internal class Pdf417BarcodeConfigurationSection
    {
        int barHeightInPixels = -1;
        int barWidthInPixels = -1;
        int columns = 6;
        int errorCorrectionLevel = 3;
        int leftMargin = 0;
        int maxBitMapHeight = 400;
        int maxBitMapWidth = 300;
        int rows = -1;
        int topMargin = 0;

        public int BarHeightInPixels
        {
            get
            {
                return barHeightInPixels;
            }
            set
            {
                barHeightInPixels = value;
            }
        }

        public int BarWidthInPixels
        {
            get
            {
                return barWidthInPixels;
            }
            set
            {
                barWidthInPixels = value;
            }
        }

        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        public int ErrorCorrectionLevel
        {
            get
            {
                return errorCorrectionLevel;
            }
            set
            {
                errorCorrectionLevel = value;
            }
        }

        public int LeftMargin
        {
            get
            {
                return leftMargin;
            }
            set
            {
                leftMargin = value;
            }
        }

        public int MaxBitMapHeight
        {
            get
            {
                return maxBitMapHeight;
            }
            set
            {
                maxBitMapHeight = value;
            }
        }

        public int MaxBitMapWidth
        {
            get
            {
                return maxBitMapWidth;
            }
            set
            {
                maxBitMapWidth = value;
            }
        }

        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }

        public int TopMargin
        {
            get
            {
                return topMargin;
            }
            set
            {
                topMargin = value;
            }
        }
    }


