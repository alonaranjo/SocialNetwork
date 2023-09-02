using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderDeleted",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Messages",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "SenderName",
                table: "Messages",
                newName: "SenderUserName");

            migrationBuilder.RenameColumn(
                name: "RecipientName",
                table: "Messages",
                newName: "RecipientUserName");

            migrationBuilder.AddColumn<DateTime>(
                name: "MessageSent",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageSent",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Messages",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "SenderUserName",
                table: "Messages",
                newName: "SenderName");

            migrationBuilder.RenameColumn(
                name: "RecipientUserName",
                table: "Messages",
                newName: "RecipientName");

            migrationBuilder.AddColumn<bool>(
                name: "RecipientDeleted",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SenderDeleted",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
