using System.ComponentModel.DataAnnotations;

namespace Parser1.Models
{
    public class ScientistWork
    {
        [Key]
        public int Id { get; set; }
        public int ScientistId { get; set; }
        public Scientist? Scientist { get; set; }

        public int WorkId { get; set; }

        public Work? Work { get; set; }
    }
}
