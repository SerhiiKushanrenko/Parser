namespace BLL.Interfaces
{
    public interface IRatingServise
    {
        //  public void GetRatingForScientists(string direction);

        // public void GetRatingToAllFromGovUa(string direction);
        int GetRatingForScientist(string name);

        public int GetRatingGoogleScholar(string name);
    }
}
