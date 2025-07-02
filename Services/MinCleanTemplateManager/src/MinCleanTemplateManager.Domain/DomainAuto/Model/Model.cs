using DomainBase;


namespace MinCleanTemplateManager.Domain.Entities
{
    public partial class Model : BaseEntity
    {
        //Left To make SampleModel Work in the template
        private Model() { }
        public string ModelName { get; init; } = string.Empty;
        public string SampleModelName { get; init; } = string.Empty;
        public SampleModel? SampleModel { get; init; }
        //private  List <ModelVersion> _ModelVersions { get;  set;}  = new List<ModelVersion>();
        //public  IReadOnlyCollection<ModelVersion> ModelVersions => _ModelVersions;
        //// public Guid GuidId    { get; init; } 

        public static Model Create(string modelName, string SampleModelName, Guid guidId)
        {
            if (guidId == Guid.Empty)
            {
                throw new ArgumentException($"Model Guid value cannot be empty {nameof(guidId)}");
            }
            return new()
            {
                ModelName = modelName,
                SampleModelName = SampleModelName,
                GuidId = guidId,
            };
        }
    }
}