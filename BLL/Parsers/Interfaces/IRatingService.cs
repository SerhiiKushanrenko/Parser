namespace BLL.Parsers.Interfaces
{
    public interface IRatingService
    {
        //  public void GetRatingForScientists(string direction);

        // public void GetRatingToAllFromGovUa(string direction);
        int GetRatingForScientist(string name);

        public int GetRatingGoogleScholar(string name);
    }
}
