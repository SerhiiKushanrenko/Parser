using DAL.Models;

namespace BLL.Interfaces
{
    public interface ISupportParser
    {
        public void AddWorkToScientists(string direction);
        public void GetGeneralInfo(string directionForSearch, string directionForScienticst);

        public void AddWorkToScientist(string name, List<string> listOfWork);
        public void AddListOfWorkAndDegree();

        public void AddScietistSubdirAndAddDirectionToDb(List<string> subDirection, string direction, string scientistName);

        //  public void AddSocialNetworkToScientist(List<string> listOfSocial, Scientist scientist);
        public List<ScientistSocialNetwork> GetSocialNetwork(string scientistName, ref int rating);

        public string GetSocialUrl(string socialElement);
    }
}
