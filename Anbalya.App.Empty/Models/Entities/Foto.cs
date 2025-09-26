namespace Models.Entities
{
    public class Foto : BaseEntity
    {
        public string Address { get; set; } = "";
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
        public Foto()
        {

        }
        public Foto(string address) => Address = address;
    }


}