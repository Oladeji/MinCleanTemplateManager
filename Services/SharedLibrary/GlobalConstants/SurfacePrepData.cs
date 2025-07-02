namespace GlobalConstants
{
    public class SurfacePrepData
    {

        public SurfacePrepData()
        { }
        public SurfacePrepData(string modelName, string productId, string outPut, string capacity, string barCodeImg)
        {
            ModelName = modelName;
            ProductId = productId;
            OutPut = outPut;
            Capacity = capacity;
            BarCodeImg = barCodeImg;
        }

        public string ModelName { get; set; }
        //public string SerialNo { get; set; }
        public string ProductId { get; set; }
        public string OutPut { get; set; }
        public string Capacity { get; set; }
        public string BarCodeImg { get; set; }
    }

    public record StageCard(string StageCode, string StageName);


    public record ProductFullCertificateData(Int32 ProductId, string CertificateNo, string CertificateType, string JsonCertificateTypeDetails, byte[] signature, DateTime Timestamp, Guid GuidId);


    public class ActualOutputData
    {

        public ActualOutputData()
        { }
        public ActualOutputData(string productId, string outPut, string barCodeImg)
        {

            ProductId = productId;
            OutPut = outPut;

            BarCodeImg = barCodeImg;
        }


        public string ProductId { get; set; }
        public string OutPut { get; set; }

        public string BarCodeImg { get; set; }
    }

}

