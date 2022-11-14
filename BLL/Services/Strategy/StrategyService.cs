using BLL.Parsers.Interfaces;

namespace BLL.Services.Strategy
{
    public class StrategyService
    {
        private IParse mainParser;

        public void SetMainParser(IParse _mainParser)
        {
            mainParser = _mainParser;
        }

        public async Task ExecuteStrategy()
        {
            await mainParser.StartParsing();
        }
    }
}
