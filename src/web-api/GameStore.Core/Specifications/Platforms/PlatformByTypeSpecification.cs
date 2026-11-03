namespace GameStore.Core.Specifications.Platforms
{
    internal class PlatformByTypeSpecification(string type) 
        : PlatformSpecificationBase(platform => platform.Type == type);
}