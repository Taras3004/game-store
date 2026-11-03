using Mono.Cecil;
using NetArchTest.Rules;

namespace GameStore.ArchitectureTests.Core
{
    public class BeRecordClassRule : ICustomRule
    {
        public bool MeetsRule(TypeDefinition type)
        {
            return type.Properties.Any(x => x.Name == "EqualityContract");
        }
    }
}