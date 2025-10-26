using System;
using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
#pragma warning disable CA1814 
namespace Course4.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PublishedYear = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateOfBirth", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1828, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лев Толстой" },
                    { 2, new DateTime(1821, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Фёдор Достоевский" },
                    { 3, new DateTime(1860, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Антон Чехов" }
                });
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "PublishedYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1869, "Война и мир" },
                    { 2, 1, 1877, "Анна Каренина" },
                    { 3, 2, 1866, "Преступление и наказание" },
                    { 4, 3, 1904, "Вишнёвый сад" },
                    { 5, 2, 1880, "Братья Карамазовы" }
                });
            migrationBuilder.CreateIndex(
                name: "IX_Authors_Name",
                table: "Authors",
                column: "Name");
            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");
            migrationBuilder.CreateIndex(
                name: "IX_Books_PublishedYear",
                table: "Books",
                column: "PublishedYear");
            migrationBuilder.CreateIndex(
                name: "IX_Books_Title",
                table: "Books",
                column: "Title");
        }
        ///<inheritdoc />
       protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
