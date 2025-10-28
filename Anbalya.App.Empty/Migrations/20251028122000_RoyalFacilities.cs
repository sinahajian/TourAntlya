using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Anbalya.App.Empty.Migrations
{
    /// <inheritdoc />
    public partial class RoyalFacilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoyalFacilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IconClass = table.Column<string>(type: "text", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    TitleEn = table.Column<string>(type: "text", nullable: false),
                    TitleDe = table.Column<string>(type: "text", nullable: false),
                    TitleTr = table.Column<string>(type: "text", nullable: false),
                    TitleFa = table.Column<string>(type: "text", nullable: false),
                    TitleRu = table.Column<string>(type: "text", nullable: false),
                    TitlePl = table.Column<string>(type: "text", nullable: false),
                    TitleAr = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionDe = table.Column<string>(type: "text", nullable: false),
                    DescriptionTr = table.Column<string>(type: "text", nullable: false),
                    DescriptionFa = table.Column<string>(type: "text", nullable: false),
                    DescriptionRu = table.Column<string>(type: "text", nullable: false),
                    DescriptionPl = table.Column<string>(type: "text", nullable: false),
                    DescriptionAr = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoyalFacilities", x => x.Id);
                });

            var seedTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

            migrationBuilder.InsertData(
                table: "RoyalFacilities",
                columns: new[]
                {
                    "Id", "IconClass", "DisplayOrder",
                    "TitleEn", "TitleDe", "TitleTr", "TitleFa", "TitleRu", "TitlePl", "TitleAr",
                    "DescriptionEn", "DescriptionDe", "DescriptionTr", "DescriptionFa", "DescriptionRu", "DescriptionPl", "DescriptionAr",
                    "CreationTime"
                },
                values: new object[,]
                {
                    { 1, "lnr lnr-bus", 1, "Hotel Pick-Up & Drop-Off", "Hotelabholung & Rücktransfer", "Otelden Alım & Bırakma", "ترنسفر رفت و برگشت هتل", "Трансфер из отеля и обратно", "Transfer z hotelu w cenie", "خدمة نقل من وإلى الفندق", "Complimentary transfer from your Antalya hotel to the Kemer yacht harbor and back.", "Kostenloser Transfer von Ihrem Hotel in Antalya zum Hafen von Kemer und zurück.", "Antalya'daki otelinizden Kemer limanına ve dönüşte tekrar otele ücretsiz transfer.", "حمل‌ونقل رایگان از هتل شما در آنتالیا تا بندر یات کمر و بالعکس.", "Бесплатный трансфер из вашего отеля в Анталье до яхтенной марины Кемера и обратно.", "Bezpłatny dojazd z hotelu w Antalyi do mariny w Kemer i z powrotem.", "نقل مجاني من فندقك في أنطاليا إلى مرسى كيمر لليخوت والعودة مرة أخرى.", seedTime },
                    { 2, "lnr lnr-dinner", 2, "Fresh Lunch on Board", "Frisches Mittagessen an Bord", "Teknede Sıcak Öğle Yemeği", "ناهار تازه روی عرشه", "Свежий обед на борту", "Świeży obiad na pokładzie", "غداء طازج على متن القارب", "Grilled chicken, salads, and seasonal fruit served during the cruise.", "Gegrilltes Hähnchen, Salate und saisonales Obst während der Kreuzfahrt.", "Tur boyunca ızgara tavuk, salatalar ve mevsim meyveleri servis edilir.", "در طول کروز مرغ کبابی، سالاد و میوه فصل سرو می‌شود.", "Во время круиза подают жареную курицу, салаты и сезонные фрукты.", "Podczas rejsu serwujemy grillowanego kurczaka, sałatki i sezonowe owoce.", "يُقدَّم خلال الرحلة دجاج مشوي وسلطات وفواكه موسمية.", seedTime },
                    { 3, "lnr lnr-coffee-cup", 3, "Unlimited Soft Drinks", "Unbegrenzte Softdrinks", "Sınırsız Alkolsüz İçecekler", "نوشیدنی نامحدود", "Неограниченные напитки", "Nielimitowane napoje", "مشروبات بلا حدود", "Tea, coffee, and refreshing soft drinks available throughout the day.", "Tee, Kaffee und erfrischende Softdrinks stehen den ganzen Tag bereit.", "Gün boyu çay, kahve ve ferahlatıcı içecekler sunulur.", "چای، قهوه و نوشیدنی‌های خنک تمام روز در دسترس است.", "Чай, кофе и освежающие безалкогольные напитки доступны весь день.", "Herbata, kawa i orzeźwiające napoje bezalkoholowe dostępne przez cały dzień.", "شاي وقهوة ومشروبات منعشة متاحة طوال اليوم.", seedTime },
                    { 4, "lnr lnr-sun", 4, "Paradise Bays Swim Stops", "Badestopps in Paradiesbuchten", "Cennet Koylarında Yüzme Molaları", "توقف برای شنا در خلیج‌های بهشتی", "Купание в райских бухтах", "Przystanki na kąpiel w rajskich zatokach", "توقفات سباحة في خلجان الجنة", "Dive into crystal bays like Phaselis Island and Cennet Cove with 45-minute breaks.", "Schwimmen in kristallklaren Buchten wie Phaselis Island und Cennet Cove mit 45-Minuten-Pausen.", "Phaselis Adası ve Cennet Koyu gibi turkuaz koylarda 45 dakikalık yüzme molaları.", "با توقف‌های ۴۵ دقیقه‌ای در آب‌های شفاف جزیره فاسلیس و خلیج جنّت شنا کنید.", "Купание в прозрачных водах бухт Фазелис и Дженнет с остановками по 45 минут.", "45-minutowe postoje na kąpiel w krystalicznych wodach Phaselis i Cennet.", "اسبح في مياه فيروزية عند جزيرة فاسيليس وخليج جنّت مع توقفات لمدة 45 دقيقة.", seedTime },
                    { 5, "lnr lnr-users", 5, "Live Multilingual Guide", "Live-Guide in mehreren Sprachen", "Canlı Çok Dilli Rehber", "راهنمای زنده چندزبانه", "Живой многоязычный гид", "Żywy przewodnik wielojęzyczny", "مرشد حي متعدد اللغات", "Live English, French, German, Polish, Russian speaking guide shares stories and safety tips.", "Moderation auf Englisch, Französisch, Deutsch, Polnisch und Russisch mit Geschichten und Sicherheitstipps.", "İngilizce, Fransızca, Almanca, Lehçe ve Rusça konuşان rehber hikayeler ve güvenlik bilgileri paylaşır.", "میزبان انگلیسی، فرانسوی، آلمانی، لهستانی و روسی داستان‌ها و نکات ایمنی را بیان می‌کند.", "Гид на английском, французском, немецком, польском и русском делится историями и советами по безопасности.", "Prowadzący mówi po angielsku, francusku, niemiecku, polsku i rosyjsku, dzieląc się historiami i zasadami bezpieczeństwa.", "مرشد يتحدث الإنجليزية والفرنسية والألمانية والبولندية والروسية يشارك القصص وإرشادات السلامة.", seedTime },
                    { 6, "lnr lnr-music-note", 6, "Sun Deck Loungers & Music", "Sonnendeck-Liegen & Musik", "Güneşlenme Güvertesi ve Müzik", "آفتاب‌گرفتن و موسیقی روی عرشه", "Солярий и музыка на палубе", "Leżaki słoneczne i muzyka", "سطح شمسي وموسيقى", "Relax on upper-deck sunbeds with chill-out music and an optional foam party.", "Entspannen Sie auf dem Oberdeck mit Liegen, Chill-out-Musik und optionaler Schaumparty.", "Üst güvertede şezlonglarda dinlenin, chill-out müzik ve isteğe bağlı köpük partisiyle eğленin.", "روی صندلی‌های آفتاب‌گیر عرشه بالا استراحت کنید و از موسیقی و فوم‌پارٹی اختیاری لذت ببرید.", "Отдыхайте на лежаках верхней палубы под расслабляющую музыку и пенную вечеринку по желанию.", "Relaks na górnym pokładzie z leżakami, chilloutową muzyką i opcjonalną imprezą pianową.", "استرخِ على أسرة التشمس في السطح العلوي مع موسيقى هادئة وحفلة رغوة اختيارية.", seedTime }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoyalFacilities");
        }
    }
}
