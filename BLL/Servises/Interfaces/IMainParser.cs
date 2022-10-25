namespace BLL.Interfaces
{
    public interface IMainParser
    {
        public Task StartParsing();

        // public Task<List<string>> GetDirection();

        public Task CheckOnEquals(string direction);

        Task ParseNewScientist(string direction);
    }
}
