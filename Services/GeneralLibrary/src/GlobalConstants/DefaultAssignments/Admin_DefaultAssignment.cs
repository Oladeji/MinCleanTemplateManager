

namespace GlobalConstants
{
    public static partial  class DefaultAssignments
    {


        //admin
        public static readonly DefaultAssignment adminDefaultAssignedRegistrationManagerRoles = new DefaultAssignment(new string[]
            { RegistrationManagerFunctionsGroup.Accounts.functionAlias,
                RegistrationManagerFunctionsGroup.UserPermission.functionAlias,
                RegistrationManagerFunctionsGroup.Barcode.functionAlias,
                RegistrationManagerFunctionsGroup.Notifications.functionAlias,
                RegistrationManagerFunctionsGroup.AddUserGroup.functionAlias,
                RegistrationManagerFunctionsGroup.AddResourceAuthority.functionAlias,
                RegistrationManagerFunctionsGroup.GeneralAccess.functionAlias
            },
            MassLoadAppNames.RegistrationManagerAppCode);

        public static readonly DefaultAssignment adminDefaultAssignedModelManagerRoles = new DefaultAssignment(new string[] {
            ModelManagerFunctionsGroup.ProcessFlowGroup.functionAlias,
            ModelManagerFunctionsGroup.ModelsType.functionAlias,
            ModelManagerFunctionsGroup.PerformAnalysis.functionAlias,
            ModelManagerFunctionsGroup.AddDocuments.functionAlias,
            ModelManagerFunctionsGroup.ViewDocuments.functionAlias,
            ModelManagerFunctionsGroup.ModelsModelAndVersion.functionAlias,
            ModelManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.ModelManagerAppCode);

        public static readonly DefaultAssignment adminDefaultAssignedProductManagerRoles = new DefaultAssignment(new string[] {
            ProductManagerFunctionsGroup.SurfacePreparation.functionAlias,
            ProductManagerFunctionsGroup.Gauging.functionAlias,
            ProductManagerFunctionsGroup.GaugeInspection.functionAlias,
            ProductManagerFunctionsGroup.Wiring.functionAlias,
            ProductManagerFunctionsGroup.Cabling.functionAlias,
            ProductManagerFunctionsGroup.InitialVerificationManual.functionAlias,
            ProductManagerFunctionsGroup.InitialVerificationAuto.functionAlias,
            ProductManagerFunctionsGroup.AddResistor.functionAlias,
            ProductManagerFunctionsGroup.Seal.functionAlias,
            ProductManagerFunctionsGroup.FinalVerificationManual.functionAlias,
            ProductManagerFunctionsGroup.FinalVerificationAuto.functionAlias,
            ProductManagerFunctionsGroup.Labelling.functionAlias,
            ProductManagerFunctionsGroup.Inventory.functionAlias,
            ProductManagerFunctionsGroup.Shipping.functionAlias,
            ProductManagerFunctionsGroup.RecordDefect.functionAlias,
            ProductManagerFunctionsGroup.BatchSerialNo.functionAlias,
            ProductManagerFunctionsGroup.TrackReport.functionAlias,
            ProductManagerFunctionsGroup.AddPicture.functionAlias,
            ProductManagerFunctionsGroup.Defect.functionAlias,
            ProductManagerFunctionsGroup.PrintStagecard.functionAlias,
            ProductManagerFunctionsGroup.VersionReassignment.functionAlias,
            ProductManagerFunctionsGroup.ChangeStage.functionAlias,
            ProductManagerFunctionsGroup.NCRList.functionAlias,
            ProductManagerFunctionsGroup.AddReferenceCell.functionAlias,
            ProductManagerFunctionsGroup.GeneralAccess.functionAlias,
                       ProductManagerFunctionsGroup.AddTrimShuntResistor.functionAlias,
            ProductManagerFunctionsGroup.AddIndicatorModel.functionAlias
        },
        MassLoadAppNames.ProductManagerAppCode);

        public static readonly DefaultAssignment adminDefaultAssignedRMAManagerRoles = new DefaultAssignment(new string[] {
            RMAManagerFunctionsGroup.CreateRMA.functionAlias,
            RMAManagerFunctionsGroup.ReceiveRMA.functionAlias,
            RMAManagerFunctionsGroup.AssessRMA.functionAlias,
            RMAManagerFunctionsGroup.AddSalesOrder.functionAlias,
            RMAManagerFunctionsGroup.RepairRMA.functionAlias,
            RMAManagerFunctionsGroup.CloseRMA.functionAlias,
            RMAManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RMAManagerAppCode);

        public static readonly DefaultAssignment adminDefaultAssignedTimeRegistryRoles = new DefaultAssignment(new string[] {
            TimeRegistryFunctionsGroup.TimeRegistrySample.functionAlias
        },
        MassLoadAppNames.TimeRegistryAppCode);
    }
}
