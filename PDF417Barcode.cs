
    using J4L.RBarcode;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    internal class PDF417Barcode
    {
        private RPDF417Web _pdf417Barcode = new RPDF417Web();

        public PDF417Barcode()
        {
            this._pdf417Barcode.PDFECLevel = 3;
        }
        
        public Image GenerateFormattedPDF417Barcode(string FormattedBarCode, int maxBitMapWidth, int maxBitMapHeight)
        {
            this.Barcode.PDFCode = FormattedBarCode;
            this.Barcode.PDFMode = RPDF417Web.tCompaction.PDF_TEXT;
            Bitmap image = new Bitmap(maxBitMapWidth, maxBitMapHeight);
            Graphics g = Graphics.FromImage(image);
            this.Barcode.paintBarCode(g);
            Image img = image;
            Bitmap bitmap2 = image.Clone(new Rectangle(0, 0, this.Barcode.PaintedWidth, this.Barcode.PaintedHeight), image.PixelFormat);
            MemoryStream stream = new MemoryStream();
            bitmap2.Save(stream, ImageFormat.Gif);
            g.Dispose();
            bitmap2.Dispose();
            return img;
        }

        /*
        public byte[] GenerateFormattedPDF417Barcode(string FormattedBarCode, int maxBitMapWidth, int maxBitMapHeight)
        {
            this.Barcode.PDFCode = FormattedBarCode;
            this.Barcode.PDFMode = RPDF417Web.tCompaction.PDF_BINARY;
            Bitmap image = new Bitmap(maxBitMapWidth, maxBitMapHeight);
            Graphics g = Graphics.FromImage(image);
            this.Barcode.paintBarCode(g);
            Bitmap bitmap2 = image.Clone(new Rectangle(0, 0, this.Barcode.PaintedWidth, this.Barcode.PaintedHeight), image.PixelFormat);
            MemoryStream stream = new MemoryStream();
            bitmap2.Save(stream, ImageFormat.Gif);
            g.Dispose();
            image.Dispose();
            bitmap2.Dispose();
            return stream.ToArray();
        }
        */
        public void SetPDF417BarCodeOptions(int barHeightInPixels, int barWidthInPixels, int columns, int rows, int errorCorrectionLevel, int topMargin, int leftMargin)
        {
            if (barHeightInPixels > -1)
            {
                this.Barcode.PDFBarHeight = barHeightInPixels;
            }
            if (barWidthInPixels > -1)
            {
                this.Barcode.PDFBarWidth = barWidthInPixels;
            }
            if (columns > -1)
            {
                this.Barcode.PDFColumns = columns;
            }
            if (rows > -1)
            {
                this.Barcode.PDFRows = rows;
            }
            if (errorCorrectionLevel > -1)
            {
                this.Barcode.PDFECLevel = errorCorrectionLevel;
            }
            if (topMargin > -1)
            {
                this.Barcode.PDFTopMargin = topMargin;
            }
            if (leftMargin > -1)
            {
                this.Barcode.PDFLeftMargin = leftMargin;
            }
        }

        public RPDF417Web Barcode
        {
            get
            {
                return this._pdf417Barcode;
            }
        }
    }


