using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Monitor.Infra.Migrations
{
    public partial class Initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    MonitorPeriod = table.Column<int>(nullable: false),
                    IsMonitorActivated = table.Column<bool>(nullable: false),
                    MonitorActivationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourcesHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResourceId = table.Column<string>(nullable: true),
                    RequestDate = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    ResourceId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcesHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourcesHistory_Resources_ResourceId1",
                        column: x => x.ResourceId1,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourcesHistory_ResourceId1",
                table: "ResourcesHistory",
                column: "ResourceId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourcesHistory");

            migrationBuilder.DropTable(
                name: "UserAction");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
