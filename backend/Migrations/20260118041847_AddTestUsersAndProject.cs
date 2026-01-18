using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddTestUsersAndProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$Xg7kZ5jJQ5xqJ0K9wK5Y5u7ZJ5FJ5J5J5J5J5J5J5J5J5J5J5J5JO");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "$2a$11$Xg7kZ5jJQ5xqJ0K9wK5Y5u7ZJ5FJ5J5J5J5J5J5J5J5J5J5J5J5JO");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "$2a$11$Xg7kZ5jJQ5xqJ0K9wK5Y5u7ZJ5FJ5J5J5J5J5J5J5J5J5J5J5J5JO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$Q1wZMDzJPtUYIuKkcDSdR.Nj4cpqLaSJ1KOnxM8/d9pZwmnQNbG3W");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "$2a$11$Q1wZMDzJPtUYIuKkcDSdR.Nj4cpqLaSJ1KOnxM8/d9pZwmnQNbG3W");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "$2a$11$Q1wZMDzJPtUYIuKkcDSdR.Nj4cpqLaSJ1KOnxM8/d9pZwmnQNbG3W");
        }
    }
}
