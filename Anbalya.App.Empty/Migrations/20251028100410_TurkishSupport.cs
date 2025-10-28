using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anbalya.App.Empty.Migrations
{
    /// <inheritdoc />
    public partial class TurkishSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionTr",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiniDescriptionTr",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTr",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaglineTr",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleTr",
                table: "LandingContents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "LandingContents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionTr", "TaglineTr", "TitleTr" },
                values: new object[] { "Rutinden uzaklaşın: sakin tekne turları, macera dolu aktiviteler ve güneşli kaçamaklar sizi bekliyor.", "Monoton hayattan uzaklaşın", "Zihninizi rahatlatın" });

            migrationBuilder.UpdateData(
                table: "PayPalSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: 1761645849L);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Denizde huzurlu bir günün tadını çıkarın! Antalya'daki otelinizden alındıktan sonra Kemer Limanı'na transfer edilirsiniz. \nKorsan temalı tekneye binerek Türkiye Rivierası'nın turkuaz kıyıları boyunca yol alın. \nÇam ormanları ve Toros Dağları manzaraları eşliğinde Phaselis'e doğru ilerlerken berrak sularda yüzün ve şnorkelle keşfe çıkın. \nOlympos Körfezi'nde öğle yemeğinin tadını çıkarın, ardından Kemer'e dönmeden önce Üç Adalar'ın manzaralı koylarını keşfedin. \nKısa ve zahmetsiz bir tur – yüzmek, dinlenmek ve tatil bronzluğunu tamamlamak için ideal!", "Antalya'dan Kemer'e sakin tekne turu: turkuaz koylarda yüzme ve şnorkel, Olympos'ta öğle yemeği, Üç Adalar manzaraları. Kolay ve güneşli bir gün.", new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Snorkelling opportunity", "Lunch" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Denizde eğlence dolu bir gün sizi bekliyor! Antalya'daki otelinizden alındıktan sonra Antalya Limanı'na gidip geniş korsan temalı tekneye geçiyorsunuz. \nTürkiye'nin turkuaz güney kıyıları boyunca Düden Şelaleleri'ne doğru yol alırken, mağara yanındaki küçük adada yüzme molası veriyorsunuz. \nBerrak sularda yüzüp şnorkelle keşif yaparken Türk Rivierası manzaralarının keyfine varın. \nŞelalelere yaklaşırken teknede taze hazırlanmış öğle yemeği servis ediliyor. \nŞelalenin denizle buluştuğu noktada demirleyip bu etkileyici manzarayı izleyebilir, dilerseniz küçük bir tekneyle şelalenin altına kadar gidebilirsiniz. \nDönüşte güvertede köpük partisine katılın, dans edip eğlendikten sonra duş alıp Antalya Limanı'na geri dönün. \nKısa ama dopdolu bir tur – yüzme, güneşlenme ve bol kahkaha için birebir!", "Düden Şelalesi'ne korsan teknesiyle aile turu: yüzme molaları, köpük partisi ve teknede öğle yemeği. Kısa ama enerji dolu.", new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Foam party", "Lunch", "Onboard showers" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Toros Dağları'nda jeep safari ile doğayla buluşun. Otelinizden alındıktan sonra rehberinizle buluşup günün planını öğrenirsiniz. Tozlu dağ yollarından köylerin arasına ilerlerken çam kokulu havayı solur, panoramik manzaralarda fotoğraf molaları verirsiniz. Haciosmanlar köyünde geleneksel bir evi gezip kırsal yaşamı tanır, öğle yemeğinde ızgara balık, tavuk ya da omlet ile ev yapımı lezzetler tadarsınız. Ardından Akdeniz kırsalında heyecanlı off-road sürüşü ve Hatipler'de serinletici yüzme molası sizi bekler. Gün sonunda her yaşa hitap eden bu maceradan dopdolu anılarla otele dönersiniz.", "Toroslarda jeep safari: köy durakları, çam kokusu, lezzetli öğle yemeği ve Hatipler'de yüzme molası.", new List<string> { "Hotel pickup & drop-off", "Professional guide", "Lunch", "Off-road driving", "Photo stops & village visit", "Swimming stop (Hatipler)", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Manavgat Nehri'nde adrenalin dolu bir güne hazır olun! Köpüklü akıntılarda profesyonel rehberler eşliğinde rafting yaparken köprü kanyonunu geçecek, iki yakadaki milli park manzaralarına hayran kalacaksınız. \n1970'lerden beri en heyecan verici doğa sporlarından biri olan rafting, deneyimli ekibin güvenliği ve enerjisiyle daha keyifli hale geliyor. \nÖğle arasında nehir kenarında açık büfe ve mangal eşliğinde dinleniyor, serin sulara girerek şehrin stresinden uzaklaşıyorsunuz. \nHer yeni rapid sizi hem fiziksel olarak zorluyor hem de zihninizi tazeliyor; heyecan, doğa ve eğlence tek turda birleşiyor.", "Manavgat'ta rafting: köpüklü akıntılar, kanyon manzarası ve nehir kenarında BBQ. Doğada saf adrenalin.", new List<string> { "Hotel pickup & drop-off", "Rafting equipment", "Professional rafting guides", "Open-buffet & BBQ lunch", "Insurance", "Swimming breaks", "Bridged canyon passage" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Pamukkale, meaning 'Cotton Castle' in Turkish, is one of the most breathtaking natural wonders in the world – often called the 8th wonder. \nThis unique formation features natural hot springs and terraces of white carbonate minerals, visible from up to 20 km away near the town of Denizli. \n\nOn top of this dazzling white 'castle' lies the ancient city of Hierapolis. Here, you will explore remarkable sites including a Roman amphitheater, \na historical museum (once a Turkish bath), the optional Cleopatra swimming pool, the tomb of St. Philippe, and the magnificent Necropolis of Anatolia. \n\nRecognized for its natural beauty and historical value, Pamukkale and Hierapolis were added to the UNESCO World Heritage List in 1988. \nA journey full of history, culture, and natural beauty awaits you!", "Day trip to the ‘Cotton Castle’ & Hierapolis: travertine terraces, Roman theatre, museum and optional Cleopatra Pool – UNESCO classic.", new List<string> { "Hotel pickup & drop-off", "Professional guide", "Transport Antalya–Pamukkale", "Visit to Hierapolis", "Free time at travertines", "UNESCO heritage site", "Optional Cleopatra Pool (extra)" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Let the sun and surf of the Sea Lions and Dolphin Show bring out the child in you! \nThis daily excursion includes complimentary hotel pick-up at 13:45 and drop-off at 17:00. \n\nEnjoy an afternoon of fun with incredible water attractions and an unforgettable live show featuring playful sea lions and talented dolphins. \nMarvel at their amazing skills and stunts – a performance guaranteed to impress both children and adults alike. \n\nThis half-day tour is the perfect family activity, combining entertainment, relaxation, and the joy of being close to these fascinating marine animals!", "Half-day fun for all ages: playful sea lions, smart dolphins, stunts and smiles with hotel pickup & drop-off.", new List<string> { "Hotel pickup (13:45) & drop-off (17:00)", "Live dolphin & sea lion show", "Seating at the venue", "Free time for photos", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Discover the wonders beneath Turkey’s southern Mediterranean coast on this full-day scuba diving adventure. \nEnjoy recreational and professional dives at different sites, plus a delicious lunch during this 8-hour excursion from Antalya and Kemer. \n\nBoth beginners and experienced divers can explore the colorful underwater world in crystal-clear waters. \nBefore the first dive, professional instructors will provide a full briefing on diving techniques and equipment usage. \nAfter a few simple skill tests, you’ll dive into an unforgettable experience – swimming alongside exotic fish and exploring fascinating caves. \n\nSafety is always a priority, with expert instructors guiding you throughout. \nIf you prefer not to dive, you can still join the boat trip and enjoy lunch as a visitor.", "Two guided dives in crystal-clear Med waters, exotic fish, cave peek-ins and lunch on board — beginner-friendly & safe.", new List<string> { "Hotel pickup & drop-off", "Diving equipment", "Professional instructors", "2 dives (conditions permitting)", "Boat trip", "Lunch", "Insurance", "Option to join as visitor" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Embark on a river cruise adventure to discover Antalya like never before! \nYour journey begins at the Marina, where we set sail for a relaxing day surrounded by stunning scenery. \n\nDuring this peaceful cruise, take in the breathtaking beauty of Antalya’s coastline and enjoy: \n- The refreshing sea breeze \n- Spectacular views of the Taurus Mountains \n- A visit to the Turkish Bazaar and the famous Manavgat Waterfall \n- Impressive yachts and leisure boats along the way \n- Wildlife and marine life, with chances to spot jumping fish or even dolphins if you are lucky! \n\nAfter exploring the Turkish Market, sit back, relax, and soak in all the incredible views that make Antalya such a unique destination. \nAn unforgettable experience for the whole family!", "Easygoing river cruise with sea breeze, Taurus views, Manavgat Waterfall stop and time to wander the lively Turkish bazaar.", new List<string> { "Hotel pickup & drop-off", "River/boat cruise", "Visit Turkish Bazaar", "Visit Manavgat Waterfall", "Free time for shopping", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Visiting the Maldives of Turkey, Suluada, is a true bucket-list experience – and this tour will not disappoint. \nWith dreamy views, diverse marine life, and romantic sunsets, Suluada is one of the top destinations for honeymooners. \n\nLooking for romance or planning to propose? The captain knows the area perfectly and can customize the trip to make your moment unforgettable. \nBut Suluada isn’t only for couples – it’s also ideal for a girls’ getaway, a family trip, or simply anyone in search of relaxation. \n\nThis tropical island is famous for its laid-back atmosphere and exotic beauty. \nSwim, relax, and soak in the peaceful vibes of this magical destination. \n\nThe boat returns to Adrasan by 4:00 PM, where we arrange your transfer back to your hotel or the original meeting point.", "Dreamy island escape to Suluada—turquoise coves, soft sands and chill vibes for couples, friends and families.", new List<string> { "Hotel pickup & drop-off", "Boat trip from Adrasan", "Swimming stops", "Free time on beaches", "Captain trip customization (on request)", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Pamper yourself with a visit to a traditional Turkish Bath in Antalya and enjoy an experience of relaxation and renewal. \nUnwind in the soothing heated waters, let the stress melt away with a refreshing body scrub, and complete the treatment with a relaxing full-body massage. \n\nTo finish, sip a revitalizing Turkish tea and leave feeling rejuvenated, refreshed, and full of new energy. \nA perfect way to experience an authentic part of Turkish culture while taking care of your body and soul.", "Classic hammam ritual in Antalya—sauna, scrub, foam and oil massage, finished with soothing Turkish tea.", new List<string> { "Sauna/steam room", "Body scrub (peeling)", "Foam massage", "Aromatherapy oil massage", "Turkish tea", "Changing room & locker", "Towels", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Get ready for an adrenaline-filled adventure with the Antalya Buggy Safari! \nThis 3-hour expedition takes you through the stunning and ever-changing sand dunes of the Taurus Mountains. \n\nAfter hotel pick-up from Kemer, you’ll be transferred to the foothills of the rugged (and sometimes snow-capped) Taurus Mountains. \nHere, you’ll receive a full safety briefing and learn essential tips before heading out on your buggy. \n\nDrive across the desert-like terrain that separates the Mediterranean coast from the Anatolian Plateau. \nAlong the way, you’ll take breaks to swim, relax, or capture the breathtaking landscapes with your camera. \n\nAfter the ride, return to the safari center before being comfortably transferred back to your hotel in Antalya. \nA perfect mix of thrill, nature, and fun awaits you!", "3-hour buggy blast across Taurus dunes—briefing, helmets, splash stops and big Mediterranean views.", new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Buggy ride", "Helmet", "Swimming/photo breaks", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Get ready for an adrenaline-packed adventure with the Antalya Quad Safari! \nThis 3-hour expedition takes you on a thrilling ride through the ever-changing sand dunes and rugged landscapes of the Taurus Mountains. \n\nAfter hotel pick-up from Kemer, you’ll be transferred to the foothills of the Taurus Mountains, where you’ll receive a full safety briefing and learn essential riding tips before setting out on your quad bike. \n\nRide across the desert-like terrain that separates the Mediterranean coast from the Anatolian Plateau. \nAlong the way, enjoy several breaks to swim, relax, or capture the stunning scenery with your camera. \n\nAfter an unforgettable adventure, return to the safari center and then relax on your transfer back to your hotel in Antalya. \nA perfect combination of excitement, nature, and fun awaits you!", "Throttle up a quad through Taurus trails—safety briefing, scenic dirt tracks and refreshing swim breaks.", new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Quad bike ride", "Helmet", "Swimming/photo breaks", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Step aboard a dinner cruise and enjoy a magical evening on the waters of Antalya. \nAs the boat gently sails, take in the beautiful sights of the city’s coastline illuminated in the evening light. \n\nRelax on deck, feel the soft sea breeze, and enjoy a freshly prepared dinner served while you cruise. \nThe calm atmosphere, delicious food, and scenic views create the perfect recipe for a memorable night out in Antalya. \n\nAn ideal choice for couples, friends, or families who want to combine dining with a unique cruising experience.", "Gentle evening cruise with coastline views and a freshly prepared dinner—romantic, relaxed, memorable.", new List<string> { "Hotel pickup & drop-off", "Evening boat cruise", "Freshly prepared dinner", "Scenic coastline views" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "A visit to Antalya isn’t complete without exploring its famous waterfalls! \nOn this full-day excursion, discover 3 of Antalya’s most beautiful waterfalls and enjoy the breathtaking natural scenery. \n\nThe tour also includes a relaxing Manavgat river cruise, where you can sit back and enjoy the views, and a shopping experience at the lively Turkish bazaar. \nThis combination of nature, culture, and local life makes for a truly memorable day in Antalya. \n\nPerfect for families, friends, or solo travelers who want to experience more than just the beaches!", "Full-day nature combo: three iconic waterfalls, Manavgat river cruise and time at the lively Turkish bazaar.", new List<string> { "Hotel pickup & drop-off", "Visit 3 waterfalls", "Manavgat river cruise", "Visit Turkish bazaar", "Free time for shopping" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Travel back in time with a full-day excursion to some of the most fascinating historical and natural sights near Antalya. \n\nBegin with Perge, located just 15 km from Antalya. According to legend, it was founded after the Trojan War by the prophet Calchas. \nToday, it is one of the region’s most impressive archaeological sites, featuring Roman-era ruins such as an amphitheater, stadium, city walls, agora, basilica, Roman baths, and more. \nStroll along the 500-meter-long main street lined with columns and visit the acropolis and ancient fountain. \n\nContinue to Aspendos, one of Pamphylia’s most important cities. Its world-famous Roman amphitheater, built in 155 AD, is considered one of the best-preserved examples of Roman theatre architecture and still hosts cultural events and festivals today. \nClimb the steps, admire the galleries, and imagine the grand performances that still bring the theatre to life. \n\nAfter lunch, head to Side, a spectacular ancient city on a small peninsula. \nHere you can explore ruins from the Hellenistic, Roman, and Byzantine eras, including the agora, amphitheater, Roman baths, basilica, and the stunning Temple of Apollo overlooking the turquoise sea. \n\nFinish the day with a visit to the beautiful Kursunlu Waterfalls Natural Park. \nEnjoy the fresh pine-scented air, shaded walking paths, turquoise pools, and cascading waterfalls, making this the perfect place to relax after a day of exploration. \nDon’t forget to capture a photo behind the waterfall for an unforgettable memory. \n\nA journey combining history, culture, and natural beauty – an Antalya must-do!", "Time-travel day: Roman theatres, Apollo’s Temple, ancient ruins and a refreshing stop at Kursunlu Falls.", new List<string> { "Hotel pickup & drop-off", "Professional guide", "Visit Perge", "Visit Aspendos amphitheatre", "Visit Side (Temple of Apollo)", "Visit Kursunlu Waterfalls", "Lunch" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Enjoy a full-day boat trip into the heart of nature at the beautiful Green Canyon. \nYour day begins with a pick-up from your hotel and a bus ride to the canyon, where the adventure starts. \n\nCruise through the 14-kilometer-long Grand Canyon and the 3-kilometer-long Little Canyon, with stops along the way for swimming in the refreshing waters. \nAdmire the emerald-green lake surrounded by lush landscapes, and keep an eye out for the rare brown fish owl! \n\nLater, enjoy a swimming break and a delicious lunch at a local restaurant overlooking the lake. \nAfter lunch, continue exploring the other side of the canyon, where stunning views of the mountains and woodlands await. \n\nEnd the day with another swim stop before returning comfortably to your resort. \nA relaxing yet unforgettable adventure into Antalya’s natural beauty.", "Relaxing cruise through emerald lakes and canyons with swim stops and a lake-view lunch.", new List<string> { "Hotel pickup & drop-off", "Boat cruise (Grand & Little Canyon)", "Swimming stops", "Lunch at lake-view restaurant", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Discover the best of Antalya in one full-day city tour that combines history, culture, nature, and relaxation. \nYour day begins around 09:00 with a visit to Kaleici – Antalya’s charming old town. \nFollow your guide to see highlights such as Hadrian’s Gate, the Clock Tower, Kesik Minare, and the impressive city walls from the 2nd century. \nYou’ll also have time to wander through boutiques, art galleries, and small museums in this historic area. \n\nNext, enjoy a visit to the Düden Waterfalls National Park. \nWalk through the lush park to admire the spectacular Karpuzkaldıran (Lower Düden) Waterfall, also called the Alexander Falls. \nAt 30 meters high, it is the tallest cascade in the Antalya region and offers stunning views over the coastline. \n\nThe tour continues with a stop at a traditional handicraft workshop. \nHere you’ll learn about centuries-old Turkish craftsmanship, still carried on by small family businesses in Antalya. \n\nFinally, end the day with a relaxing boat trip on the Mediterranean Sea. \nSoak in the refreshing breeze and enjoy breathtaking panoramic views of Antalya’s old town and sparkling coastline. \nA perfect mix of culture, history, and natural beauty!", "Old town walk, Düden Waterfalls, handicrafts and a scenic Mediterranean boat trip – all in one day.", new List<string> { "Hotel pickup & drop-off", "Guided Kaleici walking tour", "Visit Hadrian’s Gate, Clock Tower, Kesik Minare", "Visit Düden Waterfalls (Lower Düden)", "Handicraft workshop stop", "Mediterranean boat trip", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Experience an action-packed full-day adventure in Koprulu Canyon National Park. \nGet your adrenaline pumping as you raft through the thrilling whitewater rapids of the Kopru River and explore the stunning natural beauty of the canyon. \n\nYour journey begins with hotel pick-up from Antalya and nearby resorts before heading into the heart of the Taurus Mountains. \nProfessional guides provide a safety briefing and equipment, ensuring you are ready for an unforgettable experience. \n\nRaft along the turquoise waters surrounded by steep canyon walls, dramatic cliffs, and lush pine forests. \nEnjoy swimming breaks in crystal-clear water and take in the breathtaking scenery. \n\nAdd even more excitement with canyoning, as you climb, swim, and hike through narrow gorges and waterfalls. \nWhether you’re a beginner or an adventure lover, this day guarantees thrills, laughter, and memories to last a lifetime. \n\nA must-do for anyone looking for adrenaline and natural beauty in Antalya!", "Adrenaline day: whitewater rafting, canyoning, swimming in crystal waters and epic Taurus landscapes.", new List<string> { "Hotel pickup & drop-off", "Safety briefing & equipment", "Rafting on Köprü River", "Canyoning segment", "Swimming stops", "Professional guides", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Discover the magic of the Mediterranean Sea with a scuba diving trip from Kemer. \nMarvel at the vibrant underwater life, swim alongside colorful fish, and explore fascinating rock formations beneath the surface. \n\nWhether you’re a beginner or an experienced diver, professional instructors will provide full guidance and ensure your safety throughout the experience. \nAfter a short briefing and practice, dive into the clear turquoise waters for an unforgettable adventure. \n\nA perfect choice for anyone who wants to explore the hidden beauty of the sea and enjoy a day filled with excitement and relaxation.", "Dive into Kemer’s turquoise waters – colorful fish, rock formations and safe guidance for all levels.", new List<string> { "Hotel pickup & drop-off", "Professional instructors", "Full diving briefing", "Diving equipment", "2 dives (conditions permitting)", "Boat trip", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Embark on an unforgettable 2-day guided tour to Cappadocia, one of the most unique regions in the world. \nJourney through extraordinary landscapes filled with unusual rock formations, valleys, and the famous 'fairy chimneys'. \n\nStep inside ancient rock-cut churches and cave dwellings that tell the stories of early civilizations. \nLearn about the fascinating history and culture of this magical land from your expert guide. \n\nFor the ultimate Cappadocia experience, take the opportunity to join an optional hot air balloon ride. \nSoar over the breathtaking valleys and fairy chimneys at sunrise for a once-in-a-lifetime view. \n\nA perfect combination of history, culture, and natural wonder – this 2-day trip will leave you with unforgettable memories of Turkey’s most enchanting destination.", "Two days in magical Cappadocia: fairy chimneys, cave churches and optional sunrise balloon ride.", new List<string> { "Hotel pickup & drop-off", "Return transfers", "Professional guide", "Overnight accommodation (standard, unless otherwise stated)", "Visits to valleys & rock-cut churches", "Optional hot air balloon ride (extra)" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Come face to face with the most magical marine creatures at Antalya’s Tunnel Aquarium – the world’s largest tunnel aquarium! \nHome to over 250 species and 40 thematic aquariums, it offers an unforgettable journey through the underwater world. \n\nSee real sharks swimming above and around you, explore colorful tropical fish, and admire the sheer-size plane and ship wrecks displayed inside. \nOne highlight is the wreck of a 1935 Italian Savoia-Marchetti SM79 'Sparviero' bomber, which was hit and sank off the coast of Meis. \n\nEnjoy interactive exhibits that explore the Mediterranean’s diverse marine life, and even take part in unique experiences like feeding stingrays, koi fish, or sharks. \nCapture unforgettable moments with rare species, or simply marvel at the wonders of the underwater realm. \n\nA perfect attraction for families, friends, and curious travelers alike – fun, educational, and awe-inspiring!", "Walk through the world’s largest tunnel aquarium – sharks, tropical fish and sunken plane wrecks.", new List<string> { "Entrance to Tunnel Aquarium", "Access to thematic aquariums", "Wreck exhibits (plane & ship)", "Interactive displays" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Take some time for yourself and enjoy the timeless oriental tradition of a Turkish Bath in Antalya – an experience not to be missed! \nIn just 2 hours, this ritual of relaxation and rejuvenation will leave you refreshed and ready for more sightseeing. \n\nThe history of the hammam goes back centuries, inspired by Greek and Roman bathing practices, and has always been more than just skin cleansing. \nIt’s about the unique atmosphere and total experience that combines body, mind, and spirit. \n\nThis Turkish bath package includes a sauna, body peeling, foam massage, and a soothing aromatherapy oil massage. \nThe combination will make your body feel revitalized and your spirit renewed. \n\nIndulge yourself during your holiday with a body scrub and soap massage – one of Turkey’s most unique and pleasurable traditions.", "2-hour hammam ritual with sauna, scrub, foam and oil massage – relax like never before.", new List<string> { "Sauna", "Body peeling", "Foam massage", "Aromatherapy oil massage", "Turkish tea" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Embark on a full-day journey to Kekova, Myra, and the Church of St. Nicholas for a perfect mix of history, culture, and natural beauty. \n\nStart with a scenic boat trip to Kekova, one of the most picturesque yachting destinations in the region. \nHere, marvel at the extraordinary underwater ruins of the sunken city of Simena and visit the ancient Lycian necropolis at Teimiussa. \nThere’s also time to swim and snorkel in the turquoise waters, so don’t forget your bathing suit! \n\nContinue by land to Myra, one of the six major cities of ancient Lycia. \nEnjoy a delicious lunch before exploring its impressive archaeological remains, including the Acropolis, Roman Theater, and Baths. \nAdmire the Lycian rock tombs carved into towering cliffs – a breathtaking sight. \n\nThe final stop is the Church of St. Nicholas in Myra. \nSt. Nicholas, the 4th-century Bishop of Myra, later became world-renowned as Father Christmas (Santa Claus). \nThe restored church offers a fascinating glimpse into this important historical figure. \n\nAfter your visit, return comfortably to Antalya with unforgettable memories of history and legend combined.", "Full-day trip: Kekova’s sunken city, Lycian tombs in Myra and St. Nicholas Church in Demre.", new List<string> { "Hotel pickup & drop-off", "Boat trip to Kekova", "Swimming/snorkeling stops", "Lunch", "Guided visit to Myra ruins", "Visit to St. Nicholas Church", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DescriptionTr", "MiniDescriptionTr", "Services" },
                values: new object[] { "Set sail on a full-day pirate boat adventure from Kemer Marina! \nAfter hotel pick-up and transfer to the harbor, board your pirate-style boat for a day filled with history, relaxation, and fun on the water. \n\nYour first destination is the ancient city of Phaselis, once a major trading hub with 3 harbors. \nHere you can explore fascinating ruins such as aqueducts, the agora, Roman baths, and an ancient theater. \nThe bay’s pebble beaches and calm, shallow waters make it a perfect place to swim. \n\nAfter enjoying lunch at the bay, the journey continues to Paradise Island or Mehmet Ali Bükü, where you can take a refreshing swim. \nThe final stop is at Alaca Water, a beautiful inlet ideal for swimming and snorkeling before sailing back to Kemer. \n\nA fantastic blend of history, nature, and leisure – perfect for families and travelers of all ages!", "Pirate-style cruise from Kemer with Phaselis ruins, swim stops, lunch and family fun.", new List<string> { "Hotel pickup & drop-off", "Pirate boat cruise", "Lunch on board", "Swimming stops", "Stop at Phaselis (ancient city)", "Paradise Island or Mehmet Ali Bükü stop", "Alaca Water swimming/snorkeling" } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionTr",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MiniDescriptionTr",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DescriptionTr",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TaglineTr",
                table: "LandingContents");

            migrationBuilder.DropColumn(
                name: "TitleTr",
                table: "LandingContents");

            migrationBuilder.UpdateData(
                table: "PayPalSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: 1761598172L);

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
