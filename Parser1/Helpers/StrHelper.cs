namespace Parser1.Helpers
{
    public class StrHelper
    {
        public static string GetOnlyName(string name)
        {
            return name.Contains('(')
                ? name.Split("(").First().TrimEnd(' ')
                : name;
        }

        public static void SplitYearAndWork(string work)
        {
            if (work.Contains('['))
            {
                var tempArray = work.Split(']');
                var getYearOfWork = tempArray[1].Split(')');

                //var yearOfWork = int.Parse(tempArray[0].Substring(9));
            }
        }
    }
}
