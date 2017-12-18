using System.Collections.Generic;
using System.Linq;

namespace BasketballPlayerSimulator
{
    public class Season
    {
        public int SeasonLength;
        public readonly Schedule Schedule;
        public List<Standing> Standings;
        public List<MatchupInformation> MatchupInformations { get; set; }

        public Season(int length, Schedule schedule)
        {
            SeasonLength = length;
            MatchupInformations = new List<MatchupInformation>();
            Schedule = schedule;
        }

        public void SimulateSeason()
        {
            foreach (var game in Schedule.Games)
            {
                game.SimulateGame();
                var homeTeamStanding = Standings.Single(x => x.Team.Name == game.HomeTeam.Name);
                var awayTeamStanding = Standings.Single(x => x.Team.Name == game.AwayTeam.Name);

                if (game.Score.Home > game.Score.Away)
                {
                    homeTeamStanding.Wins++;
                    awayTeamStanding.Losses++;
                }
                else
                {
                    awayTeamStanding.Wins++;
                    homeTeamStanding.Losses++;
                }
            }
        }
    }
}