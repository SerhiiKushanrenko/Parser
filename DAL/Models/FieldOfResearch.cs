namespace DAL.Models
{
    public class FieldOfResearch
    {
        public int Id { get; set; }
        public int ANZSRC { get; set; }
        public string Title { get; set; }

        public int? ParentFieldOfResearchId { get; set; }
        public FieldOfResearch? ParentFieldOfResearch { get; set; }

        public virtual ICollection<ScientistFieldOfResearch> ScientistsFieldsOfResearch { get; set; }
        public virtual ICollection<FieldOfResearch> ChildFieldsOfResearch { get; set; }

    }
}
