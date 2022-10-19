namespace Parser1.Interfaces
{
    public interface ISupportParser
    {
        public void AddWorkToScientists(string direction);
        public void GetGeneralInfo(string directionForSearch, string directionForScienticst);

        public void addWorktoScientist(string name, List<string> listOfWork);
        public (List<string>, string degree) GetListOfWork(string name);
    }
}
