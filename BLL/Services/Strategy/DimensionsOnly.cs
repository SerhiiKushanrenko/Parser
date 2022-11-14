using BLL.Parsers.Interfaces;
using DAL.Models;

namespace BLL.Services.Strategy
{
    public class DimensionsOnly : IMainParser
    {
        private readonly IParserDimensions _parserDimensions;
        public DimensionsOnly(IParserDimensions parserDimensions)
        {
            _parserDimensions = parserDimensions;
        }

        /// <summary>
        /// the main parser 
        /// </summary>
        /// <returns></returns>
        public async Task StartParsing()
        {
            await _parserDimensions.StartParse();
        }

        public Task StartParsing(ParsingType type)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetDirection()
        {
            throw new NotImplementedException();
        }
    }
}
