namespace Parser1.Models
{
    public class ScientistWork
    {
        public int ScientistId { get; set; }
        public Scientist? Scientist { get; set; }

        public int WorkOfScientistId { get; set; }

        public WorkOfScientist? WorkOfScientist { get; set; }
    }
}
