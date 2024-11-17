using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommuniQueue.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangingProjectDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projects_containers_root_container_id",
                table: "projects");

            migrationBuilder.DropIndex(
                name: "ix_projects_root_container_id",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "root_container_id",
                table: "projects");

            migrationBuilder.CreateTable(
                name: "api_keys",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key_hash = table.Column<string>(type: "text", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_expired = table.Column<bool>(type: "boolean", nullable: false),
                    scopes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_keys", x => x.id);
                    table.ForeignKey(
                        name: "fk_api_keys_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_api_keys_project_id",
                table: "api_keys",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_keys");

            migrationBuilder.AddColumn<Guid>(
                name: "root_container_id",
                table: "projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_projects_root_container_id",
                table: "projects",
                column: "root_container_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_projects_containers_root_container_id",
                table: "projects",
                column: "root_container_id",
                principalTable: "containers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
