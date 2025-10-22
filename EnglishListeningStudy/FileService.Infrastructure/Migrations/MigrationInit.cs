using Microsoft.EntityFrameworkCore.Migrations;
namespace EnglishListeningStudy.FileService.Infrastructure.Migrations {
    public class MigrationInit : Migration{
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "T_FS_UploadedItems",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datatime2", nullable:false),
                    FileSHA256Hash = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    BackupUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemoteUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)

                },
                constraints: table => {
                    table.PrimaryKey("PK_T_FS_UploadedItems", x => x.Id).Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_FS_UploadedItems_FileSHA256Hash_FileSizeInBytes",
                table: "T_FS_UploadedItems",
                columns: ["FileSHA256Hash", "FileSizeInBytes"]);
        }
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "T_FS_UploadedItems");
        }
    }
}
