namespace Parser1.Models
{
    public class Scientist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Organization { get; set; }

        public int Rating { get; set; }
        public int DirectionId { get; set; }
        public Direction? Direction { get; set; }

        public ICollection<ScientistWork> ScientistsWorks { get; set; }
    }
}
