using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace monitor_infra.Migrations
{
    public partial class ChangeMonitorEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonitorHistory_MonitorItems_MonitorItemId",
                table: "MonitorHistory");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "MonitorHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "MonitorItemId",
                table: "MonitorHistory",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorHistory_MonitorItems_MonitorItemId",
                table: "MonitorHistory",
                column: "MonitorItemId",
                principalTable: "MonitorItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonitorHistory_MonitorItems_MonitorItemId",
                table: "MonitorHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "MonitorItemId",
                table: "MonitorHistory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceId",
                table: "MonitorHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorHistory_MonitorItems_MonitorItemId",
                table: "MonitorHistory",
                column: "MonitorItemId",
                principalTable: "MonitorItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
