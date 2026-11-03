namespace GameStore.Core.Specifications.Platforms
{
    internal class PlatformInRangeSpecification(IReadOnlyCollection<Guid> range)
        : PlatformSpecificationBase(platform => range.Contains(platform.Id));
}