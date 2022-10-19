namespace Parser1.Models
{
    public class SubdirectionOfWork
    {
        public int SubdirectionOfWorkId { get; set; }
        public string Name { get; set; }

        public int DirectionId { get; set; }
        public Direction Direction { get; set; }
    }
}
