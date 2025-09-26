

namespace Models.Entities
{
    public class Tour : BaseEntity
    {
        public string TourName { get; set; } = "";
        public int Price { get; set; }
        public int KinderPrice { get; set; }
        public int InfantPrice { get; set; }
        public float LocLat { get; set; }
        public float LocLon { get; set; }
        public Category Category { get; set; }
        public string DescriptionEn { get; set; } = "";
        public string DescriptionDe { get; set; } = "";
        public string DescriptionRu { get; set; } = "";
        public string DescriptionPo { get; set; } = "";
        public string DescriptionAr { get; set; } = "";
        public string DescriptionPe { get; set; } = "";
        public string MiniDescriptionEn { get; set; } = "";
        public string MiniDescriptionDe { get; set; } = "";
        public string MiniDescriptionRu { get; set; } = "";
        public string MiniDescriptionPo { get; set; } = "";
        public string MiniDescriptionAr { get; set; } = "";
        public string MiniDescriptionPe { get; set; } = "";
        public int DurationHours { get; set; }
        public List<string> Services { get; set; } = new List<string>();
        public string? Foto { get; set; }
        public IEnumerable<Foto> Fotos { get; set; } = Enumerable.Empty<Foto>();
        public int ActiveDay { get; set; }
        public Tour(int id, string name, int price, int kinderPrice, int infantPrice, Category category, float locLat, float locLon, string descriptionEn, string descriptionDe, string descriptionRu, string descriptionPo, string descriptionPe, string descriptionAr, string miniDescriptionEn, string miniDescriptionDe, string miniDescriptionRu, string miniDescriptionPo, string miniDescriptionPe, string miniDescriptionAr, List<Foto> fotos, int activeDay, int durationHours, List<string> services)
        {
            Id = id;
            TourName = name;
            Price = price;
            KinderPrice = kinderPrice;
            InfantPrice = infantPrice;
            LocLat = locLat;
            LocLon = locLon;
            DescriptionEn = descriptionEn;
            DescriptionDe = descriptionDe;
            DescriptionRu = descriptionRu;
            DescriptionPo = descriptionPo;
            DescriptionAr = descriptionAr;
            DescriptionPe = descriptionPe;

            MiniDescriptionEn = miniDescriptionEn;
            MiniDescriptionDe = miniDescriptionDe;
            MiniDescriptionPo = miniDescriptionPo;
            MiniDescriptionAr = miniDescriptionAr;
            MiniDescriptionPe = miniDescriptionPe;
            MiniDescriptionRu = miniDescriptionRu;

            Foto = fotos.First().Address;
            Category = category;
            ActiveDay = activeDay;
            DurationHours = durationHours;
            Services = services;

        }
        public void SetPrice(int price, int kinderPrice)
        {
            Price = price;
            KinderPrice = kinderPrice;
        }
        public void SetLoc(float locLat, float locLon)
        {
            LocLat = locLat;
            LocLon = locLon;
        }


        public void SetActiveDay(int activeDay)
        {
            ActiveDay = activeDay;
        }
        public Tour()
        {

        }

    }

}