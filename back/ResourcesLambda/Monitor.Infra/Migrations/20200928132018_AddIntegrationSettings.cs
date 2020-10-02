using Microsoft.EntityFrameworkCore.Migrations;

namespace Monitor.Infra.Migrations
{
    public partial class AddIntegrationSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationSettings",
                columns: table => new
                {
                    UserEmail = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    NotificationEmail = table.Column<bool>(nullable: false),
                    NotificationSlack = table.Column<bool>(nullable: false),
                    SlackChannelUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationSettings", x => x.UserEmail);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationSettings");
        }
    }
}
