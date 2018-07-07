using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blue.Api.Migrations
{
    public partial class UpdateRoleTableAndSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Active", "Code", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "IsDeleted", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { true, "SUPERADMIN", "b31ff791-edb3-4887-9223-3f1f4b5e1a1d", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 567, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), true, "Super Adminitrator", "SUPER ADMINISTRATOR", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5a61f008-0ce1-4b60-8c0c-c12c721e475d"),
                columns: new[] { "Active", "Code", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { true, "ADMIN", "918e3830-be83-43b2-b7bd-c8e3b57f834d", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Admin", "ADMINISTRATOR", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("61f3dc6e-3863-40e5-ba2b-a6334b5590ac"),
                columns: new[] { "Active", "Code", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { true, "EMPMGR", "3c8e5a17-52b4-4960-95b6-ee8553b2cf15", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Employ Manager", "EMPLOY MANAGER", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7a749297-4edf-4d16-a769-d3bada83247e"),
                columns: new[] { "Active", "Code", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { true, "USER", "c6edc7fa-fc91-4f1a-9612-97c3fe75522d", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "User", "USER", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Active", "Code", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Description", "IsDeleted", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("94efbd9d-49f3-48b6-ba6d-57ace0d753f4"), true, "EMPSEC", "d8891966-429c-4fc2-8e4b-6836017ecdcb", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, false, "Employ Secretary", "EMPLOY SECRETARY", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 569, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("53319368-f467-4d49-a1b0-8da303b6c24a"),
                columns: new[] { "Active", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Email", "FirstName", "NormalizedEmail", "NormalizedUserName", "SecurityStamp", "UpdatedBy", "UpdatedDate", "UserName" },
                values: new object[] { true, "14995032-64f0-4323-924e-7f6e3ab7807b", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 571, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "superadmin@gmail.com", "Super", "SUPERADMIN@GMAIL.COM", "SUPERADMIN", "15f0f1f1-9006-4b50-bcb8-848760421641", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 571, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "superadmin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55e816d6-34e1-4a1c-9940-4bc5381b21b8"),
                columns: new[] { "Active", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "LastName", "SecurityStamp", "UpdatedBy", "UpdatedDate" },
                values: new object[] { true, "edaea02b-c53b-4ff5-abc7-62063553f851", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 571, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Administrator", "eda73ca6-325f-44a1-b662-d3405013b256", "superadmin", new DateTimeOffset(new DateTime(2018, 7, 8, 5, 7, 12, 571, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { new Guid("94efbd9d-49f3-48b6-ba6d-57ace0d753f4"), "d8891966-429c-4fc2-8e4b-6836017ecdcb" });

            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "IsDeleted", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { "026cd145-134b-4fb4-98c4-5356753942e9", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 885, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), false, "systemadmin", "SYSTEMADMIN", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 887, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5a61f008-0ce1-4b60-8c0c-c12c721e475d"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { "ba15c394-85f0-4ac6-9fd7-0b98bbfc08c6", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 887, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "admin", "ADMIN", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 887, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("61f3dc6e-3863-40e5-ba2b-a6334b5590ac"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { "f850915f-86e9-429a-9eee-9f86ba3aade2", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 887, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "guest", "GUEST", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 887, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7a749297-4edf-4d16-a769-d3bada83247e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Name", "NormalizedName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { "4fe491e2-3d42-4ada-92c7-e58e9a4e6a73", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 887, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "customer", "CUSTOMER", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 887, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("53319368-f467-4d49-a1b0-8da303b6c24a"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Email", "FirstName", "NormalizedEmail", "NormalizedUserName", "SecurityStamp", "UpdatedBy", "UpdatedDate", "UserName" },
                values: new object[] { "e8cfeaf7-c729-409a-979b-babb5d728e71", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 891, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "system@gmail.com", "System", "SYSTEM@SIMPLCOMMERCE.COM", "SYSTEMADMIN", "2db49b80-c0ec-4c9d-a8ee-5660b72b9949", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 891, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "systemadmin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55e816d6-34e1-4a1c-9940-4bc5381b21b8"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "LastName", "SecurityStamp", "UpdatedBy", "UpdatedDate" },
                values: new object[] { "5325a787-805a-4626-a0bb-945f7cca4c4d", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 891, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Admin", "4d37163c-2455-46b4-b5b7-b92faf8df0ea", "systemadmin", new DateTimeOffset(new DateTime(2018, 7, 5, 20, 5, 0, 891, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
