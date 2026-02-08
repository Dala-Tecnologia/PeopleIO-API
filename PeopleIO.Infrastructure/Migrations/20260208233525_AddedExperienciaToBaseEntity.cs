using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleIO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedExperienciaToBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "experiencia",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "experiencia",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "experiencia",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "experiencia",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "experiencia");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "experiencia");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "experiencia");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "experiencia");
        }
    }
}
