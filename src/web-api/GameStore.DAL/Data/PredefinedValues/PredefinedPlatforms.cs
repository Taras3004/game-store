using GameStore.Models.Entities;

namespace GameStore.DAL.Data.PredefinedValues
{
    internal static class PredefinedGenres
    {
        public static readonly Genre[] Genres =
        [
            new("Strategy", id: Ids.StrategyId),
            new("RPG", id: Ids.RpgId),
            new("Sports", id: Ids.SportsId),
            new("Races", id: Ids.RacesId),
            new("Adventure", id: Ids.AdventureId),
            new("Puzzle & Skill", id: Ids.PuzzleSkillId),
            new("Arcade", id: Ids.ArcadeId),
            new("Action", id: Ids.ActionId),
            new("Formula", Ids.RacesId, id: Ids.FormulaId),
            new("Off-road", Ids.RacesId, id: Ids.OffRoadId),
            new("Rally", Ids.RacesId, id: Ids.RallyId),
            new("FPS", Ids.ActionId, id: Ids.FpsId),
            new("TPS", Ids.ActionId, id: Ids.TpsId),
            new("RTS", Ids.StrategyId, id: Ids.RtsId),
            new("TBS", Ids.StrategyId, id: Ids.TbsId),
        ];

        private static class Ids
        {
            public static readonly Guid StrategyId = Guid.Parse("B2FAD613-F3BB-4BB1-8934-26BDEDCF8E4E");
            public static readonly Guid RtsId = Guid.Parse("F56AE5B6-C13C-442F-8BCC-C21A6C19EC68");
            public static readonly Guid TbsId = Guid.Parse("F7B6ABD9-8B9D-4BF4-9084-D3D651B59EF7");
            public static readonly Guid RpgId = Guid.Parse("03A26411-5CC5-4D33-9EA4-6A37C0EF4B80");
            public static readonly Guid SportsId = Guid.Parse("BB677F47-21B3-439B-B5A1-BECA6676D458");
            public static readonly Guid RacesId = Guid.Parse("9975044E-CE10-4E53-B016-CB297BB24342");
            public static readonly Guid RallyId = Guid.Parse("92BDE005-3F33-409F-89FE-3E0835ACF46A");
            public static readonly Guid ArcadeId = Guid.Parse("EEAD926D-2812-447D-B22D-1B32FA2D4867");
            public static readonly Guid FormulaId = Guid.Parse("391E724C-1F17-4404-8CCC-E3C1F3921C42");
            public static readonly Guid OffRoadId = Guid.Parse("46D251BD-CE71-46F8-A18A-2A7AF3677677");
            public static readonly Guid ActionId = Guid.Parse("60AB9513-66BA-4BE0-9D24-12022B7B08E0");
            public static readonly Guid FpsId = Guid.Parse("F8004A98-1A49-4370-97C6-94589D0A6335");
            public static readonly Guid TpsId = Guid.Parse("88F158E3-8FE7-4E0F-BA0F-5810819E6824");
            public static readonly Guid AdventureId = Guid.Parse("EE4DA31C-68A1-488C-9371-43BE311989E9");
            public static readonly Guid PuzzleSkillId = Guid.Parse("AEE1DE92-B2FC-4C1C-8123-9E650434F66E");
        }
    }
}
