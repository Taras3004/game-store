using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NorthwindSupportImplemented : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Games_GameId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Games_ProductId",
                table: "OrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_GameId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactTitle",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegacyId",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Freight",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequiredDate",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipCity",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipCountry",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipPostalCode",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipRegion",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippedDate",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductKey",
                table: "OrderProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegacyId",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LegacyId",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuantityPerUnit",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReorderLevel",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitsOnOrder",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductKey",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductKey" });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("03a26411-5cc5-4d33-9ea4-6a37c0ef4b80"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("391e724c-1f17-4404-8ccc-e3c1f3921c42"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("46d251bd-ce71-46f8-a18a-2a7af3677677"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("60ab9513-66ba-4be0-9d24-12022b7b08e0"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("88f158e3-8fe7-4e0f-ba0f-5810819e6824"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("92bde005-3f33-409f-89fe-3e0835acf46a"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("9975044e-ce10-4e53-b016-cb297bb24342"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("aee1de92-b2fc-4c1c-8123-9e650434f66e"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b2fad613-f3bb-4bb1-8934-26bdedcf8e4e"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("bb677f47-21b3-439b-b5a1-beca6676d458"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ee4da31c-68a1-488c-9371-43be311989e9"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("eead926d-2812-447d-b22d-1b32fa2d4867"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f56ae5b6-c13c-442f-8bcc-c21a6c19ec68"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f7b6abd9-8b9d-4bf4-9084-d3d651b59ef7"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f8004a98-1a49-4370-97c6-94589d0a6335"),
                columns: new[] { "Description", "LegacyId", "Picture" },
                values: new object[] { null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductKey",
                table: "OrderProducts",
                column: "ProductKey")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductKey",
                table: "Comments",
                column: "ProductKey")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_ProductKey",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProductKey",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "ContactTitle",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RequiredDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipCity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipCountry",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipPostalCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipRegion",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductKey",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "UnitsOnOrder",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ProductKey",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_GameId",
                table: "Comments",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Games_GameId",
                table: "Comments",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Games_ProductId",
                table: "OrderProducts",
                column: "ProductId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
