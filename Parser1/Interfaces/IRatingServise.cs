namespace Parser1.Interfaces
{
    public interface IRatingServise
    {
        public void GetRatingForScientists(string direction);
        int GetRating();

        public void GetRatingToAllFromGovUa(string direction);
        void GetRatingForScientist(string name);
    }
}
