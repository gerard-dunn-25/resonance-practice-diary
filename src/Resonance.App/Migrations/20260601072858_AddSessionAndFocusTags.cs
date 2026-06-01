using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resonance.App.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionAndFocusTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInstruments_AspNetUsers_UserId",
                table: "UserInstruments");

            migrationBuilder.CreateTable(
                name: "FocusTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocusTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FocusTags_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserInstrumentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Reflection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoodTagId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sessions_MoodTags_MoodTagId",
                        column: x => x.MoodTagId,
                        principalTable: "MoodTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sessions_UserInstruments_UserInstrumentId",
                        column: x => x.UserInstrumentId,
                        principalTable: "UserInstruments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SessionTags",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    FocusTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTags", x => new { x.SessionId, x.FocusTagId });
                    table.ForeignKey(
                        name: "FK_SessionTags_FocusTags_FocusTagId",
                        column: x => x.FocusTagId,
                        principalTable: "FocusTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionTags_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FocusTags_UserId",
                table: "FocusTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_MoodTagId",
                table: "Sessions",
                column: "MoodTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserInstrumentId",
                table: "Sessions",
                column: "UserInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionTags_FocusTagId",
                table: "SessionTags",
                column: "FocusTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInstruments_AspNetUsers_UserId",
                table: "UserInstruments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInstruments_AspNetUsers_UserId",
                table: "UserInstruments");

            migrationBuilder.DropTable(
                name: "SessionTags");

            migrationBuilder.DropTable(
                name: "FocusTags");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInstruments_AspNetUsers_UserId",
                table: "UserInstruments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
