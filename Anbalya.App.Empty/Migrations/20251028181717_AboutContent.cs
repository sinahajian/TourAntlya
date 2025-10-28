using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Anbalya.App.Empty.Migrations
{
    /// <inheritdoc />
    public partial class AboutContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    ButtonUrl = table.Column<string>(type: "text", nullable: false),
                    TitleLine1En = table.Column<string>(type: "text", nullable: false),
                    TitleLine1De = table.Column<string>(type: "text", nullable: false),
                    TitleLine1Tr = table.Column<string>(type: "text", nullable: false),
                    TitleLine1Fa = table.Column<string>(type: "text", nullable: false),
                    TitleLine1Ru = table.Column<string>(type: "text", nullable: false),
                    TitleLine1Pl = table.Column<string>(type: "text", nullable: false),
                    TitleLine1Ar = table.Column<string>(type: "text", nullable: false),
                    TitleLine2En = table.Column<string>(type: "text", nullable: false),
                    TitleLine2De = table.Column<string>(type: "text", nullable: false),
                    TitleLine2Tr = table.Column<string>(type: "text", nullable: false),
                    TitleLine2Fa = table.Column<string>(type: "text", nullable: false),
                    TitleLine2Ru = table.Column<string>(type: "text", nullable: false),
                    TitleLine2Pl = table.Column<string>(type: "text", nullable: false),
                    TitleLine2Ar = table.Column<string>(type: "text", nullable: false),
                    BodyEn = table.Column<string>(type: "text", nullable: false),
                    BodyDe = table.Column<string>(type: "text", nullable: false),
                    BodyTr = table.Column<string>(type: "text", nullable: false),
                    BodyFa = table.Column<string>(type: "text", nullable: false),
                    BodyRu = table.Column<string>(type: "text", nullable: false),
                    BodyPl = table.Column<string>(type: "text", nullable: false),
                    BodyAr = table.Column<string>(type: "text", nullable: false),
                    ButtonTextEn = table.Column<string>(type: "text", nullable: false),
                    ButtonTextDe = table.Column<string>(type: "text", nullable: false),
                    ButtonTextTr = table.Column<string>(type: "text", nullable: false),
                    ButtonTextFa = table.Column<string>(type: "text", nullable: false),
                    ButtonTextRu = table.Column<string>(type: "text", nullable: false),
                    ButtonTextPl = table.Column<string>(type: "text", nullable: false),
                    ButtonTextAr = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutContents", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AboutContents",
                columns: new[]
                {
                    "Id",
                    "ImagePath",
                    "ButtonUrl",
                    "TitleLine1En", "TitleLine1De", "TitleLine1Tr", "TitleLine1Fa", "TitleLine1Ru", "TitleLine1Pl", "TitleLine1Ar",
                    "TitleLine2En", "TitleLine2De", "TitleLine2Tr", "TitleLine2Fa", "TitleLine2Ru", "TitleLine2Pl", "TitleLine2Ar",
                    "BodyEn", "BodyDe", "BodyTr", "BodyFa", "BodyRu", "BodyPl", "BodyAr",
                    "ButtonTextEn", "ButtonTextDe", "ButtonTextTr", "ButtonTextFa", "ButtonTextRu", "ButtonTextPl", "ButtonTextAr",
                    "CreationTime"
                },
                values: new object[]
                {
                    1,
                    "/image/about_bg.jpg",
                    "#contact",
                    "About Us", "Über uns", "Hakkımızda", "درباره ما", "О нас", "O nas", "من نحن",
                    "Our History · Mission & Vision", "Unsere Geschichte · Mission & Vision", "Hikayemiz · Misyon & Vizyon", "تاریخچه · ماموریت و چشم‌انداز", "Наша история · Миссия и видение", "Nasza historia · Misja i wizja", "تاريخنا · رسالتنا ورؤيتنا",
                    "From bespoke Antalya experiences to curated tours across Turkey, we craft journeys that blend comfort with discovery.",
                    "Von maßgeschneiderten Antalya-Erlebnissen bis zu kuratierten Türkei-Rundreisen – wir verbinden Komfort mit Entdeckung.",
                    "Antalya'daki özel deneyimlerden Türkiye genelindeki seçkin turlara kadar, konforu keşif duygusuyla harmanlayan yolculuklar tasarlıyoruz.",
                    "از تجربه‌های اختصاصی آنتالیا تا تورهای خاص سراسر ترکیه، سفری می‌سازیم که راحتی و کشف را در کنار هم قرار می‌دهد.",
                    "От индивидуальных впечатлений в Анталии до авторских туров по всей Турции — мы соединяем комфорт и открытие нового.",
                    "Od szytych na miarę przeżyć w Antalyi po starannie dobrane wycieczki po Turcji – łączymy komfort z odkrywaniem.",
                    "من تجارب أنطاليا المصممة حسب الطلب إلى جولات منسقة في عموم تركيا، نصنع رحلات تجمع الراحة بالاكتشاف.",
                    "Request Custom Price", "Individuelles Angebot", "Özel Fiyat Talebi", "درخواست قیمت اختصاصی", "Запросить индивидуальную цену", "Poproś o wycenę", "اطلب عرض سعر خاص",
                    0L
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutContents");
        }
    }
}
