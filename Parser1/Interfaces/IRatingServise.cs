namespace Parser1.Interfaces
{
    public interface IRatingServise
    {
        public void GetRatingForScientists(string direction);

        public void GetRatingToAllFromGovUa(string direction);
        void GetRatingForScientist(string name);

        public int GerRatingGoogleScholar(string name);
    }
}
