using DomainBase;

namespace MinCleanTemplateManager.Domain.Entities
{
    public partial class SampleModel : BaseEntity
    {
        private SampleModel() { }
        public string SampleModelName { get; init; } = string.Empty;
        // private  List <Model> _Models { get;  set;}  = new List<Model>();
        // public  IReadOnlyCollection<Model> Models => _Models;
        // public Guid GuidId    { get; init; } 

        public static SampleModel Create(string SampleModelName, Guid guidId)
        {
            if (guidId == Guid.Empty)
            {
                throw new ArgumentException($"SampleModel Guid value cannot be empty {nameof(guidId)}");
            }
            return new()
            {
                SampleModelName = SampleModelName,
                GuidId = guidId,
            };
        }
    }
}