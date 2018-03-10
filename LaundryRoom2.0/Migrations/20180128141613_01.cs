using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryRoom20.Migrations
{
    public partial class _01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookerId",
                table: "User",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_BookingId",
                table: "User",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Booking_BookingId",
                table: "User",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Booking_BookingId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_BookingId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BookerId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "User");
        }
    }
}
