using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace ReportGeneratorLib.Utils
{
    public class ZingHelper
    {

        public static string GetBarCodeBS64StringOld(string text, EncodingOptions? options, BarcodeFormat? format = BarcodeFormat.CODE_128)
        {
            // Adjust the width based on the length of the text input
            //int width = Math.Max(100, text.Length * 40); // Example: 40 pixels per character
            int width = Math.Max(600, text.Length * 40); // Increase width for clarity
            int height = 150; // Increase height for better scanning

            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = format ?? BarcodeFormat.CODE_128,
                Options = options ?? new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    PureBarcode = true,
                    Margin = 2 // Reduce margin for more barcode area
                }
            };

#pragma warning restore CA1416 // Validate platform compatibility

            Bitmap barcodeBitmap = barcodeWriter.Write(text); // Generate the barcode image

            MemoryStream stream = new MemoryStream();
                barcodeBitmap.SetResolution(300, 300); // Set high DPI for clarity
            barcodeBitmap.Save(stream, ImageFormat.Png); // Save the barcode image to a stream

            // return File(stream.ToArray(), "image/png"); // Return the barcode image as a file
            var byteArray = stream.ToArray();
            // return byteArray;
            return Convert.ToBase64String(byteArray);

        }


        public static string GetBarCodeBS64String(string text, EncodingOptions? options, BarcodeFormat? format = BarcodeFormat.CODE_128)
        {
            // Set desired physical size and DPI
            int dpi = 300;
            double inchesWide = 2.0; // Desired width in inches
            double inchesHigh = 1.0; // Desired height in inches

            int width = (int)(dpi * inchesWide);   // 600 pixels for 2 inches at 300 DPI
            int height = (int)(dpi * inchesHigh);  // 300 pixels for 1 inch at 300 DPI

            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = format ?? BarcodeFormat.CODE_128,
                Options = options ?? new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    PureBarcode = true,
                    Margin = 2
                }
            };

            using Bitmap barcodeBitmap = barcodeWriter.Write(text);
            barcodeBitmap.SetResolution(dpi, dpi); // Set DPI metadata

            using MemoryStream stream = new MemoryStream();
            barcodeBitmap.Save(stream, ImageFormat.Png);

            return Convert.ToBase64String(stream.ToArray());
        }

        public static string GetBarCodeIStream1(string text)
        {
            var barcodeBitmap = CreateQrCode(text);
            MemoryStream stream = new MemoryStream();
            barcodeBitmap.Save(stream, ImageFormat.Bmp);
            var byteArray = stream.ToArray();

            return Convert.ToBase64String(byteArray);

        }



        public static string GetQRCode64String(string text)
        {
            var barcodeBitmap = CreateQrCode(text);
            MemoryStream stream = new MemoryStream();
            barcodeBitmap.Save(stream, ImageFormat.Bmp);
            var byteArray = stream.ToArray();

            return Convert.ToBase64String(byteArray);

        }
        private static Bitmap CreateQrCode(string data)
        {
            //specify desired options
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                CharacterSet = "UTF-8",
                DisableECI = true,
                Width = 250,
                Height = 250
            };

            //create new instance and set properties
            BarcodeWriter qrCodewriter = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };

            //create QR code and return Bitmap
            return qrCodewriter.Write(data);
        }


    }
}
