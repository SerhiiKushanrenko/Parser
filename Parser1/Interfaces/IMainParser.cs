using Parser1.Models;

namespace Parser1.Interfaces
{
    public interface IMainParser
    {
        public List<Scientist> ParseGeneralInfo();

        public List<string> GetDirection();

        public void CheckOnEquals(string direction);

        void ParseNewScientist(string direction);





    }
}
