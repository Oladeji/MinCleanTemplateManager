using Domain.DBContext;

using Microsoft.Extensions.Logging;
using MinCleanTemplateManager.Domain.Entities;
using MinCleanTemplateManager.Domain.Interfaces;
namespace MinCleanTemplateManager.Infrastructure.Persistence.Repositories

{
    public class SampleModelRepository : GenericRepository<SampleModel, MinCleanTemplateManagerContext>, ISampleModelRepository
    {
        public SampleModelRepository(MinCleanTemplateManagerContext ctx, ILogger<GenericRepository<SampleModel, MinCleanTemplateManagerContext>> logger) : base(ctx, logger)
        { }
    }
}