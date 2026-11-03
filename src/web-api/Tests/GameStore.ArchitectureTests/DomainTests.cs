using System.Reflection;
using GameStore.Models.Entities.ConfigurationEntities;
using NetArchTest.Rules;
using Xunit;

namespace GameStore.ArchitectureTests
{
    public class DomainTests
    {
        private static readonly Assembly DomainAssembly = typeof(BaseEntity).Assembly;

        [Fact]
        public void Entities_Should_HavePrivateParameterlessConstructor()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .ResideInNamespace("GameStore.Models.Entities")
                .And()
                .DoNotResideInNamespace("GameStore.Models.Entities.ConfigurationEntities")
                .And()
                .DoNotResideInNamespace("GameStore.Models.Entities.Northwind")
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var entityType in entityTypes)
            {
                var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

                if (!constructors.Any(x => x.IsPrivate && x.GetParameters().Length == 0))
                {
                    failingTypes.Add(entityType);
                }
            }

            Assert.Empty(failingTypes);
        }
    }
}