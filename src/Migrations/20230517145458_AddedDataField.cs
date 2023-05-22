using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServerConfig.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data",
                schema: "IdentityServerConfig",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                schema: "IdentityServerConfig",
                table: "AuditLog");
        }
    }
}
