
using MinCleanTemplateManager.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace MinCleanTemplateManager.Infrastructure.Persistence
{
    public static class TrySeedData
    {
        public async static Task SeedMinCleanTemplateManagerTestingDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            {
                try
                {
                  
                    var ctx = scope.ServiceProvider.GetRequiredService<MinCleanTemplateManagerContext>();
                    await ctx.Database.MigrateAsync();
                    {
                  
                        var testData = ctx.SampleModels.Any();
                        if (!testData)
                        {
                            var data = new List<SampleModel>
                            {
                                SampleModel.Create("SampleModelA", Guid.CreateVersion7()),
                                SampleModel.Create("SampleModelB", Guid.CreateVersion7()),
                                SampleModel.Create("SampleModelC", Guid.CreateVersion7()),

                            };
                            ctx.SampleModels.AddRange(data);
                            ctx.SaveChanges();
                        }


                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }



        }

        public static async Task SeedMinCleanTemplateManagerRealDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
          
            {
                try
                {

                    var ctx = scope.ServiceProvider.GetRequiredService<MinCleanTemplateManagerContext>();
                 
                    if (await ctx.Database.EnsureCreatedAsync()) // I want to be intentional about applying the migration in Production
                    {


                        var testData = ctx.SampleModels.Any();
                        if (!testData)
                        {
                            var data = new List<SampleModel>
                            {
                                SampleModel.Create("SampleModelA", Guid.CreateVersion7()),
                                SampleModel.Create("SampleModelB", Guid.CreateVersion7()),
                                SampleModel.Create("SampleModelC", Guid.CreateVersion7()),

                            };
                            ctx.SampleModels.AddRange(data);
                            ctx.SaveChanges();
                        }



                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


    }
}
