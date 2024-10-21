using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ultralinks.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_completo = table.Column<string>(type: "text", nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cpf = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    senha = table.Column<string>(type: "text", nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "endereco",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cep = table.Column<string>(type: "text", nullable: false),
                    complemento = table.Column<string>(type: "text", nullable: false),
                    numero = table.Column<string>(type: "text", nullable: false),
                    logradouro = table.Column<string>(type: "text", nullable: false),
                    bairro = table.Column<string>(type: "text", nullable: false),
                    localidade = table.Column<string>(type: "text", nullable: false),
                    uf = table.Column<string>(type: "text", nullable: false),
                    ibge = table.Column<string>(type: "text", nullable: false),
                    gia = table.Column<string>(type: "text", nullable: false),
                    ddd = table.Column<string>(type: "text", nullable: false),
                    siafi = table.Column<string>(type: "text", nullable: false),
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endereco", x => x.id);
                    table.ForeignKey(
                        name: "FK_endereco_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_id_origem = table.Column<int>(type: "integer", nullable: false),
                    usuario_id_destino = table.Column<int>(type: "integer", nullable: false),
                    valor = table.Column<decimal>(type: "numeric", nullable: false),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "text", nullable: false),
                    codigo_autorizacao = table.Column<string>(type: "text", nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_transacao_usuario_usuario_id_destino",
                        column: x => x.usuario_id_destino,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transacao_usuario_usuario_id_origem",
                        column: x => x.usuario_id_origem,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_endereco_usuario_id",
                table: "endereco",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transacao_usuario_id_destino",
                table: "transacao",
                column: "usuario_id_destino");

            migrationBuilder.CreateIndex(
                name: "IX_transacao_usuario_id_origem",
                table: "transacao",
                column: "usuario_id_origem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "endereco");

            migrationBuilder.DropTable(
                name: "transacao");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
