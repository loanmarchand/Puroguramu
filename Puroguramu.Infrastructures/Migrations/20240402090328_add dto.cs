using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puroguramu.Infrastructures.Migrations
{
    public partial class adddto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lecons",
                columns: table => new
                {
                    IdLecons = table.Column<string>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    estVisible = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecons", x => x.IdLecons);
                });

            migrationBuilder.CreateTable(
                name: "Exercices",
                columns: table => new
                {
                    IdExercice = table.Column<string>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", nullable: false),
                    Enonce = table.Column<string>(type: "TEXT", nullable: false),
                    Modele = table.Column<string>(type: "TEXT", nullable: false),
                    Solution = table.Column<string>(type: "TEXT", nullable: false),
                    EstVisible = table.Column<bool>(type: "INTEGER", nullable: false),
                    Difficulte = table.Column<string>(type: "TEXT", nullable: false),
                    LeconsIdLecons = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercices", x => x.IdExercice);
                    table.ForeignKey(
                        name: "FK_Exercices_Lecons_LeconsIdLecons",
                        column: x => x.LeconsIdLecons,
                        principalTable: "Lecons",
                        principalColumn: "IdLecons");
                });

            migrationBuilder.CreateTable(
                name: "PositionLecons",
                columns: table => new
                {
                    IdPositionLecons = table.Column<string>(type: "TEXT", nullable: false),
                    LeconsIdLecons = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionLecons", x => x.IdPositionLecons);
                    table.ForeignKey(
                        name: "FK_PositionLecons_Lecons_LeconsIdLecons",
                        column: x => x.LeconsIdLecons,
                        principalTable: "Lecons",
                        principalColumn: "IdLecons");
                });

            migrationBuilder.CreateTable(
                name: "PositionExercices",
                columns: table => new
                {
                    IdPositionExercices = table.Column<string>(type: "TEXT", nullable: false),
                    ExercicesIdExercice = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionExercices", x => x.IdPositionExercices);
                    table.ForeignKey(
                        name: "FK_PositionExercices_Exercices_ExercicesIdExercice",
                        column: x => x.ExercicesIdExercice,
                        principalTable: "Exercices",
                        principalColumn: "IdExercice");
                });

            migrationBuilder.CreateTable(
                name: "StatutExercices",
                columns: table => new
                {
                    IdStatutExercice = table.Column<string>(type: "TEXT", nullable: false),
                    ExerciceIdExercice = table.Column<string>(type: "TEXT", nullable: true),
                    EtudiantId = table.Column<string>(type: "TEXT", nullable: true),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false),
                    SolutionTempo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatutExercices", x => x.IdStatutExercice);
                    table.ForeignKey(
                        name: "FK_StatutExercices_AspNetUsers_EtudiantId",
                        column: x => x.EtudiantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StatutExercices_Exercices_ExerciceIdExercice",
                        column: x => x.ExerciceIdExercice,
                        principalTable: "Exercices",
                        principalColumn: "IdExercice");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Matricule",
                table: "AspNetUsers",
                column: "Matricule",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercices_LeconsIdLecons",
                table: "Exercices",
                column: "LeconsIdLecons");

            migrationBuilder.CreateIndex(
                name: "IX_Lecons_Titre",
                table: "Lecons",
                column: "Titre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PositionExercices_ExercicesIdExercice",
                table: "PositionExercices",
                column: "ExercicesIdExercice");

            migrationBuilder.CreateIndex(
                name: "IX_PositionLecons_LeconsIdLecons",
                table: "PositionLecons",
                column: "LeconsIdLecons");

            migrationBuilder.CreateIndex(
                name: "IX_StatutExercices_EtudiantId",
                table: "StatutExercices",
                column: "EtudiantId");

            migrationBuilder.CreateIndex(
                name: "IX_StatutExercices_ExerciceIdExercice",
                table: "StatutExercices",
                column: "ExerciceIdExercice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PositionExercices");

            migrationBuilder.DropTable(
                name: "PositionLecons");

            migrationBuilder.DropTable(
                name: "StatutExercices");

            migrationBuilder.DropTable(
                name: "Exercices");

            migrationBuilder.DropTable(
                name: "Lecons");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Matricule",
                table: "AspNetUsers");
        }
    }
}
