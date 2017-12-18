namespace BasketballPlayerSimulator
{
    public class Score
    {
        public int Home { get; private set; }
        public int Away { get; private set; }

        public Score()
        {
            Home = 0;
            Away = 0;
        }

        public void UpdateScore(int homeScore, int awayScore)
        {
            Home += homeScore;
            Away += awayScore;
        }
    }
}