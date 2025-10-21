using Microsoft.Extensions.Options;
using Microsoft.Reporting.NETCore;
using ZXing.Common;


namespace ReportGeneratorLib.Utils
{
    public class UserBarCodeGenerator
    {
        public static Task<string> GenerateUserBarCode(string root, string BarCode, string Name)
        {

            //string base64String = ZingHelper.GetQRCode64String(BarCode);
            //string base64String = ZingHelper.GetBarCodeBS64String(BarCode);
            const string _reportDefinitionTemplateFolder = "BarcodeReports";


            //var options = new EncodingOptions
            //{
            //    Height = 600, // Specify the height of the barcode image
            //    Width = 150,   // Specify the width of the barcode image
            //    PureBarcode = true
            //    // Specify the width of the barcode image,
            //};

            string base64String = ZingHelper.GetBarCodeBS64String(BarCode,  null);

            var path = Path.Combine(root, _reportDefinitionTemplateFolder, "NewUserBarCode.rdlc");
            string imagetype = "pdf";

            //2.125" x 3.375" x 0.030" (5.40 cm x 8.57cm x 0.076 cm)
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                LocalReport locareport = new();
                locareport.LoadReportDefinition(stream);
                locareport.EnableExternalImages = true;

                ReportParameterCollection reportParameters = new()
                {
                    new ReportParameter("Names", Name),
                    new ReportParameter("BarCodeImg", base64String, true)
                };
                locareport.SetParameters(reportParameters);
                byte[] renderedBytes;
                renderedBytes = locareport.Render("PDF");
                string stringvalue = Convert.ToBase64String(renderedBytes);
                return Task.FromResult(stringvalue);


            }
        }

    }
}
