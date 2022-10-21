namespace Parser1.Models
{
    public class ScientistSubdirection
    {
        public int ScientistId { get; set; }
        public Scientist? Scientist { get; set; }

        public int? SubdirectionId { get; set; }

        public Subdirection? Subdirection { get; set; }
    }
}
