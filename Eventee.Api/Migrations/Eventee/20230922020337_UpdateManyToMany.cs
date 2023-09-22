using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventee.Api.Migrations.Eventee
{
    public partial class UpdateManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetTogethers_Users_HosterId",
                table: "GetTogethers");

            migrationBuilder.AddForeignKey(
                name: "FK_GetTogethers_Users_HosterId",
                table: "GetTogethers",
                column: "HosterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetTogethers_Users_HosterId",
                table: "GetTogethers");

            migrationBuilder.AddForeignKey(
                name: "FK_GetTogethers_Users_HosterId",
                table: "GetTogethers",
                column: "HosterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
