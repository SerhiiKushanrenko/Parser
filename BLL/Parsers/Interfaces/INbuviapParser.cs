namespace BLL.Parsers.Interfaces
{
    public interface INbuviapParser
    {
        public Task<List<string>> GetDirection();
        public Task StartParsing();
        public Task ParsingOfMissingScientists();
    }
}
