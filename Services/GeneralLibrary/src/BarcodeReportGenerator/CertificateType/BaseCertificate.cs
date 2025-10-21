namespace CertificateType
{
    public class BaseCertificate
    {
        public BaseCertificate()
        { }

    }


    public class CalibrationDataPlusScaleError : CalibrationData
    {
        private double _fullScaleError;

        public double FullScaleError
        {
            get => Math.Round(_fullScaleError, 3);
            set => _fullScaleError = value;
        }


        private double _expectedReadOut;

        public double ExpectedReadOut
        {
            get => Math.Round(_expectedReadOut, 2);
            set => _expectedReadOut = value;
        }

    }
    public class CalibrationData
    {
        private double _weightTester;
        private double _readOut;

        public string Id { get; set; }

        public double WeightTester
        {
            get => Math.Round(_weightTester, 0);
            set => _weightTester = value;
        }

        public double ReadOut
        {
            get => Math.Round(_readOut, 2);
            set => _readOut = value;
        }
    }


}