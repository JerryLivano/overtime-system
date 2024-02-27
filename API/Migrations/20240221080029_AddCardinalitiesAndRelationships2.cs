using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddCardinalitiesAndRelationships2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_m_employees_tbl_m_employees_manager_id",
                table: "tbl_m_employees");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_m_employees_tbl_m_employees_manager_id",
                table: "tbl_m_employees",
                column: "manager_id",
                principalTable: "tbl_m_employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_m_employees_tbl_m_employees_manager_id",
                table: "tbl_m_employees");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_m_employees_tbl_m_employees_manager_id",
                table: "tbl_m_employees",
                column: "manager_id",
                principalTable: "tbl_m_employees",
                principalColumn: "id");
        }
    }
}
