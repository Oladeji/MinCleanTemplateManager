

namespace GlobalConstants
{
    public static partial class DefaultAssignments
    {

        // Production or assemblers
        public static readonly DefaultAssignment assemblerDefultAssignedRegistarionManagerRoles = new DefaultAssignment(new string[] {
            RegistrationManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RegistrationManagerAppCode);

        public static readonly DefaultAssignment assemblerDefaultAssignedModelManagerRoles = new DefaultAssignment(new string[] {
        },
        MassLoadAppNames.ModelManagerAppCode);

        public static readonly DefaultAssignment assemblerDefaultAssignedProductManagerRoles = new DefaultAssignment(new string[] {
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
 
        },
        MassLoadAppNames.ProductManagerAppCode);

        public static readonly DefaultAssignment assemblerDefaultAssignedRMAManagerRoles = new DefaultAssignment(new string[] {
            RMAManagerFunctionsGroup.GeneralAccess.functionAlias
        },
        MassLoadAppNames.RMAManagerAppCode);
        public static readonly DefaultAssignment assemblerDefaultAssignedTimeRegistryRoles = new DefaultAssignment(new string[] {
            TimeRegistryFunctionsGroup.TimeRegistrySample.functionAlias
        },
MassLoadAppNames.TimeRegistryAppCode);
    }
}
