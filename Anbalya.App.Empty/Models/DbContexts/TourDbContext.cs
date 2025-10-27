
using System;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
namespace Models.DbContexts
{

    public class TourDbContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<LandingContent> LandingContents { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<PaymentOption> PaymentOptions { get; set; }
        public TourDbContext(DbContextOptions<TourDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().HasData(new Manager() { Id = 1, Name = "name", UserName = "Sina", Password = "12sina122", UserEmail = "Sina.hajian@gmail.com" });

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Tour)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PaymentOption>()
                .HasIndex(p => p.Method)
                .IsUnique();

            var paymentSeedTimestamp = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

            modelBuilder.Entity<PaymentOption>().HasData(
                new PaymentOption
                {
                    Id = 1,
                    Method = PaymentMethod.PayPal,
                    DisplayName = "PayPal",
                    AccountIdentifier = "paypal@example.com",
                    Instructions = "Send the payment to the PayPal account above and include your reservation ID in the notes.",
                    IsEnabled = true,
                    UpdatedAt = paymentSeedTimestamp
                },
                new PaymentOption
                {
                    Id = 2,
                    Method = PaymentMethod.Visa,
                    DisplayName = "Visa Card",
                    AccountIdentifier = "**** **** **** 4242",
                    Instructions = "Contact our team to complete the Visa payment securely.",
                    IsEnabled = true,
                    UpdatedAt = paymentSeedTimestamp
                },
                new PaymentOption
                {
                    Id = 3,
                    Method = PaymentMethod.Revolut,
                    DisplayName = "Revolut",
                    AccountIdentifier = "REVOLUT-12345678",
                    Instructions = "Use Revolut transfer and note your reservation ID for quick confirmation.",
                    IsEnabled = true,
                    UpdatedAt = paymentSeedTimestamp
                });

            modelBuilder.Entity<Foto>().HasOne(t => t.Tour).WithMany(t => t.Fotos).HasForeignKey(t => t.TourId);
            modelBuilder.Entity<LandingContent>()
                .HasIndex(c => c.Language)
                .IsUnique();
            modelBuilder.Entity<LandingContent>().HasData(
                new LandingContent
                {
                    Id = 1,
                    Language = "en",
                    Tagline = "Away from monotonous life",
                    Title = "Relax Your Mind",
                    Description = "Step away from routine: gentle cruises, adventures, and sunny escapes are ready for you.",
                    TaglineEn = "Away from monotonous life",
                    TitleEn = "Relax Your Mind",
                    DescriptionEn = "Step away from routine: gentle cruises, adventures, and sunny escapes are ready for you.",
                    TaglineDe = "Raus aus dem Alltag",
                    TitleDe = "Entspann deinen Geist",
                    DescriptionDe = "Lass den Alltag hinter dir: sanfte Bootstouren, Naturerlebnisse und Ausflüge voller Sonne warten bereits auf dich.",
                    TaglineFa = "دور از زندگی یکنواخت",
                    TitleFa = "ذهن خود را آرام کنید",
                    DescriptionFa = "با تورهای ما از شلوغی دور شوید؛ سفرهایی آرام، ماجراجویانه و سرشار از تجربه‌های تازه.",
                    TaglineRu = "Подальше от монотонной жизни",
                    TitleRu = "Расслабьте свой разум",
                    DescriptionRu = "Устройте себе отдых: море, приключения и новые впечатления ждут вас каждый день.",
                    TaglinePl = "Z dala od monotonii",
                    TitlePl = "Zrelaksuj swój umysł",
                    DescriptionPl = "Odpocznij od codzienności: czekają na Ciebie rejsy, przygody i chwile czystego relaksu.",
                    TaglineAr = "ابتعد عن الحياة الرتيبة",
                    TitleAr = "أرخِ ذهنك",
                    DescriptionAr = "امنح نفسك استراحة حقيقية: رحلات بحرية، مغامرات وتجارب مميزة تلائم كل الأذواق.",
                    BackgroundImage = null,
                    CreationTime = 0
                }
            );
            modelBuilder.Entity<Tour>().HasData(


new Tour(
    id: 1,
    name: "Antalya Lazy Day Boat Trip",
    price: 47,
    kinderPrice: 24,
    infantPrice: 0,
    category: Category.Relaxing,
    locLat: 0f,
    locLon: 0f,

    descriptionEn:
@"Enjoy a relaxing day at sea! After pick-up from your hotel in Antalya, you will be transferred to Kemer Marina. 
Step aboard a pirate-style boat and cruise along the turquoise coast of the Turkish Riviera. 
Sail towards Phaselis with views of pine forests and the Taurus Mountains. Swim and snorkel in clear-blue waters, 
then enjoy lunch at Olympos Bay. Later, explore the scenic Three Islands before heading back to Kemer Marina. 
A short, easy trip – perfect for swimming, relaxing, and working on your holiday tan!",

    descriptionDe:
@"Ein entspannter Tag auf dem Meer! Nach der Abholung von Ihrem Hotel in Antalya geht es zum Hafen von Kemer. 
Gehen Sie an Bord eines Piratenschiffes und fahren Sie entlang der türkisfarbenen Riviera. 
Auf dem Weg nach Phaselis genießen Sie den Blick auf Pinienwälder und das Taurusgebirge. 
Machen Sie Halt zum Schwimmen und Schnorcheln im klaren Wasser und essen Sie zu Mittag in der Olympos-Bucht. 
Anschließend geht es weiter zu den malerischen Drei Inseln, bevor Sie zurück nach Kemer fahren. 
Eine kurze, leichte Tour – ideal zum Baden, Entspannen und Sonnen!",

    descriptionRu:
@"Расслабляющий день на море! После трансфера из отеля в Анталии вас ждёт поездка в марину Кемер. 
Садитесь на пиратский корабль и отправляйтесь в круиз вдоль бирюзового побережья Турецкой Ривьеры. 
По пути к Фаселису вы увидите сосновые леса и горы Тавра. Возможность поплавать и понырять в чистейшей воде, 
обед в бухте Олимпос и прогулка к Трём островам. 
Лёгкая и приятная экскурсия – для купания, отдыха и идеального загара!",

    descriptionPo:
@"Relaksujący dzień na morzu! Po odebraniu z hotelu w Antalyi zostaniesz przewieziony do portu w Kemer. 
Wejdź na statek piracki i popłyń wzdłuż turkusowego wybrzeża Riwiery Tureckiej. 
Podczas rejsu do Phaselis zobaczysz sosnowe lasy i góry Taurus. 
Czeka Cię kąpiel i snorkeling w krystalicznie czystej wodzie, obiad w zatoce Olympos oraz wizyta przy Trzech Wyspach. 
Krótka i łatwa wycieczka – idealna na pływanie, relaks i złapanie opalenizny!",

    descriptionPe:
@"یک روز آرامش‌بخش در دریا! پس از ترانسفر از هتل شما در آنتالیا به بندر کمر می‌روید. 
سوار کشتی دزدان دریایی شوید و در امتداد سواحل فیروزه‌ای ریویرا ترکیه کروز کنید. 
در مسیر به فاسلیس، از مناظر جنگل‌های کاج و کوه‌های توروس لذت ببرید. 
فرصت شنا و غواصی در آب‌های زلال خواهید داشت و ناهار را در خلیج الیمپوس صرف می‌کنید. 
سپس به سمت منطقه سه جزیره ادامه داده و در نهایت به بندر کمر بازمی‌گردید. 
یک روز کوتاه و ساده – اما کافی برای شنا، استراحت و برنزه شدن!",

    descriptionAr:
@"استمتع بيوم مريح في البحر! بعد اصطحابك من فندقك في أنطاليا، تنتقل إلى مرسى كيمر. 
اصعد على متن سفينة القراصنة وأبحر على طول الساحل الفيروزي للريفييرا التركية. 
في الطريق إلى فاسيليس ستشاهد غابات الصنوبر وجبال طوروس. 
لديك فرصة رائعة للسباحة والغوص في المياه الصافية، ثم تناول الغداء في خليج أوليمبوس. 
بعدها استمتع بجولة عند الجزر الثلاث قبل العودة إلى مرسى كيمر. 
رحلة قصيرة وسهلة – مثالية للسباحة والاسترخاء واكتساب سمرة رائعة!",

    miniDescriptionEn: "Gentle cruise from Antalya to Kemer: swim & snorkel in turquoise bays, lunch at Olympos, and views of the Three Islands. Easy, sun-kissed day.",
    miniDescriptionDe: "Sanfte Bootstour von Antalya nach Kemer: Schwimmen & Schnorcheln, Mittag an der Olympos-Bucht und Ausblicke auf die Drei Inseln – entspannt und sonnig.",
    miniDescriptionRu: "Неспешный круиз из Анталии в Кемер: купание и сноркелинг в бирюзовых бухтах, обед в Олимпос и виды на Три острова. Лёгкий, солнечный день.",
    miniDescriptionPo: "Spokojny rejs z Antalyi do Kemer: kąpiele i snorkeling w turkusowych zatokach, lunch w Olympos i widoki Trzech Wysp. Luźny, słoneczny dzień.",
    miniDescriptionPe: "کروز آرام از آنتالیا تا کمر: شنا و اسنورکل در آب‌های فیروزه‌ای، ناهار در الیمپوس و چشم‌انداز «سه جزیره». یک روز راحت و آفتابی.",
    miniDescriptionAr: "رحلة بحرية هادئة من أنطاليا إلى كيمر: سباحة وغوص سطحي في خلجان فيروزية، غداء في أوليمبوس وإطلالات على الجزر الثلاث. يوم سهل ومشمس.",

    fotos: new List<Foto>
    {
        new Foto("/tourimage/Antalya Lazy Day Boat Trip.jpg")
    },
    activeDay: 1111111,
    durationHours: 8,
    services: new List<string>
    {
        "Hotel pickup & drop-off",
        "Boat cruise",
        "Swimming stops",
        "Snorkelling opportunity",
        "Lunch"
    }
)
,
new Tour(
    id: 2,
    name: "Antalya Pirate Boat Trip",
    price: 34,
    kinderPrice: 20,
    infantPrice: 0,
    category: Category.Family,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Enjoy a fun day at sea! After pick-up from your hotel in Antalya, transfer to Antalya Marina and board a spacious pirate-style boat. 
Cruise along the turquoise south coast of Turkey and head towards the famous Düden Waterfalls. 
On the way, stop by an island for a swimming break at a cosy beach next to a cave. 
Swim and snorkel in crystal-clear waters while enjoying the views of the Turkish Riviera. 
A freshly prepared lunch will be served on board as you sail to the waterfalls. 
Once there, drop anchor and admire the breathtaking sight where the waterfalls meet the sea. 
Take a swim here or optionally rent a smaller boat to go under the falls. 
Afterwards, join the onboard foam party – dance, have fun, and rinse off with a shower before sailing back to Antalya Marina. 
A short, easy day – but long enough for swimming, sunbathing, and plenty of fun!",
    descriptionDe:
@"Ein erlebnisreicher Tag auf dem Meer! Nach der Abholung von Ihrem Hotel in Antalya fahren Sie zum Hafen von Antalya und gehen an Bord eines geräumigen Piratenschiffes. 
Kreuzen Sie entlang der türkisfarbenen Südküste der Türkei in Richtung der berühmten Düden-Wasserfälle. 
Unterwegs gibt es einen Stopp auf einer kleinen Insel mit Bademöglichkeit an einem gemütlichen Strand neben einer Höhle. 
Genießen Sie das Schwimmen und Schnorcheln im klaren Wasser mit herrlichem Blick auf die Türkische Riviera. 
An Bord wird Ihnen ein frisch zubereitetes Mittagessen serviert, während Sie zu den Wasserfällen fahren. 
Vor Ort ankern Sie und bewundern das beeindruckende Naturschauspiel, wo die Wasserfälle ins Meer stürzen. 
Sie können hier schwimmen oder optional mit einem kleinen Boot direkt unter die Wasserfälle fahren. 
Danach erwartet Sie eine Schaumparty an Bord – tanzen, Spaß haben und sich anschließend abduschen, bevor es zurück zum Hafen von Antalya geht. 
Ein kurzer, entspannter Ausflug – aber lang genug zum Baden, Sonnen und Feiern!",
    descriptionRu:
@"Весёлый день на море! После трансфера из отеля в Анталии вас доставят в марину Анталии, где вы сядете на просторный пиратский корабль. 
Совершите круиз вдоль бирюзового южного побережья Турции и направляйтесь к знаменитым водопадам Дюден. 
По пути остановка на острове с купанием на уютном пляже возле пещеры. 
Наслаждайтесь купанием и сноркелингом в кристально чистой воде с видами на Турецкую Ривьеру. 
На борту вам подадут свежеприготовленный обед во время плавания к водопадам. 
По прибытии – якорная стоянка и потрясающий вид, где водопады встречаются с морем. 
Купайтесь здесь или дополнительно возьмите лодку, чтобы подплыть прямо под водопады. 
Затем присоединяйтесь к пенной вечеринке на борту – танцы, веселье и душ перед возвращением в марину Анталии. 
Короткая и лёгкая прогулка – но достаточно длинная для купания, загара и настоящего веселья!",
    descriptionPo:
@"Dzień pełen zabawy na morzu! Po odebraniu z hotelu w Antalyi zostaniesz przewieziony do portu w Antalyi i wejdziesz na pokład przestronnego statku pirackiego. 
Popłyniesz wzdłuż turkusowego, południowego wybrzeża Turcji w kierunku słynnych wodospadów Düden. 
Po drodze zatrzymasz się na wyspie, aby popływać przy przytulnej plaży obok jaskini. 
Ciesz się pływaniem i snorkelingiem w krystalicznie czystej wodzie z pięknymi widokami na Riwierę Turecką. 
Na pokładzie zostanie podany świeżo przygotowany lunch podczas rejsu do wodospadów. 
Na miejscu zakotwiczysz i podziwiać będziesz niesamowity widok, gdzie wodospady wpadają prosto do morza. 
Możesz tu popływać lub opcjonalnie wynająć mniejszą łódkę, aby podpłynąć pod wodospady. 
Następnie baw się na pokładowej imprezie z pianą – tańcz, baw się i skorzystaj z prysznica przed powrotem do portu w Antalyi. 
Krótka, łatwa wycieczka – ale wystarczająco długa, by popływać, opalić się i dobrze się bawić!",
    descriptionPe:
@"یک روز پرهیجان در دریا! پس از ترانسفر از هتل‌تان در آنتالیا، به بندر آنتالیا می‌روید و سوار یک کشتی بزرگ سبک دزدان دریایی می‌شوید. 
در امتداد سواحل فیروزه‌ای جنوب ترکیه به سمت آبشارهای مشهور دودن حرکت می‌کنید. 
در مسیر، در یک جزیره توقف کرده و در ساحل دنج کنار غار شنا خواهید کرد. 
شنا و غواصی در آب‌های شفاف همراه با مناظر زیبای ریویرا ترکیه تجربه‌ای فراموش‌نشدنی خواهد بود. 
ناهار تازه آماده شده روی عرشه سرو می‌شود، در حالی که به سمت آبشارها حرکت می‌کنید. 
به مقصد که رسیدید، کشتی لنگر می‌اندازد و می‌توانید از منظره فوق‌العاده برخورد آبشارها با دریا لذت ببرید. 
اینجا می‌توانید شنا کنید یا با قایق کوچک (به صورت انتخابی) زیر آبشار بروید. 
بعد از آن، یک مهمانی کف شاد روی کشتی برگزار می‌شود – برقصید، خوش بگذرانید و قبل از بازگشت به بندر دوش بگیرید. 
یک روز کوتاه و آسان – اما کافی برای شنا، آفتاب گرفتن و کلی شادی!",
    descriptionAr:
@"يوم ممتع في البحر! بعد اصطحابك من فندقك في أنطاليا، يتم نقلك إلى مرسى أنطاليا حيث تصعد على متن سفينة واسعة على طراز القراصنة. 
أبحر على طول الساحل الجنوبي الفيروزي لتركيا متجهاً نحو شلالات دودان الشهيرة. 
في الطريق، تتوقف عند جزيرة للسباحة على شاطئ مريح بجانب كهف. 
استمتع بالسباحة والغوص في المياه الصافية مع مناظر خلابة للريفييرا التركية. 
يتم تقديم غداء طازج على متن السفينة أثناء الإبحار نحو الشلالات. 
عند الوصول، نلقي المرساة لمشاهدة المشهد المذهل حيث تلتقي الشلالات بالبحر. 
يمكنك السباحة هنا أو استئجار قارب صغير (نشاط اختياري) للذهاب تحت الشلالات. 
بعدها، استمتع بحفلة رغوة على متن السفينة – ارقص، استمتع، وخذ دشاً قبل العودة إلى مرسى أنطاليا. 
رحلة قصيرة وسهلة – لكنها كافية للسباحة، الاسترخاء، وحفلة مليئة بالمرح!",

    // Mini summaries
    miniDescriptionEn: "Family-friendly pirate cruise to Düden Waterfalls with swim stops, foam party, and lunch on board – easy, splashy fun all day.",
    miniDescriptionDe: "Familienfreundliche Piratenfahrt zu den Düden-Wasserfällen mit Badestopps, Schaumparty und Mittag an Bord – entspannt & spritzig.",
    miniDescriptionRu: "Семейный пиратский круиз к водопадам Дюден: купание, пенная вечеринка и обед на борту – легко, ярко и весело.",
    miniDescriptionPo: "Rodzinny rejs piracki do wodospadów Düden: kąpiele, impreza z pianą i lunch na pokładzie – lekko i pełne zabawy.",
    miniDescriptionPe: "کروز خانوادگی به آبشار دودن با توقف‌های شنا، پارتی کف و ناهار روی عرشه – آسان، شاد و پر آب‌تنی.",
    miniDescriptionAr: "رحلة قراصنة عائلية إلى شلالات دودان مع محطات سباحة وحفلة رغوة وغداء على المتن – يوم ممتع وسهل للجميع.",

    fotos: new List<Foto> {
        new Foto("/tourimage/Antalya Pirate Boat Trip.jpg")
    },
    activeDay: 1111111,
    durationHours: 7,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Boat cruise",
        "Swimming stops",
        "Foam party",
        "Lunch",
        "Onboard showers"
    }
),
new Tour(
    id: 3,
    name: "Jeep Safari and Off-road Adventures",
    price: 47,
    kinderPrice: 26,
    infantPrice: 0,
    category: Category.Adventure,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Set off on a thrilling jeep safari into the Taurus Mountains and the Turkish countryside on this full-day adventure. 
After pick-up from your hotel, you’ll be driven to the starting point where you will meet your guide and get a briefing on the day’s program. 
Get ready for a day full of nature, fun, and unforgettable moments! 

The safari convoy will travel through dusty, winding roads, passing small mountain villages. 
There will be several stops to take photos, enjoy the fresh air scented with pine and wildflowers, and admire panoramic views. 
At around 700 meters altitude, you’ll visit Haciosmanlar village, where you can explore a traditional Turkish home and learn about rural village life. 

For lunch, enjoy grilled fish, chicken, or omelette, served with rice, salad, fries or baked potatoes, seasonal fruit, and Turkish tea. 
Well-rested and well-fed, continue the journey with an exciting off-road drive through stunning Mediterranean countryside. 
Don’t forget your swimsuit – a refreshing swimming break awaits at Hatipler! 

At the end of the day, you’ll return to your hotel with unforgettable memories. 
This adventure has no age limit – everyone loves it!",
    descriptionDe:
@"Starten Sie zu einem aufregenden Jeep-Safari-Abenteuer in die Taurus-Berge und das türkische Hinterland. 
Nach der Abholung von Ihrem Hotel fahren Sie zum Ausgangspunkt, wo Sie Ihren Guide treffen und eine kurze Einführung in das Tagesprogramm erhalten. 
Freuen Sie sich auf einen Tag voller Natur, Spaß und unvergesslicher Eindrücke! 

Der Jeep-Konvoi fährt über staubige Straßen und durch kleine Bergdörfer. 
Es gibt mehrere Stopps, bei denen Sie Fotos machen, die frische Luft voller Pinien- und Blütenduft genießen und Panoramablicke bewundern können. 
Auf etwa 700 Metern Höhe besuchen Sie das Dorf Haciosmanlar, wo Sie ein traditionelles türkisches Haus besichtigen und das Dorfleben kennenlernen. 

Zum Mittagessen gibt es gegrillten Fisch, Hähnchen oder Omelett, dazu Reis, Salat, Pommes oder Ofenkartoffeln, frisches Obst und türkischen Tee. 
Gestärkt geht es weiter mit einer aufregenden Offroad-Fahrt durch die mediterrane Landschaft. 
Vergessen Sie Ihre Badesachen nicht – ein erfrischender Badestopp in Hatipler erwartet Sie! 

Am Ende des Tages kehren Sie voller unvergesslicher Eindrücke ins Hotel zurück. 
Ohne Altersbeschränkung – dieses Abenteuer begeistert alle!",
    descriptionRu:
@"Отправьтесь в увлекательное джип-сафари по горам Тавра и турецкой сельской местности во время этого насыщенного дня приключений. 
После трансфера из отеля вас доставят на стартовую точку, где вы встретитесь с гидом и получите инструктаж. 
Вас ждёт день, полный природы, веселья и незабываемых впечатлений! 

Караван джипов проедет по пыльным дорогам через небольшие деревни. 
Будут остановки, чтобы сделать фото, вдохнуть свежий воздух с ароматом сосен и горных цветов, а также насладиться панорамными видами. 
На высоте около 700 метров вы посетите деревню Хаджосманлар, где сможете заглянуть в традиционный дом и узнать о сельской жизни. 

На обед вам предложат рыбу на гриле, курицу или омлет, рис, салат, картофель фри или запечённый картофель, сезонные фрукты и традиционный турецкий чай. 
После отдыха продолжите путь по захватывающим внедорожным маршрутам Средиземноморья. 
Не забудьте купальник – в Хатиплере вас ждёт освежающее купание! 

В конце дня вы вернётесь в отель с яркими и незабываемыми впечатлениями. 
Ограничений по возрасту нет – это приключение нравится всем!",
    descriptionPo:
@"Wyrusz na pełną wrażeń jeep safari w góry Taurus i tureckie tereny wiejskie podczas całodniowej przygody. 
Po odebraniu z hotelu pojedziesz do punktu startowego, gdzie spotkasz przewodnika i poznasz plan dnia. 
Czeka Cię dzień pełen natury, zabawy i niezapomnianych chwil! 

Konwój jeepów pojedzie po zakurzonych drogach, mijając małe górskie wioski. 
Podczas postojów będziesz mógł robić zdjęcia, podziwiać panoramy i oddychać świeżym powietrzem pachnącym sosnami i kwiatami. 
Na wysokości około 700 m odwiedzisz wioskę Haciosmanlar i zobaczysz tradycyjny turecki dom, poznając wiejskie życie. 

Na lunch serwowany będzie grillowany dorsz, kurczak lub omlet, ryż, sałatka, frytki lub pieczone ziemniaki, owoce sezonowe i turecka herbata. 
Po posiłku czas na ekscytującą jazdę off-road przez śródziemnomorskie tereny. 
Nie zapomnij stroju kąpielowego – czeka Cię orzeźwiająca kąpiel w Hatipler! 

Na koniec dnia wrócisz do hotelu pełen niezapomnianych wspomnień. 
Brak ograniczeń wiekowych – wszyscy to uwielbiają!",
    descriptionPe:
@"یک سافاری هیجان‌انگیز با جیپ به کوه‌های توروس و روستاهای بکر ترکیه! 
پس از ترانسفر از هتل، به نقطه شروع تور می‌رسید و با راهنمای خود آشنا می‌شوید. 
یک روز پر از طبیعت، هیجان و خاطرات فراموش‌نشدنی در انتظار شماست. 

کاروان جیپ‌ها از جاده‌های خاکی و پرگرد و غبار عبور کرده و از میان روستاهای کوچک می‌گذرد. 
چند توقف برای عکاسی، نفس کشیدن در هوای تازه معطر به کاج و گل‌های کوهی و تماشای مناظر پانوراما خواهید داشت. 
در ارتفاع ۷۰۰ متری به روستای حاجی‌عثمانلار می‌رسید و از خانه‌ای سنتی بازدید می‌کنید تا با زندگی روستایی ترکیه آشنا شوید. 

ناهار خوشمزه شامل ماهی یا مرغ گریل‌شده یا املت به همراه برنج کره‌ای، سالاد سبز، سیب‌زمینی سرخ‌شده یا پخته و میوه‌های فصل سرو می‌شود. 
چای ترکی نیز مهمان ماست! 
سپس آماده ماجراجویی آفرود و جاده‌های خاکی به سمت روستای دیگری در منطقه مدیترانه شوید. 
شنا در منطقه هاتیپلر نیز در برنامه است – پس مایو شنا فراموش نشود! 

در پایان روز با خاطراتی پر از هیجان و انرژی تازه به هتل بازمی‌گردید. 
این تور هیچ محدودیت سنی ندارد – همه عاشق آن می‌شوند!",
    descriptionAr:
@"انطلق في رحلة سفاري مثيرة بسيارات الجيب إلى جبال طوروس والريف التركي في مغامرة ليوم كامل. 
بعد اصطحابك من الفندق، ستصل إلى نقطة الانطلاق حيث ستلتقي بالمرشد الذي يشرح لك برنامج اليوم. 
يوم مليء بالطبيعة والمتعة والذكريات التي لا تُنسى بانتظارك! 

سيمر موكب سيارات الجيب عبر قرى صغيرة على طرق ترابية مليئة بالغبار. 
ستكون هناك محطات لالتقاط الصور والاستمتاع بالهواء النقي المليء برائحة أشجار الصنوبر وأزهار الجبال، مع مناظر بانورامية خلابة. 
عند ارتفاع 700 متر ستزور قرية حاجي عثمانلار وتشاهد منزلاً تقليدياً للتعرف على حياة الريف التركي. 

لتناول الغداء ستُقدَّم لك وجبة لذيذة من السمك أو الدجاج المشوي أو الأومليت مع الأرز، السلطة، البطاطا المقلية أو المشوية، الفاكهة الموسمية، بالإضافة إلى الشاي التركي. 
بعد الراحة ستواصل مغامرتك مع تجربة الطرق الوعرة وسط طبيعة البحر الأبيض المتوسط البكر. 
لا تنس إحضار ملابس السباحة – فهناك محطة سباحة ممتعة في منطقة هاتيبلر! 

في نهاية اليوم ستعود إلى الفندق محملاً بالذكريات والانطباعات الرائعة. 
لا يوجد حد للعمر – الجميع يعشق هذه المغامرة!",

    // Mini summaries
    miniDescriptionEn: "Dusty trails, pine-scented air, village visit and a cool swim stop – a full-day off-road escape in the Taurus Mountains.",
    miniDescriptionDe: "Staubige Pisten, Pinienduft, Dorfbesuch und Badestopp – ein ganzer Offroad-Tag im Taurusgebirge.",
    miniDescriptionRu: "Пыльные тропы, аромат сосен, визит в деревню и купание – насыщенный оффроуд-день в горах Тавра.",
    miniDescriptionPo: "Piaszczyste szlaki, zapach sosen, wizyta w wiosce i kąpiel – całodniowa off-road przygoda w Taurach.",
    miniDescriptionPe: "جاده‌های خاکی، عطر کاج، بازدید روستا و یک توقف خنک برای شنا – یک روز تمام آفرود در کوه‌های توروس.",
    miniDescriptionAr: "مسارات ترابية، عبير الصنوبر، زيارة قرية وتوقف للسباحة – يوم كامل من الطرق الوعرة في جبال طوروس.",

    fotos: new List<Foto> {
        new Foto("/tourimage/Jeep Safari and Off-road Adventures.jpg")
    },
    activeDay: 1111111,
    durationHours: 8,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Professional guide",
        "Lunch",
        "Off-road driving",
        "Photo stops & village visit",
        "Swimming stop (Hatipler)",
        "Insurance"
    }
)
,
new Tour(
    id: 4,
    name: "Rafting Manavgat River Tour",
    price: 50,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Adventure,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Get ready for an adrenaline-filled day on the Manavgat River! 
This rafting adventure takes you deep into nature, riding wild waters and white foam rapids. 
You’ll pass through the bridged canyon and admire the natural park along both riverbanks. 

Rafting, developed since the 1970s, is now considered one of the most exciting extreme sports. 
Professional, experienced, and well-trained rafting guides will accompany you throughout the day to ensure your safety and enjoyment. 

At midday, enjoy a tasty open buffet and barbecue lunch served right on the riverbank. 

Few other sports offer so much at once – physical challenge, mental focus, and unforgettable landscapes. 
With every rapid, you’ll be too busy with excitement to overthink or feel bored. 
A perfect mix of thrill, nature, and fun awaits you on this rafting tour!",
    descriptionDe:
@"Bereiten Sie sich auf einen Tag voller Adrenalin am Fluss Manavgat vor! 
Dieses Rafting-Abenteuer führt Sie mitten in die Natur, durch wilde Strömungen und weiße Schaumwellen. 
Sie durchqueren die Brückenschlucht und genießen die Aussicht auf den Naturpark an beiden Flussufern. 

Seit den 1970er Jahren hat sich Rafting entwickelt und gilt heute als eine der aufregendsten Extremsportarten. 
Erfahrene, gut ausgebildete Guides begleiten Sie, um Sicherheit und Spaß zu garantieren. 

Zur Mittagszeit erwartet Sie ein leckeres Buffet mit Barbecue direkt am Flussufer. 

Kaum ein anderer Sport bietet so viele Erlebnisse auf einmal – körperliche Herausforderung, mentale Konzentration und unvergessliche Naturkulissen. 
Jede Stromschnelle sorgt für Spannung und lässt keine Langeweile aufkommen!",
    descriptionRu:
@"Приготовьтесь к дню, полному адреналина, на реке Манавгат! 
Это рафтинг-приключение проведёт вас через бурные воды и белую пену стремнины. 
Вы пройдёте через каньон с мостом и полюбуетесь природным парком по обоим берегам реки. 

Рафтинг, развивающийся с 1970-х годов, сегодня считается одним из самых захватывающих экстремальных видов спорта. 
Профессиональные и опытные инструкторы будут сопровождать вас весь день, обеспечивая безопасность и удовольствие. 

В середине дня вас ждёт вкусный обед «шведский стол» и барбекю прямо на берегу реки. 

Мало какой другой спорт сочетает столько всего сразу – физический вызов, концентрацию и незабываемые пейзажи. 
Каждый порог наполняет волнением и не оставляет места скуке!",
    descriptionPo:
@"Przygotuj się na dzień pełen adrenaliny nad rzeką Manavgat! 
Ta przygoda raftingowa zabierze Cię w głąb natury, przez dzikie wody i spienione fale. 
Popłyniesz przez kanion z mostem i zobaczysz piękny park przyrodniczy po obu brzegach rzeki. 

Rafting, rozwijany od lat 70., dziś uważany jest za jeden z najbardziej ekscytujących sportów ekstremalnych. 
Profesjonalni i doświadczeni przewodnicy będą Ci towarzyszyć, dbając o bezpieczeństwo i dobrą zabawę. 

W połowie dnia czeka Cię pyszny lunch – bufet i grill serwowany tuż nad brzegiem rzeki. 

Niewiele innych sportów daje tyle naraz – wyzwanie fizyczne, skupienie mentalne i niezapomniane widoki. 
Każda fala i każdy nurt to czysta ekscytacja, bez czasu na nudę!",
    descriptionPe:
@"یک روز پر از آدرنالین در رودخانه ماناوگات! 
این تور رفتینگ شما را به دل طبیعت می‌برد؛ جایی که باید از میان آب‌های خروشان و کف‌های سفید عبور کنید. 
از دره‌ی پل‌دار خواهید گذشت و از مناظر پارک طبیعی در هر دو سوی رودخانه لذت خواهید برد. 

رفتینگ که از دهه ۱۹۷۰ توسعه یافته، امروز یکی از هیجان‌انگیزترین ورزش‌های افراطی محسوب می‌شود. 
راهنماهای حرفه‌ای و باتجربه در طول مسیر همراه شما خواهند بود تا امنیت و لذتتان تضمین شود. 

در میانه روز، ناهاری خوشمزه شامل بوفه باز و باربیکیو در ساحل رودخانه سرو می‌شود. 

کمتر ورزشی می‌تواند همزمان این‌همه تجربه به شما بدهد – چالش جسمی، تمرکز ذهنی و مناظر فراموش‌نشدنی. 
هر تندآب سرشار از هیجان است و اجازه هیچگونه خستگی یا کسالت نمی‌دهد!",
    descriptionAr:
@"استعد ليوم مليء بالإثارة على نهر مانافغات! 
تأخذك هذه المغامرة في عالم الطبيعة عبر المياه البرية ورغوة النهر البيضاء. 
ستعبر الوادي ذي الجسر بينما تستمتع بالمنتزه الطبيعي الممتد على ضفتي النهر. 

منذ السبعينيات تطور التجديف النهري وأصبح يعد من الرياضات المتطرفة المثيرة. 
سيرافقك مرشدون محترفون وذوو خبرة لضمان سلامتك ومتعتك. 

في منتصف اليوم ستستمتع بغداء لذيذ على ضفة النهر يتضمن بوفيهاً مفتوحاً ومشاوي. 

قليل من الرياضات تمنحك هذا الكم من التجارب في وقت واحد – تحدٍ جسدي، تركيز ذهني، ومناظر طبيعية لا تُنسى. 
كل منحدر مائي مليء بالمفاجآت والإثارة، فلا وقت للملل!",

  miniDescriptionEn: "Whitewater thrills on the Manavgat: pro guides, canyon views, swim breaks and riverside BBQ lunch – pure adrenaline in nature.",
    miniDescriptionDe: "Wildwasser auf dem Manavgat: Profibegleitung, Canyonblicke, Badestopps & BBQ am Ufer – Adrenalin pur in der Natur.",
    miniDescriptionRu: "Рафтинг по Манавгату: профи-гида, виды каньона, купание и BBQ на берегу – адреналин и природа на весь день.",
    miniDescriptionPo: "Rafting na Manavgat: profesjonaliści, widoki kanionu, przerwy na kąpiel i BBQ nad rzeką – czysta adrenalina.",
    miniDescriptionPe: "رفتینگ ماناوگات با راهنمای حرفه‌ای، مناظر کانین، توقف‌های شنا و ناهار BBQ کنار رود – آدرنالین ناب در دل طبیعت.",
    miniDescriptionAr: "تجديف مانافغات مع مرشدين محترفين، مناظر الوادي، محطات سباحة وغداء شواء على الضفة – أدرينالين وسط الطبيعة.",

    fotos: new List<Foto> {
        new Foto("/tourimage/Rafting Manavgat River Tour.jpg")
    },
    activeDay: 1111111,
    durationHours: 8,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Rafting equipment",
        "Professional rafting guides",
        "Open-buffet & BBQ lunch",
        "Insurance",
        "Swimming breaks",
        "Bridged canyon passage"
    }
),
new Tour(
    id: 5,
    name: "Pamukkale Trip from Antalya",
    price: 114,
    kinderPrice: 57,
    infantPrice: 0,
    category: Category.History,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Pamukkale, meaning 'Cotton Castle' in Turkish, is one of the most breathtaking natural wonders in the world – often called the 8th wonder. 
This unique formation features natural hot springs and terraces of white carbonate minerals, visible from up to 20 km away near the town of Denizli. 

On top of this dazzling white 'castle' lies the ancient city of Hierapolis. Here, you will explore remarkable sites including a Roman amphitheater, 
a historical museum (once a Turkish bath), the optional Cleopatra swimming pool, the tomb of St. Philippe, and the magnificent Necropolis of Anatolia. 

Recognized for its natural beauty and historical value, Pamukkale and Hierapolis were added to the UNESCO World Heritage List in 1988. 
A journey full of history, culture, and natural beauty awaits you!",
    descriptionDe:
@"Pamukkale, auf Türkisch 'Baumwollschloss', ist eines der atemberaubendsten Naturwunder der Welt – oft als achtes Weltwunder bezeichnet. 
Diese einzigartige Formation besteht aus heißen Quellen und weißen Kalksinterterrassen, die man schon aus 20 km Entfernung nahe Denizli sehen kann. 

Auf diesem weißen 'Schloss' wurde die antike Stadt Hierapolis errichtet. Dort besuchen Sie beeindruckende Sehenswürdigkeiten wie ein römisches Amphitheater, 
ein historisches Museum (ehemals ein türkisches Bad), das optionale Kleopatra-Schwimmbecken, das Grab des heiligen Philippus und die prächtige Nekropole Anatoliens. 

Aufgrund seiner außergewöhnlichen Natur- und Kulturwerte wurde Pamukkale zusammen mit Hierapolis 1988 in die UNESCO-Welterbeliste aufgenommen. 
Eine Reise voller Geschichte, Kultur und Naturwunder erwartet Sie!",
    descriptionRu:
@"Памуккале, что в переводе с турецкого означает 'Хлопковый замок', считается восьмым чудом света. 
Эта уникальная природная формация включает термальные источники и белоснежные травертиновые террасы, которые видны даже за 20 км от города Денизли. 

На вершине этого белого 'замка' расположился древний город Иераполис. 
Вы посетите римский амфитеатр, исторический музей (бывшую турецкую баню), бассейн Клеопатры (по желанию), гробницу святого Филиппа и величественный некрополь Анатолии. 

За природную красоту и историческое значение Памуккале и Иераполис были включены в список Всемирного наследия ЮНЕСКО в 1988 году. 
Вас ждет удивительное путешествие, наполненное историей, культурой и природой!",
    descriptionPo:
@"Pamukkale, co po turecku oznacza 'Bawełniany Zamek', to jedno z najbardziej niezwykłych cudów natury na świecie – często nazywane ósmym cudem świata. 
Formacja ta składa się z gorących źródeł i białych tarasów trawertynowych, które można zobaczyć nawet z odległości 20 km w pobliżu miasta Denizli. 

Na szczycie tego białego 'zamku' zbudowano starożytne miasto Hierapolis. 
Podczas wizyty zobaczysz rzymski amfiteatr, muzeum historyczne (dawniej łaźnia turecka), opcjonalny basen Kleopatry, grobowiec św. Filipa oraz wspaniałą nekropolię Anatolii. 

Ze względu na swoją wyjątkową wartość przyrodniczą i historyczną, Pamukkale i Hierapolis zostały wpisane na listę światowego dziedzictwa UNESCO w 1988 roku. 
Czeka Cię podróż pełna historii, kultury i naturalnego piękna!",
    descriptionPe:
@"پاموکاله به معنی 'قلعه پنبه‌ای' در زبان ترکی، یکی از شگفت‌انگیزترین جاذبه‌های طبیعی جهان است که به هشتمین عجایب دنیا شهرت دارد. 
این پدیده طبیعی شامل چشمه‌های آب گرم و تراس‌های سفید تراورتن است که از فاصله ۲۰ کیلومتری در شهر دنیزلی قابل مشاهده‌اند. 

در بالای این 'قلعه سفید' شهر باستانی هیراپولیس ساخته شده است. 
در اینجا از مکان‌هایی چون آمفی‌تئاتر رومی، موزه تاریخی (حمام قدیمی ترکی)، استخر باستانی کلئوپاترا (اختیاری)، مقبره سنت فیلیپ و نکرپولیس باشکوه آناتولی بازدید خواهید کرد. 

به دلیل زیبایی طبیعی و ارزش تاریخی، پاموکاله و هیراپولیس در سال ۱۹۸۸ در فهرست میراث جهانی یونسکو ثبت شدند. 
سفری پر از تاریخ، فرهنگ و شگفتی‌های طبیعی در انتظار شماست!",
    descriptionAr:
@"باموكالي، وتعني 'قلعة القطن' بالتركية، تُعد من أعظم عجائب الطبيعة في العالم وغالبًا ما يُطلق عليها العجيبة الثامنة. 
تتميز بتكويناتها الطبيعية من الينابيع الساخنة وتراسات الحجر الجيري الأبيض، والتي يمكن رؤيتها من مسافة تصل إلى 20 كم في مدينة دنيزلي. 

على قمة هذا 'القصر الأبيض' بُنيت مدينة هيرابوليس القديمة. 
هنا ستزور المسرح الروماني، المتحف التاريخي (حمام تركي قديم)، المسبح الشهير لكلـيوپاترا (اختياري)، ضريح القديس فيليب، والمقبرة العظيمة في الأناضول. 

وبفضل جمالها الطبيعي وقيمتها التاريخية، أُدرجت باموكالي وهيرابوليس في قائمة التراث العالمي لليونسكو عام 1988. 
رحلة غنية بالتاريخ والثقافة والجمال الطبيعي بانتظارك!",
 miniDescriptionEn: "Day trip to the ‘Cotton Castle’ & Hierapolis: travertine terraces, Roman theatre, museum and optional Cleopatra Pool – UNESCO classic.",
    miniDescriptionDe: "Tagesausflug zum „Baumwollschloss“ & Hierapolis: Travertinen, römisches Theater, Museum & optionaler Kleopatra-Pool – UNESCO-Klassiker.",
    miniDescriptionRu: "Памуккале и Иераполис: травертины, римский театр, музей и опциональный бассейн Клеопатры – объект ЮНЕСКО.",
    miniDescriptionPo: "Pamukkale & Hierapolis: tarasy trawertynowe, teatr rzymski, muzeum i opcjonalny basen Kleopatry – klasyk UNESCO.",
    miniDescriptionPe: "سفر روزانه به «قلعه پنبه‌ای» و هیراپولیس: تراس‌های تراورتن، تئاتر رومی، موزه و استخر اختیاری کلئوپاترا – میراث یونسکو.",
    miniDescriptionAr: "رحلة يومية إلى باموكالي وهيرابوليس: مصاطب الترافرتين، المسرح الروماني، المتحف وحوض كليوباترا الاختياري – موقع يونسكو.",

    fotos: new List<Foto> {
        new Foto("/tourimage/Pamukkale Trip from Antalya.jpg")
    },
    activeDay: 1111111,
    durationHours: 14,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Professional guide",
        "Transport Antalya–Pamukkale",
        "Visit to Hierapolis",
        "Free time at travertines",
        "UNESCO heritage site",
        "Optional Cleopatra Pool (extra)"
    }
),
new Tour(
    id: 6,
    name: "Sea Lions and Dolphin Show",
    price: 47,
    kinderPrice: 47,
    infantPrice: 0,
    category: Category.Family,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Let the sun and surf of the Sea Lions and Dolphin Show bring out the child in you! 
This daily excursion includes complimentary hotel pick-up at 13:45 and drop-off at 17:00. 

Enjoy an afternoon of fun with incredible water attractions and an unforgettable live show featuring playful sea lions and talented dolphins. 
Marvel at their amazing skills and stunts – a performance guaranteed to impress both children and adults alike. 

This half-day tour is the perfect family activity, combining entertainment, relaxation, and the joy of being close to these fascinating marine animals!",
    descriptionDe:
@"Lassen Sie sich beim Sea Lions and Dolphin Show von Sonne und Meer verzaubern! 
Dieses tägliche Ausflugsangebot beinhaltet die Abholung vom Hotel um 13:45 Uhr sowie die Rückfahrt um 17:00 Uhr. 

Freuen Sie sich auf einen Nachmittag voller Spaß mit aufregenden Wasserattraktionen und einer unvergesslichen Live-Show mit verspielten Seelöwen und talentierten Delfinen. 
Bestaunen Sie ihre Kunststücke und Fähigkeiten – ein Erlebnis, das Kinder und Erwachsene gleichermaßen begeistert. 

Dieser Halbtagesausflug ist die perfekte Familienaktivität, die Unterhaltung, Entspannung und den Kontakt zu faszinierenden Meerestieren vereint!",
    descriptionRu:
@"Пусть шоу морских львов и дельфинов вернёт вас в детство! 
Экскурсия проводится ежедневно и включает бесплатный трансфер из отеля в 13:45 и обратно в 17:00. 

Вас ждёт незабываемое представление с морскими львами и дельфинами, полное веселья, трюков и удивительных номеров. 
Эти умные и дружелюбные животные покорят и детей, и взрослых. 

Полудневная поездка в Dolphin Show – это идеальное семейное мероприятие, которое подарит радость, эмоции и море впечатлений!",
    descriptionPo:
@"Niech słońce i fale podczas pokazu lwów morskich i delfinów obudzą w Tobie dziecko! 
Ta codzienna wycieczka obejmuje bezpłatny transfer z hotelu o 13:45 i powrót o 17:00. 

Czeka Cię popołudnie pełne atrakcji wodnych oraz niezapomniany pokaz na żywo z udziałem zabawnych lwów morskich i utalentowanych delfinów. 
Ich niesamowite umiejętności i sztuczki zachwycą zarówno dzieci, jak i dorosłych. 

Ta półdniowa wycieczka to idealna atrakcja rodzinna – rozrywka, relaks i bliski kontakt z fascynującymi zwierzętami morskimi!",
    descriptionPe:
@"نمایش شیرهای دریایی و دلفین‌ها شما را دوباره به دنیای کودکانه می‌برد! 
این تور روزانه شامل ترانسفر رایگان از هتل در ساعت ۱۳:۴۵ و بازگشت در ساعت ۱۷:۰۰ است. 

از بعدازظهری پرهیجان لذت ببرید؛ با دیدن نمایش زنده شیرهای دریایی بازیگوش و دلفین‌های بااستعداد. 
هنرنمایی‌ها و حرکات شگفت‌انگیز آنها مطمئناً هم کودکان و هم بزرگسالان را مجذوب خواهد کرد. 

این تور نیم‌روزه بهترین فعالیت خانوادگی است که سرگرمی، آرامش و تجربه نزدیکی با حیوانات دریایی شگفت‌انگیز را ترکیب می‌کند!",
    descriptionAr:
@"دع عرض البحر والشمس في عرض أسود البحر والدلافين يعيدك إلى طفولتك! 
تُقدَّم هذه الرحلة يومياً وتشمل خدمة النقل المجاني من الفندق الساعة 13:45 والعودة الساعة 17:00. 

استمتع بعصر مليء بالمرح مع عروض مائية مذهلة واستعراض مباشر لا يُنسى يقدمه أسود البحر والدلافين الموهوبون. 
مهاراتهم وعروضهم المدهشة ستبهر الصغار والكبار على حد سواء. 

هذه الرحلة نصف اليومية هي النشاط العائلي المثالي الذي يجمع بين الترفيه والاسترخاء ومتعة الاقتراب من هذه الكائنات البحرية الرائعة!",
miniDescriptionEn: "Half-day fun for all ages: playful sea lions, smart dolphins, stunts and smiles with hotel pickup & drop-off.",
miniDescriptionDe: "Halbtägiger Familienspaß: verspielte Seelöwen, kluge Delfine, Kunststücke & Lachen – inkl. Hoteltransfer.",
miniDescriptionRu: "Полдня веселья: морские львы, умные дельфины, трюки и радость – с трансфером от/до отеля.",
miniDescriptionPo: "Pół dnia zabawy: zabawne lwy morskie, inteligentne delfiny, sztuczki i uśmiechy – z odbiorem z hotelu.",
miniDescriptionPe: "نیم‌روز شادی برای همه سنین: شیرهای دریایی بازیگوش، دلفین‌های باهوش و حرکات دیدنی – همراه با ترانسفر هتل.",
miniDescriptionAr: "نصف يوم من المرح: أسود بحر مرحة، دلافين ذكية، عروض مذهلة وابتسامات – مع نقل من وإلى الفندق.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Sea Lions and Dolphin Show.jpg")
    },
    activeDay: 1111111,
    durationHours: 3,
    services: new List<string> {
        "Hotel pickup (13:45) & drop-off (17:00)",
        "Live dolphin & sea lion show",
        "Seating at the venue",
        "Free time for photos",
        "Insurance"
    }
)
,

new Tour(
    id: 7,
    name: "Scuba Diving",
    price: 56,
    kinderPrice: 25,
    infantPrice: 0,
    category: Category.Adventure,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Discover the wonders beneath Turkey’s southern Mediterranean coast on this full-day scuba diving adventure. 
Enjoy recreational and professional dives at different sites, plus a delicious lunch during this 8-hour excursion from Antalya and Kemer. 

Both beginners and experienced divers can explore the colorful underwater world in crystal-clear waters. 
Before the first dive, professional instructors will provide a full briefing on diving techniques and equipment usage. 
After a few simple skill tests, you’ll dive into an unforgettable experience – swimming alongside exotic fish and exploring fascinating caves. 

Safety is always a priority, with expert instructors guiding you throughout. 
If you prefer not to dive, you can still join the boat trip and enjoy lunch as a visitor.",
    descriptionDe:
@"Entdecken Sie die Wunder unter der Wasseroberfläche an der südlichen Mittelmeerküste der Türkei bei einem ganztägigen Tauchabenteuer. 
Genießen Sie Freizeit- und Profitauchgänge an verschiedenen Plätzen sowie ein köstliches Mittagessen während dieses 8-stündigen Ausflugs ab Antalya und Kemer. 

Sowohl Anfänger als auch erfahrene Taucher können die farbenfrohe Unterwasserwelt in kristallklarem Wasser erkunden. 
Vor dem ersten Tauchgang erhalten Sie eine ausführliche Einweisung in Technik und Ausrüstung. 
Nach ein paar einfachen Übungen tauchen Sie ein in ein unvergessliches Erlebnis – schwimmen Sie mit exotischen Fischen und erkunden Sie faszinierende Höhlen. 

Die Sicherheit steht dabei immer an erster Stelle, professionelle Tauchlehrer begleiten Sie den ganzen Tag. 
Wer nicht tauchen möchte, kann die Bootstour und das Mittagessen auch nur als Besucher genießen.",
    descriptionRu:
@"Откройте для себя чудеса подводного мира южного Средиземноморья Турции во время этого дневного тура по дайвингу. 
Вас ждут любительские и профессиональные погружения в разных местах, а также вкусный обед во время 8-часовой экскурсии из Анталии и Кемера. 

И новички, и опытные дайверы смогут исследовать яркий подводный мир в кристально чистой воде. 
Перед первым погружением инструкторы проведут инструктаж по технике безопасности и использованию снаряжения. 
После нескольких простых упражнений вы сможете насладиться незабываемыми впечатлениями – поплавать с экзотическими рыбами и исследовать загадочные пещеры. 

Безопасность гарантирована – вас сопровождают профессиональные инструкторы. 
Если вы не хотите нырять, можно присоединиться к прогулке на лодке и обеду в качестве гостя.",
    descriptionPo:
@"Odkryj cuda południowego wybrzeża Morza Śródziemnego w Turcji podczas całodniowej przygody nurkowej. 
Czekają Cię nurkowania rekreacyjne i profesjonalne w różnych miejscach oraz pyszny lunch w trakcie 8-godzinnej wycieczki z Antalyi i Kemer. 

Zarówno początkujący, jak i doświadczeni nurkowie mogą odkrywać kolorowe życie podwodne w krystalicznie czystej wodzie. 
Przed pierwszym zejściem instruktorzy przeprowadzą szczegółowy briefing na temat techniki nurkowania i sprzętu. 
Po kilku prostych ćwiczeniach rozpoczniesz niezapomnianą przygodę – pływanie z egzotycznymi rybami i odkrywanie fascynujących jaskiń. 

Bezpieczeństwo jest priorytetem – przez cały czas towarzyszą Ci profesjonalni instruktorzy. 
Jeśli nie chcesz nurkować, możesz zarezerwować tylko rejs statkiem i lunch jako odwiedzający.",
    descriptionPe:
@"یک ماجراجویی هیجان‌انگیز زیر آب در سواحل جنوبی مدیترانه ترکیه! 
این تور ۸ ساعته از آنتالیا و کمر شامل غواصی تفریحی و حرفه‌ای در نقاط مختلف و همچنین ناهار خوشمزه است. 

هم مبتدیان و هم غواصان باتجربه می‌توانند دنیای رنگارنگ زیر آب را در آب‌های شفاف کشف کنند. 
پیش از نخستین غواصی، مربیان حرفه‌ای توضیحات کامل درباره تکنیک‌ها و نحوه استفاده از تجهیزات خواهند داد. 
پس از چند تست ساده مهارت، آماده غواصی در دنیای جذاب ماهی‌های عجیب و غارهای شگفت‌انگیز خواهید بود. 

امنیت شما در اولویت است و مربیان مجرب در تمام مسیر همراه شما هستند. 
اگر هم قصد غواصی ندارید، می‌توانید تنها در تور قایق‌سواری و صرف ناهار به عنوان بازدیدکننده شرکت کنید.",
    descriptionAr:
@"استمتع بمغامرة غوص رائعة على الساحل الجنوبي للبحر الأبيض المتوسط في تركيا! 
تشمل هذه الرحلة التي تستغرق 8 ساعات من أنطاليا وكيمر غوصاً ترفيهياً واحترافياً في مواقع مختلفة بالإضافة إلى غداء لذيذ. 

يمكن لكل من المبتدئين والغواصين ذوي الخبرة استكشاف عالم ما تحت الماء الملون في مياه صافية تماماً. 
قبل الغوص الأول سيقدم المدربون المحترفون شرحاً كاملاً عن تفاصيل الغوص واستخدام المعدات. 
بعد بعض التدريبات البسيطة ستنطلق لتجربة لا تُنسى – السباحة مع أسماك غريبة واستكشاف الكهوف الرائعة. 

السلامة مضمونة مع وجود مدربين ذوي خبرة طوال اليوم. 
إذا لم ترغب في الغوص، يمكنك حجز الرحلة البحرية مع الغداء فقط كزائر.",
   miniDescriptionEn: "Two guided dives in crystal-clear Med waters, exotic fish, cave peek-ins and lunch on board — beginner-friendly & safe.",
miniDescriptionDe: "Zwei geführte Tauchgänge im glasklaren Mittelmeer, bunte Fische, Höhlen-Highlights & Mittagessen an Bord – auch für Anfänger.",
miniDescriptionRu: "Два погружения в прозрачном Средиземном море, экзотические рыбы, пещеры и обед на борту — безопасно и для новичков.",
miniDescriptionPo: "Dwa nurkowania z instruktorem w krystalicznej wodzie, egzotyczne ryby, zajrzenie do jaskiń i lunch na pokładzie – także dla początkujących.",
miniDescriptionPe: "دو غواصی هدایت‌شده در آب‌های شفاف مدیترانه، ماهی‌های رنگارنگ، سرک کشیدن به غارها و ناهار روی قایق – مناسب مبتدی‌ها.",
miniDescriptionAr: "غوصتان مُوجهان في مياه صافية، أسماك ملوّنة ولمحات من الكهوف مع غداء على القارب – مناسب للمبتدئين وآمن.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Scuba Diving.jpg")
    },
    activeDay: 1111111,
    durationHours: 8,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Diving equipment",
        "Professional instructors",
        "2 dives (conditions permitting)",
        "Boat trip",
        "Lunch",
        "Insurance",
        "Option to join as visitor"
    }
),

new Tour(
    id: 8,
    name: "Antalya Manavgat Waterfall & Turkish Bazaar",
    price: 47,
    kinderPrice: 24,
    infantPrice: 0,
    category: Category.Family,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Embark on a river cruise adventure to discover Antalya like never before! 
Your journey begins at the Marina, where we set sail for a relaxing day surrounded by stunning scenery. 

During this peaceful cruise, take in the breathtaking beauty of Antalya’s coastline and enjoy: 
- The refreshing sea breeze 
- Spectacular views of the Taurus Mountains 
- A visit to the Turkish Bazaar and the famous Manavgat Waterfall 
- Impressive yachts and leisure boats along the way 
- Wildlife and marine life, with chances to spot jumping fish or even dolphins if you are lucky! 

After exploring the Turkish Market, sit back, relax, and soak in all the incredible views that make Antalya such a unique destination. 
An unforgettable experience for the whole family!",
    descriptionDe:
@"Begeben Sie sich auf ein Flusskreuzfahrt-Abenteuer und entdecken Sie Antalya auf eine völlig neue Art! 
Ihre Reise beginnt im Hafen, wo Sie zu einem entspannten Tag inmitten atemberaubender Landschaften in See stechen. 

Während der ruhigen Fahrt genießen Sie: 
- Die erfrischende Meeresbrise 
- Spektakuläre Ausblicke auf das Taurusgebirge 
- Einen Besuch des türkischen Basars und des berühmten Manavgat-Wasserfalls 
- Eindrucksvolle Yachten und Freizeitboote entlang der Küste 
- Tier- und Meeresleben, mit etwas Glück sogar springende Fische oder Delfine 

Nach dem Bummel über den türkischen Markt lehnen Sie sich zurück und lassen die wunderbaren Eindrücke wirken. 
Ein unvergessliches Erlebnis für die ganze Familie!",
    descriptionRu:
@"Отправьтесь в речной круиз и откройте для себя Анталию с новой стороны! 
Ваше путешествие начинается в марине, откуда вы отправитесь в путь, наслаждаясь живописными пейзажами. 

Во время этой спокойной прогулки вы сможете насладиться: 
- Освежающим морским бризом 
- Захватывающими видами на горы Тавр 
- Посещением турецкого базара и знаменитого водопада Манавгат 
- Впечатляющими яхтами и прогулочными лодками 
- Дикой природой и морской жизнью, с шансом увидеть прыгающих рыб или даже дельфинов 

После прогулки по турецкому рынку расслабьтесь и насладитесь всеми красотами, которые делают Анталию особенной. 
Незабываемый опыт для всей семьи!",
    descriptionPo:
@"Wyrusz w rejs rzeczny, aby odkryć Antalyę w zupełnie nowy sposób! 
Twoja przygoda rozpoczyna się w marinie, skąd wypłyniesz w spokojny rejs pełen niesamowitych widoków. 

Podczas tej relaksującej wycieczki będziesz mógł podziwiać: 
- Orzeźwiającą morską bryzę 
- Spektakularne widoki na góry Taurus 
- Turecki bazar i słynny wodospad Manavgat 
- Wspaniałe jachty i łodzie rekreacyjne 
- Życie dzikiej przyrody i morskie, a przy odrobinie szczęścia nawet skaczące ryby lub delfiny 

Po wizycie na tureckim bazarze usiądź wygodnie, zrelaksuj się i podziwiaj wyjątkowe krajobrazy, które sprawiają, że Antalya jest tak niezwykła. 
Niezapomniana przygoda dla całej rodziny!",
    descriptionPe:
@"یک ماجراجویی با قایق در رودخانه برای کشف آنتالیا به شکلی متفاوت! 
سفر شما از مارینا آغاز می‌شود، جایی که با آرامش به دل دریا می‌روید و مناظر خیره‌کننده اطراف را تماشا می‌کنید. 

در طول این کروز لذت‌بخش تجربه خواهید کرد: 
- نسیم خنک دریا 
- مناظر باشکوه کوه‌های توروس 
- بازار ترکی و آبشار مشهور ماناوگات 
- تماشای قایق‌ها و یات‌های بزرگ تفریحی 
- حیات‌وحش و زندگی دریایی، همراه با ماهی‌های جهنده و حتی دلفین‌ها در صورت خوش‌شانسی 

پس از گشت‌وگذار در بازار ترکی، بنشینید، آرام شوید و از تمام این مناظر شگفت‌انگیز لذت ببرید. 
یک تجربه فراموش‌نشدنی برای تمام خانواده!",
    descriptionAr:
@"انطلق في رحلة نهرية لاكتشاف أنطاليا كما لم ترها من قبل! 
تبدأ مغامرتك من المرسى حيث نستعد للإبحار في يوم هادئ تحيط به مناظر خلابة. 

خلال هذه الرحلة ستستمتع بـ: 
- نسيم البحر المنعش 
- مناظر رائعة لجبال طوروس 
- زيارة البازار التركي وشلال مانافجات الشهير 
- مشاهدة اليخوت والقوارب الترفيهية 
- الحياة البرية والبحرية، مع فرصة لرؤية الأسماك وهي تقفز وربما الدلافين 

بعد مغامرة التسوق في السوق التركي، اجلس واسترخ واستمتع بالمشاهد المذهلة التي تجعل أنطاليا مميزة للغاية. 
تجربة لا تُنسى لجميع أفراد العائلة!",
   miniDescriptionEn: "Easygoing river cruise with sea breeze, Taurus views, Manavgat Waterfall stop and time to wander the lively Turkish bazaar.",
miniDescriptionDe: "Entspannte Flussfahrt mit Meeresbrise, Taurus-Panorama, Stopp am Manavgat-Wasserfall und Bummel über den türkischen Basar.",
miniDescriptionRu: "Неспешный речной круиз: бриз, виды Тавра, остановка у водопада Манавгат и время на турецком базаре.",
miniDescriptionPo: "Spokojny rejs rzeką: morska bryza, widoki Taurusu, przystanek przy wodospadzie Manavgat i czas na turecki bazar.",
miniDescriptionPe: "کروز رودخانه‌ای راحت با نسیم دریا، مناظر توروس، توقف آبشار ماناوگات و وقت آزاد در بازار ترکی.",
miniDescriptionAr: "رحلة نهرية هادئة مع نسيم البحر، إطلالات طوروس، توقف عند شلال مانافغات ووقت للتسوق في البازار التركي.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Antalya Manavgat Waterfall & Turkish Bazaar.jpg")
    },
    activeDay: 1111111,
    durationHours: 7,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "River/boat cruise",
        "Visit Turkish Bazaar",
        "Visit Manavgat Waterfall",
        "Free time for shopping",
        "Insurance"
    }
),

new Tour(
    id: 9,
    name: "Suluada The Maldives of Turkey",
    price: 47,
    kinderPrice: 24,
    infantPrice: 0,
    category: Category.Relaxing,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Visiting the Maldives of Turkey, Suluada, is a true bucket-list experience – and this tour will not disappoint. 
With dreamy views, diverse marine life, and romantic sunsets, Suluada is one of the top destinations for honeymooners. 

Looking for romance or planning to propose? The captain knows the area perfectly and can customize the trip to make your moment unforgettable. 
But Suluada isn’t only for couples – it’s also ideal for a girls’ getaway, a family trip, or simply anyone in search of relaxation. 

This tropical island is famous for its laid-back atmosphere and exotic beauty. 
Swim, relax, and soak in the peaceful vibes of this magical destination. 

The boat returns to Adrasan by 4:00 PM, where we arrange your transfer back to your hotel or the original meeting point.",
    descriptionDe:
@"Ein Besuch auf Suluada – den Malediven der Türkei – gehört definitiv auf die Bucket List. 
Traumhafte Aussichten, vielfältiges Meeresleben und romantische Sonnenuntergänge machen die Insel zu einem Top-Ziel für Paare. 

Planen Sie einen besonderen Moment? Der Kapitän kennt die Gegend bestens und kann die Tour individuell gestalten. 
Aber Suluada ist nicht nur für Paare – perfekt auch für Freundesgruppen oder Familien, die Entspannung suchen. 

Die tropische Insel ist für ihre entspannte Atmosphäre und exotische Schönheit berühmt. 
Schwimmen, relaxen und die ruhige Stimmung genießen – das ist Suluada. 

Das Boot kehrt gegen 16:00 Uhr nach Adrasan zurück, von wo der Rücktransfer organisiert wird.",
    descriptionRu:
@"Сулуада – «Мальдивы Турции» – обязательно к посещению! 
Завораживающие виды, разнообразный подводный мир и романтичные закаты делают остров идеальным местом для пар. 

Планируете особенный момент? Капитан отлично знает район и поможет настроить маршрут под ваши планы. 
Но Сулуада подходит не только для пар – это также отличный вариант для друзей и семей. 

Тропический остров славится расслабленной атмосферой и экзотической красотой. 
Купайтесь, отдыхайте и ловите атмосферу этого волшебного места. 

Возвращение лодки в Адрасан примерно к 16:00, затем трансфер в отель/نقطة اللقاء.",
    descriptionPo:
@"Suluada – Malediwy Turcji – to prawdziwy punkt obowiązkowy. 
Bajkowe widoki, bogate życie morskie i romantyczne zachody słońca – idealne dla par. 

Szukasz romantycznej chwili? Kapitan dopasuje rejs do Twoich planów. 
Ale wyspa jest też świetna dla rodzin i grup znajomych szukających relaksu. 

Tropikalny klimat, luz i egzotyczne piękno – po prostu pływaj i odpoczywaj. 
Powrót łodzi do Adrasan ok. 16:00, z zapewnionym transferem.",
    descriptionPe:
@"سولوآدا، «مالدیوهای ترکیه»، واقعاً یک تجربه خاص و تکرارنشدنی است. 
مناظر رویایی، دنیای زیرآب متنوع و غروب‌های رمانتیک، اینجا را به مقصدی محبوب برای زوج‌ها تبدیل کرده است. 
اما برای خانواده‌ها و دوستان هم عالی است – جایی برای آرامش واقعی. 

کاپیتان می‌تواند برنامه را مطابق لحظه خاص شما شخصی‌سازی کند. 
در این جزیره استوایی، فقط کافی‌ست شنا کنید، نفس بکشید و ریلکس کنید. 
قایق حوالی ساعت ۴ عصر به آدراسان بازمی‌گردد و از آنجا ترانسفر شما انجام می‌شود.",
    descriptionAr:
@"تُعد سولوادا «مالديف تركيا» تجربة لا تُفوَّت. 
مناظر حالمة وحياة بحرية متنوعة وغروب رومانسي – مثالية للأزواج وكذلك للعائلات والأصدقاء الباحثين عن الاسترخاء. 

يمكن للقبطان تخصيص الرحلة للحظاتك الخاصة. 
اسبح واستمتع بالأجواء الاستوائية الهادئة. 
تعود القارب إلى أدرسان حوالي الساعة 16:00 مع ترتيب النقل إلى الفندق.",
miniDescriptionEn: "Dreamy island escape to Suluada—turquoise coves, soft sands and chill vibes for couples, friends and families.",
miniDescriptionDe: "Trauminsel Suluada: türkisfarbene Buchten, feiner Sand und entspannte Vibes – für Paare, Freunde und Familien.",
miniDescriptionRu: "Сулуада: бирюзовые бухты, мягкий песок и полное расслабление — для пар, друзей и семей.",
miniDescriptionPo: "Suluada: turkusowe zatoczki, miękki piasek i totalny chill – dla par, znajomych i rodzin.",
miniDescriptionPe: "فرار به جزیره سولوآدا؛ خلیج‌های فیروزه‌ای، شن نرم و حال‌وهوای ریلکس برای زوج‌ها، دوستان و خانواده‌ها.",
miniDescriptionAr: "هروب جزيرة إلى سولوادا: خلجان فيروزية ورمال ناعمة وأجواء مريحة – للأزواج والأصدقاء والعائلات.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Suluada The Maldives of Turkey.jpg")
    },
    activeDay: 1111111,
    durationHours: 8,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Boat trip from Adrasan",
        "Swimming stops",
        "Free time on beaches",
        "Captain trip customization (on request)",
        "Insurance"
    }
),

new Tour(
    id: 10,
    name: "Antalya Traditional Turkish Bath with Massage",
    price: 30,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Relaxing,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Pamper yourself with a visit to a traditional Turkish Bath in Antalya and enjoy an experience of relaxation and renewal. 
Unwind in the soothing heated waters, let the stress melt away with a refreshing body scrub, and complete the treatment with a relaxing full-body massage. 

To finish, sip a revitalizing Turkish tea and leave feeling rejuvenated, refreshed, and full of new energy. 
A perfect way to experience an authentic part of Turkish culture while taking care of your body and soul.",
    descriptionDe:
@"Gönnen Sie sich einen Besuch in einem traditionellen türkischen Bad in Antalya und erleben Sie vollkommene Entspannung und Erneuerung. 
Entspannen Sie im warmen Wasser, lassen Sie sich bei einem belebenden Körperpeeling verwöhnen und genießen Sie anschließend eine wohltuende Ganzkörpermassage. 

Zum Abschluss erwartet Sie eine Tasse erfrischender türkischer Tee. 
Ein authentisches Kulturerlebnis und gleichzeitig eine Wohltat für Körper und Seele.",
    descriptionRu:
@"Побалуйте себя визитом в традиционную турецкую баню в Анталии и насладитесь восстановлением тела и души. 
Расслабьтесь в тёплых водах, получите бодрящий пилинг и завершите процедуру расслабляющим массажем всего тела. 

В конце вас угостят освежающим турецким чаем. 
Это идеальный способ прикоснуться к турецкой культуре и подарить себе новые силы и энергию.",
    descriptionPo:
@"Zafunduj sobie wizytę w tradycyjnej łaźni tureckiej w Antalyi i doświadcz pełnego relaksu i odnowy. 
Zrelaksuj się w podgrzewanych wodach, pozwól się rozpieścić orzeźwiającemu peelingowi ciała, a następnie odpręż się podczas relaksującego masażu całego ciała. 

Na zakończenie skosztuj orzeźwiającej tureckiej herbaty. 
To doskonały sposób, aby poznać autentyczną kulturę turecką i jednocześnie zadbać o ciało i duszę.",
    descriptionPe:
@"به خودتان فرصت آرامش بدهید و در یک حمام سنتی ترکی در آنتالیا تجربه‌ای متفاوت از آرامش و تازگی داشته باشید. 
در آب‌های گرم و آرامش‌بخش استراحت کنید، با یک لایه‌برداری بدن احساس سبکی کنید و سپس با یک ماساژ کامل بدن به آرامش برسید. 

در پایان با یک فنجان چای تازه‌دم ترکی سرحال شوید و با انرژی تازه و روحی شاداب از حمام بیرون بیایید. 
راهی عالی برای تجربه بخشی اصیل از فرهنگ ترکیه همراه با مراقبت از بدن و روح.",
    descriptionAr:
@"دلل نفسك بزيارة حمام تركي تقليدي في أنطاليا واستمتع بتجربة مفعمة بالاسترخاء والتجديد. 
استرخِ في المياه الدافئة، وانعم بتقشير منعش للجسم، ثم استمتع بتدليك مريح لكامل الجسم. 

وفي الختام، تذوق كوباً من الشاي التركي المنعش. 
إنها الطريقة المثالية لاختبار جزء أصيل من الثقافة التركية مع العناية بجسمك وروحك.",
   miniDescriptionEn: "Classic hammam ritual in Antalya—sauna, scrub, foam and oil massage, finished with soothing Turkish tea.",
miniDescriptionDe: "Klassischer Hamam in Antalya – Sauna, Peeling, Schaummassage & Ölmassage, abgerundet mit türkischem Tee.",
miniDescriptionRu: "Классический хаммам в Анталье: сауна, пилинг, пенный и масляный массаж + чашка турецкого чая.",
miniDescriptionPo: "Klasyczny hammam w Antalyi – sauna, peeling, masaż pianą i olejkami, na koniec turecka herbata.",
miniDescriptionPe: "حمام سنتی آنتالیا؛ سونا، لایه‌برداری، ماساژ کف و ماساژ روغن، با یک چای ترکی داغ برای پایان.",
miniDescriptionAr: "حمّام تركي أصيل في أنطاليا: ساونا، تقشير، تدليك بالرغوة والزيوت مع ختام بشاي تركي.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Antalya Traditional Turkish Bath with Massage.jpg")
    },
    activeDay: 1111111,
    durationHours: 2,
    services: new List<string> {
        "Sauna/steam room",
        "Body scrub (peeling)",
        "Foam massage",
        "Aromatherapy oil massage",
        "Turkish tea",
        "Changing room & locker",
        "Towels",
        "Insurance"
    }
),

new Tour(
    id: 11,
    name: "Antalya Buggy Safari",
    price: 61,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Adventure,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Get ready for an adrenaline-filled adventure with the Antalya Buggy Safari! 
This 3-hour expedition takes you through the stunning and ever-changing sand dunes of the Taurus Mountains. 

After hotel pick-up from Kemer, you’ll be transferred to the foothills of the rugged (and sometimes snow-capped) Taurus Mountains. 
Here, you’ll receive a full safety briefing and learn essential tips before heading out on your buggy. 

Drive across the desert-like terrain that separates the Mediterranean coast from the Anatolian Plateau. 
Along the way, you’ll take breaks to swim, relax, or capture the breathtaking landscapes with your camera. 

After the ride, return to the safari center before being comfortably transferred back to your hotel in Antalya. 
A perfect mix of thrill, nature, and fun awaits you!",
    descriptionDe:
@"Bereiten Sie sich auf ein Abenteuer voller Adrenalin vor – beim Antalya Buggy Safari! 
Diese 3-stündige Expedition führt Sie durch die beeindruckenden und abwechslungsreichen Sanddünen des Taurusgebirges. 

Nach der Abholung vom Hotel in Kemer fahren Sie in die Ausläufer des Taurusgebirges, wo Sie eine Sicherheitseinweisung und Tipps zur Fahrt erhalten. 

Dann geht es los: Erkunden Sie das wüstenartige Gelände, das die Mittelmeerküste vom anatolischen Hochplateau trennt. 
Unterwegs gibt es Stopps zum Schwimmen, Entspannen oder um die atemberaubenden Landschaften mit der Kamera festzuhalten. 

Nach der Rückkehr zum Safari-Zentrum werden Sie bequem zu Ihrem Hotel in Antalya zurückgebracht. 
Ein perfekter Mix aus Abenteuer, Natur und Spaß!",
    descriptionRu:
@"Приготовьтесь к порции адреналина на сафари-туре на багги в Анталии! 
За 3 часа вы прокатитесь по величественным и постоянно меняющимся песчаным дюнам гор Тавра. 

После трансфера из отеля в Кемере вас доставят к подножию суровых (иногда покрытых снегом) гор Тавра. 
Здесь вы получите инструктаж по технике безопасности и советы перед началом поездки. 

Вы исследуете пустынный рельеф, разделяющий Средиземноморское побережье и Анатолийское плато. 
По пути будут остановки для купания, отдыха или фотографирования захватывающих пейзажей. 

После возвращения в сафари-центр вас отвезут обратно в отель в Анталии. 
Это идеальное сочетание драйва, природы и веселья!",
    descriptionPo:
@"Przygotuj się na dzień pełen adrenaliny podczas Antalya Buggy Safari! 
Ta 3-godzinna wyprawa zabierze Cię przez wspaniałe, zmieniające się wydmy gór Taurus. 

Po odebraniu z hotelu w Kemer zostaniesz przewieziony do podnóży surowych (czasem ośnieżonych) gór Taurus, gdzie odbędzie się instruktaż bezpieczeństwa. 

Następnie ruszysz w teren buggy, odkrywając pustynny krajobraz oddzielający wybrzeże Morza Śródziemnego od anatolijskiego płaskowyżu. 
Po drodze przewidziane są przerwy na kąpiel, relaks i zdjęcia pięknych widoków. 

Po powrocie do centrum safari zostaniesz odwieziony z powrotem do hotelu w Antalyi. 
To doskonałe połączenie przygody, natury i zabawy!",
    descriptionPe:
@"آماده یک ماجراجویی پر از آدرنالین شوید با تور باگی در آنتالیا! 
این سفر ۳ ساعته شما را از میان تپه‌های شنی بی‌نظیر و همیشه در حال تغییر کوه‌های توروس عبور می‌دهد. 

پس از ترانسفر از هتل در کمر، به دامنه‌های کوه‌های توروس (که گاهی پوشیده از برف هستند) برده می‌شوید. 
در آنجا توضیحات ایمنی و نکات لازم برای رانندگی با باگی داده می‌شود. 

سپس وارد مسیر می‌شوید و از زمین‌های شبیه بیابان که سواحل مدیترانه را از فلات آناتولی جدا می‌کند عبور می‌کنید. 
در طول مسیر توقف‌هایی برای شنا، استراحت یا گرفتن عکس از مناظر فوق‌العاده خواهید داشت. 

در پایان، به مرکز سافاری بازمی‌گردید و سپس به راحتی به هتل خود در آنتالیا منتقل می‌شوید. 
ترکیبی کامل از هیجان، طبیعت و تفریح در انتظار شماست!",
    descriptionAr:
@"استعد لمغامرة مليئة بالإثارة مع جولة عربات الباجي في أنطاليا! 
تأخذك هذه الرحلة التي تستغرق 3 ساعات عبر الكثبان الرملية المذهلة والمتغيرة باستمرار في جبال طوروس. 

بعد اصطحابك من الفندق في كيمر، سيتم نقلك إلى سفوح جبال طوروس الوعرة (وأحياناً المغطاة بالثلوج) حيث ستتلقى تعليمات السلامة. 

ثم تنطلق في مغامرتك بالباچي، مكتشفاً التضاريس الصحراوية التي تفصل الساحل المتوسطي عن هضبة الأناضول. 
تتوقف عدة مرات للسباحة أو الاسترخاء أو التقاط صور للمناظر الطبيعية الخلابة. 

بعد العودة إلى مركز السفاري، سيتم نقلك مرة أخرى إلى فندقك في أنطاليا. 
إنها تجربة مثالية تمزج بين الإثارة، الطبيعة والمرح!",
miniDescriptionEn: "3-hour buggy blast across Taurus dunes—briefing, helmets, splash stops and big Mediterranean views.",
miniDescriptionDe: "3 Stunden Buggy-Action über Dünen im Taurus – Einweisung, Helme, Bade-Stopps und Mittelmeer-Panoramen.",
miniDescriptionRu: "3 часа багги по дюнам Тавра — инструктаж, шлемы, водные остановки и средиземноморские виды.",
miniDescriptionPo: "3 godziny jazdy buggy przez wydmy Taurusu – szkolenie, kaski, postoje na kąpiel i widoki na Morze Śródziemne.",
miniDescriptionPe: "3 ساعت هیجان باگی روی تپه‌های شنی توروس – آموزش، کلاه ایمنی، توقف‌های آبی و مناظر مدیترانه.",
miniDescriptionAr: "جولة باجي 3 ساعات عبر كثبان طوروس – تعليمات أمان، خوذات، توقفات للسباحة ومناظر المتوسط.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Antalya Buggy Safari.jpg")
    },
    activeDay: 1111111,
    durationHours: 3,
    services: new List<string> {
        "Hotel pickup & drop-off (Kemer/Antalya)",
        "Safety briefing",
        "Buggy ride",
        "Helmet",
        "Swimming/photo breaks",
        "Insurance"
    }
),

new Tour(
    id: 12,
    name: "Antalya Quad Safari",
    price: 47,
    kinderPrice: 47,
    infantPrice: 0,
    category: Category.Adventure,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Get ready for an adrenaline-packed adventure with the Antalya Quad Safari! 
This 3-hour expedition takes you on a thrilling ride through the ever-changing sand dunes and rugged landscapes of the Taurus Mountains. 

After hotel pick-up from Kemer, you’ll be transferred to the foothills of the Taurus Mountains, where you’ll receive a full safety briefing and learn essential riding tips before setting out on your quad bike. 

Ride across the desert-like terrain that separates the Mediterranean coast from the Anatolian Plateau. 
Along the way, enjoy several breaks to swim, relax, or capture the stunning scenery with your camera. 

After an unforgettable adventure, return to the safari center and then relax on your transfer back to your hotel in Antalya. 
A perfect combination of excitement, nature, and fun awaits you!",
    descriptionDe:
@"Machen Sie sich bereit für ein Abenteuer voller Adrenalin beim Antalya Quad Safari! 
Diese 3-stündige Expedition führt Sie durch die abwechslungsreichen Sanddünen und die zerklüftete Landschaft des Taurusgebirges. 

Nach der Abholung vom Hotel in Kemer fahren Sie zu den Ausläufern des Taurusgebirges, gdzie Sie eine Sicherheitseinweisung und Fahrhinweise erhalten. 

Dann geht es los: Mit dem Quad durchqueren Sie das wüstenartige Gelände, das die Mittelmeerküste vom anatolischen Hochplateau trennt. 
Unterwegs gibt es Stopps zum Schwimmen, Entspannen oder Fotografieren der beeindruckenden Landschaft. 

Nach dem Abenteuer kehren Sie ins Safari-Zentrum zurück und werden anschließend zu Ihrem Hotel in Antalya gebracht. 
Ein perfekter Mix aus Spannung, Natur und Spaß erwartet Sie!",
    descriptionRu:
@"Приготовьтесь к приключению, полному адреналина, на квадроциклах в Анталии! 
За 3 часа вы отправитесь в захватывающее путешествие по песчаным дюнам и суровым пейзажам гор Тавра. 

После трансфера из отеля в Кемере вас доставят к подножию Таврских гор, где проведут инструктаж по безопасности и дадут советы по вождению квадроцикла. 

Вы будете кататься по пустынной местности, разделяющей Средиземноморское побережье и Анатолийское плато. 
В пути будут остановки для купания, отдыха и фотографирования живописных видов. 

После незабываемого приключения вы вернётесь в сафари-центр, а затем – обратно в отель в Анталии. 
Идеальное сочетание драйва, природы и веселья!",
    descriptionPo:
@"Przygotuj się na pełną adrenaliny przygodę podczas Antalya Quad Safari! 
Ta 3-godzinna wyprawa zabierze Cię przez zmieniające się wydmy i surowe krajobrazy gór Taurus. 

Po odebraniu z hotelu w Kemer zostaniesz przewieziony do podnóży gór Taurus, gdzie odbędzie się instruktaż bezpieczeństwa i nauka podstaw jazdy na quadzie. 

Następnie wyruszysz w teren، pokonując pustynny krajobraz oddzielający wybrzeże Morza Śródziemnego od anatolijskiego płaskowyżu. 
Po drodze przewidziane są przerwy na kąpiel, relaks lub zdjęcia pięknych widoków. 

Po powrocie do centrum safari zostaniesz odwieziony z powrotem do hotelu w Antalyi. 
To doskonałe połączenie przygody, natury i świetnej zabawy!",
    descriptionPe:
@"آماده یک ماجراجویی پرهیجان شوید با تور کوادسافاری در آنتالیا! 
این سفر ۳ ساعته شما را از میان تپه‌های شنی و مناظر کوه‌های توروس عبور می‌دهد. 

پس از ترانسفر از هتل در کمر، به دامنه‌های کوه‌های توروس برده می‌شوید. 
در آنجا توضیحات ایمنی و نکات لازم برای رانندگی با موتور چهارچرخ ارائه می‌شود. 

سپس مسیر خود را در زمین‌های شبیه بیابان آغاز می‌کنید؛ جایی که سواحل مدیترانه از فلات آناتولی جدا می‌شود. 
در طول راه توقف‌هایی برای شنا، استراحت و عکاسی از مناظر بی‌نظیر خواهید داشت. 

در پایان، به مرکز سافاری بازمی‌گردید و سپس به راحتی به هتل خود در آنتالیا منتقل می‌شوید. 
ترکیبی عالی از هیجان، طبیعت و تفریح در انتظار شماست!",
    descriptionAr:
@"استعد لمغامرة مليئة بالإثارة مع جولة الدراجات الرباعية في أنطاليا! 
تأخذك هذه الرحلة التي تستغرق 3 ساعات عبر الكثبان الرملية المتغيرة باستمرار والمناظر الطبيعية الوعرة في جبال طوروس. 

بعد اصطحابك من الفندق في كيمر، سيتم نقلك إلى سفوح جبال طوروس حيث ستحصل على تعليمات السلامة ونصائح القيادة قبل الانطلاق. 

انطلق في مغامرتك بالدراجة الرباعية عبر التضاريس الصحراوية التي تفصل الساحل المتوسطي عن هضبة الأناضول. 
ستتوقف عدة مرات للسباحة أو الاسترخاء أو التقاط صور للمناظر الخلابة. 

بعد انتهاء المغامرة، ستعود إلى مركز السفاري، ومن ثم تُنقل براحة إلى فندقك في أنطاليا. 
إنها تجربة مثالية تمزج بين الإثارة والطبيعة والمرح!",
miniDescriptionEn: "Throttle up a quad through Taurus trails—safety briefing, scenic dirt tracks and refreshing swim breaks.",
miniDescriptionDe: "Quad-Spaß im Taurus – Sicherheitseinweisung, Schotterpisten mit Aussicht und erfrischende Badepausen.",
miniDescriptionRu: "Квадросафари в Тавре — брифинг по безопасности, живописные грунтовки и освежающие купания.",
miniDescriptionPo: "Quad safari w Tauruse – instruktaż, malownicze szlaki off-road i orzeźwiające postoje na kąpiel.",
miniDescriptionPe: "کوادسافاری در توروس – توضیحات ایمنی، مسیرهای آفرود دیدنی و توقف‌های خنک برای شنا.",
miniDescriptionAr: "سفاري دراجات رباعية في طوروس – توجيه أمني، مسارات وعرة خلابة وتوقفات منعشة للسباحة.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Antalya Quad Safari.jpg")
    },
    activeDay: 1111111,
    durationHours: 3,
    services: new List<string> {
        "Hotel pickup & drop-off (Kemer/Antalya)",
        "Safety briefing",
        "Quad bike ride",
        "Helmet",
        "Swimming/photo breaks",
        "Insurance"
    }
)
,
new Tour(
    id: 13,
    name: "Dinner Cruise",
    price: 40,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Relaxing,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Step aboard a dinner cruise and enjoy a magical evening on the waters of Antalya. 
As the boat gently sails, take in the beautiful sights of the city’s coastline illuminated in the evening light. 

Relax on deck, feel the soft sea breeze, and enjoy a freshly prepared dinner served while you cruise. 
The calm atmosphere, delicious food, and scenic views create the perfect recipe for a memorable night out in Antalya. 

An ideal choice for couples, friends, or families who want to combine dining with a unique cruising experience.",
    descriptionDe:
@"Gehen Sie an Bord einer Dinner Cruise und genießen Sie einen zauberhaften Abend auf dem Wasser vor Antalya. 
Während das Boot sanft dahingleitet, erleben Sie die wunderschöne, abendlich beleuchtete Küstenlinie der Stadt. 

Entspannen Sie an Deck, spüren Sie die leichte Meeresbrise und genießen Sie ein frisch zubereitetes Abendessen während der Fahrt. 
Die ruhige Atmosphäre, das köstliche Essen und die herrlichen Ausblicke sorgen für einen unvergesslichen Abend in Antalya. 

Eine ideale Wahl für Paare, Freunde oder Familien, die Essen mit einem einzigartigen Kreuzfahrterlebnis verbinden möchten.",
    descriptionRu:
@"Отправьтесь в вечерний круиз и насладитесь волшебной атмосферой на воде в Анталии. 
Пока лодка мягко скользит по морю, любуйтесь красотой прибрежных огней города. 

Расслабьтесь на палубе, почувствуйте лёгкий морской бриз и насладитесь вкусным ужином, приготовленным прямо для вас. 
Спокойная атмосфера, вкусная еда и живописные виды создают идеальный вечер в Анталии. 

Прекрасный выбор для пар, друзей и семей, которые хотят совместить ужин с уникальной морской прогулкой.",
    descriptionPo:
@"Wejdź na pokład kolacyjnego rejsu i spędź magiczny wieczór na wodach Antalyi. 
Podczas gdy łódź powoli płynie, podziwiaj piękną linię brzegową miasta skąpaną w wieczornym świetle. 

Zrelaksuj się na pokładzie, poczuj morską bryzę i delektuj się świeżo przygotowaną kolacją serwowaną podczas rejsu. 
Spokojna atmosfera, pyszne jedzenie i malownicze widoki tworzą idealny przepis na niezapomniany wieczór w Antalyi. 

To doskonały wybór dla par, przyjaciół czy rodzin, którzy chcą połączyć kolację z wyjątkowym rejsem.",
    descriptionPe:
@"سوار یک کشتی شام شوید و یک شب جادویی را در آب‌های آنتالیا تجربه کنید. 
در حالی که قایق آرام حرکت می‌کند، از مناظر زیبای خط ساحلی آنتالیا در نور غروب و شب لذت ببرید. 

روی عرشه استراحت کنید، نسیم خنک دریا را حس کنید و از یک شام خوشمزه که در طول سفر سرو می‌شود لذت ببرید. 
فضای آرام، غذای لذیذ و مناظر دیدنی ترکیبی عالی برای یک شب به‌یادماندنی در آنتالیاست. 

انتخابی ایده‌آل برای زوج‌ها، دوستان یا خانواده‌هایی که می‌خواهند یک شام ویژه را با تجربه‌ای دریایی همراه کنند.",
    descriptionAr:
@"انضم إلى رحلة عشاء بحرية واستمتع بسهرة ساحرة على مياه أنطاليا. 
بينما يتحرك القارب بلطف، ستتمكن من مشاهدة مناظر الساحل المضيء بأضواء المساء. 

استرخِ على سطح القارب، واستشعر نسيم البحر العليل، وتناول عشاءً لذيذاً مُحضراً خصيصاً لك. 
الجو الهادئ والطعام الشهي والمناظر الخلابة هي الوصفة المثالية لليلة لا تُنسى في أنطاليا. 

خيار مثالي للأزواج أو الأصدقاء أو العائلات الراغبين في الجمع بين العشاء وتجربة بحرية فريدة.",

   miniDescriptionEn: "Gentle evening cruise with coastline views and a freshly prepared dinner—romantic, relaxed, memorable.",
miniDescriptionDe: "Sanfter Abendkruiz mit Küstenblick & frisch zubereitetem Dinner – romantisch, entspannt, unvergesslich.",
miniDescriptionRu: "Вечерний круиз с ужином и огнями побережья — романтично, спокойно, запоминается.",
miniDescriptionPo: "Wieczorny rejs z widokiem na wybrzeże i świeżą kolacją – romantycznie, spokojnie, niezapomnianie.",
miniDescriptionPe: "کروز شام آرام با منظره ساحل و شامی تازه – رمانتیک، ریلکس و ماندگار.",
miniDescriptionAr: "رحلة عشاء مسائية بهدوء البحر وإطلالات الساحل – طعام طازج وأجواء رومانسية لا تُنسى.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Dinner Cruise.jpg")
    },
    activeDay: 1111111,
    durationHours: 3,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Evening boat cruise",
        "Freshly prepared dinner",
        "Scenic coastline views"
    }
),

new Tour(
    id: 14,
    name: "3 Different Waterfalls in Antalya",
    price: 40,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Nature,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"A visit to Antalya isn’t complete without exploring its famous waterfalls! 
On this full-day excursion, discover 3 of Antalya’s most beautiful waterfalls and enjoy the breathtaking natural scenery. 

The tour also includes a relaxing Manavgat river cruise, where you can sit back and enjoy the views, and a shopping experience at the lively Turkish bazaar. 
This combination of nature, culture, and local life makes for a truly memorable day in Antalya. 

Perfect for families, friends, or solo travelers who want to experience more than just the beaches!",
    descriptionDe:
@"Ein Besuch in Antalya ist nicht vollständig ohne die berühmten Wasserfälle zu erkunden! 
Auf diesem Tagesausflug entdecken Sie 3 der schönsten Wasserfälle Antalyas und genießen die atemberaubende Naturkulisse. 

Die Tour beinhaltet außerdem eine entspannende Flussfahrt auf dem Manavgat sowie ein Einkaufserlebnis im lebhaften türkischen Basar. 
Diese Kombination aus Natur، Kultur und traditionellem Leben macht den Tag unvergesslich. 

Ideal für Familien، Freunde oder Alleinreisende، die mehr als nur Strand erleben möchten!",
    descriptionRu:
@"Посещение Анталии будет неполным без знакомства с её знаменитыми водопадами! 
Во время этой экскурсии вы увидите 3 самых красивых водопада Анталии и насладитесь потрясающими пейзажами. 

В программу также входит речной круиз по Манавгату и шопинг на оживлённом турецком базаре. 
Сочетание природы، культуры и местного колорита делает этот день незабываемым. 

Отличный выбор для семей، друзей или индивидуальных путешественников، которые хотят увидеть больше، чем пляжи!",
    descriptionPo:
@"Wizyta w Antalyi nie będzie pełna bez zobaczenia jej słynnych wodospadów! 
Podczas tej całodniowej wycieczki odkryjesz 3 najpiękniejsze wodospady Antalyi i zachwycisz się cudowną przyrodą. 

W programie znajduje się również rejs po rzece Manavgat oraz zakupy na tętniącym życiem tureckim bazarze. 
To połączenie natury، kultury i lokalnego życia sprawia، że dzień staje się niezapomniany. 

Idealna propozycja dla rodzin، przyjaciół i osób podróżujących solo، którzy chcą doświadczyć czegoś więcej niż tylko plaż!",
    descriptionPe:
@"سفر به آنتالیا بدون دیدن آبشارهای معروف آن کامل نمی‌شود! 
در این تور یک‌روزه با ۳ آبشار زیبا و دیدنی آنتالیا آشنا خواهید شد و از مناظر طبیعی خیره‌کننده لذت می‌برید. 

همچنین شامل یک کروز آرام بر روی رودخانه ماناوگات و تجربه خرید در بازار پرجنب‌وجوش ترکی است. 
ترکیبی از طبیعت، فرهنگ و زندگی محلی که روزی فراموش‌نشدنی را برایتان رقم می‌زند. 

گزینه‌ای عالی برای خانواده‌ها، دوستان یا مسافران تنها که می‌خواهند چیزی فراتر از سواحل را تجربه کنند!",
    descriptionAr:
@"لا تكتمل زيارة أنطاليا دون استكشاف شلالاتها الشهيرة! 
في هذه الرحلة التي تستغرق يوماً كاملاً ستزور 3 من أجمل شلالات أنطاليا وتستمتع بجمال الطبيعة الخلاب. 

تشمل الجولة أيضاً رحلة نهرية هادئة في مانافغات وتجربة تسوق في السوق التركي النابض بالحياة. 
هذا المزيج من الطبيعة والثقافة والحياة المحلية يجعل يومك لا يُنسى. 

خيار مثالي للعائلات أو الأصدقاء أو المسافرين المنفردين الذين يرغبون في تجربة أكثر من مجرد الشواطئ!",
   miniDescriptionEn: "Full-day nature combo: three iconic waterfalls, Manavgat river cruise and time at the lively Turkish bazaar.",
miniDescriptionDe: "Natur pur an einem Tag: drei Wasserfälle, Manavgat-Flussfahrt und Zeit auf dem lebhaften türkischen Basar.",
miniDescriptionRu: "День природы: три водопада, речной круиз по Манавгату и прогулка по турецкому базару.",
miniDescriptionPo: "Cały dzień w naturze: 3 wodospady, rejs Manavgat i czas na tętniący życiem turecki bazar.",
miniDescriptionPe: "یک روزِ طبیعت: سه آبشار معروف، کروز رود ماناوگات و وقتی خوش در بازار پرجنب‌وجوش ترکی.",
miniDescriptionAr: "يوم طبيعي متكامل: ثلاثة شلالات، رحلة نهرية في مانافغات ووقت في البازار التركي النابض.",
    fotos: new List<Foto> {
        new Foto("/tourimage/3 Different Waterfalls in Antalya.jpg")
    },
    activeDay: 1111111,
    durationHours: 9,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Visit 3 waterfalls",
        "Manavgat river cruise",
        "Visit Turkish bazaar",
        "Free time for shopping"
    }
),

new Tour(
    id: 15,
    name: "Perge, Side, Aspendos & Kursunlu Waterfalls",
    price: 100,
    kinderPrice: 55,
    infantPrice: 0,
    category: Category.History,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Travel back in time with a full-day excursion to some of the most fascinating historical and natural sights near Antalya. 

Begin with Perge, located just 15 km from Antalya. According to legend, it was founded after the Trojan War by the prophet Calchas. 
Today, it is one of the region’s most impressive archaeological sites, featuring Roman-era ruins such as an amphitheater, stadium, city walls, agora, basilica, Roman baths, and more. 
Stroll along the 500-meter-long main street lined with columns and visit the acropolis and ancient fountain. 

Continue to Aspendos, one of Pamphylia’s most important cities. Its world-famous Roman amphitheater, built in 155 AD, is considered one of the best-preserved examples of Roman theatre architecture and still hosts cultural events and festivals today. 
Climb the steps, admire the galleries, and imagine the grand performances that still bring the theatre to life. 

After lunch, head to Side, a spectacular ancient city on a small peninsula. 
Here you can explore ruins from the Hellenistic, Roman, and Byzantine eras, including the agora, amphitheater, Roman baths, basilica, and the stunning Temple of Apollo overlooking the turquoise sea. 

Finish the day with a visit to the beautiful Kursunlu Waterfalls Natural Park. 
Enjoy the fresh pine-scented air, shaded walking paths, turquoise pools, and cascading waterfalls, making this the perfect place to relax after a day of exploration. 
Don’t forget to capture a photo behind the waterfall for an unforgettable memory. 

A journey combining history, culture, and natural beauty – an Antalya must-do!",
    descriptionDe:
@"Gehen Sie auf eine Zeitreise mit diesem Ganztagesausflug zu den faszinierendsten historischen und natürlichen Sehenswürdigkeiten in der Nähe von Antalya. 

Beginnen Sie in Perge, nur 15 km von Antalya entfernt. Der Legende nach wurde die Stadt nach dem Trojanischen Krieg vom Propheten Kalchas gegründet. 
Heute zählt sie zu den beeindruckendsten archäologischen Stätten der Region mit Ruinen aus der Römerzeit: Amphitheater, Stadion، Stadtmauern، Agora، Basilika، römische Bäder und vieles mehr. 
Spazieren Sie die 500 Meter lange Hauptstraße entlang und besichtigen Sie die Akropolis und den antiken Brunnen. 

Weiter geht es nach Aspendos، eine der wichtigsten Städte Pamphyliens. 
Das weltberühmte römische Amphitheater، 155 n. Chr. erbaut، ist eines der besterhaltenen Beispiele römischer Theaterarchitektur und wird bis heute für Festivals und Aufführungen genutzt. 

Nach dem Mittagessen besuchen Sie Side، eine spektakuläre antike Stadt auf einer kleinen Halbinsel. 
Hier erwarten Sie zahlreiche Ruinen aus der hellenistischen، römischen und byzantinischen Zeit، darunter die Agora، das Amphitheater، die römischen Bäder، die Basilika und der Tempel des Apollo mit Blick auf das Meer. 

Zum Abschluss des Tages besichtigen Sie den wunderschönen Naturpark Kursunlu-Wasserfälle. 
Genießen Sie die frische Luft، den Duft der Pinien، die türkisfarbenen Wasserbecken und die spektakulären Kaskaden. 
Ein perfekter Ort، um nach einem ereignisreichen Tag zu entspannen und unvergessliche Fotos zu machen.",
    descriptionRu:
@"Отправьтесь в увлекательное путешествие по самым знаменитым историческим и природным достопримечательностям региона Анталии. 

Первым пунктом будет Перге، расположенный всего в 15 км от города. 
По легенде، город был основан после Троянской войны пророком Калхасом. 
Сегодня это впечатляющий археологический комплекс с амфитеатром، стадионом، городскими стенами، агорой، римскими банями и другими постройками. 
Прогуляйтесь по главной улице с колоннами и посетите акрополь. 

Затем вы отправитесь в Аспендос – один из важнейших городов Памфилии. 
Здесь находится знаменитый римский амфитеатр، построенный в 155 году н. э. и отлично сохранившийся до наших дней. 
Он до сих пор используется для культурных мероприятий и фестивалей. 

После обеда экскурсия продолжится в древнем городе Сиде، расположенном на небольшом полуострове. 
Вы увидите агору، амфитеатр، римские бани، базилику и знаменитый храм Аполлона на берегу моря. 

В завершение дня вы посетите природный парк Куршунлу с его живописными водопадами. 
Свежий воздух، запах сосен، прохлада и бирюзовая вода создают прекрасную атмосферу для отдыха и фотографий. 

Идеальное сочетание истории، культуры и природы!",
    descriptionPo:
@"Wyrusz w podróż w czasie podczas całodniowej wycieczki do najpiękniejszych miejsc historycznych i przyrodniczych w okolicach Antalyi. 

Pierwszym przystankiem będzie Perge، oddalone o 15 km od Antalyi. 
Według legendy miasto zostało założone po wojnie trojańskiej przez proroka Kalchasa. 
Dziś to imponujące stanowisko archeologiczne z ruinami z czasów rzymskich: amfiteatr، stadion، mury miejskie، agora، bazylika، rzymskie łaźnie i wiele innych. 

Następnie Aspendos – jedno z najważniejszych miast Pamfilii. 
Słynie z rzymskiego amfiteatru zbudowanego w 155 r. n.e., który do dziś jest jednym z najlepiej zachowanych teatrów rzymskich na świecie i miejscem festiwali kulturalnych. 

Po obiedzie zwiedzisz Side – spektakularne starożytne miasto położone na małym półwyspie. 
Zobaczysz tu agorę، amfiteatr، łaźnie rzymskie، bazylikę oraz słynną świątynię Apolla z widokiem na morze. 

Na zakończenie dnia odwiedzisz piękny park przyrodniczy z wodospadami Kursunlu. 
Świeże powietrze، zapach sosen، turkusowe baseny i malownicze kaskady sprawiają، że to idealne miejsce na odpoczynek i pamiątkowe zdjęcia. 

Połączenie historii، kultury i natury، którego nie można przegapić!",
    descriptionPe:
@"یک سفر یک‌روزه برای بازگشت به تاریخ و کشف جاذبه‌های طبیعی و فرهنگی آنتالیا! 

ابتدا به شهر باستانی پرگه می‌روید که تنها ۱۵ کیلومتر با آنتالیا فاصله دارد. 
بر اساس افسانه‌ها، پرگه پس از جنگ تروآ توسط پیامبر کالخاس تأسیس شد. 
امروز یکی از چشمگیرترین محوطه‌های باستان‌شناسی منطقه است و شامل آمفی‌تئاتر رومی، استادیوم، دیوارهای شهر، بازار، باسیلیکا و حمام‌های رومی است. 

سپس به آسپندوس خواهید رفت – یکی از مهم‌ترین شهرهای پامفیلیا. 
اینجا آمفی‌تئاتر مشهور رومی قرار دارد که در سال ۱۵۵ میلادی ساخته شد و یکی از بهترین نمونه‌های باقی‌مانده معماری رومی در جهان است. 
این مکان هنوز هم برای رویدادهای فرهنگی و جشنواره‌ها مورد استفاده قرار می‌گیرد. 

بعد از ناهار، به شهر باستانی سیده می‌روید. 
این شهر روی یک شبه‌جزیره قرار دارد و شامل بقایای دوره‌های هلنیستی، رومی و بیزانسی است. 
معبد آپولو در بندر قدیمی سیده یکی از زیباترین مناظر است که نباید از دست داد. 

در پایان روز، از پارک طبیعی آبشارهای کورشونلو بازدید می‌کنید. 
هوای تازه، عطر درختان کاج، محیط خنک و آبشارهای زیبا تجربه‌ای عالی برای استراحت پس از یک روز طولانی خواهد بود. 
حتماً عکس‌های فوق‌العاده‌ای پشت آبشار بگیرید!",
    descriptionAr:
@"انطلق في رحلة ليوم كامل لاستكشاف أبرز المعالم التاريخية والطبيعية بالقرب من أنطاليا. 

نبدأ بزيارة مدينة بيرجه القديمة، التي تبعد 15 كم فقط عن أنطاليا. 
وفقاً للأسطورة، تأسست بعد حرب طروادة على يد النبي كالكاس. 
اليوم هي موقع أثري مثير يضم مسرحاً رومانياً، استاداً، أسواراً، أغورا، بازيليكا وحمامات رومانية. 

ثم نواصل إلى أسبندوس – إحدى أهم مدن بامفيليا. 
تشتهر بمسرحها الروماني الذي بُني عام 155 م ويُعد من أفضل النماذج المحفوظة عالمياً ويستضيف مهرجانات وعروضاً حتى اليوم. 

بعد الغداء نزور مدينة سايد القديمة، الواقعة على شبه جزيرة صغيرة. 
هنا سترى الآغورا، المسرح، الحمامات الرومانية، البازيليكا، وأبرزها معبد أبولو المطل على البحر. 

وأخيراً، نزور شلالات كورشونلو في حديقة طبيعية خلابة. 
استمتع بالهواء النقي ورائحة الصنوبر والأجواء المنعشة والمياه الفيروزية المتدفقة. 
مكان رائع للاسترخاء والتقاط صور لا تُنسى خلف الشلال.",
miniDescriptionEn: "Time-travel day: Roman theatres, Apollo’s Temple, ancient ruins and a refreshing stop at Kursunlu Falls.",
miniDescriptionDe: "Zeitreise: römische Theater, Apollon-Tempel, antike Ruinen und erfrischende Kursunlu-Wasserfälle.",
miniDescriptionRu: "День истории: римские театры, храм Аполлона, древние руины и отдых у водопада Куршунлу.",
miniDescriptionPo: "Podróż w czasie: teatry rzymskie, świątynia Apolla i relaks przy wodospadach Kursunlu.",
miniDescriptionPe: "یک روز تاریخی: تئاترهای رومی، معبد آپولو، ویرانه‌های باستانی و آبشارهای کورشونلو.",
miniDescriptionAr: "رحلة عبر الزمن: مسارح رومانية، معبد أبولو وآثار قديمة مع وقفة منعشة عند شلالات كورشونلو.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Perge, Side, Aspendos & Kursunlu Waterfalls.jpg")
    },
    activeDay: 1111111,
    durationHours: 10,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Professional guide",
        "Visit Perge",
        "Visit Aspendos amphitheatre",
        "Visit Side (Temple of Apollo)",
        "Visit Kursunlu Waterfalls",
        "Lunch"
    }
),

new Tour(
    id: 16,
    name: "Green Canyon Boat Trip (Full-Day)",
    price: 57,
    kinderPrice: 29,
    infantPrice: 0,
    category: Category.Nature,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Enjoy a full-day boat trip into the heart of nature at the beautiful Green Canyon. 
Your day begins with a pick-up from your hotel and a bus ride to the canyon, where the adventure starts. 

Cruise through the 14-kilometer-long Grand Canyon and the 3-kilometer-long Little Canyon, with stops along the way for swimming in the refreshing waters. 
Admire the emerald-green lake surrounded by lush landscapes, and keep an eye out for the rare brown fish owl! 

Later, enjoy a swimming break and a delicious lunch at a local restaurant overlooking the lake. 
After lunch, continue exploring the other side of the canyon, where stunning views of the mountains and woodlands await. 

End the day with another swim stop before returning comfortably to your resort. 
A relaxing yet unforgettable adventure into Antalya’s natural beauty.",
    descriptionDe:
@"Genießen Sie einen ganztägigen Bootsausflug in die herrliche Natur des Green Canyon. 
Der Tag beginnt mit der Abholung von Ihrem Hotel und einer Busfahrt zum Canyon, wo Ihr Abenteuer startet. 

Sie fahren mit dem Boot durch den 14 Kilometer langen Grand Canyon und den 3 Kilometer langen Little Canyon, mit Badestopps unterwegs. 
Bewundern Sie den smaragdgrünen See, umgeben von üppiger Landschaft, und halten Sie Ausschau nach der seltenen braunen Fischeule! 

Später gibt es eine Badepause sowie ein köstliches Mittagessen in einem lokalen Restaurant mit Blick auf den See. 
Anschließend entdecken Sie die andere Seite des Canyons mit spektakulären Ausblicken auf die Berge und Wälder. 

Zum Abschluss gibt es noch eine Schwimmpause, bevor Sie bequem in Ihr Hotel zurückgebracht werden. 
Ein entspannendes, aber unvergessliches Naturerlebnis!",
    descriptionRu:
@"Отправьтесь в однодневное путешествие в сердце природы – в живописный Зелёный каньон. 
Ваш день начнётся с трансфера из отеля и поездки на автобусе к каньону. 

Вы совершите прогулку на лодке по Большому каньону (14 км) и Малому каньону (3 км), останавливаясь для купания. 
Наслаждайтесь изумрудным озером, окружённым зелёными пейзажами, и постарайтесь заметить редкую коричневую рыбную сову! 

Позже вас ждёт купание и вкусный обед в местном ресторане с видом на озеро. 
После обеда продолжите исследовать другую часть каньона, наслаждаясь потрясающими видами гор и лесов. 

В завершение будет ещё одна остановка для купания, а затем возвращение в ваш отель. 
Это расслабляющее, но незабываемое путешествие в красоту Анталии.",
    descriptionPo:
@"Przeżyj całodniową wycieczkę statkiem w samym sercu natury – w pięknym Zielonym Kanionie. 
Dzień rozpoczyna się odbiorem z hotelu i przejazdem autobusem do kanionu. 

Popłyniesz przez 14-kilometrowy Wielki Kanion i 3-kilometrowy Mały Kanion, zatrzymując się po drodze na kąpiel. 
Podziwiaj szmaragdowe jezioro otoczone bujną przyrodą i wypatruj rzadkiej sowy rybołownej! 

Później czeka Cię przerwa na kąpiel oraz pyszny lunch w lokalnej restauracji z widokiem na jezioro. 
Po obiedzie kontynuuj odkrywanie drugiej części kanionu, podziwiając wspaniałe widoki gór i lasów. 

Na zakończenie jeszcze jedna przerwa na pływanie, a potem powrót do hotelu. 
Relaksująca, a zarazem niezapomniana przygoda w naturze Antalyi.",
    descriptionPe:
@"یک سفر یک‌روزه با قایق به قلب طبیعت – دره زیبای گرین کنیون! 
روز شما با ترانسفر از هتل و حرکت با اتوبوس به سمت کنیون آغاز می‌شود. 

قایق‌سواری در گرند کنیون به طول ۱۴ کیلومتر و لیتل کنیون به طول ۳ کیلومتر همراه با توقف‌هایی برای شنا در آب‌های زلال تجربه‌ای فراموش‌نشدنی خواهد بود. 
از دریاچه سبز زمردی که با مناظر سرسبز احاطه شده لذت ببرید و به دنبال جغد نادر ماهی‌خوار قهوه‌ای باشید! 

سپس در کنار دریاچه استراحت کرده و ناهاری خوشمزه در یک رستوران محلی با چشم‌انداز زیبا میل کنید. 
بعد از ناهار، بخش دیگر کنیون را کشف کرده و از مناظر فوق‌العاده کوه‌ها و جنگل‌ها لذت ببرید. 

در پایان، یک بار دیگر برای شنا توقف خواهید داشت و سپس به راحتی به هتل بازمی‌گردید. 
ترکیبی از آرامش و ماجراجویی در دل طبیعت آنتالیا!",
    descriptionAr:
@"استمتع برحلة بحرية ليوم كامل إلى قلب الطبيعة في الوادي الأخضر (Green Canyon). 
يبدأ يومك باصطحابك من الفندق وركوب الحافلة إلى الوادي. 

ستبحر بالقارب عبر الوادي الكبير (14 كم) والوادي الصغير (3 كم)، مع توقفات للسباحة على طول الطريق. 
أعجب بجمال البحيرة الخضراء الزمردية والمناظر الطبيعية الخلابة من حولك، ولا تنسَ البحث عن البومة النادرة البنية! 

بعد الاستكشاف، استمتع بوقت للسباحة وغداء لذيذ في مطعم محلي يطل على البحيرة. 
بعد الغداء، واصل الرحلة إلى الجزء الآخر من الوادي، حيث تنتظرك مناظر خلابة للجبال والغابات. 

تتوقف مرة أخرى للسباحة قبل العودة إلى الفندق. 
مغامرة مريحة ولا تُنسى في قلب طبيعة أنطاليا.",
miniDescriptionEn: "Relaxing cruise through emerald lakes and canyons with swim stops and a lake-view lunch.",
miniDescriptionDe: "Bootstour durch smaragdgrüne Schluchten mit Badestopps und Mittagessen am See.",
miniDescriptionRu: "Прогулка по изумрудному каньону: купание, отдых и обед с видом на озеро.",
miniDescriptionPo: "Rejs po Zielonym Kanionie – kąpiele, relaks i obiad z widokiem na jezioro.",
miniDescriptionPe: "قایق‌سواری در گرین کنیون؛ شنا، آرامش و ناهار با چشم‌انداز دریاچه.",
miniDescriptionAr: "رحلة قارب مريحة عبر الوادي الأخضر مع محطات سباحة وغداء مطل على البحيرة.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Green Canyon Boat Trip (Full-Day).jpg")
    },
    activeDay: 1111111,
    durationHours: 9,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Boat cruise (Grand & Little Canyon)",
        "Swimming stops",
        "Lunch at lake-view restaurant",
        "Insurance"
    }
)
,
new Tour(
    id: 17,
    name: "Antalya City Tour, Boat Trip & Waterfalls",
    price: 41,
    kinderPrice: 21,
    infantPrice: 0,
    category: Category.Family,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Discover the best of Antalya in one full-day city tour that combines history, culture, nature, and relaxation. 
Your day begins around 09:00 with a visit to Kaleici – Antalya’s charming old town. 
Follow your guide to see highlights such as Hadrian’s Gate, the Clock Tower, Kesik Minare, and the impressive city walls from the 2nd century. 
You’ll also have time to wander through boutiques, art galleries, and small museums in this historic area. 

Next, enjoy a visit to the Düden Waterfalls National Park. 
Walk through the lush park to admire the spectacular Karpuzkaldıran (Lower Düden) Waterfall, also called the Alexander Falls. 
At 30 meters high, it is the tallest cascade in the Antalya region and offers stunning views over the coastline. 

The tour continues with a stop at a traditional handicraft workshop. 
Here you’ll learn about centuries-old Turkish craftsmanship, still carried on by small family businesses in Antalya. 

Finally, end the day with a relaxing boat trip on the Mediterranean Sea. 
Soak in the refreshing breeze and enjoy breathtaking panoramic views of Antalya’s old town and sparkling coastline. 
A perfect mix of culture, history, and natural beauty!",
    descriptionDe:
@"Entdecken Sie die Höhepunkte von Antalya bei einer ganztägigen Stadtrundfahrt, die Geschichte, Kultur, Natur und Erholung vereint. 
Der Tag beginnt gegen 09:00 Uhr mit einem Besuch in Kaleici – der Altstadt von Antalya. 
Mit Ihrem Guide erkunden Sie Sehenswürdigkeiten wie das Hadrianstor, den Uhrturm, die Kesik Minare und die Stadtmauer aus dem 2. Jahrhundert. 
Sie haben zudem Zeit, durch kleine Boutiquen, Kunstgalerien und Museen zu schlendern. 

Anschließend besuchen Sie den Düden-Wasserfall-Nationalpark. 
Spazieren Sie durch den Park und bewundern Sie den spektakulären Karpuzkaldıran-Wasserfall (Unterer Düden), der mit 30 Metern Höhe der größte Wasserfall der Region ist. 

Danach machen Sie Halt in einer traditionellen Handwerkswerkstatt. 
Hier erfahren Sie mehr über jahrhundertealte türkische Handwerkskunst, die bis heute von Familienbetrieben in Antalya gepflegt wird. 

Zum Abschluss genießen Sie eine entspannte Bootsfahrt auf dem Mittelmeer. 
Lehnen Sie sich zurück, genießen Sie die frische Brise und den Panoramablick auf die Altstadt und die Küste Antalyas. 
Ein perfekter Mix aus Kultur, Geschichte und Natur!",
    descriptionRu:
@"Откройте для себя всё самое лучшее в Анталии во время этого насыщенного тура, включающего историю, культуру, природу и отдых. 
День начнётся около 09:00 с экскурсии по Калейчи – старому городу Анталии. 
Вы увидите Ворота Адриана, Часовую башню, Минарет Кесик и городские стены II века, а также прогуляетесь по магазинам, галереям и музеям. 

Затем вы отправитесь в национальный парк водопадов Дюден. 
Прогуляйтесь по зелёной территории и полюбуйтесь водопадом Карпузкалдынан (Нижний Дюден), также известным как водопад Александра. 
Его высота – 30 метров, и он является самым высоким водопадом региона, открывающим великолепные виды на побережье. 

После этого вас ждёт визит в традиционную ремесленную мастерскую. 
Вы узнаете о многовековых турецких ремесленных традициях, которые до сих пор сохраняют местные семейные предприятия. 

Завершится тур морской прогулкой по Средиземному морю. 
Вы насладитесь свежим бризом и панорамными видами старого города и побережья. 
Идеальное сочетание культуры, истории и природной красоты!",
    descriptionPo:
@"Odkryj najważniejsze atrakcje Antalyi podczas całodniowej wycieczki, która łączy historię, kulturę, naturę i relaks. 
Dzień rozpoczyna się około godziny 09:00 od wizyty w Kaleici – starej części miasta. 
Z przewodnikiem zobaczysz Bramę Hadriana, Wieżę Zegarową, Kesik Minare i mury miejskie z II wieku. 
Będzie też czas na wizytę w butikach, galeriach sztuki i muzeach. 

Następnie udasz się do Parku Narodowego Wodospadów Düden. 
Spaceruj po parku i podziwiaj spektakularny wodospad Karpuzkaldıran (Dolny Düden), znany także jako Wodospad Aleksandra. 
Ma on 30 metrów wysokości i jest najwyższym wodospadem w regionie Antalyi. 

Kolejnym punktem programu jest wizyta w tradycyjnej pracowni rzemieślniczej. 
Poznasz tu wielowiekowe tureckie rzemiosło, które wciąż kultywują lokalne rodziny. 

Na zakończenie weźmiesz udział w relaksującym rejsie po Morzu Śródziemnym. 
Ciesz się świeżą bryzą i niesamowitymi panoramami starego miasta i wybrzeża Antalyi. 
To idealne połączenie kultury, historii i pięknej natury!",
    descriptionPe:
@"یک تور کامل شهری در آنتالیا که ترکیبی از تاریخ، فرهنگ، طبیعت و آرامش است! 
روز شما حدود ساعت ۹ صبح با بازدید از منطقه کاله‌ایچی – شهر قدیم آنتالیا آغاز می‌شود. 
با راهنما از جاذبه‌هایی مانند دروازه هادریان، برج ساعت، مسجد کسیک و دیوارهای شهر متعلق به قرن دوم دیدن خواهید کرد. 
همچنین فرصتی برای گردش در بوتیک‌ها، گالری‌های هنری و موزه‌های کوچک خواهید داشت. 

سپس به پارک ملی آبشارهای دودن می‌روید. 
در این پارک سرسبز قدم بزنید و از آبشار باشکوه کارپوزکالدیران (دودن پایینی) که ۳۰ متر ارتفاع دارد و بلندترین آبشار منطقه است، لذت ببرید. 

بعد از آن، بازدیدی از کارگاه‌های صنایع‌دستی سنتی خواهید داشت؛ جایی که با هنر چندصدساله ترک آشنا می‌شوید که هنوز توسط خانواده‌های محلی ادامه دارد. 

در پایان، با یک سفر دریایی آرامش‌بخش در دریای مدیترانه روز خود را کامل می‌کنید. 
از نسیم دریا و مناظر پانورامای شهر قدیم و خط ساحلی آنتالیا لذت ببرید. 
ترکیبی عالی از فرهنگ، تاریخ و زیبایی‌های طبیعی!",
    descriptionAr:
@"اكتشف أجمل معالم أنطاليا في جولة مدينة كاملة تجمع بين التاريخ والثقافة والطبيعة والاسترخاء. 
يبدأ يومك حوالي الساعة 09:00 صباحاً بزيارة كاليجي – المدينة القديمة. 
مع المرشد سترى بوابة هادريان، برج الساعة، منارة كسك والأسوار الرومانية من القرن الثاني. 
كما سيكون لديك وقت للتجول بين البوتيكات والمعارض الفنية والمتاحف الصغيرة. 

بعد ذلك، زيارة إلى حديقة شلالات دودان الوطنية. 
تمتع بالمشي بين الطبيعة الخضراء ومشاهدة شلال كاربوزكالدران (دودان السفلي) الذي يبلغ ارتفاعه 30 متراً، وهو الأعلى في منطقة أنطاليا. 

تشمل الجولة أيضاً التوقف عند ورشة يدوية تقليدية حيث ستتعرف على الحرف التركية العريقة التي ما زالت تمارسها العائلات المحلية. 

وأخيراً، اختتم يومك برحلة بحرية مريحة في البحر الأبيض المتوسط. 
استمتع بنسيم البحر العليل والإطلالات البانورامية على المدينة القديمة وساحل أنطاليا. 
مزيج مثالي من الثقافة والتاريخ والطبيعة!",
   miniDescriptionEn: "Old town walk, Düden Waterfalls, handicrafts and a scenic Mediterranean boat trip – all in one day.",
miniDescriptionDe: "Altstadt-Tour, Düden-Wasserfälle, Handwerk und Bootsfahrt im Mittelmeer an einem Tag.",
miniDescriptionRu: "Старая Анталия, водопады Дюден, ремесла и морская прогулка за один день.",
miniDescriptionPo: "Zwiedzanie starego miasta, wodospady Düden i rejs po Morzu Śródziemnym – wszystko w jeden dzień.",
miniDescriptionPe: "گردش در شهر قدیم، آبشار دودن، صنایع دستی و کروز مدیترانه‌ای – همه در یک روز.",
miniDescriptionAr: "جولة مدينة، شلالات دودان، ورشة تقليدية ورحلة بحرية متوسطية في يوم واحد.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Antalya City Tour, Boat Trip & Waterfalls.jpg")
    },
    activeDay: 1111111,
    durationHours: 8,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Guided Kaleici walking tour",
        "Visit Hadrian’s Gate, Clock Tower, Kesik Minare",
        "Visit Düden Waterfalls (Lower Düden)",
        "Handicraft workshop stop",
        "Mediterranean boat trip",
        "Insurance"
    }
),
new Tour(
    id: 18,
    name: "Koprulu Canyon Rafting and Canyoning",
    price: 49,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Adventure,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Experience an action-packed full-day adventure in Koprulu Canyon National Park. 
Get your adrenaline pumping as you raft through the thrilling whitewater rapids of the Kopru River and explore the stunning natural beauty of the canyon. 

Your journey begins with hotel pick-up from Antalya and nearby resorts before heading into the heart of the Taurus Mountains. 
Professional guides provide a safety briefing and equipment, ensuring you are ready for an unforgettable experience. 

Raft along the turquoise waters surrounded by steep canyon walls, dramatic cliffs, and lush pine forests. 
Enjoy swimming breaks in crystal-clear water and take in the breathtaking scenery. 

Add even more excitement with canyoning, as you climb, swim, and hike through narrow gorges and waterfalls. 
Whether you’re a beginner or an adventure lover, this day guarantees thrills, laughter, and memories to last a lifetime. 

A must-do for anyone looking for adrenaline and natural beauty in Antalya!",
    descriptionDe:
@"Erleben Sie ein actionreiches Ganztagesabenteuer im Nationalpark Köprülü-Canyon. 
Spüren Sie den Adrenalinkick beim Rafting auf den wilden Stromschnellen des Köprü-Flusses und entdecken Sie die atemberaubende Natur des Canyons. 

Nach der Abholung vom Hotel in Antalya und den umliegenden Resorts fahren Sie in das Herz des Taurusgebirges. 
Professionelle Guides geben eine Sicherheitseinweisung und stellen die Ausrüstung bereit, bevor das Abenteuer beginnt. 

Sie fahren auf dem türkisfarbenen Wasser zwischen steilen Felswänden، dramatischen Klippen und Pinienwäldern. 
Unterwegs gibt es Pausen zum Schwimmen und Genießen der Landschaft. 

Zusätzlich können Sie beim Canyoning klettern، schwimmen und durch enge Schluchten und Wasserfälle wandern. 
Ob Anfänger oder Abenteurer – dieser Tag verspricht Spannung، Spaß und unvergessliche Erlebnisse. 

Ein absolutes Muss für alle, die Adrenalin und Natur in Antalya suchen!",
    descriptionRu:
@"Отправьтесь в насыщенное приключение на целый день в национальный парк Кёпрюлю-Каньон. 
Почувствуйте прилив адреналина, проходя пороги реки Кёпрю на рафтах, и откройте для себя невероятную природу каньона. 

Ваш день начнётся с трансфера из Анталии и близлежащих курортов в горы Тавра. 
Профессиональные инструкторы проведут инструктаж по безопасности и подготовят снаряжение. 

Вы будете сплавляться по бирюзовой реке, окружённой крутыми скалами, ущельями и сосновыми лесами. 
В пути предусмотрены остановки для купания в кристально чистой воде. 

Дополнительно программа включает каньонинг – восхождение, плавание и прогулку через ущелья и водопады. 
Подходит как новичкам, так и любителям острых ощущений. 

Незабываемый день, полный драйва и красоты природы Анталии!",
    descriptionPo:
@"Przeżyj pełen wrażeń dzień w Parku Narodowym Kanionu Köprülü. 
Poczuj przypływ adrenaliny podczas raftingu na rwącej rzece Köprü i odkryj piękno przyrody kanionu. 

Wycieczka rozpoczyna się od odbioru z hotelu w Antalyi i okolicznych kurortach, a następnie przejazdu w góry Taurus. 
Profesjonalni przewodnicy zapewnią instruktaż bezpieczeństwa i sprzęt. 

Spływaj po turkusowej rzece otoczonej stromymi ścianami kanionu, klifami i lasami sosnowymi. 
Po drodze przewidziane są przerwy na kąpiel w krystalicznie czystej wodzie. 

Jeszcze więcej emocji zapewni kanioning – wspinaczka, pływanie i przejścia przez wąskie wąwozy i wodospady. 
Dzień pełen adrenaliny, śmiechu i niezapomnianych wspomnień gwarantowany! 

Idealna wycieczka dla osób poszukujących przygody i piękna natury w Antalyi.",
    descriptionPe:
@"یک روز پرهیجان در پارک ملی کپرولو کانیون! 
با رفتینگ در رودخانه کپرولو آدرنالین خود را به اوج برسانید و از طبیعت خیره‌کننده دره لذت ببرید. 

روز شما با ترانسفر از هتل در آنتالیا و دیگر مناطق اطراف آغاز می‌شود. 
راهنماهای حرفه‌ای نکات ایمنی و تجهیزات لازم را در اختیار شما قرار می‌دهند. 

قایق‌سواری در آب‌های فیروزه‌ای میان صخره‌های بلند، جنگل‌های کاج و مناظر دیدنی تجربه‌ای بی‌نظیر خواهد بود. 
در طول مسیر توقف‌هایی برای شنا در آب زلال خواهید داشت. 

هیجان خود را با کنیونینگ بیشتر کنید – صعود، شنا و عبور از میان شکاف‌ها و آبشارها. 
فرقی نمی‌کند مبتدی باشید یا ماجراجو، این روز پر از هیجان و خاطره‌های فراموش‌نشدنی خواهد بود. 

یک انتخاب عالی برای کسانی که به دنبال آدرنالین و طبیعت در آنتالیا هستند!",
    descriptionAr:
@"استعد ليوم مليء بالمغامرة في حديقة كوبرولو كانيون الوطنية! 
عِش إثارة ركوب الأمواج في نهر كوبرولو واكتشف جمال الطبيعة الخلابة للكانيون. 

يبدأ اليوم بنقلك من الفندق في أنطاليا والمنتجعات القريبة إلى جبال طوروس. 
يقدم المرشدون المحترفون شرحاً وافياً عن إجراءات السلامة ويزودونك بالمعدات اللازمة. 

استمتع بركوب القوارب على المياه الفيروزية المحاطة بالمنحدرات الشاهقة والغابات الصنوبرية. 
تتخلل الرحلة محطات للسباحة في المياه الصافية. 

زد من حماسك مع نشاط الكانيونينغ – تسلق، سباحة وعبور الممرات الضيقة والشلالات. 
مغامرة مناسبة للمبتدئين ومحبي التشويق على حد سواء. 

يوم مليء بالإثارة والجمال الطبيعي لا يُنسى بانتظارك في أنطاليا!",
miniDescriptionEn: "Adrenaline day: whitewater rafting, canyoning, swimming in crystal waters and epic Taurus landscapes.",
miniDescriptionDe: "Adrenalin pur: Rafting, Canyoning, Baden im klaren Wasser und Taurus-Landschaften.",
miniDescriptionRu: "Адреналин: рафтинг, каньонинг, купание в кристальной воде и пейзажи Тавра.",
miniDescriptionPo: "Dzień adrenaliny: rafting, kanioning, kąpiele i widoki gór Taurus.",
miniDescriptionPe: "یک روز پرهیجان: رفتینگ، کنیونینگ، شنا در آب زلال و مناظر کوه‌های توروس.",
miniDescriptionAr: "يوم مليء بالأدرينالين: رافتينغ، كانيونينغ وسباحة في مياه نقية وسط جبال طوروس.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Koprulu Canyon Rafting and Canyoning.jpg")
    },
    activeDay: 1111111,
    durationHours: 9,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Safety briefing & equipment",
        "Rafting on Köprü River",
        "Canyoning segment",
        "Swimming stops",
        "Professional guides",
        "Insurance"
    }
),
new Tour(
    id: 19,
    name: "Kemer Scuba Diving",
    price: 52,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Adventure,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Discover the magic of the Mediterranean Sea with a scuba diving trip from Kemer. 
Marvel at the vibrant underwater life, swim alongside colorful fish, and explore fascinating rock formations beneath the surface. 

Whether you’re a beginner or an experienced diver, professional instructors will provide full guidance and ensure your safety throughout the experience. 
After a short briefing and practice, dive into the clear turquoise waters for an unforgettable adventure. 

A perfect choice for anyone who wants to explore the hidden beauty of the sea and enjoy a day filled with excitement and relaxation.",
    descriptionDe:
@"Entdecken Sie die Magie des Mittelmeers bei einem Tauchgang ab Kemer. 
Bestaunen Sie das farbenfrohe Unterwasserleben, schwimmen Sie mit exotischen Fischen und erkunden Sie faszinierende Felsformationen unter Wasser. 

Egal ob Anfänger oder erfahrener Taucher – professionelle Tauchlehrer geben eine Einweisung und begleiten Sie während des gesamten Abenteuers. 
Nach einer kurzen Übung tauchen Sie ein in das klare, türkisfarbene Wasser. 

Ein perfektes Erlebnis für alle, die die verborgene Schönheit des Meeres entdecken und einen spannenden, aber auch erholsamen Tag genießen möchten.",
    descriptionRu:
@"Откройте для себя магию Средиземного моря во время дайвинг-тура из Кемера. 
Полюбуйтесь яркой подводной жизнью, поплавайте среди разноцветных рыб и исследуйте удивительные скальные образования под водой. 

Подходит как для новичков, так и для опытных дайверов – профессиональные инструкторы проведут инструктаж и будут сопровождать вас на протяжении всего погружения. 
После краткого обучения вы отправитесь в кристально чистые бирюзовые воды для незабываемого приключения. 

Идеальный выбор для тех, кто хочет открыть скрытую красоту моря и провести день, полный впечатлений и отдыха.",
    descriptionPo:
@"Odkryj magię Morza Śródziemnego podczas nurkowania z Kemer. 
Podziwiaj kolorowe życie podwodne, pływaj wśród egzotycznych ryb i odkrywaj fascynujące formacje skalne. 

Zarówno początkujący, jak i doświadczeni nurkowie są mile widziani – profesjonalni instruktorzy zapewnią instruktaż i bezpieczeństwo. 
Po krótkim szkoleniu zanurzysz się w krystalicznie czystych, turkusowych wodach. 

Idealny wybór dla każdego, kto chce poznać ukryte piękno morza i spędzić dzień pełen emocji i relaksu.",
    descriptionPe:
@"زیبایی‌های پنهان دریای مدیترانه را با یک سفر غواصی از کمر کشف کنید. 
از دنیای رنگارنگ زیر آب شگفت‌زده شوید، همراه با ماهی‌های زیبا شنا کنید و سازه‌های سنگی جذاب زیر دریا را ببینید. 

فرقی نمی‌کند مبتدی باشید یا باتجربه – مربیان حرفه‌ای شما را راهنمایی کرده و امنیتتان را تضمین می‌کنند. 
پس از یک آموزش کوتاه، به آب‌های فیروزه‌ای و شفاف شیرجه زده و یک ماجراجویی فراموش‌نشدنی را تجربه کنید. 

انتخابی عالی برای کسانی که می‌خواهند زیبایی پنهان دریا را کشف کنند و روزی پر از هیجان و آرامش داشته باشند.",
    descriptionAr:
@"اكتشف سحر البحر الأبيض المتوسط في رحلة غوص من كيمر. 
اندهش من روعة الحياة البحرية الملونة، واسبح بجانب الأسماك الاستوائية، واستكشف التكوينات الصخرية الرائعة تحت الماء. 

سواء كنت مبتدئاً أو غواصاً متمرساً، سيرافقك مدربون محترفون ويضمنون سلامتك طوال التجربة. 
بعد شرح قصير وتدريب عملي ستنطلق إلى المياه الفيروزية الصافية لمغامرة لا تُنسى. 

خيار مثالي لكل من يرغب في استكشاف الجمال الخفي للبحر وقضاء يوم مليء بالإثارة والاسترخاء.",
   miniDescriptionEn: "Dive into Kemer’s turquoise waters – colorful fish, rock formations and safe guidance for all levels.",
miniDescriptionDe: "Tauchen in Kemer: türkisfarbenes Wasser, bunte Fische, Felsformationen und sichere Begleitung.",
miniDescriptionRu: "Дайвинг в Кемере: бирюзовая вода, яркие рыбы и надёжное сопровождение инструкторов.",
miniDescriptionPo: "Nurkowanie w Kemer – turkusowa woda, kolorowe ryby i pełne wsparcie instruktorów.",
miniDescriptionPe: "غواصی در کمر: آب‌های فیروزه‌ای، ماهی‌های رنگارنگ و آموزش ایمن برای همه.",
miniDescriptionAr: "غوص في كيمر: مياه فيروزية، أسماك ملونة وإشراف آمن للمبتدئين والمحترفين.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Kemer Scuba Diving.jpg")
    },
    activeDay: 1111111,
    durationHours: 7,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Professional instructors",
        "Full diving briefing",
        "Diving equipment",
        "2 dives (conditions permitting)",
        "Boat trip",
        "Insurance"
    }
),
new Tour(
    id: 20,
    name: "Cappadocia (2-Days)",
    price: 82,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Special,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Embark on an unforgettable 2-day guided tour to Cappadocia, one of the most unique regions in the world. 
Journey through extraordinary landscapes filled with unusual rock formations, valleys, and the famous 'fairy chimneys'. 

Step inside ancient rock-cut churches and cave dwellings that tell the stories of early civilizations. 
Learn about the fascinating history and culture of this magical land from your expert guide. 

For the ultimate Cappadocia experience, take the opportunity to join an optional hot air balloon ride. 
Soar over the breathtaking valleys and fairy chimneys at sunrise for a once-in-a-lifetime view. 

A perfect combination of history, culture, and natural wonder – this 2-day trip will leave you with unforgettable memories of Turkey’s most enchanting destination.",
    descriptionDe:
@"Begeben Sie sich auf eine unvergessliche 2-tägige Tour nach Kappadokien – eine der außergewöhnlichsten Regionen der Welt. 
Erkunden Sie die bizarren Felsformationen, Täler und die berühmten 'Feenkamine'. 

Besichtigen Sie uralte in den Fels gehauene Kirchen und Höhlenwohnungen, die Geschichten früherer Zivilisationen erzählen. 
Ihr Guide vermittelt Ihnen spannende Einblicke in die Geschichte und Kultur dieser magischen Region. 

Das Highlight: Nutzen Sie die Gelegenheit für eine optionale Heißluftballonfahrt. 
Schweben Sie bei Sonnenaufgang über die Täler und Feenkamine – ein unvergessliches Erlebnis. 

Eine perfekte Kombination aus Geschichte, Kultur und Naturwundern – diese 2-tägige Reise hinterlässt unvergessliche Erinnerungen.",
    descriptionRu:
@"Отправьтесь в незабываемое 2-дневное путешествие по Каппадокии – одному из самых удивительных регионов мира. 
Полюбуйтесь необычными скальными образованиями, долинами и знаменитыми 'каменными столбами' (фейскими трубами). 

Посетите древние церкви и жилища, вырубленные в скалах, и узнайте об истории и культуре этой волшебной земли от вашего гида. 

Для уникального опыта воспользуйтесь возможностью совершить полёт на воздушном шаре. 
На рассвете вы увидите Каппадокию с высоты птичьего полёта – зрелище, которое запомнится на всю жизнь. 

Идеальное сочетание истории, культуры и природных чудес – это 2-дневное путешествие подарит вам незабываемые впечатления.",
    descriptionPo:
@"Wybierz się na niezapomnianą, 2-dniową wycieczkę z przewodnikiem do Kapadocji – jednej z najbardziej wyjątkowych krain na świecie. 
Podziwiaj niezwykłe formacje skalne, doliny oraz słynne 'kominy wróżek'. 

Wejdź do starożytnych kościołów wykutych w skale i jaskiń, które opowiadają historie dawnych cywilizacji. 
Twój przewodnik przybliży Ci historię i kulturę tej magicznej krainy. 

Największa atrakcja: opcjonalny lot balonem na gorące powietrze. 
Unieś się o wschodzie słońca nad dolinami i kominami wróżek, aby zobaczyć widoki, których nigdy nie zapomnisz. 

To idealne połączenie historii, kultury i cudów natury – 2-dniowa podróż, która pozostawi niezapomniane wspomnienia.",
    descriptionPe:
@"به یک تور ۲ روزه فراموش‌نشدنی به کاپادوکیا بروید – یکی از خاص‌ترین مناطق جهان. 
مناظر خارق‌العاده با صخره‌های عجیب، دره‌ها و «دودکش‌های پری» معروف را از نزدیک ببینید. 

از کلیساها و خانه‌های سنگی باستانی بازدید کنید که داستان تمدن‌های اولیه را روایت می‌کنند. 
با کمک راهنمای حرفه‌ای خود، با تاریخ و فرهنگ شگفت‌انگیز این سرزمین جادویی آشنا شوید. 

برای تجربه‌ای بی‌نظیر، می‌توانید در پرواز اختیاری بالن شرکت کنید. 
در طلوع خورشید بر فراز دره‌ها و دودکش‌های پری پرواز کنید و منظره‌ای را ببینید که یک عمر در خاطرتان خواهد ماند. 

ترکیبی عالی از تاریخ، فرهنگ و شگفتی‌های طبیعی – این سفر ۲ روزه یادگاری‌های ماندگاری از ترکیه برای شما رقم خواهد زد.",
    descriptionAr:
@"انطلق في جولة إرشادية لا تُنسى لمدة يومين إلى كابادوكيا – واحدة من أكثر المناطق تميزاً في العالم. 
استمتع بمشاهدة التشكيلات الصخرية الفريدة والوديان و'مداخن الجنيات' الشهيرة. 

قم بزيارة الكنائس القديمة والمساكن المحفورة في الصخور، وتعرف على تاريخ وثقافة هذه الأرض الساحرة مع مرشدك. 

وللحصول على تجربة استثنائية، يمكنك الانضمام إلى رحلة بالمنطاد عند شروق الشمس. 
حلّق فوق الوديان ومداخن الجنيات واستمتع بمنظر يخطف الأنفاس سيبقى في ذاكرتك مدى الحياة. 

مزيج مثالي من التاريخ والثقافة وروائع الطبيعة – رحلة لمدة يومين ستمنحك ذكريات لا تُنسى من تركيا.",
miniDescriptionEn: "Two days in magical Cappadocia: fairy chimneys, cave churches and optional sunrise balloon ride.",
miniDescriptionDe: "Zwei Tage Kappadokien: Feenkamine, Höhlenkirchen und optional Ballonfahrt bei Sonnenaufgang.",
miniDescriptionRu: "2 дня в Каппадокии: фейские трубы, пещерные церкви и полёт на воздушном шаре.",
miniDescriptionPo: "2 dni w Kapadocji: kominy wróżek, kościoły skalne i opcjonalny lot balonem o świcie.",
miniDescriptionPe: "۲ روز کاپادوکیا: دودکش‌های پری، کلیساهای سنگی و پرواز بالن اختیاری هنگام طلوع.",
miniDescriptionAr: "رحلة يومين في كابادوكيا: مداخن الجن، كنائس الكهوف ورحلة بالون اختيارية عند الشروق.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Cappadocia (2-Days).jpg")
    },
    activeDay: 1111111,
    durationHours: 36,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Return transfers",
        "Professional guide",
        "Overnight accommodation (standard, unless otherwise stated)",
        "Visits to valleys & rock-cut churches",
        "Optional hot air balloon ride (extra)"
    }
),
new Tour(
    id: 21,
    name: "Visit the World's Largest Tunnel Aquarium",
    price: 65,
    kinderPrice: 0,
    infantPrice: 0,
    category: Category.Family,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Come face to face with the most magical marine creatures at Antalya’s Tunnel Aquarium – the world’s largest tunnel aquarium! 
Home to over 250 species and 40 thematic aquariums, it offers an unforgettable journey through the underwater world. 

See real sharks swimming above and around you, explore colorful tropical fish, and admire the sheer-size plane and ship wrecks displayed inside. 
One highlight is the wreck of a 1935 Italian Savoia-Marchetti SM79 'Sparviero' bomber, which was hit and sank off the coast of Meis. 

Enjoy interactive exhibits that explore the Mediterranean’s diverse marine life, and even take part in unique experiences like feeding stingrays, koi fish, or sharks. 
Capture unforgettable moments with rare species, or simply marvel at the wonders of the underwater realm. 

A perfect attraction for families, friends, and curious travelers alike – fun, educational, and awe-inspiring!",
    descriptionDe:
@"Begegnen Sie den faszinierendsten Meeresbewohnern im Tunnel Aquarium in Antalya – dem größten Tunnel-Aquarium der Welt! 
Mehr als 250 Arten und 40 Themenaquarien erwarten Sie auf einer unvergesslichen Reise in die Unterwasserwelt. 

Beobachten Sie Haie, die über und neben Ihnen schwimmen, entdecken Sie tropische Fische und bewundern Sie die imposanten Schiffs- und Flugzeugwracks. 
Ein besonderes Highlight ist das Wrack eines italienischen Bombers Savoia-Marchetti SM79 'Sparviero' von 1935, der vor der Küste von Meis versank. 

Interaktive Ausstellungen zeigen die Vielfalt des Mittelmeeres, und es gibt die Möglichkeit, besondere Erlebnisse hinzuzufügen, wie das Füttern von Rochen, Kois oder Haien. 
Machen Sie Fotos mit seltenen Arten und lassen Sie sich von der Schönheit der Unterwasserwelt verzaubern. 

Eine ideale Attraktion für Familien, Freunde und neugierige Reisende – spannend, lehrreich und beeindruckend!",
    descriptionRu:
@"Встретьтесь лицом к лицу с удивительными морскими существами в тоннельном аквариуме Анталии – самом большом аквариуме-тоннеле в мире! 
Здесь обитают более 250 видов и 40 тематических аквариумов, представляющих увлекательный подводный мир. 

Наблюдайте за акулами, плавающими вокруг вас, любуйтесь тропическими рыбами и исследуйте огромные экспозиции с кораблекрушениями и авиакатастрофами. 
Особая гордость – экспонат итальянского бомбардировщика Savoia-Marchetti SM79 'Sparviero' 1935 года, сбитого и затонувшего у побережья Мейса. 

Интерактивные экспозиции знакомят с богатой флорой и фауной Средиземного моря. 
Можно даже принять участие в кормлении скатов, карпов кои или акул, а также сделать фото с редкими видами. 

Идеальное место для семей, друзей и всех, кто интересуется природой – увлекательно, познавательно и вдохновляюще!",
    descriptionPo:
@"Spotkaj się twarzą w twarz z najwspanialszymi stworzeniami morskimi w Antalya Tunnel Aquarium – największym akwarium tunelowym na świecie! 
Na odwiedzających czeka ponad 250 gatunków i 40 tematycznych akwariów, które zabiorą Cię w niezapomnianą podróż po świecie podwodnym. 

Zobacz rekiny pływające tuż nad Tobą, odkryj tropikalne ryby i podziwiaj ogromne wraki statków i samolotów. 
Jednym z najciekawszych eksponatów jest wrak włoskiego bombowca Savoia-Marchetti SM79 'Sparviero' z 1935 roku, który został zestrzelony i zatonął u wybrzeży Meis. 

Interaktywne wystawy pokazują różnorodność Morza Śródziemnego, a dodatkowe atrakcje obejmują karmienie płaszczek, koi lub rekinów. 
Zrób zdjęcia z rzadkimi gatunkami i daj się zachwycić magii podwodnego świata. 

To idealna atrakcja dla rodzin, przyjaciół i ciekawych podróżników – zabawna, edukacyjna i inspirująca!",
    descriptionPe:
@"در آکواریوم تونلی آنتالیا – بزرگ‌ترین آکواریوم تونلی جهان – از نزدیک با شگفت‌انگیزترین موجودات دریایی روبه‌رو شوید! 
این مجموعه با بیش از ۲۵۰ گونه و ۴۰ آکواریوم موضوعی، شما را به سفری فراموش‌نشدنی در دنیای زیر آب می‌برد. 

کوسه‌های واقعی را تماشا کنید که اطراف شما شنا می‌کنند، از ماهی‌های استوایی رنگارنگ لذت ببرید و کشتی‌ها و هواپیماهای غرق‌شده را ببینید. 
یکی از مهم‌ترین جاذبه‌ها، لاشه یک بمب‌افکن ایتالیایی مدل Savoia-Marchetti SM79 'Sparviero' مربوط به سال ۱۹۳۵ است که در نزدیکی سواحل میس غرق شده است. 

نمایشگاه‌های تعاملی، تنوع دریای مدیترانه را معرفی می‌کنند و حتی می‌توانید در فعالیت‌هایی مانند غذا دادن به سفره‌ماهی‌ها، ماهی‌های کوی یا کوسه‌ها شرکت کنید. 
عکس‌های بی‌نظیر با گونه‌های کمیاب بگیرید و از دنیای شگفت‌انگیز زیر آب شگفت‌زده شوید. 

یک انتخاب عالی برای خانواده‌ها، دوستان و همه علاقه‌مندان به طبیعت – سرگرم‌کننده، آموزشی و الهام‌بخش!",
    descriptionAr:
@"واجه أروع الكائنات البحرية في أكواريوم أنطاليا الأنبوبي – أكبر أكواريوم أنبوبي في العالم! 
يضم أكثر من 250 نوعاً و40 حوضاً موضوعياً، ليأخذك في رحلة لا تُنسى إلى عالم ما تحت الماء. 

شاهد أسماك القرش تسبح من حولك، واستمتع بمشاهدة الأسماك الاستوائية الملونة، وتأمل حطام السفن والطائرات المعروض. 
ومن أبرز المعروضات حطام قاذفة إيطالية من طراز Savoia-Marchetti SM79 'Sparviero' من عام 1935، التي سقطت وغرقت قبالة سواحل ميس. 

تقدم المعارض التفاعلية لمحة عن تنوع الحياة في البحر الأبيض المتوسط، ويمكنك المشاركة في أنشطة إضافية مثل إطعام أسماك الراي أو الكوي أو حتى أسماك القرش. 
التقط صوراً مع أنواع نادرة ودع نفسك تنبهر بروعة العالم البحري. 

وجهة مثالية للعائلات والأصدقاء ومحبي الطبيعة – ممتعة، تعليمية وملهمة!",
   miniDescriptionEn: "Walk through the world’s largest tunnel aquarium – sharks, tropical fish and sunken plane wrecks.",
miniDescriptionDe: "Größtes Tunnel-Aquarium der Welt: Haie, tropische Fische und spektakuläre Wracks.",
miniDescriptionRu: "Крупнейший тоннельный аквариум: акулы, тропические рыбы и затонувшие экспонаты.",
miniDescriptionPo: "Największe akwarium tunelowe na świecie: rekiny, ryby tropikalne i wraki.",
miniDescriptionPe: "بزرگ‌ترین آکواریوم تونلی جهان؛ کوسه‌ها، ماهی‌های استوایی و لاشه هواپیما.",
miniDescriptionAr: "أكبر أكواريوم أنبوبي في العالم: أسماك قرش، أسماك استوائية وحطام طائرات.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Visit the World's Largest Tunnel Aquarium.jpg")
    },
    activeDay: 1111111,
    durationHours: 3,
    services: new List<string> {
        "Entrance to Tunnel Aquarium",
        "Access to thematic aquariums",
        "Wreck exhibits (plane & ship)",
        "Interactive displays"
    }
),
new Tour(
    id: 22,
    name: "Massage and Scrub at a Turkish Bath in Antalya",
    price: 32,
    kinderPrice: 17,
    infantPrice: 0,
    category: Category.Relaxing,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Take some time for yourself and enjoy the timeless oriental tradition of a Turkish Bath in Antalya – an experience not to be missed! 
In just 2 hours, this ritual of relaxation and rejuvenation will leave you refreshed and ready for more sightseeing. 

The history of the hammam goes back centuries, inspired by Greek and Roman bathing practices, and has always been more than just skin cleansing. 
It’s about the unique atmosphere and total experience that combines body, mind, and spirit. 

This Turkish bath package includes a sauna, body peeling, foam massage, and a soothing aromatherapy oil massage. 
The combination will make your body feel revitalized and your spirit renewed. 

Indulge yourself during your holiday with a body scrub and soap massage – one of Turkey’s most unique and pleasurable traditions.",
    descriptionDe:
@"Gönnen Sie sich etwas Zeit für sich selbst und erleben Sie die jahrhundertealte orientalische Tradition eines türkischen Bades in Antalya – ein Erlebnis, das Sie nicht verpassen sollten! 
In nur 2 Stunden fühlen Sie sich nach diesem entspannenden Ritual erfrischt und bereit für weitere Erkundungen. 

Die Geschichte des Hamams reicht Jahrhunderte zurück und ist von den Badepraktiken der Griechen und Römer inspiriert. 
Dabei geht es nicht nur um die Reinigung der Haut, sondern um ein ganzheitliches Erlebnis in einer besonderen Atmosphäre. 

Dieses Paket umfasst Sauna, Peeling, Schaummassage und eine beruhigende Aromatherapie-Ölmassage. 
Die Kombination lässt Körper und Geist revitalisiert zurück. 

Verwöhnen Sie sich im Urlaub mit einem Peeling und einer Seifenmassage – eine der einzigartigsten und angenehmsten Traditionen der Türkei.",
    descriptionRu:
@"Подарите себе немного времени и откройте для себя восточную традицию – турецкую баню (хаммам) в Анталии. 
Всего за 2 часа это расслабляющее и восстанавливающее ритуальное действие подарит вам свежесть и энергию для новых открытий. 

История хаммама уходит корнями в античные времена и основывается на традициях греческих и римских бань. 
Это не просто очищение кожи, а целый опыт в особой атмосфере. 

В программу входят сауна, пилинг, пенный массаж и массаж с ароматическими маслами. 
Эта комбинация освежает тело и обновляет дух. 

Побалуйте себя во время отпуска пилингом и мыльным массажем – одной из самых уникальных и приятных традиций Турции.",
    descriptionPo:
@"Zrób coś tylko dla siebie i skorzystaj z ponadczasowej tradycji łaźni tureckiej w Antalyi – doświadczenia, którego nie można przegapić! 
W ciągu zaledwie 2 godzin ten rytuał relaksu i odnowy sprawi, że poczujesz się świeżo i pełen energii do dalszego zwiedzania. 

Historia hammamu sięga wieków wstecz i inspirowana jest praktykami kąpielowymi Greków i Rzymian. 
To nie tylko oczyszczanie skóry, ale całościowe przeżycie w wyjątkowej atmosferze. 

Pakiet obejmuje saunę, peeling, masaż pianą oraz kojący masaż olejkami aromatycznymi. 
Połączenie to odnowi ciało i ducha. 

Rozpieść się podczas wakacji peelingiem i masażem mydłem – jedną z najbardziej wyjątkowych i przyjemnych tradycji Turcji.",
    descriptionPe:
@"برای خودتان زمانی بگذارید و یکی از سنت‌های قدیمی شرقی را تجربه کنید – حمام ترکی در آنتالیا! 
فقط در دو ساعت، این مراسم آرامش‌بخش و انرژی‌بخش شما را تازه کرده و برای ادامه گردش آماده می‌کند. 

تاریخ حمام سنتی (حمام یا حمام بخار) به قرن‌ها پیش بازمی‌گردد و الهام‌گرفته از سنت‌های یونانی و رومی است. 
این فقط شست‌وشوی پوست نیست، بلکه تجربه‌ای کامل در فضایی خاص است. 

این پکیج شامل سونا، لایه‌برداری بدن، ماساژ کف و ماساژ آرامش‌بخش با روغن‌های آروماتراپی است. 
این ترکیب بدن شما را شاداب و روحتان را تازه می‌کند. 

در سفر خود با یک لایه‌برداری و ماساژ صابونی از خودتان پذیرایی کنید – یکی از سنت‌های خاص و لذت‌بخش ترکیه.",
    descriptionAr:
@"امنح نفسك بعض الوقت واستمتع بالتقاليد الشرقية العريقة في الحمام التركي بأنطاليا – تجربة لا تُفوَّت! 
خلال ساعتين فقط، سيمنحك هذا الطقس المريح شعوراً بالانتعاش والاستعداد لمواصلة جولتك. 

تعود أصول الحمام التركي إلى قرون مضت، مستوحاة من ممارسات الاستحمام اليونانية والرومانية القديمة. 
لم يكن الأمر مجرد تنظيف للبشرة، بل تجربة متكاملة في أجواء خاصة. 

تشمل هذه الباقة ساونا، تقشير للجسم، تدليك بالرغوة وتدليك بزيوت عطرية. 
هذا الروتين سيجدد نشاط جسدك ويبعث الحيوية في روحك. 

دلل نفسك في عطلتك بتقشير للجسم وتدليك بالصابون – واحدة من أمتع وأعرق التقاليد التركية.",
miniDescriptionEn: "2-hour hammam ritual with sauna, scrub, foam and oil massage – relax like never before.",
miniDescriptionDe: "2 Stunden Hamam: Sauna, Peeling, Schaum- und Ölmassage – Entspannung pur.",
miniDescriptionRu: "2 часа хаммама: сауна, пилинг, пенный и масляный массаж – полное расслабление.",
miniDescriptionPo: "2-godzinny hammam: sauna, peeling, masaż pianą i olejkami – pełen relaks.",
miniDescriptionPe: "۲ ساعت حمام ترکی؛ سونا، لایه‌برداری، ماساژ کف و ماساژ روغنی – آرامش ناب.",
miniDescriptionAr: "ساعتان في الحمام التركي: ساونا، تقشير، تدليك بالرغوة والزيوت – استرخاء تام.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Massage and Scrub at a Turkish Bath in Antalya.jpg")
    },
    activeDay: 1111111,
    durationHours: 2,
    services: new List<string> {
        "Sauna",
        "Body peeling",
        "Foam massage",
        "Aromatherapy oil massage",
        "Turkish tea"
    }
),
new Tour(
    id: 23,
    name: "Kekova Sunken City, Myra & St. Nicholas Church",
    price: 120,
    kinderPrice: 60,
    infantPrice: 0,
    category: Category.History,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Embark on a full-day journey to Kekova, Myra, and the Church of St. Nicholas for a perfect mix of history, culture, and natural beauty. 

Start with a scenic boat trip to Kekova, one of the most picturesque yachting destinations in the region. 
Here, marvel at the extraordinary underwater ruins of the sunken city of Simena and visit the ancient Lycian necropolis at Teimiussa. 
There’s also time to swim and snorkel in the turquoise waters, so don’t forget your bathing suit! 

Continue by land to Myra, one of the six major cities of ancient Lycia. 
Enjoy a delicious lunch before exploring its impressive archaeological remains, including the Acropolis, Roman Theater, and Baths. 
Admire the Lycian rock tombs carved into towering cliffs – a breathtaking sight. 

The final stop is the Church of St. Nicholas in Myra. 
St. Nicholas, the 4th-century Bishop of Myra, later became world-renowned as Father Christmas (Santa Claus). 
The restored church offers a fascinating glimpse into this important historical figure. 

After your visit, return comfortably to Antalya with unforgettable memories of history and legend combined.",
    descriptionDe:
@"Begeben Sie sich auf eine ganztägige Reise nach Kekova, Myra und zur Kirche des Heiligen Nikolaus – eine perfekte Mischung aus Geschichte, Kultur und Natur. 

Beginnen Sie mit einer Bootsfahrt nach Kekova, einem der schönsten Yachting-Ziele der Region. 
Hier bestaunen Sie die außergewöhnlichen Unterwasser-Ruinen der versunkenen Stadt Simena und besichtigen die lykische Nekropole von Teimiussa. 
Es bleibt auch Zeit zum Schwimmen und Schnorcheln im türkisfarbenen Wasser – also Badesachen nicht vergessen! 

Weiter geht es nach Myra, eine der sechs bedeutendsten Städte des antiken Lykien. 
Nach einem Mittagessen erkunden Sie die archäologischen Überreste wie die Akropolis, das Römische Theater und die Bäder. 
Besonders beeindruckend sind die in die Felsen gehauenen lykischen Felsengräber. 

Zum Abschluss besuchen Sie die Kirche des Heiligen Nikolaus in Myra. 
Nikolaus war im 4. Jahrhundert Bischof von Myra und wurde später weltweit als Weihnachtsmann bekannt. 
Die restaurierte Kirche bietet faszinierende Einblicke in sein Leben. 

Danach Rückkehr nach Antalya mit unvergesslichen Eindrücken von Geschichte und Legenden.",
    descriptionRu:
@"Отправьтесь в однодневное путешествие в Кекова, Миры и церковь Святого Николая – идеальное сочетание истории, культуры и природы. 

Начните с живописной прогулки на лодке в Кекова – один из самых красивых яхтенных курортов региона. 
Полюбуйтесь необычными подводными руинами затонувшего города Симена и посетите древний ликийский некрополь в Теимиуссе. 
Будет время искупаться и заняться сноркелингом, так что не забудьте купальник! 

Затем отправляйтесь в Миры – один из шести главных городов древней Ликии. 
После обеда исследуйте археологические памятники, включая акрополь, римский театр и бани. 
Особенно впечатляют ликийские гробницы, вырезанные прямо в скалах. 

Последняя остановка – церковь Святого Николая в Мирах. 
Святой Николай, епископ Мир Ликийских в IV веке, стал известен во всем мире как Санта-Клаус. 
Восстановленная церковь подарит вам уникальный взгляд в прошлое. 

В завершение тура – возвращение в Анталию с яркими воспоминаниями о легендах и истории.",
    descriptionPo:
@"Wybierz się na całodniową wycieczkę do Kekovy, Myry i Kościoła św. Mikołaja – połączenie historii, kultury i pięknej przyrody. 

Rozpocznij od rejsu łodzią do Kekovy, jednego z najbardziej malowniczych miejsc regionu. 
Podziwiaj niezwykłe podwodne ruiny zatopionego miasta Simena i odwiedź starożytną nekropolię licyjską w Teimiussa. 
Będzie też czas na kąpiel i snorkeling w turkusowych wodach – nie zapomnij stroju kąpielowego! 

Następnie udaj się do Myry, jednego z sześciu głównych miast starożytnej Licji. 
Po smacznym obiedzie zwiedzisz Akropol, teatr rzymski i łaźnie. 
Zachwycą Cię również licyjskie grobowce wykute w skałach. 

Ostatnim punktem wycieczki jest Kościół św. Mikołaja w Myrze. 
Św. Mikołaj, biskup Myry w IV wieku, znany jest dziś na całym świecie jako Santa Claus. 
Odrestaurowany kościół pozwala spojrzeć na jego historię z bliska. 

Po zwiedzaniu powrót do Antalyi z niezapomnianymi wspomnieniami.",
    descriptionPe:
@"در یک سفر یک‌روزه از آنتالیا، ترکیبی بی‌نظیر از تاریخ، فرهنگ و طبیعت را تجربه کنید. 

ابتدا با قایق به سمت ککووا می‌روید، یکی از زیباترین مقاصد دریانوردی منطقه. 
در اینجا از ویرانه‌های زیرآبی شهر غرق‌شده سیمنا دیدن می‌کنید و از نکرپولیس باستانی لیکیایی در تیمیوسا بازدید خواهید داشت. 
همچنین فرصت شنا و غواصی با اسنورکل خواهید داشت. 

سپس به سمت شهر باستانی میرا می‌روید، یکی از شش شهر مهم لیکیا. 
بعد از ناهار، از آثار باستانی از جمله آکروپولیس، تئاتر رومی و حمام‌ها بازدید می‌کنید. 
گورهای سنگی لیکیایی که در دل صخره‌ها تراشیده شده‌اند، از دیدنی‌ترین بخش‌ها هستند. 

ایستگاه بعدی کلیسای سنت نیکلاس است. 
نیکلاس، اسقف میرا در قرن چهارم، بعدها به عنوان بابانوئل در سراسر جهان شناخته شد. 
این کلیسای بازسازی‌شده، تجربه‌ای ارزشمند از تاریخ و سنت‌ها به شما می‌دهد. 

در پایان روز، با خاطراتی فراموش‌نشدنی از تاریخ و افسانه‌ها به آنتالیا بازمی‌گردید.",
    descriptionAr:
@"انطلق في جولة ليوم كامل إلى كيكوفا ومدينة ميرا وكنيسة القديس نيكولاس – مزيج مثالي من التاريخ والثقافة والطبيعة. 

تبدأ الرحلة بركوب القارب إلى كيكوفا، أحد أجمل الوجهات البحرية في المنطقة. 
شاهد الآثار الغارقة للمدينة المفقودة سيمينا وزُر مقبرة لِسية القديمة في تيميوسا. 
هناك وقت للسباحة والغطس، لذا لا تنسَ ملابس السباحة. 

بعد ذلك، توجّه إلى ميرا، إحدى المدن الرئيسية الست في ليكيا القديمة. 
استمتع بالغداء، ثم اكتشف بقايا أثرية مذهلة مثل الأكروبوليس والمسرح الروماني والحمامات. 
ستُدهشك المقابر الليسية المنحوتة في المنحدرات الصخرية. 

المحطة الأخيرة هي كنيسة القديس نيكولاس في ميرا. 
كان نيكولاس أسقفاً في القرن الرابع، ثم اشتهر لاحقاً في العالم كله باسم سانتا كلوز. 
الكنيسة المرممة تمنحك لمحة فريدة عن هذا الإرث التاريخي. 

بعد الزيارة، العودة إلى أنطاليا محمّلاً بذكريات لا تُنسى عن التاريخ والأساطير.",
miniDescriptionEn: "Full-day trip: Kekova’s sunken city, Lycian tombs in Myra and St. Nicholas Church in Demre.",
miniDescriptionDe: "Ganztagesausflug: versunkene Stadt Kekova, lykische Gräber in Myra und Nikolauskirche.",
miniDescriptionRu: "День экскурсий: затонувший город Кекова, гробницы в Мирах и церковь Николая.",
miniDescriptionPo: "Całodniowa wycieczka: zatopione miasto Kekova, grobowce w Myrze i Kościół św. Mikołaja.",
miniDescriptionPe: "یک روز کامل: شهر غرق‌شده ککووا، مقبره‌های لیکیایی در میرا و کلیسای سنت نیکلاس.",
miniDescriptionAr: "رحلة يوم كامل: مدينة كيكوفا الغارقة، مقابر ليكية في ميرا وكنيسة القديس نيكولاس.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Kekova Sunken City, Myra & St. Nicholas Church.jpg")
    },
    activeDay: 1111111,
    durationHours: 12,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Boat trip to Kekova",
        "Swimming/snorkeling stops",
        "Lunch",
        "Guided visit to Myra ruins",
        "Visit to St. Nicholas Church",
        "Insurance"
    }
),
new Tour(
    id: 24,
    name: "Kemer Full-Day Pirate Boat Trip with Lunch",
    price: 49,
    kinderPrice: 25,
    infantPrice: 14,
    category: Category.Family,
    locLat: 0f,
    locLon: 0f,
    descriptionEn:
@"Set sail on a full-day pirate boat adventure from Kemer Marina! 
After hotel pick-up and transfer to the harbor, board your pirate-style boat for a day filled with history, relaxation, and fun on the water. 

Your first destination is the ancient city of Phaselis, once a major trading hub with 3 harbors. 
Here you can explore fascinating ruins such as aqueducts, the agora, Roman baths, and an ancient theater. 
The bay’s pebble beaches and calm, shallow waters make it a perfect place to swim. 

After enjoying lunch at the bay, the journey continues to Paradise Island or Mehmet Ali Bükü, where you can take a refreshing swim. 
The final stop is at Alaca Water, a beautiful inlet ideal for swimming and snorkeling before sailing back to Kemer. 

A fantastic blend of history, nature, and leisure – perfect for families and travelers of all ages!",
    descriptionDe:
@"Gehen Sie an Bord eines Piratenschiffes und erleben Sie einen unvergesslichen Ganztagesausflug ab Kemer! 
Nach der Abholung vom Hotel fahren Sie zum Hafen, wo Ihr Abenteuer auf dem Wasser beginnt. 

Erstes Ziel ist die antike Stadt Phaselis, einst ein bedeutendes Handelszentrum mit drei Häfen. 
Hier entdecken Sie faszinierende Ruinen wie Aquädukte, das Agora, römische Bäder und ein antikes Theater. 
Die geschützten Strände und das ruhige, flache Wasser laden zum Schwimmen ein. 

Nach einem Mittagessen in der Bucht geht es weiter zur Paradiesinsel oder zur idyllischen Mehmet Ali Bükü-Bucht, wo eine Badepause auf Sie wartet. 
Der letzte Halt ist Alaca Water, eine traumhafte Bucht, ideal zum Schwimmen und Schnorcheln, bevor es zurück nach Kemer geht. 

Eine perfekte Kombination aus Geschichte, Natur und Entspannung – für die ganze Familie geeignet!",
    descriptionRu:
@"Отправьтесь в однодневное приключение на пиратском корабле из Кемера! 
После трансфера из отеля вы подниметесь на борт и начнёте незабываемое путешествие по Средиземному морю. 

Первая остановка – древний город Фаселис, когда-то важный торговый центр с тремя гаванями. 
Здесь вы увидите руины акведуков, агоры, римских бань и античного театра. 
Пляжи с галькой и спокойные мелководные воды отлично подходят для купания. 

После обеда в бухте корабль отправится к Острову Рай или бухте Мехмет Али Бюкю, где вас ждёт купание. 
Заключительная остановка – бухта Алака, прекрасное место для плавания и сноркелинга. 

Идеальное сочетание истории, природы и отдыха – подходит для всей семьи и туристов любого возраста!",
    descriptionPo:
@"Wejdź na pokład statku pirackiego i spędź niezapomniany dzień podczas pełnej atrakcji wycieczki z Kemer! 
Po odbiorze z hotelu zostaniesz przewieziony do portu, gdzie rozpoczniesz swoją morską przygodę. 

Pierwszym przystankiem jest starożytne miasto Phaselis, niegdyś ważny ośrodek handlowy z trzema portami. 
Na miejscu zobaczysz ruiny akweduktów, agory, rzymskich łaźni i antycznego teatru. 
Plaże z drobnymi kamieniami i spokojne, płytkie wody są idealne do pływania. 

Po obiedzie w zatoce rejs będzie kontynuowany na Paradise Island lub do urokliwej zatoki Mehmet Ali Bükü, gdzie czeka Cię przerwa na kąpiel. 
Ostatnim przystankiem będzie Alaca Water – piękna zatoczka, doskonała do pływania i nurkowania z rurką, zanim wrócisz do Kemer. 

Świetne połączenie historii, natury i relaksu – idealna wycieczka dla całej rodziny!",
    descriptionPe:
@"یک سفر یک‌روزه با قایق دزدان دریایی از کمر! 
پس از ترانسفر از هتل به بندر، سوار قایق شده و روزی پر از تاریخ، طبیعت و آرامش را تجربه کنید. 

اولین مقصد شهر باستانی فاسلیس است؛ شهری که روزگاری یک مرکز تجاری مهم با سه بندر بود. 
در اینجا می‌توانید از خرابه‌های آکودوکت‌ها، آگورا، حمام‌های رومی و تئاتر باستانی دیدن کنید. 
سواحل شنی-سنگی و آب‌های آرام و کم‌عمق این خلیج برای شنا عالی هستند. 

پس از صرف ناهار در خلیج، سفر به سمت جزیره بهشت یا خلیج زیبای محمدعلی بیکو ادامه می‌یابد، جایی که توقفی برای شنا خواهید داشت. 
آخرین ایستگاه آب‌های آلاکا است؛ یک ورودی زیبا و عالی برای شنا و غواصی قبل از بازگشت به کمر. 

ترکیبی فوق‌العاده از تاریخ، طبیعت و تفریح – مناسب برای خانواده‌ها و مسافران در هر سنی!",
    descriptionAr:
@"انطلق في رحلة بحرية ليوم كامل على متن سفينة قراصنة من كيمر! 
بعد اصطحابك من الفندق، سيتم نقلك إلى المرسى حيث تبدأ مغامرتك. 

أول محطة هي مدينة فاسيليس القديمة، التي كانت مركزاً تجارياً هاماً وتضم ثلاثة موانئ. 
هنا ستشاهد بقايا الأقنية المائية، الأغورا، الحمامات الرومانية والمسرح القديم. 
تتميز شواطئها بالحصى ومياهها الضحلة والهادئة المناسبة للسباحة. 

بعد تناول الغداء في الخليج، يواصل القارب الإبحار إلى جزيرة الجنة أو خليج محمد علي بيوكو لقضاء وقت ممتع في السباحة. 
أما المحطة الأخيرة فهي مياه ألاكا، خليج جميل مثالي للسباحة والغطس قبل العودة إلى كيمر. 

مزيج رائع من التاريخ والطبيعة والاسترخاء – مناسب للعائلات والمسافرين من جميع الأعمار!",
miniDescriptionEn: "Pirate-style cruise from Kemer with Phaselis ruins, swim stops, lunch and family fun.",
miniDescriptionDe: "Piratenschiff ab Kemer: Ruinen von Phaselis, Badepausen, Mittagessen und Familienspaß.",
miniDescriptionRu: "Пиратский круиз из Кемера: руины Фаселиса, купания, обед и веселье для всей семьи.",
miniDescriptionPo: "Rejs piracki z Kemer: ruiny Phaselis, postoje na kąpiel, lunch i zabawa dla rodzin.",
miniDescriptionPe: "کروز دزدان دریایی از کمر؛ خرابه‌های فاسلیس، شنا، ناهار و سرگرمی خانوادگی.",
miniDescriptionAr: "رحلة قراصنة من كيمر: أطلال فاسيليس، محطات سباحة، غداء ومتعة للعائلة.",
    fotos: new List<Foto> {
        new Foto("/tourimage/Kemer Full-Day Pirate Boat Trip with Lunch.jpg")
    },
    activeDay: 1111111,
    durationHours: 8,
    services: new List<string> {
        "Hotel pickup & drop-off",
        "Pirate boat cruise",
        "Lunch on board",
        "Swimming stops",
        "Stop at Phaselis (ancient city)",
        "Paradise Island or Mehmet Ali Bükü stop",
        "Alaca Water swimming/snorkeling"
    }
)









            );
            base.OnModelCreating(modelBuilder);

        }
    }

}
