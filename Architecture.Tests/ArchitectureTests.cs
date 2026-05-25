using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Common.Base;
using NetArchTest.Rules;
using Xunit;

namespace Architecture.Tests
{
    public class ArchitectureTests
    {
        private static readonly Assembly DomainAssembly = typeof(Domain.AssemblyReference).Assembly;
        private static readonly Assembly ApplicationAssembly = typeof(Application.AssemblyReference).Assembly;
        private static readonly Assembly InfraAssembly = typeof(Infra.AssemblyReference).Assembly;
        private static readonly Assembly ApiAssembly = typeof(API.AssemblyReference).Assembly;

        [Fact]
        public void Dominio_Nao_Deve_Depender_De_Aplicacao_Infra_Api()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOnAny("Application", "Infra", "API")
                .GetResult();

            Assert.True(result.IsSuccessful, "O Domínio não deve conhecer as camadas de Application, Infra ou API.");
        }

        [Fact]
        public void Aplicacao_Nao_Deve_Depender_De_Infra()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOnAny("Infra")
                .GetResult();

            Assert.True(result.IsSuccessful, "A Aplicação deve depender apenas de abstrações, não da Infraestrutura concreta.");
        }

        [Fact]
        public void Aplicacao_Nao_Deve_Depender_De_Api()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOnAny("API")
                .GetResult();

            Assert.True(result.IsSuccessful, "A camada de Aplicação não pode referenciar a API de entrada.");
        }

        [Fact]
        public void Entidades_Nao_Devem_Ter_Construtores_Publicos_Sem_Parametros()
        {
            var entityTypes = DomainAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Entity)));

            var failingTypes = new List<string>();

            foreach (var type in entityTypes)
            {
                var hasPublicParameterlessConstructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .Any(c => c.GetParameters().Length == 0);

                if (hasPublicParameterlessConstructor)
                {
                    failingTypes.Add(type.Name);
                }
            }

            Assert.True(failingTypes.Count == 0,
                $"As seguintes entidades não deveriam possuir construtores públicos sem parâmetros: {string.Join(", ", failingTypes)}");
        }

        [Fact]
        public void Commands_Devem_Terminar_Com_Sufixo_Command()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ResideInNamespaceContaining("Application.Commands")
                .And()
                .AreClasses()
                .Should()
                .HaveNameEndingWith("Command")
                .Or()
                .HaveNameEndingWith("CommandHandler")
                .Or()
                .HaveNameEndingWith("Dto")
                .GetResult();

            Assert.True(result.IsSuccessful, "Classes na pasta 'Commands' devem terminar com 'Command', 'CommandHandler' ou 'Dto'.");
        }

        [Fact]
        public void Abstracoes_Devem_Ser_Interfaces_E_Comecar_Com_I()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ResideInNamespaceContaining("Application.Abstractions.Persistence")
                .Should()
                .BeInterfaces()
                .And()
                .HaveNameStartingWith("I")
                .GetResult();

            Assert.True(result.IsSuccessful, "A pasta 'Abstractions' deve conter apenas interfaces iniciadas com a letra 'I'.");
        }

        [Fact]
        public void Infra_Nao_Deve_Depender_De_Api()
        {
            var result = Types.InAssembly(InfraAssembly)
                .Should()
                .NotHaveDependencyOnAny("API")
                .GetResult();

            Assert.True(result.IsSuccessful, "A camada de Infraestrutura não deve possuir dependências da API.");
        }

        [Fact]
        public void Repositorios_Devem_Terminar_Com_Sufixo_Repository()
        {
            var result = Types.InAssembly(InfraAssembly)
                .That()
                .ResideInNamespaceContaining("Infra.Repositories")
                .And()
                .AreClasses()
                .Should()
                .HaveNameEndingWith("Repository")
                .GetResult();

            Assert.True(result.IsSuccessful, "Classes dentro da pasta 'Repositories' devem terminar com o sufixo 'Repository'.");
        }

        [Fact]
        public void Repositorios_Devem_Implementar_Pelo_Menos_Uma_Interface()
        {
            var repositoryTypes = InfraAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null && t.Namespace.Contains("Infra.Repositories"));

            var failingTypes = new List<string>();

            foreach (var type in repositoryTypes)
            {
                if (type.GetInterfaces().Length == 0)
                {
                    failingTypes.Add(type.Name);
                }
            }

            Assert.True(failingTypes.Count == 0,
                $"Os seguintes repositórios na Infraestrutura não implementam nenhuma interface: {string.Join(", ", failingTypes)}");
        }

        [Fact]
        public void Controllers_Devem_Terminar_Com_Sufixo_Controller()
        {
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("API.Controllers")
                .And()
                .AreClasses()
                .Should()
                .HaveNameEndingWith("Controller")
                .GetResult();

            Assert.True(result.IsSuccessful, "Existem classes na pasta Controllers que não terminam com o sufixo 'Controller'.");
        }

        [Fact]
        public void Controllers_Devem_Residir_No_Namespace_API_Controllers()
        {
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .AreClasses()
                .And()
                .HaveNameEndingWith("Controller")
                .Should()
                .ResideInNamespace("API.Controllers")
                .GetResult();

            Assert.True(result.IsSuccessful, "Classes que terminam com 'Controller' devem residir em 'API.Controllers'.");
        }
    }
}