namespace Parser1.Models
{
    public class Direction
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<SubdirectionOfWork> Subdirections { get; set; }
    }
}