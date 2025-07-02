using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinCleanTemplateManager.Domain.Entities;
namespace MinCleanTemplateManager.Infrastructure.Persistence.EntitiesConfig
{
    public class SampleModelConfig : IEntityTypeConfiguration<SampleModel>
    {
        public void Configure(EntityTypeBuilder<SampleModel> entity)
        {
            entity.HasKey(e => new { e.SampleModelName });
            entity.Property(e => e.SampleModelName).HasMaxLength(32); 
        }
    }
}