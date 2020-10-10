using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace monitor_infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunicationChanels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NotifyByEmail = table.Column<bool>(nullable: false),
                    NotifyBySlack = table.Column<bool>(nullable: false),
                    SlackChanel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationChanels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(maxLength: 30, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(maxLength: 1000, nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CommunicationChanelId = table.Column<Guid>(nullable: true),
                    MonitoringActivated = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_CommunicationChanels_CommunicationChanelId",
                        column: x => x.CommunicationChanelId,
                        principalTable: "CommunicationChanels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Action = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ResourceId = table.Column<Guid>(nullable: false),
                    Period = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ActivationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitorItems_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ResourceId = table.Column<Guid>(nullable: false),
                    ScanDate = table.Column<DateTime>(nullable: false),
                    Result = table.Column<string>(nullable: true),
                    MonitorItemId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitorHistory_MonitorItems_MonitorItemId",
                        column: x => x.MonitorItemId,
                        principalTable: "MonitorItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitorHistory_MonitorItemId",
                table: "MonitorHistory",
                column: "MonitorItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorItems_ResourceId",
                table: "MonitorItems",
                column: "ResourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CommunicationChanelId",
                table: "Resources",
                column: "CommunicationChanelId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_UserId",
                table: "Resources",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_UserId",
                table: "UserActions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitorHistory");

            migrationBuilder.DropTable(
                name: "UserActions");

            migrationBuilder.DropTable(
                name: "MonitorItems");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "CommunicationChanels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
