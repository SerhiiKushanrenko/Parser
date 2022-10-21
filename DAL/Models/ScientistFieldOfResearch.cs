using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class ScientistFieldOfResearch
    {
        [Key]
        public int Id { get; set; }
        public int ScientistId { get; set; }
        public Scientist Scientist { get; set; }

        public int FieldOfResearchId { get; set; }

        public FieldOfResearch FieldOfResearch { get; set; }
    }
}
