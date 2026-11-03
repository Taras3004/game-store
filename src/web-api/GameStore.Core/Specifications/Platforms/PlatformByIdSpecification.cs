namespace GameStore.Core.Specifications.Platforms
{
    internal class PlatformByIdSpecification(Guid platformId) 
        : PlatformSpecificationBase(platform => platform.Id == platformId);
}