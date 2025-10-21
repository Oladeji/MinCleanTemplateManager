
namespace GlobalConstants
{
    public record ApplicationPolicy(string Name, string Type, string ClaimValue);
    public static class ApplicationPolicies
    {
        public const string AccountsAPP01 = "Accounts_APP01";
        public static readonly ApplicationPolicy Accounts_APP01 = new(AccountsAPP01, "FUNCTION", "APP01_01");

        public const string User_PermissionAPP01 = "User_Permission_APP01";
        public static readonly ApplicationPolicy User_Permission_APP01 = new(User_PermissionAPP01, "FUNCTION", "APP01_02");

        public const string BarcodeAPP01 = "Barcode_APP01";
        public static readonly ApplicationPolicy Barcode_APP01 = new(BarcodeAPP01, "FUNCTION", "APP01_03");

        public const string NotificationsAPP01 = "Notifications_APP01";
        public static readonly ApplicationPolicy Notifications_APP01 = new(NotificationsAPP01, "FUNCTION", "APP01_04");

        public const string Add_User_GroupAPP01 = "Add_User_Group_APP01";
        public static readonly ApplicationPolicy Add_User_Group_APP01 = new(Add_User_GroupAPP01, "FUNCTION", "APP01_05");

        public const string Add_Resource_AuthorityAPP01 = "Add_Resource_Authority_APP01";
        public static readonly ApplicationPolicy Add_Resource_Authority_APP01 = new(Add_Resource_AuthorityAPP01, "FUNCTION", "APP01_06");

        public const string General_AccessAPP01 = "General_Access_APP01";
        public static readonly ApplicationPolicy General_Access_APP01 = new(General_AccessAPP01, "FUNCTION", "APP01_07");

        public const string Process_Flow_GroupAPP02 = "Process_Flow_Group_APP02";
        public static readonly ApplicationPolicy Process_Flow_Group_APP02 = new(Process_Flow_GroupAPP02, "FUNCTION", "APP02_01");

        public const string Models___TypeAPP02 = "Models___Type_APP02";
        public static readonly ApplicationPolicy Models___Type_APP02 = new(Models___TypeAPP02, "FUNCTION", "APP02_02");

        public const string Perform_AnalysisAPP02 = "Perform_Analysis_APP02";
        public static readonly ApplicationPolicy Perform_Analysis_APP02 = new(Perform_AnalysisAPP02, "FUNCTION", "APP02_03");

        public const string Add_DocumentsAPP02 = "Add_Documents_APP02";
        public static readonly ApplicationPolicy Add_Documents_APP02 = new(Add_DocumentsAPP02, "FUNCTION", "APP02_04");

        public const string View_DocumentsAPP02 = "View_Documents_APP02";
        public static readonly ApplicationPolicy View_Documents_APP02 = new(View_DocumentsAPP02, "FUNCTION", "APP02_05");

        public const string Models___Model_and_VersionAPP02 = "Models___Model_and_Version_APP02";
        public static readonly ApplicationPolicy Models___Model_and_Version_APP02 = new(Models___Model_and_VersionAPP02, "FUNCTION", "APP02_06");

        public const string General_AccessAPP02 = "General_Access_APP02";
        public static readonly ApplicationPolicy General_Access_APP02 = new(General_AccessAPP02, "FUNCTION", "APP02_07");

        public const string Surface_PreparationAPP03 = "Surface_Preparation_APP03";
        public static readonly ApplicationPolicy Surface_Preparation_APP03 = new(Surface_PreparationAPP03, "FUNCTION", "APP03_01");

        public const string GaugingAPP03 = "Gauging_APP03";
        public static readonly ApplicationPolicy Gauging_APP03 = new(GaugingAPP03, "FUNCTION", "APP03_02");

        public const string Gauge_InspectionAPP03 = "Gauge_Inspection_APP03";
        public static readonly ApplicationPolicy Gauge_Inspection_APP03 = new(Gauge_InspectionAPP03, "FUNCTION", "APP03_03");

        public const string WiringAPP03 = "Wiring_APP03";
        public static readonly ApplicationPolicy Wiring_APP03 = new(WiringAPP03, "FUNCTION", "APP03_04");

        public const string CablingAPP03 = "Cabling_APP03";
        public static readonly ApplicationPolicy Cabling_APP03 = new(CablingAPP03, "FUNCTION", "APP03_05");

        public const string Initial_Verification_ManualAPP03 = "Initial_Verification_Manual_APP03";
        public static readonly ApplicationPolicy Initial_Verification_Manual_APP03 = new(Initial_Verification_ManualAPP03, "FUNCTION", "APP03_06");

        public const string Initial_Verification_AutoAPP03 = "Initial_Verification_Auto_APP03";
        public static readonly ApplicationPolicy Initial_Verification_Auto_APP03 = new(Initial_Verification_AutoAPP03, "FUNCTION", "APP03_07");

        public const string Add_ResistorAPP03 = "Add_Resistor_APP03";
        public static readonly ApplicationPolicy Add_Resistor_APP03 = new(Add_ResistorAPP03, "FUNCTION", "APP03_08");

        public const string SealAPP03 = "Seal_APP03";
        public static readonly ApplicationPolicy Seal_APP03 = new(SealAPP03, "FUNCTION", "APP03_09");

        public const string Final_Verification_ManualAPP03 = "Final_Verification_Manual_APP03";
        public static readonly ApplicationPolicy Final_Verification_Manual_APP03 = new(Final_Verification_ManualAPP03, "FUNCTION", "APP03_10");

        public const string Final_Verification_AutoAPP03 = "Final_Verification_Auto_APP03";
        public static readonly ApplicationPolicy Final_Verification_Auto_APP03 = new(Final_Verification_AutoAPP03, "FUNCTION", "APP03_11");

        public const string LabellingAPP03 = "Labelling_APP03";
        public static readonly ApplicationPolicy Labelling_APP03 = new(LabellingAPP03, "FUNCTION", "APP03_12");

        public const string InventoryAPP03 = "Inventory_APP03";
        public static readonly ApplicationPolicy Inventory_APP03 = new(InventoryAPP03, "FUNCTION", "APP03_13");

        public const string ShippingAPP03 = "Shipping_APP03";
        public static readonly ApplicationPolicy Shipping_APP03 = new(ShippingAPP03, "FUNCTION", "APP03_14");

        public const string Record_DefectAPP03 = "Record_Defect_APP03";
        public static readonly ApplicationPolicy Record_Defect_APP03 = new(Record_DefectAPP03, "FUNCTION", "APP03_15");

        public const string Batch_Serial_NoAPP03 = "Batch_Serial_No_APP03";
        public static readonly ApplicationPolicy Batch_Serial_No_APP03 = new(Batch_Serial_NoAPP03, "FUNCTION", "APP03_16");

        public const string Track__ReportAPP03 = "Track__Report_APP03";
        public static readonly ApplicationPolicy Track__Report_APP03 = new(Track__ReportAPP03, "FUNCTION", "APP03_17");

        public const string Add_PictureAPP03 = "Add_Picture_APP03";
        public static readonly ApplicationPolicy Add_Picture_APP03 = new(Add_PictureAPP03, "FUNCTION", "APP03_18");

        public const string DefectAPP03 = "Defect_APP03";
        public static readonly ApplicationPolicy Defect_APP03 = new(DefectAPP03, "FUNCTION", "APP03_19");

        public const string Print_Stage_cardAPP03 = "Print_Stage_card_APP03";
        public static readonly ApplicationPolicy Print_Stage_card_APP03 = new(Print_Stage_cardAPP03, "FUNCTION", "APP03_20");

        public const string Version_ReassignmentAPP03 = "Version_Reassignment_APP03";
        public static readonly ApplicationPolicy Version_Reassignment_APP03 = new(Version_ReassignmentAPP03, "FUNCTION", "APP03_21");

        public const string Change_StageAPP03 = "Change_Stage_APP03";
        public static readonly ApplicationPolicy Change_Stage_APP03 = new(Change_StageAPP03, "FUNCTION", "APP03_22");

        public const string NCR_ListAPP03 = "NCR_List_APP03";
        public static readonly ApplicationPolicy NCR_List_APP03 = new(NCR_ListAPP03, "FUNCTION", "APP03_23");

        public const string Add_Reference_CellAPP03 = "Add_Reference_Cell_APP03";
        public static readonly ApplicationPolicy Add_Reference_Cell_APP03 = new(Add_Reference_CellAPP03, "FUNCTION", "APP03_24");

        public const string General_AccessAPP03 = "General_Access_APP03";
        public static readonly ApplicationPolicy General_Access_APP03 = new(General_AccessAPP03, "FUNCTION", "APP03_25");

        public const string Trim_Shunt_ResistorAPP03 = "Trim_Shunt_Resistor_APP03";
        public static readonly ApplicationPolicy Trim_Shunt_Resistor_APP03 = new(Trim_Shunt_ResistorAPP03, "FUNCTION", "APP03_26");

        public const string Add_IndicatorModel_TransmitterAPP03 = "Add_IndicatorModel_Transmitter_APP03";
        public static readonly ApplicationPolicy Add_IndicatorModel_Transmitter_APP03 = new(Add_IndicatorModel_TransmitterAPP03, "FUNCTION", "APP03_27");

        public const string Create_RMAAPP04 = "Create_RMA_APP04";
        public static readonly ApplicationPolicy Create_RMA_APP04 = new(Create_RMAAPP04, "FUNCTION", "APP04_01");

        public const string Receive_RMAAPP04 = "Receive_RMA_APP04";
        public static readonly ApplicationPolicy Receive_RMA_APP04 = new(Receive_RMAAPP04, "FUNCTION", "APP04_02");

        public const string Assess_RMAAPP04 = "Assess_RMA_APP04";
        public static readonly ApplicationPolicy Assess_RMA_APP04 = new(Assess_RMAAPP04, "FUNCTION", "APP04_03");

        public const string Add_Sales_OrderAPP04 = "Add_Sales_Order_APP04";
        public static readonly ApplicationPolicy Add_Sales_Order_APP04 = new(Add_Sales_OrderAPP04, "FUNCTION", "APP04_04");

        public const string Repair_RMAAPP04 = "Repair_RMA_APP04";
        public static readonly ApplicationPolicy Repair_RMA_APP04 = new(Repair_RMAAPP04, "FUNCTION", "APP04_05");

        public const string Close_RMAAPP04 = "Close_RMA_APP04";
        public static readonly ApplicationPolicy Close_RMA_APP04 = new(Close_RMAAPP04, "FUNCTION", "APP04_06");

        public const string General_AccessAPP04 = "General_Access_APP04";
        public static readonly ApplicationPolicy General_Access_APP04 = new(General_AccessAPP04, "FUNCTION", "APP04_07");

        public const string Time_RegistrySampleAPP05 = "Time_RegistrySample_APP05";
        public static readonly ApplicationPolicy Time_RegistrySample_APP05 = new(Time_RegistrySampleAPP05, "FUNCTION", "APP05_01");


        public static readonly ApplicationPolicy[] AllPolicies = [
        Accounts_APP01,
User_Permission_APP01,
Barcode_APP01,
Notifications_APP01,
Add_User_Group_APP01,
Add_Resource_Authority_APP01,
General_Access_APP01,
Process_Flow_Group_APP02,
Models___Type_APP02,
Perform_Analysis_APP02,
Add_Documents_APP02,
View_Documents_APP02,
Models___Model_and_Version_APP02,
General_Access_APP02,
Surface_Preparation_APP03,
Gauging_APP03,
Gauge_Inspection_APP03,
Wiring_APP03,
Cabling_APP03,
Initial_Verification_Manual_APP03,
Initial_Verification_Auto_APP03,
Add_Resistor_APP03,
Seal_APP03,
Final_Verification_Manual_APP03,
Final_Verification_Auto_APP03,
Labelling_APP03,
Inventory_APP03,
Shipping_APP03,
Record_Defect_APP03,
Batch_Serial_No_APP03,
Track__Report_APP03,
Add_Picture_APP03,
Defect_APP03,
Print_Stage_card_APP03,
Version_Reassignment_APP03,
Change_Stage_APP03,
NCR_List_APP03,
Add_Reference_Cell_APP03,
General_Access_APP03,
Trim_Shunt_Resistor_APP03,
Add_IndicatorModel_Transmitter_APP03,
Create_RMA_APP04,
Receive_RMA_APP04,
Assess_RMA_APP04,
Add_Sales_Order_APP04,
Repair_RMA_APP04,
Close_RMA_APP04,
General_Access_APP04,
Time_RegistrySample_APP05,
];
    }
}