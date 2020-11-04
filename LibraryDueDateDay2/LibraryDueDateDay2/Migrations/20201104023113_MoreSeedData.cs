using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDueDateDay2.Migrations
{
    public partial class MoreSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "ID", "AuthorID", "PublicationDate", "Title" },
                values: new object[] { -5, -5, new DateTime(2002, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter and the Goblet of Fire" });

            migrationBuilder.InsertData(
                table: "borrow",
                columns: new[] { "ID", "BookID", "CheckedOutDate", "DueDate", "ReturnedDate" },
                values: new object[] { -4, -5, new DateTime(2020, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "borrow",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "book",
                keyColumn: "ID",
                keyValue: -5);
        }
    }
}
