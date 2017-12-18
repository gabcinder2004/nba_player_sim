using System.Collections.Generic;

namespace BasketballPlayerSimulator
{
    public class Division
    {
        public string Name;
        public List<Team> Teams;
        public Conference Conference;

        public Division(string name, List<Team> teams, Conference conference)
        {
            Name = name;
            Teams = teams;
            Conference = conference;
        }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerable<Team> TeamsInSpecificDivision()
        {
            return Teams;
        }
    }
}