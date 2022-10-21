namespace Parser1.Models
{
    public class Work
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public ICollection<ScientistWork> ScientistsWorks { get; set; }
    }
}
