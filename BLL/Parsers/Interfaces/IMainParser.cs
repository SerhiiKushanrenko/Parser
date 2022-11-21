using DAL.Models;

namespace BLL.Parsers.Interfaces
{
    public interface IMainParser
    {
        Task StartParsing(ParsingType type, string? scientistSecondName);
    }
}
