using BLL.AdditionalModels;

namespace BLL.Parsers.Interfaces
{
    public interface IParsingHandler
    {
        Task StartParsing(ParsingType type);
    }
}
