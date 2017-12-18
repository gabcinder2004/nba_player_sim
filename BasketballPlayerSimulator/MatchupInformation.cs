namespace BasketballPlayerSimulator
{
    public class MatchupInformation
    {
        public Team TeamOne;
        public Team TeamTwo;
        public int TeamOneHomeGames;
        public int TeamTwoHomeGames;
        public int GamesLeftToPlay;

        public MatchupInformation(Team teamOne, Team teamTwo)
        {
            TeamOne = teamOne;
            TeamTwo = teamTwo;
        }

        public override string ToString()
        {
            return string.Format("TeamOne: {0} - Teamtwo: {1} [Played: {2}]", TeamOne.Name, TeamTwo.Name, GamesLeftToPlay);
        }
    }
}