namespace MinCleanTemplateManager.BaseModels.Entities
{
    public abstract class BaseEntity 
    {

        public Guid  GuidId { get; set; } = default;

    }


    public abstract class BaseEntity<TKey> : BaseEntity
    {
        public TKey Id { get; set; } = default;
    }
}
