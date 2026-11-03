namespace GameStore.Core.Specifications.Platforms
{
    internal class PlatformByGameKeySpecification(string gameKey)
        : PlatformSpecificationBase(platform => platform.GamePlatforms.Any(x => x.Game.Key == gameKey));
}
