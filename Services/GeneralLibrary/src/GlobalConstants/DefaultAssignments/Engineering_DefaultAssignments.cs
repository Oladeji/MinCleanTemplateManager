

namespace GlobalConstants
{
    public static partial class DefaultAssignments
    {
        //enginering
        public static readonly DefaultAssignment engineeringDefultAssignedRegistarionManagerRoles = new DefaultAssignment(new string[] {
            RegistrationManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RegistrationManagerAppCode);

        public static readonly DefaultAssignment engineeringDefaultAssignedModelManagerRoles = new DefaultAssignment(new string[] {
            ModelManagerFunctionsGroup.ProcessFlowGroup.functionAlias,
            ModelManagerFunctionsGroup.ModelsType.functionAlias,
            ModelManagerFunctionsGroup.PerformAnalysis.functionAlias,
            ModelManagerFunctionsGroup.AddDocuments.functionAlias,
            ModelManagerFunctionsGroup.ViewDocuments.functionAlias
        },
        MassLoadAppNames.ModelManagerAppCode);

        public static readonly DefaultAssignment engineeringDefaultAssignedProductManagerRoles = new DefaultAssignment(new string[] {
            ProductManagerFunctionsGroup.BatchSerialNo.functionAlias
        },
        MassLoadAppNames.ProductManagerAppCode);

        public static readonly DefaultAssignment engineeringDefaultAssignedRMAManagerRoles = new DefaultAssignment(new string[] {
           // RMAManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RMAManagerAppCode);

        public static readonly DefaultAssignment engineeringDefaultAssignedTimeRegistryRoles = new DefaultAssignment(new string[] {
          //  TimeRegistryFunctionsGroup.TimeRegistrySample.functionAlias
        },
        MassLoadAppNames.TimeRegistryAppCode);
    }
}
