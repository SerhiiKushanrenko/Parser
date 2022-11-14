using DAL.Models;

namespace BLL.Parsers.Interfaces
{
    public interface IMainParser : IParse
    {
        public Task<List<string>> GetDirection();
        Task StartParsing(ParsingType type);
    }
}
