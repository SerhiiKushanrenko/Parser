using BLL.Parsers.Interfaces;

namespace BLL.Services.Strategy
{
    public class DimensionsOnly : IParse
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

    }
}
