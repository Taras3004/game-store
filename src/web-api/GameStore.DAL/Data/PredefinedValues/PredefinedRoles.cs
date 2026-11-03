using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.DAL.Data.PredefinedValues
{
    internal static class PredefinedRoles
    {
        public static readonly Role[] Roles =
        [
            new ("User", Data.UserPermissions, Data.UserId),
            new ("Moderator", Data.ModeratorPermissions, Data.ModeratorId),
            new ("Manager", Data.ManagerPermissions, Data.ManagerId),
            new ("Admin", Data.AdminPermissions, Data.AdminId)
        ];

        internal struct Data
        {
            public static readonly Guid UserId = Guid.Parse("AF9F7438-55E4-477F-991F-CCAA097C9E3E");
            public static readonly Guid ModeratorId = Guid.Parse("D0CB22DD-5FB7-4B6F-BE61-CC738843A0A4");
            public static readonly Guid ManagerId = Guid.Parse("335C05E3-95E7-42C8-AA41-6907CD7CE794");
            public static readonly Guid AdminId = Guid.Parse("86BE52DA-2E33-45C1-B022-D481403CF9EF");

            public static readonly List<Permission> UserPermissions =
            [
                Permission.AddComment,
                Permission.DeleteOwnComment,
                Permission.CreateOrder,
                Permission.ViewCart,
                Permission.UpdateCart
            ];

            public static readonly List<Permission> ModeratorPermissions =
            [
                ..UserPermissions,
                Permission.DeleteAnyComment,
                Permission.BanUserFromCommenting
            ];

            public static readonly List<Permission> ManagerPermissions =
            [
                ..ModeratorPermissions,
                Permission.AddGame,
                Permission.UpdateGame,
                Permission.DeleteGame,
                Permission.ViewOutOfStockGames,
                Permission.ViewDeletedGame,
                Permission.AddGenre,
                Permission.UpdateGenre,
                Permission.DeleteGenre,
                Permission.AddPlatform,
                Permission.UpdatePlatform,
                Permission.DeletePlatform,
                Permission.AddPublisher,
                Permission.UpdatePublisher,
                Permission.DeletePublisher,
                Permission.UpdateOrder,
                Permission.DeleteOrder,
                Permission.ViewOrderHistory,
                Permission.ShipOrder
            ];

            public static readonly List<Permission> AdminPermissions =
            [
                ..ManagerPermissions,
                Permission.UpdateDeletedGame,
                Permission.ViewDeletedComment,
                Permission.UpdateOrderHistory,
                Permission.DeleteOrderHistory,
                Permission.AddUser,
                Permission.ViewUser,
                Permission.UpdateUser,
                Permission.DeleteUser,
                Permission.AddRole,
                Permission.ViewRole,
                Permission.UpdateRole,
                Permission.DeleteRole
            ];
        }
    }
}