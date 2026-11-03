using GameStore.Models.Entities;

namespace GameStore.DAL.Data.PredefinedValues
{
    internal static class PredefinedPlatforms
    {
        public static readonly Platform[] Platforms =
        [
            new("Mobile", Ids.MobileId),
            new("Browser", Ids.BrowserId),
            new("Desktop", Ids.DesktopId),
            new("Console", Ids.ConsoleId),
        ];

        private static class Ids
        {
            public static readonly Guid MobileId = Guid.Parse("5AFE59E6-50CB-43F1-90CD-F6BF3F04AC3E");
            public static readonly Guid BrowserId = Guid.Parse("075297B1-4919-468A-A735-066E90293B1E");
            public static readonly Guid DesktopId = Guid.Parse("7D55D6A2-204C-40C8-88E0-7E84D10F9289");
            public static readonly Guid ConsoleId = Guid.Parse("B23D6C95-DBF7-4844-88E0-7FDF3347A1F0");
        }
    }
}
