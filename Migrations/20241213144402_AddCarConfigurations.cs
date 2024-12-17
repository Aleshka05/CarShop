using Microsoft.EntityFrameworkCore.Migrations;

namespace CarShop.Migrations
{
    public partial class AddCarConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropColumn(
                name: "CarEngine",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "BodyType",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveType",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "Cars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Cars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    ConfigurationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarID = table.Column<int>(nullable: false),
                    Salon = table.Column<string>(nullable: true),
                    SafetySystems = table.Column<string>(nullable: true),
                    Airbags = table.Column<string>(nullable: true),
                    AssistanceSystems = table.Column<string>(nullable: true),
                    Exterior = table.Column<string>(nullable: true),
                    Multimedia = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.ConfigurationID);
                    table.ForeignKey(
                        name: "FK_Configuration_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "CarID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ZWLGwb3joE/XacHflcDIBuWDBZaOsdOTva.cUVVPmH8KWXGqNzpq6");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_CarID",
                table: "Configuration",
                column: "CarID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropColumn(
                name: "BodyType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DriveType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "CarEngine",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "Features", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, @"Мультимедийная система: стандартная аудиосистема с CD/MP3 плеером, поддержка радио.
                Салон: тканевый салон, механические сиденья.
                Колеса: стальные диски с базовыми шинами.
                Безопасность: стандартные подушки безопасности, система контроля давления в шинах.
                Удобства: отсутствие системы помощи при парковке, базовый кондиционер.", null, "Стандартная" },
                    { 2, @"Мультимедийная система: улучшенная аудиосистема с Bluetooth, USB и AUX, поддержка Android Auto и Apple CarPlay.
                Салон: тканевый или частично кожаный салон, сиденья с электрической регулировкой.
                Колеса: литые диски, улучшенные шины для лучшего сцепления.
                Безопасность: дополнительные подушки безопасности, система помощи при парковке с датчиками, контроль мертвых зон.
                Удобства: климат-контроль, обогрев сидений, мультимедийный экран с сенсорным управлением, камера заднего вида.", null, "Повышенной комфортности" },
                    { 3, @"Мультимедийная система: премиум аудиосистема с поддержкой всех современных технологий, сенсорный экран, встроенная навигация.
                Салон: полностью кожаный салон, сиденья с подогревом и вентиляцией, массажные сиденья.
                Колеса: литые диски с уникальным дизайном, спортивные шины.
                Безопасность: полный набор подушек безопасности, системы автоматического торможения, система помощи при парковке с камерой 360°, контроль сплошной полосы.
                Удобства: панорамная крыша, система автоматической парковки, адаптивный круиз-контроль, система предупреждения о возможном столкновении.", null, "Премиум" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$l4fZHb3urnzPLaQgOR23HO7NUkNHJbZ/amts53nIwYKesz5HPZwcy");
        }
    }
}
