using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anbalya.App.Empty.Migrations
{
    /// <inheritdoc />
    public partial class heroTranslate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionPl",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaglineAr",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaglineDe",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaglineEn",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaglineFa",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaglinePl",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaglineRu",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleDe",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleFa",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitlePl",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleRu",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "LandingContents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "DescriptionAr", "DescriptionDe", "DescriptionEn", "DescriptionFa", "DescriptionPl", "DescriptionRu", "TaglineAr", "TaglineDe", "TaglineEn", "TaglineFa", "TaglinePl", "TaglineRu", "TitleAr", "TitleDe", "TitleEn", "TitleFa", "TitlePl", "TitleRu" },
                values: new object[] { "Step away from routine: gentle cruises, adventures, and sunny escapes are ready for you.", "امنح نفسك استراحة حقيقية: رحلات بحرية، مغامرات وتجارب مميزة تلائم كل الأذواق.", "Lass den Alltag hinter dir: sanfte Bootstouren, Naturerlebnisse und Ausflüge voller Sonne warten bereits auf dich.", "Step away from routine: gentle cruises, adventures, and sunny escapes are ready for you.", "با تورهای ما از شلوغی دور شوید؛ سفرهایی آرام، ماجراجویانه و سرشار از تجربه‌های تازه.", "Odpocznij od codzienności: czekają na Ciebie rejsy, przygody i chwile czystego relaksu.", "Устройте себе отдых: море, приключения и новые впечатления ждут вас каждый день.", "ابتعد عن الحياة الرتيبة", "Raus aus dem Alltag", "Away from monotonous life", "دور از زندگی یکنواخت", "Z dala od monotonii", "Подальше от монотонной жизни", "أرخِ ذهنك", "Entspann deinen Geist", "Relax Your Mind", "ذهن خود را آرام کنید", "Zrelaksuj swój umysł", "Расслабьте свой разум" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 1,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Snorkelling opportunity", "Lunch" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 2,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Foam party", "Lunch", "Onboard showers" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 3,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional guide", "Lunch", "Off-road driving", "Photo stops & village visit", "Swimming stop (Hatipler)", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 4,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Rafting equipment", "Professional rafting guides", "Open-buffet & BBQ lunch", "Insurance", "Swimming breaks", "Bridged canyon passage" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 5,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional guide", "Transport Antalya–Pamukkale", "Visit to Hierapolis", "Free time at travertines", "UNESCO heritage site", "Optional Cleopatra Pool (extra)" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 6,
                column: "Services",
                value: new List<string> { "Hotel pickup (13:45) & drop-off (17:00)", "Live dolphin & sea lion show", "Seating at the venue", "Free time for photos", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 7,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Diving equipment", "Professional instructors", "2 dives (conditions permitting)", "Boat trip", "Lunch", "Insurance", "Option to join as visitor" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 8,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "River/boat cruise", "Visit Turkish Bazaar", "Visit Manavgat Waterfall", "Free time for shopping", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 9,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat trip from Adrasan", "Swimming stops", "Free time on beaches", "Captain trip customization (on request)", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 10,
                column: "Services",
                value: new List<string> { "Sauna/steam room", "Body scrub (peeling)", "Foam massage", "Aromatherapy oil massage", "Turkish tea", "Changing room & locker", "Towels", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 11,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Buggy ride", "Helmet", "Swimming/photo breaks", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 12,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Quad bike ride", "Helmet", "Swimming/photo breaks", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 13,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Evening boat cruise", "Freshly prepared dinner", "Scenic coastline views" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 14,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Visit 3 waterfalls", "Manavgat river cruise", "Visit Turkish bazaar", "Free time for shopping" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 15,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional guide", "Visit Perge", "Visit Aspendos amphitheatre", "Visit Side (Temple of Apollo)", "Visit Kursunlu Waterfalls", "Lunch" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 16,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat cruise (Grand & Little Canyon)", "Swimming stops", "Lunch at lake-view restaurant", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 17,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Guided Kaleici walking tour", "Visit Hadrian’s Gate, Clock Tower, Kesik Minare", "Visit Düden Waterfalls (Lower Düden)", "Handicraft workshop stop", "Mediterranean boat trip", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 18,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Safety briefing & equipment", "Rafting on Köprü River", "Canyoning segment", "Swimming stops", "Professional guides", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 19,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional instructors", "Full diving briefing", "Diving equipment", "2 dives (conditions permitting)", "Boat trip", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 20,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Return transfers", "Professional guide", "Overnight accommodation (standard, unless otherwise stated)", "Visits to valleys & rock-cut churches", "Optional hot air balloon ride (extra)" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 21,
                column: "Services",
                value: new List<string> { "Entrance to Tunnel Aquarium", "Access to thematic aquariums", "Wreck exhibits (plane & ship)", "Interactive displays" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 22,
                column: "Services",
                value: new List<string> { "Sauna", "Body peeling", "Foam massage", "Aromatherapy oil massage", "Turkish tea" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 23,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat trip to Kekova", "Swimming/snorkeling stops", "Lunch", "Guided visit to Myra ruins", "Visit to St. Nicholas Church", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 24,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Pirate boat cruise", "Lunch on board", "Swimming stops", "Stop at Phaselis (ancient city)", "Paradise Island or Mehmet Ali Bükü stop", "Alaca Water swimming/snorkeling" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "DescriptionPl",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TaglineAr",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TaglineDe",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TaglineEn",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TaglineFa",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TaglinePl",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TaglineRu",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TitleDe",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TitleFa",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TitlePl",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TitleRu",
                table: "LandingContents");

            migrationBuilder.UpdateData(
                table: "LandingContents",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "If you are looking at blank cassettes on the web, you may be very confused at the difference in price. You may see some for as low as $0.17 each.");

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 1,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Snorkelling opportunity", "Lunch" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 2,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Foam party", "Lunch", "Onboard showers" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 3,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional guide", "Lunch", "Off-road driving", "Photo stops & village visit", "Swimming stop (Hatipler)", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 4,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Rafting equipment", "Professional rafting guides", "Open-buffet & BBQ lunch", "Insurance", "Swimming breaks", "Bridged canyon passage" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 5,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional guide", "Transport Antalya–Pamukkale", "Visit to Hierapolis", "Free time at travertines", "UNESCO heritage site", "Optional Cleopatra Pool (extra)" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 6,
                column: "Services",
                value: new List<string> { "Hotel pickup (13:45) & drop-off (17:00)", "Live dolphin & sea lion show", "Seating at the venue", "Free time for photos", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 7,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Diving equipment", "Professional instructors", "2 dives (conditions permitting)", "Boat trip", "Lunch", "Insurance", "Option to join as visitor" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 8,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "River/boat cruise", "Visit Turkish Bazaar", "Visit Manavgat Waterfall", "Free time for shopping", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 9,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat trip from Adrasan", "Swimming stops", "Free time on beaches", "Captain trip customization (on request)", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 10,
                column: "Services",
                value: new List<string> { "Sauna/steam room", "Body scrub (peeling)", "Foam massage", "Aromatherapy oil massage", "Turkish tea", "Changing room & locker", "Towels", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 11,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Buggy ride", "Helmet", "Swimming/photo breaks", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 12,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Quad bike ride", "Helmet", "Swimming/photo breaks", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 13,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Evening boat cruise", "Freshly prepared dinner", "Scenic coastline views" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 14,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Visit 3 waterfalls", "Manavgat river cruise", "Visit Turkish bazaar", "Free time for shopping" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 15,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional guide", "Visit Perge", "Visit Aspendos amphitheatre", "Visit Side (Temple of Apollo)", "Visit Kursunlu Waterfalls", "Lunch" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 16,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat cruise (Grand & Little Canyon)", "Swimming stops", "Lunch at lake-view restaurant", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 17,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Guided Kaleici walking tour", "Visit Hadrian’s Gate, Clock Tower, Kesik Minare", "Visit Düden Waterfalls (Lower Düden)", "Handicraft workshop stop", "Mediterranean boat trip", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 18,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Safety briefing & equipment", "Rafting on Köprü River", "Canyoning segment", "Swimming stops", "Professional guides", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 19,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Professional instructors", "Full diving briefing", "Diving equipment", "2 dives (conditions permitting)", "Boat trip", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 20,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Return transfers", "Professional guide", "Overnight accommodation (standard, unless otherwise stated)", "Visits to valleys & rock-cut churches", "Optional hot air balloon ride (extra)" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 21,
                column: "Services",
                value: new List<string> { "Entrance to Tunnel Aquarium", "Access to thematic aquariums", "Wreck exhibits (plane & ship)", "Interactive displays" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 22,
                column: "Services",
                value: new List<string> { "Sauna", "Body peeling", "Foam massage", "Aromatherapy oil massage", "Turkish tea" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 23,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Boat trip to Kekova", "Swimming/snorkeling stops", "Lunch", "Guided visit to Myra ruins", "Visit to St. Nicholas Church", "Insurance" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 24,
                column: "Services",
                value: new List<string> { "Hotel pickup & drop-off", "Pirate boat cruise", "Lunch on board", "Swimming stops", "Stop at Phaselis (ancient city)", "Paradise Island or Mehmet Ali Bükü stop", "Alaca Water swimming/snorkeling" });
        }
    }
}
