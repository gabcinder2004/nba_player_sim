using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketballPlayerSimulator
{
    public class Game
    {
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTime Date { get; set; }
        public Score Score { get; set; }
        private List<Possession> possessions { get; set; }
        public List<Statistics> Statisticses { get; set; }
        public int Overtime = 0;

        public Game(Team homeTeam, Team awayTeam, DateTime date)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Score = new Score();
            Date = date;
        }

        public override string ToString()
        {
            return string.Format("{0} vs {1}", HomeTeam, AwayTeam);
        }

        public void SimulateGame()
        {
            possessions = new List<Possession>();

            for (var i = 0; i < 190; i++)
            {
                var possession = i % 2 == 0 ? new Possession(HomeTeam, AwayTeam) : new Possession(AwayTeam, HomeTeam);
                possession.Simulate();
                possessions.Add(possession);

                if (i % 2 == 0)
                {
                    Score.UpdateScore(possession.Points, 0);
                }
                else
                {
                    Score.UpdateScore(0, possession.Points);
                }
            }

            while (Score.Home == Score.Away)
            {
                Overtime++;
                for (var i = 0; i < 10; i++)
                {
                    var possession = i % 2 == 0 ? new Possession(HomeTeam, AwayTeam) : new Possession(AwayTeam, HomeTeam);
                    possession.Simulate();
                    possessions.Add(possession);

                    if (i % 2 == 0)
                    {
                        Score.UpdateScore(possession.Points, 0);
                    }
                    else
                    {
                        Score.UpdateScore(0, possession.Points);
                    }
                }
            }

            GetStats();
        }

        public void GetStats()
        {
            Statisticses = new List<Statistics>();

            foreach (var player in HomeTeam.Players)
            {
                Statisticses.Add(new Statistics(player));
            }

            foreach (var player in AwayTeam.Players)
            {
                Statisticses.Add(new Statistics(player));
            }

            foreach (var possession in possessions)
            {
                var stat = Statisticses.Single(stats => stats.Player.Name == possession.playerShooting.Name);
                stat.Points += possession.Points;
                stat.FieldGoalAttempts += 1;
                
                if (possession.Points != 0)
                {
                    stat.FieldGoalsMade += 1;
                    if (possession.shotType == ShotType.ThreePointer)
                    {
                        stat.ThreePointFieldGoalAttempts += 1;
                        stat.ThreePointFieldGoalMade += 1;
                    }
                }
                else if (possession.shotType == ShotType.ThreePointer)
                {
                    stat.ThreePointFieldGoalAttempts += 1;
                }

                if (stat.FieldGoalAttempts >= 0.001)
                {
                    stat.FieldGoalPercentage = Math.Round(((stat.FieldGoalsMade / stat.FieldGoalAttempts) * 100), 2);
                }

                if (stat.ThreePointFieldGoalAttempts >= 0.001)
                {
                    stat.ThreePointFieldGoalPercentage = Math.Round(((stat.ThreePointFieldGoalMade / stat.ThreePointFieldGoalAttempts) * 100), 2);
                }
            }
        }
    }

    public class Statistics
    {
        public Player Player;
        public double Points = 0;
        public double FieldGoalAttempts = 0;
        public double FieldGoalsMade = 0;
        public double ThreePointFieldGoalAttempts = 0;
        public double ThreePointFieldGoalMade = 0;
        public double FieldGoalPercentage = 0;
        public double ThreePointFieldGoalPercentage = 0;

        public Statistics(Player player)
        {
            Player = player;
        }
    }
}
