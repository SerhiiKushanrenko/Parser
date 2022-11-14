using BLL.Parsers.Interfaces;

namespace BLL.Services.Strategy
{
    internal class StrategyService
    {
        private IMainParser mainParser;

        public void SetMainParser(IMainParser _mainParser)
        {
            mainParser = _mainParser;
        }

        public void ExecuteStrategy()
        {
            mainParser.StartParsing();
        }
    }
}
