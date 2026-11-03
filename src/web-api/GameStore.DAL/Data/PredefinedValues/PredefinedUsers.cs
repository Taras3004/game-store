using GameStore.Models.Entities;

namespace GameStore.DAL.Data.PredefinedValues
{
    internal static class PredefinedUsers
    {
        public static readonly Guid UserId = Guid.Parse("A1111111-1111-1111-1111-111111111111");
        public static readonly Guid ModeratorId = Guid.Parse("B2222222-2222-2222-2222-222222222222");
        public static readonly Guid ManagerId = Guid.Parse("C3333333-3333-3333-3333-333333333333");
        public static readonly Guid AdminId = Guid.Parse("D4444444-4444-4444-4444-444444444444");

        // user/User123!, moderator/Moderator123!, manager/Manager123!, admin/Admin123!
        public static readonly User[] Users =
        [
            new("user", "B62501317368866A5EB46EE79742E13BB3AD75B91A-A508F6628C5C00CA01ABB7B08E48DCEE", "user@store.com", UserId),
            new("moderator", "60CF4B0498D466AE6160988948EBC23443EAA25B9E-A08CE9B454D3D7444EFA3B099DDEDC69", "moderator@store.com", ModeratorId),
            new("manager", "2A5928259DD921551CF98AE343CABE73026D9EF5A0-D46AA6AE4FEC3139FCA1EC7D0F901312", "manager@store.com", ManagerId),
            new("admin", "D5D55DA20BFC96C4E144D4AEC09831527999D6BFA2-D21D46178FA59F5849C50C2C613429AC", "admin@store.com", AdminId)
        ];
    }
}
