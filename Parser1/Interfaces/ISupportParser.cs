using DAL.Models;

namespace Parser1.Interfaces
{
    public interface ISupportParser
    {
        public void AddWorkToScientists(string direction);
        public void GetGeneralInfo(string directionForSearch, string directionForScienticst);

        public void AddWorkToScientist(string name, List<string> listOfWork);
        public (List<string>, string degree) GetListOfWork(string name);

        public void AddScietistSubdirAndAddDirectionToDb(List<string> subDirection, string direction, string scientistName);

        public void AddSocialNetworkToScientist(List<string> listOfSocial, Scientist scientist);
        public List<ScientistSocialNetwork> GetSocialNetwork(string scientistName);

        public string GetSocialUrl(string socialElement);
    }
}
