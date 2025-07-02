using Microsoft.Extensions.DependencyInjection;

using System.Reflection;


namespace RepositoryHelper
{
    public static class RepositoryRegistration
    {
        public static void RegisterRepositories(IServiceCollection services, Assembly assembly)
        {
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToList();

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceType = repositoryType.GetInterfaces().FirstOrDefault(i => i.Name == "I" + repositoryType.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repositoryType);
                }
            }
        }
    }
}
