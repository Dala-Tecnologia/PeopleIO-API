using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleIO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FotoUrlExperiencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FotoDataUpload",
                table: "candidato",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoTipoMime",
                table: "candidato",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoURL",
                table: "candidato",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeFoto",
                table: "candidato",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "experiencia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeEmpresa = table.Column<string>(type: "text", nullable: false),
                    TipoContrato = table.Column<string>(type: "text", nullable: false),
                    Funcao = table.Column<string>(type: "text", nullable: false),
                    TrabalhandoAtualmente = table.Column<bool>(type: "boolean", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataTermino = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Local = table.Column<string>(type: "text", nullable: false),
                    Atuacao = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    CandidatoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experiencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_experiencia_candidato_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "candidato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_experiencia_CandidatoId",
                table: "experiencia",
                column: "CandidatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "experiencia");

            migrationBuilder.DropColumn(
                name: "FotoDataUpload",
                table: "candidato");

            migrationBuilder.DropColumn(
                name: "FotoTipoMime",
                table: "candidato");

            migrationBuilder.DropColumn(
                name: "FotoURL",
                table: "candidato");

            migrationBuilder.DropColumn(
                name: "NomeFoto",
                table: "candidato");
        }
    }
}
