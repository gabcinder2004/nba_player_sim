using System;
using System.Linq;

namespace BasketballPlayerSimulator
{
    public class Player : IComparable<Player>
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public Rating Rating { get; set; }
        public Position Position { get; set; }
        public TeamName Team { get; set; }

        public Player(string name, string number, Position position, TeamName team, Rating rating)
        {
            Name = name;
            Number = number;
            Position = position;
            Rating = rating;
            Team = team;
        }

        public int CompareTo(Player other)
        {
            return Position.CompareTo(other.Position);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Team);
        }
    }
}
