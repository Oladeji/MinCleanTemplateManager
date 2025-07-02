
using DomainBase;

namespace MinCleanTemplateManager.Domain.Entities
{
    public class BaseEvent : BaseEntity
    {

        public string UserName { get; init; } = string.Empty;
        public DateTime TimeStamp { get; init; }
        public Guid GuidId { get; init; }
    }
}