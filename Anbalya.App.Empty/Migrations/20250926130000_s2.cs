using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anbalya.App.Empty.Migrations
{
    /// <inheritdoc />
    public partial class s2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MiniDescriptionAr",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiniDescriptionDe",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiniDescriptionEn",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiniDescriptionPe",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiniDescriptionPo",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiniDescriptionRu",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة بحرية هادئة من أنطاليا إلى كيمر: سباحة وغوص سطحي في خلجان فيروزية، غداء في أوليمبوس وإطلالات على الجزر الثلاث. يوم سهل ومشمس.", "Sanfte Bootstour von Antalya nach Kemer: Schwimmen & Schnorcheln, Mittag an der Olympos-Bucht und Ausblicke auf die Drei Inseln – entspannt und sonnig.", "Gentle cruise from Antalya to Kemer: swim & snorkel in turquoise bays, lunch at Olympos, and views of the Three Islands. Easy, sun-kissed day.", "کروز آرام از آنتالیا تا کمر: شنا و اسنورکل در آب‌های فیروزه‌ای، ناهار در الیمپوس و چشم‌انداز «سه جزیره». یک روز راحت و آفتابی.", "Spokojny rejs z Antalyi do Kemer: kąpiele i snorkeling w turkusowych zatokach, lunch w Olympos i widoki Trzech Wysp. Luźny, słoneczny dzień.", "Неспешный круиз из Анталии в Кемер: купание и сноркелинг в бирюзовых бухтах, обед в Олимпос и виды на Три острова. Лёгкий, солнечный день.", new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Snorkelling opportunity", "Lunch" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة قراصنة عائلية إلى شلالات دودان مع محطات سباحة وحفلة رغوة وغداء على المتن – يوم ممتع وسهل للجميع.", "Familienfreundliche Piratenfahrt zu den Düden-Wasserfällen mit Badestopps, Schaumparty und Mittag an Bord – entspannt & spritzig.", "Family-friendly pirate cruise to Düden Waterfalls with swim stops, foam party, and lunch on board – easy, splashy fun all day.", "کروز خانوادگی به آبشار دودن با توقف‌های شنا، پارتی کف و ناهار روی عرشه – آسان، شاد و پر آب‌تنی.", "Rodzinny rejs piracki do wodospadów Düden: kąpiele, impreza z pianą i lunch na pokładzie – lekko i pełne zabawy.", "Семейный пиратский круиз к водопадам Дюден: купание, пенная вечеринка и обед на борту – легко, ярко и весело.", new List<string> { "Hotel pickup & drop-off", "Boat cruise", "Swimming stops", "Foam party", "Lunch", "Onboard showers" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "مسارات ترابية، عبير الصنوبر، زيارة قرية وتوقف للسباحة – يوم كامل من الطرق الوعرة في جبال طوروس.", "Staubige Pisten, Pinienduft, Dorfbesuch und Badestopp – ein ganzer Offroad-Tag im Taurusgebirge.", "Dusty trails, pine-scented air, village visit and a cool swim stop – a full-day off-road escape in the Taurus Mountains.", "جاده‌های خاکی، عطر کاج، بازدید روستا و یک توقف خنک برای شنا – یک روز تمام آفرود در کوه‌های توروس.", "Piaszczyste szlaki, zapach sosen, wizyta w wiosce i kąpiel – całodniowa off-road przygoda w Taurach.", "Пыльные тропы, аромат сосен, визит в деревню и купание – насыщенный оффроуд-день в горах Тавра.", new List<string> { "Hotel pickup & drop-off", "Professional guide", "Lunch", "Off-road driving", "Photo stops & village visit", "Swimming stop (Hatipler)", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "تجديف مانافغات مع مرشدين محترفين، مناظر الوادي، محطات سباحة وغداء شواء على الضفة – أدرينالين وسط الطبيعة.", "Wildwasser auf dem Manavgat: Profibegleitung, Canyonblicke, Badestopps & BBQ am Ufer – Adrenalin pur in der Natur.", "Whitewater thrills on the Manavgat: pro guides, canyon views, swim breaks and riverside BBQ lunch – pure adrenaline in nature.", "رفتینگ ماناوگات با راهنمای حرفه‌ای، مناظر کانین، توقف‌های شنا و ناهار BBQ کنار رود – آدرنالین ناب در دل طبیعت.", "Rafting na Manavgat: profesjonaliści, widoki kanionu, przerwy na kąpiel i BBQ nad rzeką – czysta adrenalina.", "Рафтинг по Манавгату: профи-гида, виды каньона, купание и BBQ на берегу – адреналин и природа на весь день.", new List<string> { "Hotel pickup & drop-off", "Rafting equipment", "Professional rafting guides", "Open-buffet & BBQ lunch", "Insurance", "Swimming breaks", "Bridged canyon passage" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة يومية إلى باموكالي وهيرابوليس: مصاطب الترافرتين، المسرح الروماني، المتحف وحوض كليوباترا الاختياري – موقع يونسكو.", "Tagesausflug zum „Baumwollschloss“ & Hierapolis: Travertinen, römisches Theater, Museum & optionaler Kleopatra-Pool – UNESCO-Klassiker.", "Day trip to the ‘Cotton Castle’ & Hierapolis: travertine terraces, Roman theatre, museum and optional Cleopatra Pool – UNESCO classic.", "سفر روزانه به «قلعه پنبه‌ای» و هیراپولیس: تراس‌های تراورتن، تئاتر رومی، موزه و استخر اختیاری کلئوپاترا – میراث یونسکو.", "Pamukkale & Hierapolis: tarasy trawertynowe, teatr rzymski, muzeum i opcjonalny basen Kleopatry – klasyk UNESCO.", "Памуккале и Иераполис: травертины, римский театр, музей и опциональный бассейн Клеопатры – объект ЮНЕСКО.", new List<string> { "Hotel pickup & drop-off", "Professional guide", "Transport Antalya–Pamukkale", "Visit to Hierapolis", "Free time at travertines", "UNESCO heritage site", "Optional Cleopatra Pool (extra)" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "نصف يوم من المرح: أسود بحر مرحة، دلافين ذكية، عروض مذهلة وابتسامات – مع نقل من وإلى الفندق.", "Halbtägiger Familienspaß: verspielte Seelöwen, kluge Delfine, Kunststücke & Lachen – inkl. Hoteltransfer.", "Half-day fun for all ages: playful sea lions, smart dolphins, stunts and smiles with hotel pickup & drop-off.", "نیم‌روز شادی برای همه سنین: شیرهای دریایی بازیگوش، دلفین‌های باهوش و حرکات دیدنی – همراه با ترانسفر هتل.", "Pół dnia zabawy: zabawne lwy morskie, inteligentne delfiny, sztuczki i uśmiechy – z odbiorem z hotelu.", "Полдня веселья: морские львы, умные дельфины, трюки и радость – с трансфером от/до отеля.", new List<string> { "Hotel pickup (13:45) & drop-off (17:00)", "Live dolphin & sea lion show", "Seating at the venue", "Free time for photos", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "غوصتان مُوجهان في مياه صافية، أسماك ملوّنة ولمحات من الكهوف مع غداء على القارب – مناسب للمبتدئين وآمن.", "Zwei geführte Tauchgänge im glasklaren Mittelmeer, bunte Fische, Höhlen-Highlights & Mittagessen an Bord – auch für Anfänger.", "Two guided dives in crystal-clear Med waters, exotic fish, cave peek-ins and lunch on board — beginner-friendly & safe.", "دو غواصی هدایت‌شده در آب‌های شفاف مدیترانه، ماهی‌های رنگارنگ، سرک کشیدن به غارها و ناهار روی قایق – مناسب مبتدی‌ها.", "Dwa nurkowania z instruktorem w krystalicznej wodzie, egzotyczne ryby, zajrzenie do jaskiń i lunch na pokładzie – także dla początkujących.", "Два погружения в прозрачном Средиземном море, экзотические рыбы, пещеры и обед на борту — безопасно и для новичков.", new List<string> { "Hotel pickup & drop-off", "Diving equipment", "Professional instructors", "2 dives (conditions permitting)", "Boat trip", "Lunch", "Insurance", "Option to join as visitor" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة نهرية هادئة مع نسيم البحر، إطلالات طوروس، توقف عند شلال مانافغات ووقت للتسوق في البازار التركي.", "Entspannte Flussfahrt mit Meeresbrise, Taurus-Panorama, Stopp am Manavgat-Wasserfall und Bummel über den türkischen Basar.", "Easygoing river cruise with sea breeze, Taurus views, Manavgat Waterfall stop and time to wander the lively Turkish bazaar.", "کروز رودخانه‌ای راحت با نسیم دریا، مناظر توروس، توقف آبشار ماناوگات و وقت آزاد در بازار ترکی.", "Spokojny rejs rzeką: morska bryza, widoki Taurusu, przystanek przy wodospadzie Manavgat i czas na turecki bazar.", "Неспешный речной круиз: бриз, виды Тавра, остановка у водопада Манавгат и время на турецком базаре.", new List<string> { "Hotel pickup & drop-off", "River/boat cruise", "Visit Turkish Bazaar", "Visit Manavgat Waterfall", "Free time for shopping", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "هروب جزيرة إلى سولوادا: خلجان فيروزية ورمال ناعمة وأجواء مريحة – للأزواج والأصدقاء والعائلات.", "Trauminsel Suluada: türkisfarbene Buchten, feiner Sand und entspannte Vibes – für Paare, Freunde und Familien.", "Dreamy island escape to Suluada—turquoise coves, soft sands and chill vibes for couples, friends and families.", "فرار به جزیره سولوآدا؛ خلیج‌های فیروزه‌ای، شن نرم و حال‌وهوای ریلکس برای زوج‌ها، دوستان و خانواده‌ها.", "Suluada: turkusowe zatoczki, miękki piasek i totalny chill – dla par, znajomych i rodzin.", "Сулуада: бирюзовые бухты, мягкий песок и полное расслабление — для пар, друзей и семей.", new List<string> { "Hotel pickup & drop-off", "Boat trip from Adrasan", "Swimming stops", "Free time on beaches", "Captain trip customization (on request)", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "حمّام تركي أصيل في أنطاليا: ساونا، تقشير، تدليك بالرغوة والزيوت مع ختام بشاي تركي.", "Klassischer Hamam in Antalya – Sauna, Peeling, Schaummassage & Ölmassage, abgerundet mit türkischem Tee.", "Classic hammam ritual in Antalya—sauna, scrub, foam and oil massage, finished with soothing Turkish tea.", "حمام سنتی آنتالیا؛ سونا، لایه‌برداری، ماساژ کف و ماساژ روغن، با یک چای ترکی داغ برای پایان.", "Klasyczny hammam w Antalyi – sauna, peeling, masaż pianą i olejkami, na koniec turecka herbata.", "Классический хаммам в Анталье: сауна, пилинг, пенный и масляный массаж + чашка турецкого чая.", new List<string> { "Sauna/steam room", "Body scrub (peeling)", "Foam massage", "Aromatherapy oil massage", "Turkish tea", "Changing room & locker", "Towels", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "جولة باجي 3 ساعات عبر كثبان طوروس – تعليمات أمان، خوذات، توقفات للسباحة ومناظر المتوسط.", "3 Stunden Buggy-Action über Dünen im Taurus – Einweisung, Helme, Bade-Stopps und Mittelmeer-Panoramen.", "3-hour buggy blast across Taurus dunes—briefing, helmets, splash stops and big Mediterranean views.", "3 ساعت هیجان باگی روی تپه‌های شنی توروس – آموزش، کلاه ایمنی، توقف‌های آبی و مناظر مدیترانه.", "3 godziny jazdy buggy przez wydmy Taurusu – szkolenie, kaski, postoje na kąpiel i widoki na Morze Śródziemne.", "3 часа багги по дюнам Тавра — инструктаж, шлемы, водные остановки и средиземноморские виды.", new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Buggy ride", "Helmet", "Swimming/photo breaks", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "سفاري دراجات رباعية في طوروس – توجيه أمني، مسارات وعرة خلابة وتوقفات منعشة للسباحة.", "Quad-Spaß im Taurus – Sicherheitseinweisung, Schotterpisten mit Aussicht und erfrischende Badepausen.", "Throttle up a quad through Taurus trails—safety briefing, scenic dirt tracks and refreshing swim breaks.", "کوادسافاری در توروس – توضیحات ایمنی، مسیرهای آفرود دیدنی و توقف‌های خنک برای شنا.", "Quad safari w Tauruse – instruktaż, malownicze szlaki off-road i orzeźwiające postoje na kąpiel.", "Квадросафари в Тавре — брифинг по безопасности, живописные грунтовки и освежающие купания.", new List<string> { "Hotel pickup & drop-off (Kemer/Antalya)", "Safety briefing", "Quad bike ride", "Helmet", "Swimming/photo breaks", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة عشاء مسائية بهدوء البحر وإطلالات الساحل – طعام طازج وأجواء رومانسية لا تُنسى.", "Sanfter Abendkruiz mit Küstenblick & frisch zubereitetem Dinner – romantisch, entspannt, unvergesslich.", "Gentle evening cruise with coastline views and a freshly prepared dinner—romantic, relaxed, memorable.", "کروز شام آرام با منظره ساحل و شامی تازه – رمانتیک، ریلکس و ماندگار.", "Wieczorny rejs z widokiem na wybrzeże i świeżą kolacją – romantycznie, spokojnie, niezapomnianie.", "Вечерний круиз с ужином и огнями побережья — романтично, спокойно, запоминается.", new List<string> { "Hotel pickup & drop-off", "Evening boat cruise", "Freshly prepared dinner", "Scenic coastline views" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "يوم طبيعي متكامل: ثلاثة شلالات، رحلة نهرية في مانافغات ووقت في البازار التركي النابض.", "Natur pur an einem Tag: drei Wasserfälle, Manavgat-Flussfahrt und Zeit auf dem lebhaften türkischen Basar.", "Full-day nature combo: three iconic waterfalls, Manavgat river cruise and time at the lively Turkish bazaar.", "یک روزِ طبیعت: سه آبشار معروف، کروز رود ماناوگات و وقتی خوش در بازار پرجنب‌وجوش ترکی.", "Cały dzień w naturze: 3 wodospady, rejs Manavgat i czas na tętniący życiem turecki bazar.", "День природы: три водопада, речной круиз по Манавгату и прогулка по турецкому базару.", new List<string> { "Hotel pickup & drop-off", "Visit 3 waterfalls", "Manavgat river cruise", "Visit Turkish bazaar", "Free time for shopping" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة عبر الزمن: مسارح رومانية، معبد أبولو وآثار قديمة مع وقفة منعشة عند شلالات كورشونلو.", "Zeitreise: römische Theater, Apollon-Tempel, antike Ruinen und erfrischende Kursunlu-Wasserfälle.", "Time-travel day: Roman theatres, Apollo’s Temple, ancient ruins and a refreshing stop at Kursunlu Falls.", "یک روز تاریخی: تئاترهای رومی، معبد آپولو، ویرانه‌های باستانی و آبشارهای کورشونلو.", "Podróż w czasie: teatry rzymskie, świątynia Apolla i relaks przy wodospadach Kursunlu.", "День истории: римские театры, храм Аполлона, древние руины и отдых у водопада Куршунлу.", new List<string> { "Hotel pickup & drop-off", "Professional guide", "Visit Perge", "Visit Aspendos amphitheatre", "Visit Side (Temple of Apollo)", "Visit Kursunlu Waterfalls", "Lunch" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة قارب مريحة عبر الوادي الأخضر مع محطات سباحة وغداء مطل على البحيرة.", "Bootstour durch smaragdgrüne Schluchten mit Badestopps und Mittagessen am See.", "Relaxing cruise through emerald lakes and canyons with swim stops and a lake-view lunch.", "قایق‌سواری در گرین کنیون؛ شنا، آرامش و ناهار با چشم‌انداز دریاچه.", "Rejs po Zielonym Kanionie – kąpiele, relaks i obiad z widokiem na jezioro.", "Прогулка по изумрудному каньону: купание, отдых и обед с видом на озеро.", new List<string> { "Hotel pickup & drop-off", "Boat cruise (Grand & Little Canyon)", "Swimming stops", "Lunch at lake-view restaurant", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "جولة مدينة، شلالات دودان، ورشة تقليدية ورحلة بحرية متوسطية في يوم واحد.", "Altstadt-Tour, Düden-Wasserfälle, Handwerk und Bootsfahrt im Mittelmeer an einem Tag.", "Old town walk, Düden Waterfalls, handicrafts and a scenic Mediterranean boat trip – all in one day.", "گردش در شهر قدیم، آبشار دودن، صنایع دستی و کروز مدیترانه‌ای – همه در یک روز.", "Zwiedzanie starego miasta, wodospady Düden i rejs po Morzu Śródziemnym – wszystko w jeden dzień.", "Старая Анталия, водопады Дюден, ремесла и морская прогулка за один день.", new List<string> { "Hotel pickup & drop-off", "Guided Kaleici walking tour", "Visit Hadrian’s Gate, Clock Tower, Kesik Minare", "Visit Düden Waterfalls (Lower Düden)", "Handicraft workshop stop", "Mediterranean boat trip", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "يوم مليء بالأدرينالين: رافتينغ، كانيونينغ وسباحة في مياه نقية وسط جبال طوروس.", "Adrenalin pur: Rafting, Canyoning, Baden im klaren Wasser und Taurus-Landschaften.", "Adrenaline day: whitewater rafting, canyoning, swimming in crystal waters and epic Taurus landscapes.", "یک روز پرهیجان: رفتینگ، کنیونینگ، شنا در آب زلال و مناظر کوه‌های توروس.", "Dzień adrenaliny: rafting, kanioning, kąpiele i widoki gór Taurus.", "Адреналин: рафтинг, каньонинг, купание в кристальной воде и пейзажи Тавра.", new List<string> { "Hotel pickup & drop-off", "Safety briefing & equipment", "Rafting on Köprü River", "Canyoning segment", "Swimming stops", "Professional guides", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "غوص في كيمر: مياه فيروزية، أسماك ملونة وإشراف آمن للمبتدئين والمحترفين.", "Tauchen in Kemer: türkisfarbenes Wasser, bunte Fische, Felsformationen und sichere Begleitung.", "Dive into Kemer’s turquoise waters – colorful fish, rock formations and safe guidance for all levels.", "غواصی در کمر: آب‌های فیروزه‌ای، ماهی‌های رنگارنگ و آموزش ایمن برای همه.", "Nurkowanie w Kemer – turkusowa woda, kolorowe ryby i pełne wsparcie instruktorów.", "Дайвинг в Кемере: бирюзовая вода, яркие рыбы и надёжное сопровождение инструкторов.", new List<string> { "Hotel pickup & drop-off", "Professional instructors", "Full diving briefing", "Diving equipment", "2 dives (conditions permitting)", "Boat trip", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة يومين في كابادوكيا: مداخن الجن، كنائس الكهوف ورحلة بالون اختيارية عند الشروق.", "Zwei Tage Kappadokien: Feenkamine, Höhlenkirchen und optional Ballonfahrt bei Sonnenaufgang.", "Two days in magical Cappadocia: fairy chimneys, cave churches and optional sunrise balloon ride.", "۲ روز کاپادوکیا: دودکش‌های پری، کلیساهای سنگی و پرواز بالن اختیاری هنگام طلوع.", "2 dni w Kapadocji: kominy wróżek, kościoły skalne i opcjonalny lot balonem o świcie.", "2 дня в Каппадокии: фейские трубы, пещерные церкви и полёт на воздушном шаре.", new List<string> { "Hotel pickup & drop-off", "Return transfers", "Professional guide", "Overnight accommodation (standard, unless otherwise stated)", "Visits to valleys & rock-cut churches", "Optional hot air balloon ride (extra)" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "أكبر أكواريوم أنبوبي في العالم: أسماك قرش، أسماك استوائية وحطام طائرات.", "Größtes Tunnel-Aquarium der Welt: Haie, tropische Fische und spektakuläre Wracks.", "Walk through the world’s largest tunnel aquarium – sharks, tropical fish and sunken plane wrecks.", "بزرگ‌ترین آکواریوم تونلی جهان؛ کوسه‌ها، ماهی‌های استوایی و لاشه هواپیما.", "Największe akwarium tunelowe na świecie: rekiny, ryby tropikalne i wraki.", "Крупнейший тоннельный аквариум: акулы, тропические рыбы и затонувшие экспонаты.", new List<string> { "Entrance to Tunnel Aquarium", "Access to thematic aquariums", "Wreck exhibits (plane & ship)", "Interactive displays" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "ساعتان في الحمام التركي: ساونا، تقشير، تدليك بالرغوة والزيوت – استرخاء تام.", "2 Stunden Hamam: Sauna, Peeling, Schaum- und Ölmassage – Entspannung pur.", "2-hour hammam ritual with sauna, scrub, foam and oil massage – relax like never before.", "۲ ساعت حمام ترکی؛ سونا، لایه‌برداری، ماساژ کف و ماساژ روغنی – آرامش ناب.", "2-godzinny hammam: sauna, peeling, masaż pianą i olejkami – pełen relaks.", "2 часа хаммама: сауна, пилинг, пенный и масляный массаж – полное расслабление.", new List<string> { "Sauna", "Body peeling", "Foam massage", "Aromatherapy oil massage", "Turkish tea" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة يوم كامل: مدينة كيكوفا الغارقة، مقابر ليكية في ميرا وكنيسة القديس نيكولاس.", "Ganztagesausflug: versunkene Stadt Kekova, lykische Gräber in Myra und Nikolauskirche.", "Full-day trip: Kekova’s sunken city, Lycian tombs in Myra and St. Nicholas Church in Demre.", "یک روز کامل: شهر غرق‌شده ککووا، مقبره‌های لیکیایی در میرا و کلیسای سنت نیکلاس.", "Całodniowa wycieczka: zatopione miasto Kekova, grobowce w Myrze i Kościół św. Mikołaja.", "День экскурсий: затонувший город Кекова, гробницы в Мирах и церковь Николая.", new List<string> { "Hotel pickup & drop-off", "Boat trip to Kekova", "Swimming/snorkeling stops", "Lunch", "Guided visit to Myra ruins", "Visit to St. Nicholas Church", "Insurance" } });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "MiniDescriptionAr", "MiniDescriptionDe", "MiniDescriptionEn", "MiniDescriptionPe", "MiniDescriptionPo", "MiniDescriptionRu", "Services" },
                values: new object[] { "رحلة قراصنة من كيمر: أطلال فاسيليس، محطات سباحة، غداء ومتعة للعائلة.", "Piratenschiff ab Kemer: Ruinen von Phaselis, Badepausen, Mittagessen und Familienspaß.", "Pirate-style cruise from Kemer with Phaselis ruins, swim stops, lunch and family fun.", "کروز دزدان دریایی از کمر؛ خرابه‌های فاسلیس، شنا، ناهار و سرگرمی خانوادگی.", "Rejs piracki z Kemer: ruiny Phaselis, postoje na kąpiel, lunch i zabawa dla rodzin.", "Пиратский круиз из Кемера: руины Фаселиса, купания, обед и веселье для всей семьи.", new List<string> { "Hotel pickup & drop-off", "Pirate boat cruise", "Lunch on board", "Swimming stops", "Stop at Phaselis (ancient city)", "Paradise Island or Mehmet Ali Bükü stop", "Alaca Water swimming/snorkeling" } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiniDescriptionAr",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MiniDescriptionDe",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MiniDescriptionEn",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MiniDescriptionPe",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MiniDescriptionPo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MiniDescriptionRu",
                table: "Tours");

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
