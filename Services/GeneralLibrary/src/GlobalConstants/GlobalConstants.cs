namespace GlobalConstants
{

    public static class DefaultClaims
    {
        public static readonly string UserName = "UserName";
        public static readonly string Email = "Email";
    }
    public static class CorsConstants
    {
        public const string CorsOrigins_PermittedClients = "CorsOrigins_PermittedClients";
        public const string Cors_Policy = "CorsPolicy";
    }

    public static class JwtConstants
    {
        public static readonly string UserName = "UserName";
        public static readonly string Email = "Email";

    }

    public static class StandardUnits
    {
        public static readonly string milliVoltPerVolt = "mV/V";
        public static readonly string kilograms = "kg";
        public static readonly string Volts = "V";
        public static readonly string Ohms = "Ω";
        public static readonly string Pounds = "lbs";
        public static readonly string milliAmpere = "mA";


    }
    public static class VerificationFinalResult
    {
        public static readonly string PASS = "PASS";
        public static readonly string FAIL = "FAIL";
        public static readonly string COMPLETED = "COMPLETED";
        public static readonly string TRIM = "TRIM";



    }

    public static class AllModelTypes
    {
        public const string TensionLink = "Tension Link";
        public const string UltraSlim = "Ultra Slim";
        public const string LoadPin = "Load Pin";
        public const string WeighPad = "Weigh Pad";
        public const string LoadCell = "Load Cell";
        public const string AxlePad = "Axle Pad";
        public const string LoadBeam = "Load Beam";
        public const string CraneScale = "Crane Scale";
    }

    public static class DefaultTestingModes
    {
        public const string MANUAL = "MANUAL";
        public const string AUTOMATIC = "AUTOMATIC";

    }
    public static class GeneralCertificateTypes
    {
        public const string StandardCertificate = "StandardCertificate";
        public const string ZeroToTen = "ZeroToTen";
        public const string FourToTwenty = "FourToTwenty";
        public const string MatraCourt = "Mantracourt";
        public const string WesternScale = "WesternScale";
        public const string Others = "Other";




    }

    public static class ResourceAuthorityCodes
    {
        public const string BatchCreationNotifier = "Batch - Serial Number";
        public const string NCRCreationNotifier = "NCR Admin";
        public const string RMAAssessmentRecipientEmail = "RMA Admin";
        //public const string CertificateAuthority = "CertificateAuthority";

    }
    public static class APINames
    {
        public const string RegistrationManagerAPI = "RegistrationManagerAPI";
        public const string ProductManagerAPI = "ProductManagerAPI";
        public const string ModelManagerAPI = "ModelManagerAPI";
        public const string AuthAPI = "AuthAPI";
        public const string CompressionTesterAPI = "CompressionTesterAPI";
        public const string RMAManagerAPI = "RMAManagerAPI";
        public const string MinCleanTemplateManagerAPI = "MinCleanTemplateManagerAPI";

    }

    public static class OTLPParams
    {

        public const string EndPoint = "http://mass-dsk41.ad.massload.com:4317";
        public const string Version = "0.1";
        public const string ServiceName = "LOCATEDONSTAGINGSERVER";

    }


    public class ConnectionStringProvider
    {
        public string ConnectionString { get; set; }
    }


    



}


