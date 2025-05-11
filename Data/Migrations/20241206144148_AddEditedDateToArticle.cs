using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JeddahGreenHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEditedDateToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedDate",
                table: "Article",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedDate",
                table: "Article");
        }
    }
}
