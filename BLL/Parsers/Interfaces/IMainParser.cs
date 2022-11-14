using DAL.Models;

namespace BLL.Parsers.Interfaces
{
    public interface IMainParser
    {
        public Task StartParsing();
        public Task<List<string>> GetDirection();
        Task StartParsing(ParsingType type);
    }
}
