using System.ComponentModel.DataAnnotations;

namespace DAL.Models
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
