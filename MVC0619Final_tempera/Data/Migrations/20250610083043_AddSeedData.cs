using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVC0619Final_tempera.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductTable",
                columns: new[] { "ProductID", "ProductCategory", "ProductCompany", "ProductDate", "ProductName", "ProductPrice" },
                values: new object[,]
                {
                    { 1, "電子產品", "ABC 電子", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "筆記型電腦", 25000 },
                    { 2, "電腦周邊", "XYZ 科技", new DateTime(2023, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "無線滑鼠", 800 },
                    { 3, "電腦周邊", "Gaming Gear", new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "機械鍵盤", 3500 },
                    { 4, "電子產品", "Global Mobile", new DateTime(2023, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "智慧型手機", 18000 },
                    { 5, "音訊設備", "SoundWave", new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "藍牙耳機", 1200 },
                    { 6, "電子產品", "Visual Tech", new DateTime(2023, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "LED 顯示器", 9900 },
                    { 7, "電腦周邊", "NetCam Pro", new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "網路攝影機", 600 },
                    { 8, "電子配件", "PowerUp Inc.", new DateTime(2023, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "行動電源", 750 },
                    { 9, "網路設備", "ConnectFast", new DateTime(2023, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "路由器", 2000 },
                    { 10, "穿戴裝置", "Wearable Innovations", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "智慧手錶", 5000 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductTable",
                keyColumn: "ProductID",
                keyValue: 10);
        }
    }
}
