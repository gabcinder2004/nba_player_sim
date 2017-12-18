using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketballPlayerSimulator
{
    public class Possession
    {
        public Team TeamOnOffense { get; private set; }
        public Team TeamOnDefense { get; private set; }
        public Player playerShooting { get; private set; }
        public Player playerDefending { get; private set; }
        public int Points { get; set; }
        public ShotType shotType;

        private List<double> playerOdds { get; set; }
        private List<double> shotOdds { get; set; }

        public Possession(Team teamOnOffense, Team teamOnDefense)
        {
            TeamOnOffense = teamOnOffense;
            TeamOnDefense = teamOnDefense;
            Points = 0;
        }

        public void Simulate()
        {
            GetPossessionOdds();

            playerShooting = GetOffensivePlayer();
            playerDefending = GetDefensivePlayer(playerShooting);
            PlayerShotAttempt();
            GetTypeOfShot();
            DoesPlayerMakeShot();
        }

        public void DoesPlayerMakeShot()
        {
            double trueOdds;
            var randomNumber = Util.RandomDouble();
            var playerShotOdds = shotOdds[0] / 100;
            switch (shotType)
            {
                case ShotType.ThreePointer:
                    {
                        trueOdds = playerShooting.Rating.LongRange * playerShotOdds;
                        Points = 3;
                    }
                    break;
                case ShotType.MediumRange:
                    {
                        trueOdds = playerShooting.Rating.MediumRange * playerShotOdds;
                        Points = 2;
                    }
                    break;
                default:
                    trueOdds = playerShooting.Rating.ShortRange * playerShotOdds;
                    Points = 2;
                    break;
            }

            if (randomNumber > trueOdds)
            {
                Points = 0;
            }
        }

        private void GetTypeOfShot()
        {
            var shotTypeOdds = GetOdds(new List<double>
                                           {
                                               playerShooting.Rating.ShortRange,
                                               playerShooting.Rating.MediumRange,
                                               playerShooting.Rating.LongRange
                                           }).ToList();

            shotTypeOdds[1] = shotTypeOdds[0] + shotTypeOdds[1];
            shotTypeOdds[2] = shotTypeOdds[1] + shotTypeOdds[2];

            var randomNumber = Util.RandomDouble();
            if (randomNumber <= shotTypeOdds[0])
            {
                shotType = ShotType.CloseRange;
            }
            else if (randomNumber <= shotTypeOdds[1])
            {
                shotType = ShotType.MediumRange;
            }
            else
            {
                shotType = ShotType.ThreePointer;
            }
        }

        private void PlayerShotAttempt()
        {
            var offensivePlayerRating = playerShooting.Rating.Offense;
            var defensivePlayerRating = playerDefending.Rating.Defense;
            shotOdds = new List<double>();
            shotOdds = GetOdds(new List<double> { offensivePlayerRating, defensivePlayerRating }).ToList();
            shotOdds[1] = shotOdds[0] + shotOdds[1];
        }

        private Player GetDefensivePlayer(Player offensivePlayer)
        {
            return TeamOnDefense.Players.Single(player => player.Position == offensivePlayer.Position);
        }

        private void GetPossessionOdds(int changeFactor = 2)
        {
            playerOdds = new List<double>();
            double totalOffense = TeamOnOffense.Players.Sum(player => player.Rating.Offense);
            var averageRating = totalOffense / 5;

            foreach (var player in TeamOnOffense.Players)
            {
                var differenceFromAverage = player.Rating.Offense - averageRating;
                playerOdds.Add(((player.Rating.Offense + (changeFactor * differenceFromAverage)) / totalOffense) * 100);
            }

            playerOdds[1] = playerOdds[0] + playerOdds[1];
            playerOdds[2] = playerOdds[1] + playerOdds[2];
            playerOdds[3] = playerOdds[2] + playerOdds[3];
            playerOdds[4] = playerOdds[3] + playerOdds[4];
        }

        public IEnumerable<double> GetOdds(IEnumerable<double> playerRatings, int changeFactor = 2)
        {
            var result = new List<double>();

            double totalRating = 0;
            totalRating += playerRatings.Sum();
            var averageRating = playerRatings.Average();

            foreach (var playerRating in playerRatings)
            {
                var differenceFromAverage = playerRating - averageRating;
                result.Add(((playerRating + (changeFactor * differenceFromAverage)) / totalRating) * 100);
            }

            return result;
        }

        private Player GetOffensivePlayer()
        {
            var randomNumber = Util.RandomDouble();
            if (randomNumber <= playerOdds[0])
            {
                return TeamOnOffense.Players[0];
            }
            if (randomNumber <= playerOdds[1])
            {
                return TeamOnOffense.Players[1];
            }
            if (randomNumber <= playerOdds[2])
            {
                return TeamOnOffense.Players[2];
            }
            if (randomNumber <= playerOdds[3])
            {
                return TeamOnOffense.Players[3];
            }
            return TeamOnOffense.Players[4];
        }

        public override string ToString()
        {
            return string.Format("Offense: {0} (PlayerScoring: {1}", TeamOnOffense.Name, playerShooting.Name);
        }
    }
}