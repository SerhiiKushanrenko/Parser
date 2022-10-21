namespace DAL.Models
{
    public class Subdirection
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int DirectionId { get; set; }
        public Direction Direction { get; set; }

        public ICollection<ScientistSubdirection> ScientistSubdirections { get; set; }

        //public ICollection<Scientist> Scientists { get; set; }

    }
}
