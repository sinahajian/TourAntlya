namespace Models.Entities
{

    public class BaseEntity
    {
        public int Id { get; set; }
        public long CreationTime { get; set; }
        public BaseEntity()
        {

        }
    }

}