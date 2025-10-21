namespace CertificateType
{

    public class StandardCertificateType : BaseCertificate
    {
        //public StandardCertificateType(string certificateNo, string calibrationDate, string testResult, int productId, string modelName, decimal capacity, decimal outputAtCapacity, int cableLength, string referenceSerial, int safeLoad, int ultimateLoad, int compensatedTempRangeLow, int compensatedTempRangeHigh, int ratedExcitationMin, int ratedExcitationMax, int operatingTempMin, int operatingTempMax, int inputResistance, int outputResistance, string calibrationTechnician, string checkedBy)
        //{
        //    CertificateNo = certificateNo;
        //    CalibrationDate = calibrationDate;
        //    TestResult = testResult;
        //    ProductId = productId;
        //    ModelName = modelName;
        //    Capacity = capacity;
        //    OutputAtCapacity = outputAtCapacity;
        //    CableLength = cableLength;
        //    ReferenceSerial = referenceSerial;
        //    SafeLoad = safeLoad;
        //    UltimateLoad = ultimateLoad;
        //    CompensatedTempRangeLow = compensatedTempRangeLow;
        //    CompensatedTempRangeHigh = compensatedTempRangeHigh;
        //    RatedExcitationMin = ratedExcitationMin;
        //    RatedExcitationMax = ratedExcitationMax;
        //    OperatingTempMin = operatingTempMin;
        //    OperatingTempMax = operatingTempMax;

        //    InputResistance = inputResistance;
        //    OutputResistance = outputResistance;
        //    CalibrationTechnician = calibrationTechnician;
        //    CheckedBy = checkedBy;
        //}

        public string CertificateNo { get; set; }
        public string CalibrationDate { get; set; }
        public string TestResult { get; set; }
        public int ProductId { get; set; }
        public string ModelName { get; set; }

        public decimal Capacity { get; set; }



        private decimal _outputAtCapacity;
        public decimal OutputAtCapacity
        {
            get
            {
                return Math.Round(_outputAtCapacity, 4);
            }
            set => _outputAtCapacity = value;
        }

        public int CableLength { get; set; }

        public string ReferenceSerial { get; set; }


        public int SafeLoad { get; set; }

        public int UltimateLoad { get; set; }

        public int CompensatedTempRangeLow { get; set; }

        public int CompensatedTempRangeHigh { get; set; }


        public int RatedExcitationMin { get; set; }
        public int RatedExcitationMax { get; set; }

        public int OperatingTempMin { get; set; }
        public int OperatingTempMax { get; set; }


        public int InputResistance { get; set; }
        public int OutputResistance { get; set; }
        public string CalibrationTechnician { get; set; }
        public string CheckedBy { get; set; }
        public string CheckedByDesignation { get; set; }
        public List<CalibrationData> AfterCalibrationData { get; set; }
    }

}