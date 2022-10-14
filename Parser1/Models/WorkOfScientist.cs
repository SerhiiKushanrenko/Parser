namespace Parser1.Models
{
    public class WorkOfScientist
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ScientistWork> ScientistsWorks { get; set; }
    }
}
