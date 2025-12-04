using System;
using System.Collections.Generic;

namespace Models.Helper
{
    /// <summary>
    /// Centralized static string translations for UI text that is not stored in the database.
    /// </summary>
    public static class StaticTextCatalog
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _translations = new(StringComparer.OrdinalIgnoreCase)
        {
            ["nav.home"] = Create("Home", de: "Startseite", tr: "Ana Sayfa", fa: "خانه", ru: "Главная", pl: "Strona główna", ar: "الرئيسية"),
            ["nav.about"] = Create("About us", de: "Über uns", tr: "Hakkımızda", fa: "درباره ما", ru: "О нас", pl: "O nas", ar: "من نحن"),
            ["nav.accommodation"] = Create("Accommodation", de: "Unterkünfte", tr: "Konaklama", fa: "اقامتگاه", ru: "Проживание", pl: "Zakwaterowanie", ar: "الإقامة"),
            ["nav.gallery"] = Create("Gallery", de: "Galerie", tr: "Galeri", fa: "گالری", ru: "Галерея", pl: "Galeria", ar: "المعرض"),
            ["nav.blog"] = Create("Blog", de: "Blog", tr: "Blog", fa: "وبلاگ", ru: "Блог", pl: "Blog", ar: "المدونة"),
            ["nav.blogDetails"] = Create("Blog Details", de: "Blog-Details", tr: "Blog Detayları", fa: "جزئیات وبلاگ", ru: "Подробности блога", pl: "Szczegóły bloga", ar: "تفاصيل المدونة"),
            ["nav.elements"] = Create("Elements", de: "Elemente", tr: "Ögeler", fa: "المان‌ها", ru: "Элементы", pl: "Elementy", ar: "العناصر"),
            ["nav.contact"] = Create("Contact", de: "Kontakt", tr: "İletişim", fa: "تماس", ru: "Контакты", pl: "Kontakt", ar: "تواصل"),
            ["nav.managerLogin"] = Create("Manager Login", de: "Manager-Login", tr: "Yönetici Girişi", fa: "ورود مدیر", ru: "Вход менеджера", pl: "Logowanie menedżera", ar: "دخول المدير"),
            ["footer.newsletter.title"] = Create("Newsletter", de: "Newsletter", tr: "Bülten", fa: "خبرنامه", ru: "Рассылка", pl: "Newsletter", ar: "النشرة البريدية"),
            ["footer.newsletter.caption"] = Create(
                "For travel inspiration and exclusive deals, drop your email below.",
                de: "Reiseinspiration und exklusive Angebote direkt in dein Postfach.",
                tr: "Seyahat ilhamı ve özel fırsatlar için e-postanızı bırakın.",
                fa: "برای ایده‌های سفر و پیشنهادهای ویژه، ایمیل خود را وارد کنید.",
                ru: "Оставьте e-mail и получайте вдохновение и специальные предложения.",
                pl: "Zostaw e-mail, by otrzymywać inspiracje i wyjątkowe oferty.",
                ar: "للحصول على الإلهام والعروض الحصرية، اكتب بريدك الإلكتروني."),
            ["footer.newsletter.placeholder"] = Create("Email Address", de: "E-Mail-Adresse", tr: "E-posta adresi", fa: "آدرس ایمیل", ru: "Адрес эл. почты", pl: "Adres e-mail", ar: "البريد الإلكتروني"),
            ["footer.newsletter.submit"] = Create("Subscribe", de: "Abonnieren", tr: "Abone Ol", fa: "اشتراک", ru: "Подписаться", pl: "Subskrybuj", ar: "اشترك"),
            ["footer.follow"] = Create("Follow us", de: "Folge uns", tr: "Bizi takip edin", fa: "ما را دنبال کنید", ru: "Подписывайтесь на нас", pl: "Obserwuj nas", ar: "تابعنا"),
            ["home.facilities.title"] = Create("Anbalya Facilities", de: "Königliche Einrichtungen", tr: "Kraliyet İmkânları", fa: "امکانات ویژه", ru: "Королевские удобства", pl: "Królewskie udogodnienia", ar: "مرافق ملكية"),
            ["home.facilities.caption"] = Create(
                "Tailored services that keep every stay seamless, comfortable, and memorable.",
                de: "Individuelle Services für einen nahtlosen, komfortablen und unvergesslichen Aufenthalt.",
                tr: "Her konaklamayı sorunsuz, konforlu ve unutulmaz kılan özel hizmetler.",
                fa: "خدمات ویژه برای اقامتی بی‌دغدغه، راحت و خاطره‌انگیز.",
                ru: "Индивидуальные сервисы для безупречного, комфортного и незабываемого отдыха.",
                pl: "Spersonalizowane usługi dla pobytu bez zmartwień i pełnego komfortu.",
                ar: "خدمات مخصصة لإقامة سلسة ومريحة لا تُنسى."),
            ["home.facilities.item.restaurant"] = Create("Restaurant", de: "Restaurant", tr: "Restoran", fa: "رستوران", ru: "Ресторан", pl: "Restauracja", ar: "مطعم"),
            ["home.facilities.item.restaurant.text"] = Create(
                "Fresh Mediterranean flavors, served morning to night with sea-view tables.",
                de: "Frische Mittelmeeraromen – vom Frühstück bis zum Dinner mit Meerblick.",
                tr: "Akdeniz esintili taze lezzetler, gün boyu deniz manzaralı masalarda.",
                fa: "طعم‌های تازه مدیترانه‌ای با چشم‌انداز دریا در تمام طول روز.",
                ru: "Свежие средиземноморские вкусы и панорама моря с утра до вечера.",
                pl: "Świeże śródziemnomorskie smaki i widok na morze od rana do nocy.",
                ar: "نكهات متوسطية طازجة تُقدّم طوال اليوم مع إطلالة على البحر."),
            ["home.facilities.item.sports"] = Create("Sports Club", de: "Sportclub", tr: "Spor Kulübü", fa: "باشگاه ورزشی", ru: "Спортклуб", pl: "Klub sportowy", ar: "نادي رياضي"),
            ["home.facilities.item.sports.text"] = Create(
                "Guided workouts, bike tours, and daily fitness sessions keep energy high.",
                de: "Geführte Workouts, Radtouren und tägliche Fitness halten dich aktiv.",
                tr: "Rehberli antrenmanlar, bisiklet turları ve günlük egzersizler enerjinizi yükseltir.",
                fa: "تمرین‌های هدایت‌شده، تورهای دوچرخه و جلسات روزانه، انرژی شما را بالا می‌برد.",
                ru: "Тренировки с инструктором, велотуры и ежедневный фитнес для бодрости.",
                pl: "Prowadzone treningi, wycieczki rowerowe i codzienny fitness dodają energii.",
                ar: "تمارين موجهة، جولات بالدراجات وحصص لياقة يومية تبقيك مفعمًا بالطاقة."),
            ["home.facilities.item.pool"] = Create("Swimming Pool", de: "Pool", tr: "Yüzme Havuzu", fa: "استخر", ru: "Бассейн", pl: "Basen", ar: "مسبح"),
            ["home.facilities.item.pool.text"] = Create(
                "Heated pools and quiet sun decks designed for stress-free afternoons.",
                de: "Beheizte Pools und ruhige Sonnendecks für entspannte Nachmittage.",
                tr: "Isıtmalı havuzlar ve stresten uzak öğleden sonralar için sessiz güneşlenme alanları.",
                fa: "استخرهای گرم و تراس‌های آفتابی آرام برای بعدازظهرهای بی‌استرس.",
                ru: "Подогреваемые бассейны и спокойные шезлонги для беззаботных дней.",
                pl: "Podgrzewane baseny i zaciszne tarasy na bezstresowe popołudnia.",
                ar: "مسابح مدفأة وتراسات هادئة لأمسيات بلا توتر."),
            ["home.facilities.item.car"] = Create("Rent a Car", de: "Mietwagen", tr: "Araç Kiralama", fa: "اجاره خودرو", ru: "Прокат авто", pl: "Wypożyczalnia aut", ar: "تأجير سيارات"),
            ["home.facilities.item.car.text"] = Create(
                "Pick your car on-site and explore Antalya and beyond at your own pace.",
                de: "Direkt vor Ort ein Auto wählen und Antalya in deinem Tempo erkunden.",
                tr: "Tesiste aracınızı seçin, Antalya'yı ve çevresini kendi temponuzda keşfedin.",
                fa: "ماشین دلخواهتان را در محل تحویل بگیرید و با سرعت خودتان آنتالیا را بگردید.",
                ru: "Выбирайте авто на месте и исследуйте Анталию в своём ритме.",
                pl: "Wybierz auto na miejscu i zwiedzaj Antalyę w swoim tempie.",
                ar: "اختر سيارتك من الموقع واكتشف أنطاليا ومحيطها على طريقتك."),
            ["home.facilities.item.gym"] = Create("Gym & Wellness", de: "Fitness & Wellness", tr: "Spor Salonu ve Spa", fa: "باشگاه و اسپا", ru: "Фитнес и спа", pl: "Siłownia i SPA", ar: "نادي ولياقة"),
            ["home.facilities.item.gym.text"] = Create(
                "Modern equipment, sauna, and steam rooms for the perfect reset.",
                de: "Moderne Geräte, Sauna und Dampfbad für deine Auszeit.",
                tr: "Modern ekipman, sauna ve buhar odalarıyla tam bir yenilenme.",
                fa: "تجهیزات مدرن، سونا و اتاق بخار برای یک استراحت کامل.",
                ru: "Современные тренажёры, сауна и паровая — всё для перезагрузки.",
                pl: "Nowoczesny sprzęt, sauna i łaźnie parowe dla pełnego resetu.",
                ar: "أجهزة حديثة، ساونا وغرف بخار لتجديد كامل."),
            ["home.facilities.item.bar"] = Create("Lobby Bar", de: "Lobby-Bar", tr: "Lobi Barı", fa: "بار لابی", ru: "Лобби-бар", pl: "Bar w lobby", ar: "لوبي بار"),
            ["home.facilities.item.bar.text"] = Create(
                "Craft cocktails and barista coffee served late into the evening.",
                de: "Signature-Cocktails und Barista-Kaffee bis spät am Abend.",
                tr: "Özel kokteyller ve barista kahveleri gece boyunca servis edilir.",
                fa: "کوکتل‌های ویژه و قهوه‌های باریستا تا دیر وقت سرو می‌شوند.",
                ru: "Авторские коктейли и кофе от бариста до позднего вечера.",
                pl: "Autorskie koktajle i kawa baristy serwowane do późna.",
                ar: "كوكتيلات خاصة وقهوة باريستا حتى وقت متأخر."),
            ["home.about.title"] = Create("About Us", de: "Über uns", tr: "Hakkımızda", fa: "درباره ما", ru: "О нас", pl: "O nas", ar: "من نحن"),
            ["home.about.subtitle"] = Create("Our History · Mission & Vision", de: "Unsere Geschichte · Mission & Vision", tr: "Hikayemiz · Misyon & Vizyon", fa: "تاریخچه · ماموریت و چشم‌انداز", ru: "Наша история · Миссия и видение", pl: "Nasza historia · Misja i wizja", ar: "تاريخنا · رسالتنا ورؤيتنا"),
            ["home.about.body"] = Create(
                "From bespoke Antalya experiences to curated tours across Turkey, we craft journeys that blend comfort with discovery.",
                de: "Von maßgeschneiderten Antalya-Erlebnissen bis zu kuratierten Türkei-Rundreisen – wir verbinden Komfort mit Entdeckung.",
                tr: "Antalya'daki özel deneyimlerden Türkiye genelindeki seçkin turlara kadar, konforu keşif duygusuyla harmanlayan yolculuklar tasarlıyoruz.",
                fa: "از تجربه‌های اختصاصی آنتالیا تا تورهای خاص سراسر ترکیه، سفری می‌سازیم که راحتی و کشف را در کنار هم قرار می‌دهد.",
                ru: "От индивидуальных впечатлений в Анталии до авторских туров по всей Турции — мы соединяем комфорт и открытие нового.",
                pl: "Od szytych na miarę przeżyć w Antalyi po starannie dobrane wycieczki po Turcji – łączymy komfort z odkrywaniem.",
                ar: "من تجارب أنطاليا المصممة حسب الطلب إلى جولات منسقة في عموم تركيا، نصنع رحلات تجمع الراحة بالاكتشاف."),
            ["home.about.button"] = Create("Request Custom Price", de: "Individuelles Angebot", tr: "Özel Fiyat Talebi", fa: "درخواست قیمت اختصاصی", ru: "Запросить индивидуальную цену", pl: "Poproś o wycenę", ar: "اطلب عرض سعر خاص"),
            ["home.testimonials.title"] = Create("Guests say about us", de: "Das sagen unsere Gäste", tr: "Misafirlerimiz ne diyor?", fa: "نظر مهمانان", ru: "Отзывы гостей", pl: "Opinie gości", ar: "آراء ضيوفنا"),
            ["home.testimonials.caption"] = Create(
                "Honest feedback from travellers who turned their Antalya plans into memories.",
                de: "Echte Stimmen von Reisenden, die Antalya-Erlebnisse in Erinnerungen verwandelt haben.",
                tr: "Antalya planlarını güzel anılara dönüştüren gezginlerden samimi yorumlar.",
                fa: "دیدگاه‌های واقعی مسافرانی که سفرشان به آنتالیا را به خاطره تبدیل کرده‌اند.",
                ru: "Искренние отзывы путешественников, превративших планы в воспоминания.",
                pl: "Szczere opinie podróżników, którzy zamienili plany w wspomnienia.",
                ar: "آراء صادقة من مسافرين حوّلوا خطط أنطاليا إلى ذكريات."),
            ["contact.title"] = Create("Contact us", de: "Kontaktiere uns", tr: "Bize Ulaşın", fa: "تماس با ما", ru: "Свяжитесь с нами", pl: "Skontaktuj się z nami", ar: "اتصل بنا"),
            ["contact.subtitle"] = Create(
                "We reply within hours — let us know how we can assist with your travel plans.",
                de: "Wir antworten innerhalb weniger Stunden – sag uns, wie wir helfen können.",
                tr: "Size en geç birkaç saat içinde dönüş yapıyoruz; seyahat planınıza nasıl yardımcı olalım?",
                fa: "در کمترین زمان پاسخ می‌دهیم؛ بگویید چگونه می‌توانیم در برنامه سفر کمک کنیم.",
                ru: "Отвечаем в течение нескольких часов — расскажите, чем помочь в организации поездки.",
                pl: "Odpowiadamy w ciągu kilku godzin — napisz, jak możemy pomóc w podróży.",
                ar: "نرد على استفسارك خلال ساعات — أخبرنا كيف نساعدك في خطتك للسفر."),
            ["contact.address.title"] = Create("Office", de: "Büro", tr: "Ofis", fa: "دفتر", ru: "Офис", pl: "Biuro", ar: "المكتب"),
            ["contact.phone.title"] = Create("Phone", de: "Telefon", tr: "Telefon", fa: "تلفن", ru: "Телефон", pl: "Telefon", ar: "الهاتف"),
            ["contact.email.title"] = Create("Email", de: "E-Mail", tr: "E-posta", fa: "ایمیل", ru: "Эл. почта", pl: "E-mail", ar: "البريد"),
            ["contact.hours.title"] = Create("Working hours", de: "Öffnungszeiten", tr: "Çalışma Saatleri", fa: "ساعات کار", ru: "Часы работы", pl: "Godziny pracy", ar: "ساعات العمل"),
            ["contact.form.name"] = Create("Full name", de: "Vollständiger Name", tr: "Ad Soyad", fa: "نام و نام خانوادگی", ru: "Полное имя", pl: "Imię i nazwisko", ar: "الاسم الكامل"),
            ["contact.form.email"] = Create("Email", de: "E-Mail", tr: "E-posta", fa: "ایمیل", ru: "Эл. почта", pl: "E-mail", ar: "البريد الإلكتروني"),
            ["contact.form.message"] = Create("Message", de: "Nachricht", tr: "Mesajınız", fa: "پیام", ru: "Сообщение", pl: "Wiadomość", ar: "الرسالة"),
            ["contact.form.submit"] = Create("Send message", de: "Nachricht senden", tr: "Mesaj Gönder", fa: "ارسال پیام", ru: "Отправить сообщение", pl: "Wyślij wiadomość", ar: "أرسل الرسالة"),
            ["contact.hours.value"] = Create("Monday–Sunday: 08:00 – 22:00", de: "Montag–Sonntag: 08:00 – 22:00", tr: "Pazartesi–Pazar: 08.00 – 22.00", fa: "دوشنبه تا یکشنبه: ۰۸:۰۰ تا ۲۲:۰۰", ru: "Пн–Вс: 08:00 – 22:00", pl: "Poniedziałek–Niedziela: 08:00 – 22:00", ar: "الاثنين–الأحد: 08:00 – 22:00"),
            ["contact.success"] = Create("Thanks! We received your message and will respond shortly.", de: "Danke! Wir haben deine Nachricht erhalten und melden uns bald.", tr: "Teşekkürler! Mesajınız bize ulaştı, kısa sürede dönüş yapacağız.", fa: "متشکریم! پیام شما دریافت شد و به زودی پاسخ خواهیم داد.", ru: "Спасибо! Мы получили ваше сообщение и скоро ответим.", pl: "Dziękujemy! Otrzymaliśmy Twoją wiadomość i wkrótce odpiszemy.", ar: "شكرًا لك! استلمنا رسالتك وسنعاود الرد قريبًا.")
        };

        private static Dictionary<string, string> Create(string en, string? de = null, string? tr = null, string? fa = null, string? ru = null, string? pl = null, string? ar = null)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["en"] = en,
                ["de"] = de ?? en,
                ["tr"] = tr ?? en,
                ["fa"] = fa ?? en,
                ["ru"] = ru ?? en,
                ["pl"] = pl ?? en,
                ["ar"] = ar ?? en
            };
        }

        public static string Get(string language, string key)
        {
            var normalized = LanguageCatalog.Normalize(language);
            if (_translations.TryGetValue(key, out var map))
            {
                if (map.TryGetValue(normalized, out var value) && !string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }

                if (map.TryGetValue("en", out var fallback) && !string.IsNullOrWhiteSpace(fallback))
                {
                    return fallback;
                }
            }

            return key;
        }
    }
}
