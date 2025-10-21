
namespace GlobalConstants
{
    public static partial class DefaultAssignments
    {
        //logistics
        public static readonly DefaultAssignment logisticsDefultAssignedRegistarionManagerRoles = new DefaultAssignment(new string[] {
            RegistrationManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RegistrationManagerAppCode);

        public static readonly DefaultAssignment logisticsDefaultAssignedModelManagerRoles = new DefaultAssignment(new string[] {
            ModelManagerFunctionsGroup.ViewDocuments.functionAlias
        },
        MassLoadAppNames.ModelManagerAppCode);

        public static readonly DefaultAssignment logisticsDefaultAssignedProductManagerRoles = new DefaultAssignment(new string[] {
            ProductManagerFunctionsGroup.Inventory.functionAlias,
            ProductManagerFunctionsGroup.Shipping.functionAlias,
            ProductManagerFunctionsGroup.TrackReport.functionAlias
        },
        MassLoadAppNames.ProductManagerAppCode);

        public static readonly DefaultAssignment logisticsDefaultAssignedRMAManagerRoles = new DefaultAssignment(new string[] {
            RMAManagerFunctionsGroup.CreateRMA.functionAlias,
            RMAManagerFunctionsGroup.ReceiveRMA.functionAlias,
           // RMAManagerFunctionsGroup.AssessRMA.functionAlias,
            RMAManagerFunctionsGroup.AddSalesOrder.functionAlias,
          //  RMAManagerFunctionsGroup.RepairRMA.functionAlias,
            RMAManagerFunctionsGroup.CloseRMA.functionAlias,
            RMAManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RMAManagerAppCode);

        public static readonly DefaultAssignment logisticsDefaultAssignedTimeRegistryRoles = new DefaultAssignment(new string[] {
            TimeRegistryFunctionsGroup.TimeRegistrySample.functionAlias
        },
        MassLoadAppNames.TimeRegistryAppCode);
    }
}
