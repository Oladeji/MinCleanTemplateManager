using GlobalConstants;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CertificateType
{

    public class CertificateTypeCreatorResponse
    {
        public required List<BaseCertificate> CertificateType { get; init; }
        public string ReportRdlcName { get; init; }= string.Empty;
        public string ReportDatasetName { get; init; }= string.Empty;

        public required byte[] SignatureImg { get; init; }
   
    }

    public class StandardCertificateWtDetailCreatorResponse : CertificateTypeCreatorResponse
    {

        public string AfterCalibrationDatasetName { get; init; }

        public List<CalibrationData> AfterCalibrationData { get; init; }


    }

    public class MatraCourtCertificateTypeCreatorResponse : CertificateTypeCreatorResponse
    {

        public string BeforeCalibrationDatasetName { get; init; }
        public string AfterCalibrationDatasetName { get; init; }
        public List<CalibrationData> BeforeCalibrationData { get; init; }
        public List<CalibrationData> AfterCalibrationData { get; init; }


    }

    public class WesterScaleCertificateTypeCreatorResponse : CertificateTypeCreatorResponse
    {

        public string BeforeCalibrationDatasetName { get; init; }
        public string AfterCalibrationDatasetName { get; init; }
        public List<CalibrationData> BeforeCalibrationData { get; init; }
        public List<CalibrationData> AfterCalibrationData { get; init; }


    }

    public class FourToTwentyCertificateTypeCreatorResponse : CertificateTypeCreatorResponse
    {

        public string AfterCalibrationDatasetName { get; init; }

        public List<CalibrationDataPlusScaleError> AfterCalibrationData { get; init; }


    }
    public class ZeroToTenCertificateTypeCreatorResponse : CertificateTypeCreatorResponse
    {

        public string AfterCalibrationDatasetName { get; init; }

        public List<CalibrationDataPlusScaleError> AfterCalibrationData { get; init; }


    }

    public static class CertificateTypeCreator
    {
        public static CertificateTypeCreatorResponse Create(ProductFullCertificateData productCertificate, string type)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull

            };
            if (productCertificate.CertificateType == GeneralCertificateTypes.ZeroToTen)
            {


                var reportname = "ZeroTenCertificate.rdlc";

                try
                {
                    var data = JsonSerializer.Deserialize<ZeroTenCertificateType>(productCertificate.JsonCertificateTypeDetails, options);

                    return new ZeroToTenCertificateTypeCreatorResponse
                    {
                        CertificateType = new List<BaseCertificate> { data },
                        ReportRdlcName = reportname,
                        ReportDatasetName = "ZeroTenCertificateDataSet",

                        AfterCalibrationDatasetName = "CalibrationDataPlusDataSet",
                      //  AfterCalibrationData = data.AfterCalibrationData,
                        AfterCalibrationData = data.AfterCalibrationData.OrderBy(x => x.WeightTester) .ToList(),
    
   
                        SignatureImg = productCertificate.signature,

                    };
                }
                catch (Exception ex)
                {

                    throw;
                }



            }
            else if (productCertificate.CertificateType == GeneralCertificateTypes.StandardCertificate)
            {
                try
                {

                    var data = JsonSerializer.Deserialize<StandardCertificateType>(productCertificate.JsonCertificateTypeDetails, options);

                    if (type == "Detailed")
                    {
                        return new StandardCertificateWtDetailCreatorResponse
                        {
                            CertificateType = new List<BaseCertificate> { data },
                            ReportRdlcName = "StandardCertificatewtDetails.rdlc",
                            ReportDatasetName = "StandardCertificateDataset",

                            AfterCalibrationDatasetName = "AfterCalibrationDataset",
                          //  AfterCalibrationData = data.AfterCalibrationData,
                            AfterCalibrationData = data.AfterCalibrationData.OrderBy(x => x.WeightTester).ToList(),
                            SignatureImg = productCertificate.signature,

                        };
                    }




                    return new CertificateTypeCreatorResponse
                    {
                        CertificateType = [data],
                        ReportRdlcName = "StandardCertificate.rdlc",
                        ReportDatasetName = "StandardCertificateDataset",
                        SignatureImg = productCertificate.signature,

                    };

                }
                catch (Exception ex)
                {

                    throw;
                }


            }

            else if (productCertificate.CertificateType == GeneralCertificateTypes.FourToTwenty)
            {
                var reportname = "FourToTwentyCertificate.rdlc";

                try
                {
                    var data = JsonSerializer.Deserialize<FourToTwentyCertificateType>(productCertificate.JsonCertificateTypeDetails, options);

                    return new FourToTwentyCertificateTypeCreatorResponse
                    {
                        CertificateType = new List<BaseCertificate> { data },
                        ReportRdlcName = reportname,
                        ReportDatasetName = "FourToTwentyCertificateDataset",

                        AfterCalibrationDatasetName = "CalibrationDataPlusDataSet",
                        //AfterCalibrationData = data.AfterCalibrationData,
                        AfterCalibrationData = data.AfterCalibrationData.OrderBy(x => x.WeightTester).ToList(),
                        SignatureImg = productCertificate.signature,

                    };
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else if (productCertificate.CertificateType == GeneralCertificateTypes.MatraCourt)
            {
                try
                {
                    var reportname = "MatraCourtCertificate.rdlc";
                    var data = JsonSerializer.Deserialize<MatraCourtCertificateType>(productCertificate.JsonCertificateTypeDetails, options);


                    return new MatraCourtCertificateTypeCreatorResponse
                    {
                        CertificateType = new List<BaseCertificate> { data },
                        ReportDatasetName = "MatraCourtCertificateDataset",
                        BeforeCalibrationDatasetName = "BeforeCalibrationDataset",
                        AfterCalibrationDatasetName = "AfterCalibrationDataset",
                        ReportRdlcName = reportname,
                      //  BeforeCalibrationData = data.BeforeCalibrationData,
                       // AfterCalibrationData = data.AfterCalibrationData,
                        AfterCalibrationData = data.AfterCalibrationData.OrderBy(x => x.WeightTester).ToList(),
                        BeforeCalibrationData = data.BeforeCalibrationData.OrderBy(x => x.WeightTester).ToList(),
                        SignatureImg = productCertificate.signature,


                    };
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else if (productCertificate.CertificateType == GeneralCertificateTypes.WesternScale)
            {
                try
                {
                    var reportname = "WesternScaleCertificate.rdlc";
                    var data = JsonSerializer.Deserialize<WesternScaleCertificateType>(productCertificate.JsonCertificateTypeDetails, options);


                    return new WesterScaleCertificateTypeCreatorResponse
                    {
                        CertificateType = new List<BaseCertificate> { data },
                        ReportDatasetName = "WesternScaleCertificateDataset",
                        BeforeCalibrationDatasetName = "BeforeCalibrationDataset",
                        AfterCalibrationDatasetName = "AfterCalibrationDataset",
                        ReportRdlcName = reportname,
                        //BeforeCalibrationData = data.BeforeCalibrationData,
                        //AfterCalibrationData = data.AfterCalibrationData,

                        AfterCalibrationData = data.AfterCalibrationData.OrderBy(x => x.WeightTester).ToList(),
                        BeforeCalibrationData = data.BeforeCalibrationData.OrderBy(x => x.WeightTester).ToList(),
                        SignatureImg = productCertificate.signature,


                    };
                }
                catch (Exception ex)
                {
                    throw;
                }
            }



            else
            {
                return null;
            }
        }
        public static List<CertificateTypeCreatorResponse> Create(IEnumerable<ProductFullCertificateData> productCertificate, string type)
        {

            var result = new List<CertificateTypeCreatorResponse>();
            foreach (var item in productCertificate)
            {
                result.Add(Create(item, type));
            }

            return result;

        }

    }


}