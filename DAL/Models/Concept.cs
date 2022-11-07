namespace DAL.Models
{
    public class Concept
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ScientistId { get; set; }

        public Scientist Scientist { get; set; }
    }
}
