using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            var infraAssembly = typeof(DependencyInjection).Assembly;

            var applicationAssembly = typeof(Application.Abstractions.Persistence.IPedidoRepository).Assembly;

            var implementacoes = infraAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"));

            var interfaces = applicationAssembly.GetTypes()
                .Where(t => t.IsInterface && t.Name.StartsWith("I") && t.Name.EndsWith("Repository"))
                .ToList();

            foreach (var tipoImplementacao in implementacoes)
            {
                var interfaceCorrespondente = interfaces
                    .FirstOrDefault(i => i.Name == $"I{tipoImplementacao.Name}");

                if (interfaceCorrespondente != null)
                {
                    services.AddScoped(interfaceCorrespondente, tipoImplementacao);
                }
            }
        }
    }
}
