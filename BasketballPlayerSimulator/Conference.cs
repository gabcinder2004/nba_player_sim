using System.Collections.Generic;
using System.Linq;

namespace BasketballPlayerSimulator
{
    public class Conference
    {
        public string Name;
        public List<Division> Divisions;

        public Conference(string name, List<Division> divisions)
        {
            Name = name;
            Divisions = divisions;
        }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerable<Team> TeamsInSpecificConference()
        {
            return Divisions.SelectMany(div => div.Teams);
        }

        public Division GetDivisionOfTeam(Team team)
        {
            return Divisions.FirstOrDefault(div => div.Teams.Any(t => t.Name == team.Name));
        }
    }
}