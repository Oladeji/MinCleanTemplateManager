using DomainBase;

namespace MinCleanTemplateManager.Domain.Entities
{
    public partial class SampleModel : BaseEntity
    {
        private SampleModel() { }
        public string SampleModelName { get; init; } = string.Empty;

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