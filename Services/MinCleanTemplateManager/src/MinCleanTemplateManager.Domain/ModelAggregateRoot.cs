
using DomainBase;


namespace MinCleanTemplateManager.Domain.Entities
{
    public partial class ModelAggregateRoot : BaseEntity
    {

        private delegate void ModelDataUpdatedEventHandler(Dictionary<string, List<int>> models);



        private static ModelDataUpdatedEventHandler DataUpdated;

       

     
    }
}
