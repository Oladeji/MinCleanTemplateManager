using CertificateType;
using GlobalConstants;
using Microsoft.Reporting.NETCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace ReportGeneratorLib.Utils
{
    public static class GenerateLabels
    {

        const string _reportDefinitionTemplateFolder = "ReportDefinitions";
        public static async Task<string> GenerateActualOutputLabel(string root, List<ActualOutputData> actualOutputData)
        {


            var path = Path.Combine(_reportDefinitionTemplateFolder, "ActualOutputLabel.rdlc");

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                LocalReport locareport = new LocalReport();
                locareport.DataSources.Add(new ReportDataSource("ActualOutputList", actualOutputData));
                locareport.ReportPath = path;
                locareport.LoadReportDefinition(stream);
                byte[] renderedBytes;
                renderedBytes = locareport.Render("PDF");

                string stringvalue = Convert.ToBase64String(renderedBytes);
                return await Task.FromResult(stringvalue);
            }
        }
        private static async Task<byte[]> SetupAndRenderReport(string reportPath, string datasetName, object dataSource, CertificateTypeCreatorResponse reportData, string templateFolder)
        {
            try
            {
                using (var stream = new FileStream(reportPath, FileMode.Open, FileAccess.Read))
                {
                    LocalReport locareport = new LocalReport();
                    locareport.DataSources.Add(new ReportDataSource(datasetName, dataSource));
                    locareport.LoadReportDefinition(stream);

                    if (reportData is MatraCourtCertificateTypeCreatorResponse matraCourtCertificateType)
                    {
                        locareport.DataSources.Add(new ReportDataSource(matraCourtCertificateType.BeforeCalibrationDatasetName, matraCourtCertificateType.BeforeCalibrationData));
                        locareport.DataSources.Add(new ReportDataSource(matraCourtCertificateType.AfterCalibrationDatasetName, matraCourtCertificateType.AfterCalibrationData));
                        var beforeCalibrationVisibilityValue = matraCourtCertificateType.BeforeCalibrationData.Count > 0;
                        locareport.SetParameters(new ReportParameter("BeforeCalibrationVisibility", beforeCalibrationVisibilityValue.ToString()));

                        var afterCalibrationVisibility = matraCourtCertificateType.AfterCalibrationData.Count > 0;
                        locareport.SetParameters(new ReportParameter("AfterCalibrationVisibility", afterCalibrationVisibility.ToString()));

                    }



                    if (reportData is WesterScaleCertificateTypeCreatorResponse westerScaleCertificateType)
                    {
                        locareport.DataSources.Add(new ReportDataSource(westerScaleCertificateType.BeforeCalibrationDatasetName, westerScaleCertificateType.BeforeCalibrationData));
                        locareport.DataSources.Add(new ReportDataSource(westerScaleCertificateType.AfterCalibrationDatasetName, westerScaleCertificateType.AfterCalibrationData));

                        var beforeCalibrationVisibilityValue = westerScaleCertificateType.BeforeCalibrationData.Count > 0;
                        locareport.SetParameters(new ReportParameter("BeforeCalibrationVisibility", beforeCalibrationVisibilityValue.ToString()));

                        var afterCalibrationVisibility = westerScaleCertificateType.AfterCalibrationData.Count > 0;
                        locareport.SetParameters(new ReportParameter("AfterCalibrationVisibility", afterCalibrationVisibility.ToString()));

                    }





                    if (reportData is FourToTwentyCertificateTypeCreatorResponse fourToTwentyCertificateType)
                    {
                        locareport.DataSources.Add(new ReportDataSource(fourToTwentyCertificateType.AfterCalibrationDatasetName, fourToTwentyCertificateType.AfterCalibrationData));
                    }


                    if (reportData is ZeroToTenCertificateTypeCreatorResponse zeroTenCertificateType)
                    {
                        locareport.DataSources.Add(new ReportDataSource(zeroTenCertificateType.AfterCalibrationDatasetName, zeroTenCertificateType.AfterCalibrationData));
                    }



                    if (reportData is StandardCertificateWtDetailCreatorResponse standardweDetailType)
                    {
                        locareport.DataSources.Add(new ReportDataSource(standardweDetailType.AfterCalibrationDatasetName, standardweDetailType.AfterCalibrationData));
                    }

                    if (templateFolder != null)
                    {
                        byte[] imageBytes = reportData.SignatureImg;
                        ReportParameter imageParameter = new ReportParameter("StandardCertProdManagerSign", Convert.ToBase64String(imageBytes));
                        locareport.SetParameters(imageParameter);
                    }

                    byte[] renderedBytes = locareport.Render("PDF");
                    return await Task.FromResult(renderedBytes);
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static async Task<string> GenerateCertificateListPrintOut(string webRootPath, IEnumerable<ProductFullCertificateData> certData, string type)
        {
            var reportDatas = CertificateTypeCreator.Create(certData, type);
            var mergedPdf = new PdfDocument();

            foreach (var reportData in reportDatas)
            {

                var path = Path.Combine(_reportDefinitionTemplateFolder, reportData.ReportRdlcName);
                var renderedBytes = await SetupAndRenderReport(path, reportData.ReportDatasetName, reportData.CertificateType, reportData, _reportDefinitionTemplateFolder);

                using (var pdfStream = new MemoryStream(renderedBytes))
                {
                    var pdfDoc = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
                    for (int i = 0; i < pdfDoc.PageCount; i++)
                    {
                        mergedPdf.AddPage(pdfDoc.Pages[i]);
                    }
                }
            }

            using (var outputStream = new MemoryStream())
            {
                mergedPdf.Save(outputStream, false);
                string stringvalue = Convert.ToBase64String(outputStream.ToArray());
                return await Task.FromResult(stringvalue);
            }
        }
        public static async Task<string> GenerateCertificatePrintOut(string webRootPath, ProductFullCertificateData productCertificate, string type)
        {
            var reportData = CertificateTypeCreator.Create(productCertificate, type);
            var path = Path.Combine(_reportDefinitionTemplateFolder, reportData.ReportRdlcName);
            var renderedBytes = await SetupAndRenderReport(path, reportData.ReportDatasetName, reportData.CertificateType, reportData, _reportDefinitionTemplateFolder);

            string stringvalue = Convert.ToBase64String(renderedBytes);
            return await Task.FromResult(stringvalue);
        }




        public static async Task<string> GenerateSurfacePrepLabel(string root, List<SurfacePrepData> surfacePrepData)
        {

            foreach (var item in surfacePrepData)
            {
                item.BarCodeImg = ZingHelper.GetBarCodeBS64String(item.ProductId, null);
            }

            var path = Path.Combine(_reportDefinitionTemplateFolder, "SurfacePrepLabel.rdlc");

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                LocalReport locareport = new LocalReport();
                locareport.DataSources.Add(new ReportDataSource("SurfacePrepList", surfacePrepData));
                locareport.ReportPath = path;
                locareport.LoadReportDefinition(stream);
                byte[] renderedBytes;
                renderedBytes = locareport.Render("PDF");

                string stringvalue = Convert.ToBase64String(renderedBytes);
                return await Task.FromResult(stringvalue);



            }
        }

        public static async Task<string> PrintStageCard(string root, StageCard stageCard)
        {


            //string base64String = ZingHelper.GetQRCode64String(stageCard.StageCode);
            string base64String = ZingHelper.GetBarCodeBS64String(stageCard.StageCode, null);
            var path = Path.Combine(root, _reportDefinitionTemplateFolder, "StageBarCode.rdlc");
            string imagetype = "pdf";

            //2.125" x 3.375" x 0.030" (5.40 cm x 8.57cm x 0.076 cm)
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                LocalReport locareport = new();
                locareport.LoadReportDefinition(stream);
                locareport.EnableExternalImages = true;

                ReportParameterCollection reportParameters = new()
                {
                    new ReportParameter("Names", stageCard.StageName),
                    new ReportParameter("BarCodeImg", base64String, true)
                };
                locareport.SetParameters(reportParameters);
                byte[] renderedBytes;
                renderedBytes = locareport.Render("PDF");
                string stringvalue = Convert.ToBase64String(renderedBytes);
                return await Task.FromResult(stringvalue);
            }
        }
    }
}



