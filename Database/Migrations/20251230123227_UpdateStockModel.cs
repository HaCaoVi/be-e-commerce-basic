using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce_basic.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStockModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "Galleries");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Galleries",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
