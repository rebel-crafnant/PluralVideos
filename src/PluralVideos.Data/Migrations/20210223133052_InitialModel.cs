using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PluralVideos.Data.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    ReleaseDate = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedDate = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DurationInMilliseconds = table.Column<int>(type: "INTEGER", nullable: false),
                    HasTranscript = table.Column<int>(type: "INTEGER", nullable: false),
                    AuthorsFullnames = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsStale = table.Column<int>(type: "INTEGER", nullable: true),
                    CachedOn = table.Column<string>(type: "TEXT", nullable: true),
                    UrlSlug = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Jwt = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    JwtExpiration = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UserHandle = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Jwt);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    AuthorHandle = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DurationInMilliseconds = table.Column<int>(type: "INTEGER", nullable: false),
                    ModuleIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Module_Course_CourseName",
                        column: x => x.CourseName,
                        principalTable: "Course",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    ClipIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationInMilliseconds = table.Column<int>(type: "INTEGER", nullable: false),
                    SupportsStandard = table.Column<int>(type: "INTEGER", nullable: false),
                    SupportsWidescreen = table.Column<int>(type: "INTEGER", nullable: false),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clip_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClipTranscript",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<int>(type: "INTEGER", nullable: false),
                    EndTime = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    ClipId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClipTranscript", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClipTranscript_Clip_ClipId",
                        column: x => x.ClipId,
                        principalTable: "Clip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clip_ModuleId",
                table: "Clip",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "index_ClipTranscriptStart",
                table: "ClipTranscript",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ClipTranscript_ClipId",
                table: "ClipTranscript",
                column: "ClipId");

            migrationBuilder.CreateIndex(
                name: "IX_Module_CourseName",
                table: "Module",
                column: "CourseName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClipTranscript");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Clip");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "Course");
        }
    }
}
