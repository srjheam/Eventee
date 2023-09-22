using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventee.Api.Migrations.Eventee
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GetTogethers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HosterId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetTogethers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GetTogethers_Users_HosterId",
                        column: x => x.HosterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetTogetherUser",
                columns: table => new
                {
                    SubscribedGetTogethersId = table.Column<int>(type: "int", nullable: false),
                    SubscribersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetTogetherUser", x => new { x.SubscribedGetTogethersId, x.SubscribersId });
                    table.ForeignKey(
                        name: "FK_GetTogetherUser_GetTogethers_SubscribedGetTogethersId",
                        column: x => x.SubscribedGetTogethersId,
                        principalTable: "GetTogethers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GetTogetherUser_Users_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetTogethers_HosterId",
                table: "GetTogethers",
                column: "HosterId");

            migrationBuilder.CreateIndex(
                name: "IX_GetTogetherUser_SubscribersId",
                table: "GetTogetherUser",
                column: "SubscribersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetTogetherUser");

            migrationBuilder.DropTable(
                name: "GetTogethers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
