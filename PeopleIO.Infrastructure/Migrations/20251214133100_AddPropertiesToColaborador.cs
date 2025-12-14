using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleIO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToColaborador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArquivoCurriculoDataUpload",
                table: "colaborador",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArquivoCurriculoTipoMime",
                table: "colaborador",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArquivoCurriculoURL",
                table: "colaborador",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeArquivoCurriculo",
                table: "colaborador",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeSocial",
                table: "colaborador",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArquivoCurriculoDataUpload",
                table: "colaborador");

            migrationBuilder.DropColumn(
                name: "ArquivoCurriculoTipoMime",
                table: "colaborador");

            migrationBuilder.DropColumn(
                name: "ArquivoCurriculoURL",
                table: "colaborador");

            migrationBuilder.DropColumn(
                name: "NomeArquivoCurriculo",
                table: "colaborador");

            migrationBuilder.DropColumn(
                name: "NomeSocial",
                table: "colaborador");
        }
    }
}
