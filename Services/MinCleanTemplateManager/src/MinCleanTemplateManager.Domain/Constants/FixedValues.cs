
namespace MinCleanTemplateManager.Domain.Constants
{
    /// <summary>
    /// This is to help prevent the use of magic strings in the application
    /// </summary>
    public static class FixedValues
    {    //Database Connection String Name 
        public const string DBServerConnectionStringNameServer = "ServerConstr";
        public const string Server = "ServerMachinName";
        public const string DBDevConnectionStringName = "DevConstr";
        public const string Dev = "MASS-DSK33";

        public const string DBClientConnectionStringName = "ClientConstr";
        public const string Client = "";
        public const string ApplicationName = "RegistrationManager";
        //SampleModel Constants
        public const int SampleModelsNameMaxLength = 128;
        public const int SampleModelsNameMinLength = 2;
        public const int SampleModelsIdMinLength = 36;
        public const int SampleModelsIdMaxLength = 68;



        //Model Constants   
        public const int ModelNameMaxLength = 128;
        public const int ModelNameMinLength = 2;
        public const int ModelIdMaxLength = 30;
        public const int ModelIdMinLength = 2;


        //DocumentType Constants



        public const string DEFAULT_DOCUMENT_PATH = "F:\\MASSLOAD\\DWG\\Load Cell - Standard\\";
        public const int DocumentTypeMaxLength = 128;

        public const int DocumentTypeMinLength = 2;
    }
}
