using CodeGeneratorAttributesLibrary;


namespace MinCleanTemplateManager.BaseModels.Entities
{

    public class SampleModel : BaseEntity
    {
        [BaseModelBasicAttribute(32, 0, true, false)]
        public string SampleModelName { get; private set; } = string.Empty;
        //  [ProjectBaseModelsAttribute(30, 2, true, true, true, true, false, "")]


        // public ICollection<Model> Models { get; set; } //This is for navigation property.// to be removed if i want  to strictly follow domain driven design

    }
    [BaseModelsForeignKeyAttribute("SampleModel", "Models")]
    public partial class Model : BaseEntity
    {
        //if I am not using domain driven design then i can specify the detail model here eg 
        // public SampleModel SampleModel { get; set; }
        // public string ModelId { get; init; } = string.Empty;
        // public Guid ModelId { get; private set; }
        [BaseModelBasicAttribute(32, 0, true, false, false, true)]
        public string ModelName { get; init; } = string.Empty;

        [BaseModelBasicAttribute(32, 0, false, true, false, true)]
        public string SampleModelName { get; private set; } = string.Empty;
        public SampleModel? SampleModel { get; set; }


        //   public ICollection<ModelVersion> ModelVersions { get; set; }
        //public string SampleModelsId { get; init; } = string.Empty;

        //if I am not using domain driven design then i can specify the detail model here eg 



    }

}

