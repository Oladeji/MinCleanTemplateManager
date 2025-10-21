namespace CertificateType
{
    public class FourToTwentyCertificateType : BaseCertificate
    {
        public string CustomerName { get; set; }
        public string RMA { get; set; }
        public int ProductId { get; set; }
        public string ModelName { get; set; }
        public string SalesOrder { get; set; }
        public string CertificateNo { get; set; }
        public string CalibrationDate { get; set; }
        public string CalibrationDueDate { get; set; }
        public string TestResult { get; set; }
        public string IndicatorModel { get; set; }
        public string IndicatorSerialNo { get; set; }
        public string IndicatorModuleId { get; set; }

        public string TransmitterModel { get; set; }
        public string TransmitterSerialNo { get; set; }
        public string TransmitterModuleId { get; set; }





        public string TestPinDiameterStart { get; set; }
        public string TestPinDiameterEnd { get; set; }
        public string OutputUnit { get; set; }
        public string RefWeightUnit { get; set; }

        public string GraduationSize { get; set; }
        public string TestMode { get; set; }
        public double TargetWeight4 { get; set; }
        public double TargetWeight20 { get; set; }
        public string WiringRed { get; set; }
        public string WiringBlack { get; set; }
        public string WiringWhite { get; set; }
        public string WiringGreen { get; set; }
        public string WiringShield { get; set; }
        public double InputVoltage { get; set; }
        public string Notes { get; set; }
        public double CableLenght { get; set; }
        public string CalibrationTechnician { get; set; }

        public string CheckedBy { get; set; }
        public string CheckedByDesignation { get; set; }
        public List<CalibrationDataPlusScaleError> AfterCalibrationData { get; set; }
    }

}