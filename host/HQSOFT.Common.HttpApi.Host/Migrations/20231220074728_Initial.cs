using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.Common.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    FromUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    DocId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "FmDirectoryDescriptors",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uuid", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uuid", nullable: true),
            //        Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //        ParentId = table.Column<Guid>(type: "uuid", nullable: true),
            //        ExtraProperties = table.Column<string>(type: "text", nullable: false),
            //        ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //        CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
            //        LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FmDirectoryDescriptors", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_FmDirectoryDescriptors_FmDirectoryDescriptors_ParentId",
            //            column: x => x.ParentId,
            //            principalTable: "FmDirectoryDescriptors",
            //            principalColumn: "Id");
            //    });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    FromUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    NotiTitle = table.Column<string>(type: "text", nullable: false),
                    NotiBody = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    DocId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareWiths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocId = table.Column<Guid>(type: "uuid", nullable: false),
                    CanRead = table.Column<bool>(type: "boolean", nullable: false),
                    CanWrite = table.Column<bool>(type: "boolean", nullable: false),
                    CanSubmit = table.Column<bool>(type: "boolean", nullable: false),
                    CanShare = table.Column<bool>(type: "boolean", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    SharedToUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareWiths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    AssignedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestCommons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Idx = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCommons", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "FmFileDescriptors",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uuid", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uuid", nullable: true),
            //        DirectoryId = table.Column<Guid>(type: "uuid", nullable: true),
            //        Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //        MimeType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
            //        Size = table.Column<int>(type: "integer", maxLength: 2147483647, nullable: false),
            //        ExtraProperties = table.Column<string>(type: "text", nullable: false),
            //        ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //        CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
            //        LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FmFileDescriptors", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_FmFileDescriptors_FmDirectoryDescriptors_DirectoryId",
            //            column: x => x.DirectoryId,
            //            principalTable: "FmDirectoryDescriptors",
            //            principalColumn: "Id");
            //    });
    
            //migrationBuilder.CreateIndex(
            //    name: "IX_FmDirectoryDescriptors_ParentId",
            //    table: "FmDirectoryDescriptors",
            //    column: "ParentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FmDirectoryDescriptors_TenantId_ParentId_Name",
            //    table: "FmDirectoryDescriptors",
            //    columns: new[] { "TenantId", "ParentId", "Name" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_FmFileDescriptors_DirectoryId",
            //    table: "FmFileDescriptors",
            //    column: "DirectoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FmFileDescriptors_TenantId_DirectoryId_Name",
            //    table: "FmFileDescriptors",
            //    columns: new[] { "TenantId", "DirectoryId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ShareWiths_DocId_SharedToUserId_Url",
                table: "ShareWiths",
                columns: new[] { "DocId", "SharedToUserId", "Url" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_DocId_AssignedUserId_Url",
                table: "TaskAssignments",
                columns: new[] { "DocId", "AssignedUserId", "Url" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {       

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "FmFileDescriptors");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ShareWiths");

            migrationBuilder.DropTable(
                name: "TaskAssignments");

            migrationBuilder.DropTable(
                name: "TestCommons");
        
            //migrationBuilder.DropTable(
            //    name: "FmDirectoryDescriptors");    
        }
    }
}
