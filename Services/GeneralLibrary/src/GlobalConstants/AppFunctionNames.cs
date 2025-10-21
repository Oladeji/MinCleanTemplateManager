namespace GlobalConstants
{
    public static class MassLoadAppNames
    {
        // RegistrationManagerApp
        public const string RegistrationManagerAppCode = "APP01";
        public const string RegistrationManagerAppName = "Manage Users";
        public const string RegistrationManagerAppDescription = "Manage Users";
        // ModelManagerApp
        public const string ModelManagerAppCode = "APP02";
        public const string ModelManagerAppName = "Model Manager";
        public const string ModelManagerAppDescription = "Model Manager";
        // ProductManagerApp
        public const string ProductManagerAppCode = "APP03";
        public const string ProductManagerAppName = "Product Manager";
        public const string ProductManagerAppDescription = "Product Manager";
        // RMAManagerApp
        public const string RMAManagerAppCode = "APP04";
        public const string RMAManagerAppName = "RMA Manager";
        public const string RMAManagerAppDescription = "RMA Manager";
        // TimeRegistryApp
        public const string TimeRegistryAppCode = "APP05";
        public const string TimeRegistryAppName = "Time Registry";
        public const string TimeRegistryAppDescription = "Time Registry";
    }

    public static class RegistrationManagerFunctionsGroup
    {
        public static class Accounts
        {
            public const string Application = MassLoadAppNames.RegistrationManagerAppCode;
            public const string functionCode = "01";
            public const string functionDescription = "Accounts";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class UserPermission
        {
            public const string Application = MassLoadAppNames.RegistrationManagerAppCode;
            public const string functionCode = "02";
            public const string functionDescription = "User Permission";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Barcode
        {
            public const string Application = MassLoadAppNames.RegistrationManagerAppCode;
            public const string functionCode = "03";
            public const string functionDescription = "Barcode";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Notifications
        {
            public const string Application = MassLoadAppNames.RegistrationManagerAppCode;
            public const string functionCode = "04";
            public const string functionDescription = "Notifications";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class AddUserGroup
        {
            public const string Application = MassLoadAppNames.RegistrationManagerAppCode;
            public const string functionCode = "05";
            public const string functionDescription = "Add User Group";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class AddResourceAuthority
        {
            public const string Application = MassLoadAppNames.RegistrationManagerAppCode;
            public const string functionCode = "06";
            public const string functionDescription = "Add Resource Authority";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class GeneralAccess
        {
            public const string Application = MassLoadAppNames.RegistrationManagerAppCode;
            public const string functionCode = "07";
            public const string functionDescription = "General Access";
            public const string functionAlias = Application + "_" + functionCode;
        }
    }

    public static class ModelManagerFunctionsGroup
    {
        public static class ProcessFlowGroup
        {
            public const string Application = MassLoadAppNames.ModelManagerAppCode;
            public const string functionCode = "01";
            public const string functionDescription = "Process Flow Group";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class ModelsType
        {
            public const string Application = MassLoadAppNames.ModelManagerAppCode;
            public const string functionCode = "02";
            public const string functionDescription = "Models - Type";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class PerformAnalysis
        {
            public const string Application = MassLoadAppNames.ModelManagerAppCode;
            public const string functionCode = "03";
            public const string functionDescription = "Perform Analysis";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class AddDocuments
        {
            public const string Application = MassLoadAppNames.ModelManagerAppCode;
            public const string functionCode = "04";
            public const string functionDescription = "Add Documents";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class ViewDocuments
        {
            public const string Application = MassLoadAppNames.ModelManagerAppCode;
            public const string functionCode = "05";
            public const string functionDescription = "View Documents";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class ModelsModelAndVersion
        {
            public const string Application = MassLoadAppNames.ModelManagerAppCode;
            public const string functionCode = "06";
            public const string functionDescription = "Models  - Model and Version";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class GeneralAccess
        {
            public const string Application = MassLoadAppNames.ModelManagerAppCode;
            public const string functionCode = "07";
            public const string functionDescription = "General Access";
            public const string functionAlias = Application + "_" + functionCode;
        }


    }

    public static class ProductManagerFunctionsGroup
    {
        public static class SurfacePreparation
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "01";
            public const string functionDescription = "Surface Preparation";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Gauging
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "02";
            public const string functionDescription = "Gauging";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class GaugeInspection
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "03";
            public const string functionDescription = "Gauge Inspection";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Wiring
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "04";
            public const string functionDescription = "Wiring";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Cabling
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "05";

            public const string functionDescription = "Cabling";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class InitialVerificationManual
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "06";
            public const string functionDescription = "Initial Verification (Manual)";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class InitialVerificationAuto
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "07";
            public const string functionDescription = "Initial Verification (Auto)";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class AddResistor
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "08";
            public const string functionDescription = "Add Resistor";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Seal
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "09";
            public const string functionDescription = "Seal";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class FinalVerificationManual
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "10";
            public const string functionDescription = "Final Verification (Manual)";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class FinalVerificationAuto
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "11";
            public const string functionDescription = "Final Verification (Auto)";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Labelling
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "12";
            public const string functionDescription = "Labelling";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Inventory
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "13";
            public const string functionDescription = "Inventory";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Shipping
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "14";
            public const string functionDescription = "Shipping";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class RecordDefect
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "15";
            public const string functionDescription = "Record Defect";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class BatchSerialNo
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "16";
            public const string functionDescription = "Batch/Serial No";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class TrackReport
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "17";
            public const string functionDescription = "Track/ Report";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class AddPicture
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "18";
            public const string functionDescription = "Add Picture";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class Defect
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "19";
            public const string functionDescription = "Defect";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class PrintStagecard
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "20";
            public const string functionDescription = "Print Stage card";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class VersionReassignment
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "21";
            public const string functionDescription = "Version Reassignment";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class ChangeStage
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "22";
            public const string functionDescription = "Change Stage";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class NCRList
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "23";
            public const string functionDescription = "NCR List";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class AddReferenceCell
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "24";
            public const string functionDescription = "Add Reference Cell";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class GeneralAccess
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "25";
            public const string functionDescription = "General Access";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class AddTrimShuntResistor
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "26";
            public const string functionDescription = "Trim/Shunt Resistor";
            public const string functionAlias = Application + "_" + functionCode;
        }

        public static class AddIndicatorModel
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "27";
            public const string functionDescription = "Add IndicatorModel/Transmitter";
            public const string functionAlias = Application + "_" + functionCode;
        }
    }

    public static class RMAManagerFunctionsGroup
    {
        public static class CreateRMA
        {
            public const string Application = MassLoadAppNames.RMAManagerAppCode;
            public const string functionCode = "01";
            public const string functionDescription = "Create RMA";
            public const string functionAlias = Application + "_" + functionCode;
        }

        public static class ReceiveRMA
        {
            public const string Application = MassLoadAppNames.RMAManagerAppCode;
            public const string functionCode = "02";
            public const string functionDescription = "Receive RMA";
            public const string functionAlias = Application + "_" + functionCode;
        }

        public static class AssessRMA
        {
            public const string Application = MassLoadAppNames.RMAManagerAppCode;
            public const string functionCode = "03";
            public const string functionDescription = "Assess RMA";
            public const string functionAlias = Application + "_" + functionCode;
        }

        public static class AddSalesOrder
        {
            public const string Application = MassLoadAppNames.RMAManagerAppCode;
            public const string functionCode = "04";
            public const string functionDescription = "Add Sales Order";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class RepairRMA
        {
            public const string Application = MassLoadAppNames.RMAManagerAppCode;
            public const string functionCode = "05";
            public const string functionDescription = "Repair RMA";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class CloseRMA
        {
            public const string Application = MassLoadAppNames.RMAManagerAppCode;
            public const string functionCode = "06";
            public const string functionDescription = "Close RMA";
            public const string functionAlias = Application + "_" + functionCode;
        }
        public static class GeneralAccess
        {
            public const string Application = MassLoadAppNames.ProductManagerAppCode;
            public const string functionCode = "07";
            public const string functionDescription = "General Access";
            public const string functionAlias = Application + "_" + functionCode;
        }
    }

    public static class TimeRegistryFunctionsGroup
    {
        public static class TimeRegistrySample
        {
            public const string Application = MassLoadAppNames.TimeRegistryAppCode;
            public const string functionCode = "01";
            public const string functionDescription = "Time-RegistrySample";
            public const string functionAlias = Application + "_" + functionCode;
        }
    }
  }




