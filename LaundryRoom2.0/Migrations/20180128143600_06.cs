using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryRoom20.Migrations
{
    public partial class _06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Booking_BookingId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_BookingId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "BookerId",
                table: "Booking",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Booking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                table: "Booking",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_User_UserId",
                table: "Booking",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_User_UserId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_UserId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookerId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Booking");

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
    }
}
