using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ArchitectureRefactored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts");

            migrationBuilder.AddColumn<string>(
                name: "LegacyId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[,]
                {
                    { new Guid("335c05e3-95e7-42c8-aa41-6907cd7ce794"), "Manager", "[\"AddComment\",\"DeleteOwnComment\",\"CreateOrder\",\"ViewCart\",\"UpdateCart\",\"DeleteAnyComment\",\"BanUserFromCommenting\",\"AddGame\",\"UpdateGame\",\"DeleteGame\",\"ViewOutOfStockGames\",\"ViewDeletedGame\",\"AddGenre\",\"UpdateGenre\",\"DeleteGenre\",\"AddPlatform\",\"UpdatePlatform\",\"DeletePlatform\",\"AddPublisher\",\"UpdatePublisher\",\"DeletePublisher\",\"UpdateOrder\",\"DeleteOrder\",\"ViewOrderHistory\",\"ShipOrder\"]" },
                    { new Guid("86be52da-2e33-45c1-b022-d481403cf9ef"), "Admin", "[\"AddComment\",\"DeleteOwnComment\",\"CreateOrder\",\"ViewCart\",\"UpdateCart\",\"DeleteAnyComment\",\"BanUserFromCommenting\",\"AddGame\",\"UpdateGame\",\"DeleteGame\",\"ViewOutOfStockGames\",\"ViewDeletedGame\",\"AddGenre\",\"UpdateGenre\",\"DeleteGenre\",\"AddPlatform\",\"UpdatePlatform\",\"DeletePlatform\",\"AddPublisher\",\"UpdatePublisher\",\"DeletePublisher\",\"UpdateOrder\",\"DeleteOrder\",\"ViewOrderHistory\",\"ShipOrder\",\"UpdateDeletedGame\",\"ViewDeletedComment\",\"UpdateOrderHistory\",\"DeleteOrderHistory\",\"AddUser\",\"ViewUser\",\"UpdateUser\",\"DeleteUser\",\"AddRole\",\"ViewRole\",\"UpdateRole\",\"DeleteRole\"]" },
                    { new Guid("af9f7438-55e4-477f-991f-ccaa097c9e3e"), "User", "[\"AddComment\",\"DeleteOwnComment\",\"CreateOrder\",\"ViewCart\",\"UpdateCart\"]" },
                    { new Guid("d0cb22dd-5fb7-4b6f-be61-cc738843a0a4"), "Moderator", "[\"AddComment\",\"DeleteOwnComment\",\"CreateOrder\",\"ViewCart\",\"UpdateCart\",\"DeleteAnyComment\",\"BanUserFromCommenting\"]" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "user@store.com", "B62501317368866A5EB46EE79742E13BB3AD75B91A-A508F6628C5C00CA01ABB7B08E48DCEE", "user" },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "moderator@store.com", "60CF4B0498D466AE6160988948EBC23443EAA25B9E-A08CE9B454D3D7444EFA3B099DDEDC69", "moderator" },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), "manager@store.com", "2A5928259DD921551CF98AE343CABE73026D9EF5A0-D46AA6AE4FEC3139FCA1EC7D0F901312", "manager" },
                    { new Guid("d4444444-4444-4444-4444-444444444444"), "admin@store.com", "D5D55DA20BFC96C4E144D4AEC09831527999D6BFA2-D21D46178FA59F5849C50C2C613429AC", "admin" }
                });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesId", "UserId" },
                values: new object[,]
                {
                    { new Guid("335c05e3-95e7-42c8-aa41-6907cd7ce794"), new Guid("c3333333-3333-3333-3333-333333333333") },
                    { new Guid("86be52da-2e33-45c1-b022-d481403cf9ef"), new Guid("d4444444-4444-4444-4444-444444444444") },
                    { new Guid("af9f7438-55e4-477f-991f-ccaa097c9e3e"), new Guid("a1111111-1111-1111-1111-111111111111") },
                    { new Guid("d0cb22dd-5fb7-4b6f-be61-cc738843a0a4"), new Guid("b2222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId_ProductKey",
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserId",
                table: "RoleUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CustomerId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OrderId_ProductKey",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductKey" });
        }
    }
}
