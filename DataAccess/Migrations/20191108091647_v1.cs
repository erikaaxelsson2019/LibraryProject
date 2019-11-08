using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aisles",
                columns: table => new
                {
                    AisleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AisleNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aisles", x => x.AisleID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerAdress = table.Column<string>(nullable: true),
                    BirthDate = table.Column<string>(nullable: true),
                    Debt = table.Column<int>(nullable: false),
                    GuardianCustomerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Customers_Customers_GuardianCustomerID",
                        column: x => x.GuardianCustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shelfs",
                columns: table => new
                {
                    ShelfID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShelfNumber = table.Column<int>(nullable: false),
                    AisleID = table.Column<int>(nullable: false),
                    AisleNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelfs", x => x.ShelfID);
                    table.ForeignKey(
                        name: "FK_Shelfs_Aisles_AisleID",
                        column: x => x.AisleID,
                        principalTable: "Aisles",
                        principalColumn: "AisleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookTitle = table.Column<string>(nullable: true),
                    BookAuthor = table.Column<string>(nullable: true),
                    IsbnNumber = table.Column<string>(nullable: true),
                    BookPrice = table.Column<int>(nullable: false),
                    PurchaseYear = table.Column<int>(nullable: false),
                    BookCondition = table.Column<int>(nullable: false),
                    IsLoaned = table.Column<bool>(nullable: false),
                    ShelfID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Books_Shelfs_ShelfID",
                        column: x => x.ShelfID,
                        principalTable: "Shelfs",
                        principalColumn: "ShelfID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    BookID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanID);
                    table.ForeignKey(
                        name: "FK_Loans_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_ShelfID",
                table: "Books",
                column: "ShelfID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GuardianCustomerID",
                table: "Customers",
                column: "GuardianCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookID",
                table: "Loans",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_CustomerID",
                table: "Loans",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Shelfs_AisleID",
                table: "Shelfs",
                column: "AisleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Shelfs");

            migrationBuilder.DropTable(
                name: "Aisles");
        }
    }
}
