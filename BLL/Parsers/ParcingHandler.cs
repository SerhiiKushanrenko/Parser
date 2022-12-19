using BLL.AdditionalModels;
using BLL.Parsers.Interfaces;

namespace BLL.Parsers
{
    public class ParcingHandler : IParsingHandler
    {
        private readonly IDimensionsParser _dimensionsParser;
        private readonly INbuviapParser _nbuviapParser;

        public ParcingHandler(
            IDimensionsParser dimensionsParser,
            INbuviapParser nbuviapParser

        )
        {
            _dimensionsParser = dimensionsParser;
            _nbuviapParser = nbuviapParser;
        }

        public async Task StartParsing(ParsingType type)
        {
            switch (type)
            {
                case ParsingType.Full:
                    await _nbuviapParser.StartParsing();
                    await _dimensionsParser.StartParsing();
                    break;
                case ParsingType.BaseInformation:
                    await _nbuviapParser.StartParsing();
                    break;
                case ParsingType.AdditionalInformation:
                    await _dimensionsParser.StartParsing();
                    break;
                default:
                    break;
            }
        }
    }
}
