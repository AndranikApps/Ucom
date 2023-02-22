using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UcomGridView.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                    table.UniqueConstraint("AK_Statuses_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AvatarPath = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Email", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Users_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatusId",
                table: "Users",
                column: "StatusId");

            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[CreateUser] " +
                "(@firstname NVARCHAR(50), @lastname NVARCHAR(50), @age INT, @email NVARCHAR(128), @avatarPath NVARCHAR(42), @statusId INT) AS " +
                "BEGIN " +
                "INSERT INTO [dbo].[Users] (Firstname, Lastname, Age, Email, AvatarPath, StatusId) " +
                "OUTPUT Inserted.Id, Inserted.Firstname, Inserted.Lastname, Inserted.Age, Inserted.Email, Inserted.CreatedAt, " +
                "Inserted.UpdatedAt, Inserted.IsDeleted, Inserted.AvatarPath, Inserted.StatusId " +
                "VALUES (@firstname, @lastname, @age, @email, @avatarPath, @statusId) " +
                "END"
                );
            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[DeleteUser] " +
                "(@id INT) AS " +
                "BEGIN " +
                "DELETE FROM [dbo].[Users] WHERE Id=@id " +
                "END"
                );
            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[GetUserById] " +
                "(@id INT) AS " +
                "BEGIN " +
                "SELECT * FROM [dbo].[Users] WHERE Id=@id " +
                "END"
                );
            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[GetUsers] (@skip INT, @take INT, @ColumnName NVARCHAR(20) = 'Id', @order NVARCHAR(7) = 'ASC')\r\nAS\r\nBEGIN\r\nSET NOCOUNT ON;\r\nSELECT u.Id, u.Firstname, u.Lastname, u.Age, u.Email, u.CreatedAt, u.UpdatedAt, u.IsDeleted, u.AvatarPath, u.StatusId, s.Name AS Status\r\nFROM [dbo].[Users] AS u\r\nINNER JOIN [dbo].[Statuses] AS s ON s.Id=u.StatusId\r\nORDER BY\r\nCASE WHEN @order='ASC' AND @ColumnName='Firtsname' THEN u.Firstname END,\r\nCASE WHEN @order='DESC' AND @ColumnName='Firtsname' THEN u.Firstname END DESC,\r\nCASE WHEN @order='ASC' AND @ColumnName='Lastname' THEN u.Lastname END,\r\nCASE WHEN @order='DESC' AND @ColumnName='Lastname' THEN u.Lastname END DESC,\r\nCASE WHEN @order='ASC' AND @ColumnName='Age' THEN u.Age END,\r\nCASE WHEN @order='DESC' AND @ColumnName='Age' THEN u.Age END DESC,\r\nCASE WHEN @order='ASC' AND @ColumnName='Email' THEN u.Email END,\r\nCASE WHEN @order='DESC' AND @ColumnName='Email' THEN u.Email END DESC,\r\nCASE WHEN @order='ASC' AND @ColumnName='IsDeleted' THEN u.IsDeleted END,\r\nCASE WHEN @order='DESC' AND @ColumnName='IsDeleted' THEN u.IsDeleted END DESC,\r\nCASE WHEN @order='ASC' AND @ColumnName='CreatedAt' THEN u.CreatedAt END,\r\nCASE WHEN @order='DESC' AND @ColumnName='CreatedAt' THEN u.CreatedAt END DESC,\r\nCASE WHEN @order='ASC' AND @ColumnName='UpdatedAt' THEN u.UpdatedAt END,\r\nCASE WHEN @order='DESC' AND @ColumnName='UpdatedAt' THEN u.UpdatedAt END DESC,\r\nCASE WHEN @order='ASC' AND @ColumnName='Status' THEN s.Name END,\r\nCASE WHEN @order='DESC' AND @ColumnName='Status' THEN s.Name END DESC\r\nOFFSET @skip ROWS FETCH NEXT @take ROWS ONLY END");
            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[UpdateUser] " +
                "(@id INT, @firstname VARCHAR(50), @lastname NVARCHAR(50), @age INT, @email NVARCHAR(128), @isDeleted BIT, " +
                "@avatarPath NVARCHAR(42), @statusId INT) AS " +
                "BEGIN " +
                "UPDATE [dbo].[Users] SET Firstname=@firstname, Lastname=@lastname, Age=@age, Email=@email, IsDeleted=@isDeleted, " +
                "AvatarPath=@avatarPath, StatusId=@statusId " +
                "OUTPUT INSERTED.Id, deleted.AvatarPath " +
                "WHERE Id=@id " +
                "END");

            migrationBuilder.Sql("INSERT INTO Statuses (Name) VALUES ('Active'), ('Inactive')");

            // Need MS SQL server 2022
            //migrationBuilder.Sql("INSERT INTO [dbo].[Users] (Firstname, Lastname, Age, Email, CreatedAt, UpdatedAt, StatusId) " +
            //    "SELECT CONCAT('Firstname', value), " +
            //    "CONCAT('Lastname', value), " +
            //    "value, " +
            //    "CONCAT(CONCAT('email', value), '@gmail.com'), " +
            //    "GETDATE(), " +
            //    "GETDATE(), " +
            //    "(value % 2) + 1 " +
            //    "FROM GENERATE_SERIES(1, 1000, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateUser] " +
                "DROP PROCEDURE IF EXISTS [dbo].[DeleteUser] " +
                "DROP PROCEDURE IF EXISTS [dbo].[GetUsers] " +
                "DROP PROCEDURE IF EXISTS [dbo].[GetUserById] " +
                "DROP PROCEDURE IF EXISTS [dbo].[UpdateUser]");
        }
    }
}
