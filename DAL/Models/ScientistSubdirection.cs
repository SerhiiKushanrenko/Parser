using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class ScientistSubdirection
    {
        [Key]
        public int Id { get; set; }
        public int ScientistId { get; set; }
        public Scientist? Scientist { get; set; }

        public int? SubdirectionId { get; set; }

        public Subdirection? Subdirection { get; set; }
    }
}
