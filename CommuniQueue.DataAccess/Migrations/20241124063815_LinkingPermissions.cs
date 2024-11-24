using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommuniQueue.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LinkingPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "project_id",
                table: "permissions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_permissions_project_id",
                table: "permissions",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "fk_permissions_projects_project_id",
                table: "permissions",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_permissions_projects_project_id",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "ix_permissions_project_id",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "permissions");
        }
    }
}
