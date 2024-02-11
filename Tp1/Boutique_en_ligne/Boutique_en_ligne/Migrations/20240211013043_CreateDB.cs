using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boutique_en_ligne.Migrations
{
    /// <inheritdoc />
    public partial class CreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JeuVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    annee_sortie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    console = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    editeur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pochette_jeu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    capture_ecran = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prix_vente = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JeuVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    adresse_courriel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_naissance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ville = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mot_de_passe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    profil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    solde = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarteCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    detenteur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    numero_secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarteCredits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarteCredits_Utilisateurs_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_article = table.Column<int>(type: "int", nullable: true),
                    montant_depense = table.Column<float>(type: "real", nullable: true),
                    nombre_facture = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factures_Utilisateurs_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JeuVideoUtilisateur",
                columns: table => new
                {
                    JeuxVideosId = table.Column<int>(type: "int", nullable: false),
                    UtilisateursId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JeuVideoUtilisateur", x => new { x.JeuxVideosId, x.UtilisateursId });
                    table.ForeignKey(
                        name: "FK_JeuVideoUtilisateur_JeuVideos_JeuxVideosId",
                        column: x => x.JeuxVideosId,
                        principalTable: "JeuVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JeuVideoUtilisateur_Utilisateurs_UtilisateursId",
                        column: x => x.UtilisateursId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarteCredits_ClientId",
                table: "CarteCredits",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_ClientId",
                table: "Factures",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_JeuVideoUtilisateur_UtilisateursId",
                table: "JeuVideoUtilisateur",
                column: "UtilisateursId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarteCredits");

            migrationBuilder.DropTable(
                name: "Factures");

            migrationBuilder.DropTable(
                name: "JeuVideoUtilisateur");

            migrationBuilder.DropTable(
                name: "JeuVideos");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
