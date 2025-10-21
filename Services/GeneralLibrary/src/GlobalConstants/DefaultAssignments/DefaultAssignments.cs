namespace GlobalConstants
{
    public static partial class DefaultAssignments
    {


        //admin


        //generalUser
        public static readonly DefaultAssignment generalUserDefultAssignedRegistarionManagerRoles = new DefaultAssignment(new string[] {
            RegistrationManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RegistrationManagerAppCode);

        public static readonly DefaultAssignment generalUserDefaultAssignedModelManagerRoles = new DefaultAssignment(new string[] {
            ModelManagerFunctionsGroup.ViewDocuments.functionAlias,
            ModelManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.ModelManagerAppCode);

        public static readonly DefaultAssignment generalUserDefaultAssignedProductManagerRoles = new DefaultAssignment(new string[] {
            ProductManagerFunctionsGroup.TrackReport.functionAlias
        },
        MassLoadAppNames.ProductManagerAppCode);

        public static readonly DefaultAssignment generalUserDefaultAssignedRMAManagerRoles = new DefaultAssignment(new string[] {
            RMAManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RMAManagerAppCode);

        public static readonly DefaultAssignment generalUserDefaultAssignedTimeRegistryRoles = new DefaultAssignment(new string[] {
            TimeRegistryFunctionsGroup.TimeRegistrySample.functionAlias
        },
        MassLoadAppNames.TimeRegistryAppCode);











    }

}




