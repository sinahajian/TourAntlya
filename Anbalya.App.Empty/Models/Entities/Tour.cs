

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
        public string DescriptionPe { get; set; } = "";
        public string DescriptionAr { get; set; } = "";
        public string DescriptionTr { get; set; } = "";
        public string MiniDescriptionEn { get; set; } = "";
        public string MiniDescriptionDe { get; set; } = "";
        public string MiniDescriptionRu { get; set; } = "";
        public string MiniDescriptionPo { get; set; } = "";
        public string MiniDescriptionPe { get; set; } = "";
        public string MiniDescriptionAr { get; set; } = "";
        public string MiniDescriptionTr { get; set; } = "";
        public int DurationHours { get; set; }
        public List<string> Services { get; set; } = new List<string>();
        public string? Foto { get; set; }
        public ICollection<Foto> Fotos { get; set; } = new List<Foto>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public int ActiveDay { get; set; }
        public Tour(int id, string name, int price, int kinderPrice, int infantPrice, Category category, float locLat, float locLon, string descriptionEn, string descriptionDe, string descriptionRu, string descriptionPo, string descriptionPe, string descriptionAr, string miniDescriptionEn, string miniDescriptionDe, string miniDescriptionRu, string miniDescriptionPo, string miniDescriptionPe, string miniDescriptionAr, List<Foto> fotos, int activeDay, int durationHours, List<string> services, string? descriptionTr = null, string? miniDescriptionTr = null)
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
            DescriptionPe = descriptionPe;
            DescriptionAr = descriptionAr;
            DescriptionTr = string.IsNullOrWhiteSpace(descriptionTr) ? descriptionEn : descriptionTr.Trim();

            MiniDescriptionEn = miniDescriptionEn;
            MiniDescriptionDe = miniDescriptionDe;
            MiniDescriptionRu = miniDescriptionRu;
            MiniDescriptionPo = miniDescriptionPo;
            MiniDescriptionPe = miniDescriptionPe;
            MiniDescriptionAr = miniDescriptionAr;
            MiniDescriptionTr = string.IsNullOrWhiteSpace(miniDescriptionTr) ? miniDescriptionEn : miniDescriptionTr.Trim();

            if (fotos != null && fotos.Count > 0)
            {
                Foto = fotos[0].Address;
            }
            Fotos = new List<Foto>();
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
