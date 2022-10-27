namespace BLL.Interfaces
{
    public interface ISupportParser
    {
        public void AddWorkToScientists(string direction);
        public void GetGeneralInfo(string directionForSearch, string directionForScienticst);

        public void AddWorkToScientist(string name, List<string> listOfWork);
        public void AddListOfWorkAndDegree();

        public void AddScietistSubdirAndAddDirectionToDb(List<string> subDirection, string direction, string scientistName);

    }
}
