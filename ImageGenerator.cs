
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Web;

    public class ImageGenerator
    {
        public static Image CreatePdf417Barcode(string payload)
        {
            PDF417Barcode barcode = new PDF417Barcode();
            Pdf417BarcodeConfigurationSection config = new Pdf417BarcodeConfigurationSection();
            SetPDF417BarCodeOptions(barcode, config);
            return barcode.GenerateFormattedPDF417Barcode(payload, config.MaxBitMapWidth, config.MaxBitMapHeight);
        }
       

        private static void SetPDF417BarCodeOptions(PDF417Barcode pdf417Barcode, Pdf417BarcodeConfigurationSection config)
        {
            pdf417Barcode.SetPDF417BarCodeOptions(config.BarHeightInPixels, config.BarWidthInPixels, config.Columns, config.Rows, config.ErrorCorrectionLevel, config.TopMargin, config.LeftMargin);
        }
    }


