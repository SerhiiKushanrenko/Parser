using DAL.Models;

namespace BLL.Parsers.Interfaces
{
    public interface IParserDimensions
    {
        public Task StartParse();
        public Task StartParseByList(List<Scientist> list);
        public Task ParseDimensionsForSingleScientist(string? scientistName);
    }
}
