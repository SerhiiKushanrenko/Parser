namespace BLL.AdditionalModels
{
    public class ScientistFilter
    {
        public string? Name { get; set; }
        public int? FieldOfResearchId { get; set; }
        public int? WorkId { get; set; }
        public int? HRatingMoreThan { get; set; }
        public int? HRatingLessThan { get; set; }
        public string? ScopusUrl { get; set; }
        public string? OrcidUrl { get; set; }
        public string? WosUrl { get; set; }
        public string? ScholarUrl { get; set; }
    }
}
