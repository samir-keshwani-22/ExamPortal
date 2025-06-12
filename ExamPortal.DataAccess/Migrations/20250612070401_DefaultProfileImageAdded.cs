using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DefaultProfileImageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "2");

            migrationBuilder.AlterColumn<string>(
                name: "profile_img",
                table: "users",
                type: "character varying",
                nullable: true,
                defaultValue: "/img/default_profile_picture.png",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValueSql: "2",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<string>(
                name: "profile_img",
                table: "users",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true,
                oldDefaultValue: "/img/default_profile_picture.png");
        }
    }
}
