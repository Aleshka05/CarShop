using Microsoft.EntityFrameworkCore.Migrations;

namespace CarShop.Migrations
{
    public partial class AddPackagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Features = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "Features", "Name" },
                values: new object[,]
                {
                    { 1, @"Мультимедийная система: стандартная аудиосистема с CD/MP3 плеером, поддержка радио.
                Салон: тканевый салон, механические сиденья.
                Колеса: стальные диски с базовыми шинами.
                Безопасность: стандартные подушки безопасности, система контроля давления в шинах.
                Удобства: отсутствие системы помощи при парковке, базовый кондиционер.", "Стандартная" },
                    { 2, @"Мультимедийная система: улучшенная аудиосистема с Bluetooth, USB и AUX, поддержка Android Auto и Apple CarPlay.
                Салон: тканевый или частично кожаный салон, сиденья с электрической регулировкой.
                Колеса: литые диски, улучшенные шины для лучшего сцепления.
                Безопасность: дополнительные подушки безопасности, система помощи при парковке с датчиками, контроль мертвых зон.
                Удобства: климат-контроль, обогрев сидений, мультимедийный экран с сенсорным управлением, камера заднего вида.", "Повышенной комфортности" },
                    { 3, @"Мультимедийная система: премиум аудиосистема с поддержкой всех современных технологий, сенсорный экран, встроенная навигация.
                Салон: полностью кожаный салон, сиденья с подогревом и вентиляцией, массажные сиденья.
                Колеса: литые диски с уникальным дизайном, спортивные шины.
                Безопасность: полный набор подушек безопасности, системы автоматического торможения, система помощи при парковке с камерой 360°, контроль сплошной полосы.
                Удобства: панорамная крыша, система автоматической парковки, адаптивный круиз-контроль, система предупреждения о возможном столкновении.", "Премиум" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$qhhFcOFr1xooLq0/7nbTfewJvrXYiCNn6P4P7dLgfyn1FX6wgm7Ui");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$9xgZ7ITLKgIvnDcR75kLLOzJdY70YHplh7qoAlUUCev0rX9hYxgXS");
        }
    }
}
