using System.Reflection;
using GameStore.Core.Services;
using NetArchTest.Rules;
using Xunit;

namespace GameStore.ArchitectureTests.Core
{
    public class CoreTests
    {
        private static readonly Assembly CoreAssembly = typeof(GameService).Assembly;

        [Fact]
        public void Dto_Should_BeRecord()
        {
            var result = Types.InAssembly(CoreAssembly)
                .That()
                .ResideInNamespace("GameStore.Core.Dto")
                .Should()
                .MeetCustomRule(new BeRecordClassRule())
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Validation_Should_BePublic()
        {
            var result = Types.InAssembly(CoreAssembly)
                .That()
                .ResideInNamespace("GameStore.Core.Validation")
                .And()
                .DoNotResideInNamespace("GameStore.Core.Validation.Helpers")
                .Should()
                .BePublic()
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Services_Should_BeInternal()
        {
            var result = Types.InAssembly(CoreAssembly)
                .That()
                .ResideInNamespace("GameStore.Core.Services")
                .Should()
                .NotBePublic()
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Specifications_Should_BeInternal()
        {
            var result = Types.InAssembly(CoreAssembly)
                .That()
                .ResideInNamespace("GameStore.Core.Specifications")
                .And()
                .DoNotResideInNamespace("GameStore.Core.Specifications.BaseClass")
                .Should()
                .NotBePublic()
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void SpecificationsBase_Should_BeAbstract()
        {
            var result = Types.InAssembly(CoreAssembly)
                .That()
                .ResideInNamespace("GameStore.Core.Specifications")
                .And()
                .DoNotResideInNamespace("GameStore.Core.Specifications.BaseClass")
                .And()
                .HaveNameEndingWith("Base")
                .Should()
                .BeAbstract()
                .GetResult();

            Assert.True(result.IsSuccessful);
        }
    }
}