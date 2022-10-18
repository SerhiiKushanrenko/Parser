namespace Parser1.Interfaces
{
    public interface IMainParser
    {
        public void ParseGeneralInfo();

        public List<string> GetDirection();

        public void CheckOnEquals(string direction);

        void ParseNewScientist(string direction);





    }
}
