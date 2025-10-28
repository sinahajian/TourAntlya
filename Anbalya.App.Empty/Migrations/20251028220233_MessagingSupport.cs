using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Anbalya.App.Empty.Migrations
{
    /// <inheritdoc />
    public partial class MessagingSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UpdatedTime = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateKey = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmtpSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Host = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    EnableSsl = table.Column<bool>(type: "boolean", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FromEmail = table.Column<string>(type: "text", nullable: false),
                    FromName = table.Column<string>(type: "text", nullable: false),
                    NotificationEmail = table.Column<string>(type: "text", nullable: false),
                    ReplyToEmail = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmtpSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "Body", "CreationTime", "Language", "Subject", "TemplateKey" },
                values: new object[,]
                {
                    { 1, "Hello {FullName},\\n\\nThank you for your message. Our team will get back to you shortly.\\n\\nBest regards,\\nTour Antalya", 1735689600L, null, "Thank you for contacting Tour Antalya", "ContactUser" },
                    { 2, "A new contact request has been submitted.\\n\\nName: {FullName}\\nEmail: {Email}\\nMessage:\\n{Message}", 1735689600L, null, "New contact request from {FullName}", "ContactAdmin" },
                    { 3, "Hello {FullName},\\n\\nThank you for booking {TourName} with Tour Antalya. We will confirm the details soon.\\n\\nReservation ID: {ReservationId}\\nPreferred date: {PreferredDate}\\n\\nBest regards,\\nTour Antalya", 1735689600L, null, "Reservation received – {TourName}", "ReservationUser" },
                    { 4, "A new reservation has been placed.\\n\\nTour: {TourName}\\nCustomer: {FullName}\\nEmail: {Email}\\nPhone: {Phone}\\nReservation ID: {ReservationId}\\nPreferred date: {PreferredDate}\\nAdults: {Adults}\\nChildren: {Children}\\nInfants: {Infants}", 1735689600L, null, "New reservation – {TourName}", "ReservationAdmin" }
                });

            migrationBuilder.UpdateData(
                table: "PayPalSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: 1761688953L);

            migrationBuilder.InsertData(
                table: "SmtpSettings",
                columns: new[] { "Id", "CreationTime", "EnableSsl", "FromEmail", "FromName", "Host", "NotificationEmail", "Password", "Port", "ReplyToEmail", "Username" },
                values: new object[] { 1, 1735689600L, true, "no-reply@tourantalya.com", "Tour Antalya", "", "hello@tourantalya.com", "", 587, null, "" });

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

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_TemplateKey_Language",
                table: "EmailTemplates",
                columns: new[] { "TemplateKey", "Language" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "SmtpSettings");

            migrationBuilder.UpdateData(
                table: "PayPalSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: 1761686927L);

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
