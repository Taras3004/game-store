namespace GameStore.Models.Enums
{
    public enum Permission
    {
        AddGame,
        ViewDeletedGame,
        UpdateGame,
        UpdateDeletedGame,
        DeleteGame,
        ViewOutOfStockGames,
        // ---
        AddGenre,
        UpdateGenre,
        DeleteGenre,
        // ---
        AddPlatform,
        UpdatePlatform,
        DeletePlatform,
        // ---
        AddPublisher,
        UpdatePublisher,
        DeletePublisher,
        // ---
        AddComment,
        ViewDeletedComment,
        DeleteOwnComment,
        DeleteAnyComment,
        BanUserFromCommenting,
        // ---
        CreateOrder,
        ViewOrder,
        UpdateOrder,
        DeleteOrder,
        ViewOrderHistory,
        UpdateOrderHistory,
        DeleteOrderHistory,
        ShipOrder,
        ViewCart,
        UpdateCart,
        // ---
        AddUser,
        ViewUser,
        UpdateUser,
        DeleteUser,
        // ---
        AddRole,
        ViewRole,
        UpdateRole,
        DeleteRole,
    }
}
