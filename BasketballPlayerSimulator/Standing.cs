namespace BasketballPlayerSimulator
{
    public class Standing
    {
        public Team Team;
        public int Wins;
        public int Losses;

        public Standing(Team team, int wins, int losses)
        {
            Team = team;
            Wins = wins;
            Losses = losses;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}-{2}", Team.Name, Wins, Losses);
        }
    }
}