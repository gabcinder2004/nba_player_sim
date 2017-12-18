using System;
using System.Linq;
using System.Threading;
using BasketballPlayerSimulator;

namespace BasketballSimulator
{
    public class Program
    {
        static readonly NBA NBA = new NBA();

        public static void Main(string[] args)
        {
            NBA.Season.SimulateSeason();

            NBA.Season.Standings.ForEach(Console.WriteLine);

            Console.Read();
        }

        public static void DisplayResult(Game game)
        {
            Console.WriteLine("{0} vs {1}", game.AwayTeam.Name, game.HomeTeam.Name);
            Console.WriteLine("----------------------------------");
            Console.WriteLine(game.Overtime > 0
                                  ? string.Format("FINAL SCORE: {0} overtime quarters!", game.Overtime)
                                  : "FINAL SCORE:");
            Console.WriteLine("{0}: {1}", game.AwayTeam.Name, game.Score.Away);
            Console.WriteLine("{0}: {1}", game.HomeTeam.Name, game.Score.Home);
            Console.WriteLine();
            foreach (var player in game.AwayTeam.Players)
            {
                var playerStats = game.Statisticses.Single(stats => stats.Player.Name == player.Name);
                Console.WriteLine("{0} - {1} pts. (FG: {2}/{3} - {4}%) (3PT: {5}/{6} - {7}%)", player.Name, playerStats.Points, playerStats.FieldGoalsMade, playerStats.FieldGoalAttempts, playerStats.FieldGoalPercentage, playerStats.ThreePointFieldGoalMade, playerStats.ThreePointFieldGoalAttempts, playerStats.ThreePointFieldGoalPercentage);
            }

            Console.WriteLine();

            foreach (var player in game.HomeTeam.Players)
            {
                var playerStats = game.Statisticses.Single(stats => stats.Player.Name == player.Name);
                Console.WriteLine("{0} - {1} pts. (FG: {2}/{3} - {4}%) (3PT: {5}/{6} - {7}%)", player.Name, playerStats.Points, playerStats.FieldGoalsMade, playerStats.FieldGoalAttempts, playerStats.FieldGoalPercentage, playerStats.ThreePointFieldGoalMade, playerStats.ThreePointFieldGoalAttempts, playerStats.ThreePointFieldGoalPercentage);
            }

            Thread.Sleep(1000);
            Console.Clear();
        }
    }
}
