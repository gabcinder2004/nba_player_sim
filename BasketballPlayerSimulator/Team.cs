using System.Collections.Generic;
using System.Linq;

namespace BasketballPlayerSimulator
{
    public class Team
    {
        public int ExtraConferenceOpponentCount;
        public TeamName Name { get; set; }
        public List<Player> Players { get; set; }
        public Division Division { get; set; }
        public Rating Rating
        {
            get
            {
                var totalRating = Players.Sum(player => player.Rating.Overall);
                return new Rating(totalRating / Players.Count);
            }
        }
        public Schedule Schedule { get; set; }


        public Team(TeamName name)
        {
            Name = name;
            Players = new List<Player>();
            ExtraConferenceOpponentCount = 0;
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
