using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketballPlayerSimulator
{
    public class NBA
    {
        public List<TeamName> TeamNames;
        public List<Conference> Conferences;
        public List<Team> Teams;
        public List<Player> Players;
        public Season Season;

        public NBA()
        {
            TeamNames = new List<TeamName>();
            Teams = new List<Team>();
            Players = new List<Player>();
            Conferences = new List<Conference>();
            Players = RosterImporter.ImportPlayers();
            InstantiateTeams();
            OrganizePlayersIntoTeams();
            InstantiateTeamSetup();
            PutTeamsInDivision();
            var schedule = ScheduleImporter.ImportSchedule(this);
            Season = new Season(82, schedule) { Standings = new List<Standing>() };
            CreateStandings();
        }

        public void CreateStandings()
        {
            foreach (var team in Teams)
            {
                Season.Standings.Add(new Standing(team, 0, 0));
            }
        }

        private void InstantiateTeams()
        {
            TeamNames = Enum.GetValues(typeof(TeamName)).OfType<TeamName>().ToList();

            foreach (var t in TeamNames)
            {
                Teams.Add(new Team(t));
            }
        }

        private void PutTeamsInDivision()
        {
            foreach (var conf in Conferences)
            {
                foreach (var div in conf.Divisions)
                {
                    foreach (var team in Teams.Where(team => div.Teams.Count(t => t.Name == team.Name) > 0))
                    {
                        team.Division = div;
                    }
                }
            }
        }

        private void InstantiateTeamSetup()
        {
            Conferences.Add(new Conference("Eastern", new List<Division> { }));
            Conferences.Add(new Conference("Western", new List<Division> { }));

            var atlanticDivision = new Division("Atlantic",
                                                Teams.Where(
                                                    team =>
                                                    team.Name == TeamName.Boston_Celtics ||
                                                    team.Name == TeamName.Brooklyn_Nets ||
                                                    team.Name == TeamName.NewYork_Knicks ||
                                                    team.Name == TeamName.Philadelphia_76ers ||
                                                    team.Name == TeamName.Toronto_Raptors).ToList(), Conferences.Single(conf => conf.Name == "Eastern"));
            var pacificDivison = new Division("Pacific",
                                              Teams.Where(
                                                  team =>
                                                  team.Name == TeamName.GoldenState_Warriors ||
                                                  team.Name == TeamName.LA_Clippers || team.Name == TeamName.LA_Lakers ||
                                                  team.Name == TeamName.Phoenix_Suns ||
                                                  team.Name == TeamName.Sacramento_Kings).ToList(), Conferences.Single(conf => conf.Name == "Western"));
            var centralDivision = new Division("Central",
                                               Teams.Where(
                                                   team =>
                                                   team.Name == TeamName.Chicago_Bulls ||
                                                   team.Name == TeamName.Cleveland_Cavaliers ||
                                                   team.Name == TeamName.Detroit_Pistons ||
                                                   team.Name == TeamName.Indiana_Pacers ||
                                                   team.Name == TeamName.Milwaukee_Bucks).ToList(), Conferences.Single(conf => conf.Name == "Eastern"));

            var southwestDivision = new Division("Southwest",
                                                 Teams.Where(
                                                     team =>
                                                     team.Name == TeamName.Dallas_Mavericks ||
                                                     team.Name == TeamName.Houston_Rockets ||
                                                     team.Name == TeamName.Memphis_Grizzlies ||
                                                     team.Name == TeamName.NewOrleans_Pelicans ||
                                                     team.Name == TeamName.SanAntonio_Spurs).ToList(), Conferences.Single(conf => conf.Name == "Western"));

            var southeastDivision = new Division("Southeast",
                                                 Teams.Where(
                                                     team =>
                                                     team.Name == TeamName.Atlanta_Hawks ||
                                                     team.Name == TeamName.Charlotte_Bobcats ||
                                                     team.Name == TeamName.Miami_Heat ||
                                                     team.Name == TeamName.Orlando_Magic ||
                                                     team.Name == TeamName.Washington_Wizards).ToList(), Conferences.Single(conf => conf.Name == "Eastern"));

            var northwestDivision = new Division("Northwest",
                                                 Teams.Where(
                                                     team =>
                                                     team.Name == TeamName.Denver_Nuggets ||
                                                     team.Name == TeamName.Minnesota_Timberwolves ||
                                                     team.Name == TeamName.OklahomaCity_Thunder ||
                                                     team.Name == TeamName.Portland_Blazers ||
                                                     team.Name == TeamName.Utah_Jazz).ToList(), Conferences.Single(conf => conf.Name == "Western"));

            Conferences.Single(conf => conf.Name == "Eastern").Divisions.Add(atlanticDivision);
            Conferences.Single(conf => conf.Name == "Eastern").Divisions.Add(southeastDivision);
            Conferences.Single(conf => conf.Name == "Eastern").Divisions.Add(centralDivision);
            Conferences.Single(conf => conf.Name == "Western").Divisions.Add(pacificDivison);
            Conferences.Single(conf => conf.Name == "Western").Divisions.Add(southwestDivision);
            Conferences.Single(conf => conf.Name == "Western").Divisions.Add(northwestDivision);

        }

        public void OrganizePlayersIntoTeams()
        {
            foreach (var teamName in TeamNames)
            {
                Teams.Single(team => team.Name == teamName).Players = Players.Where(player => player.Team == teamName).ToList();
            }

            SortPlayersByPosition();
        }

        public void SortPlayersByPosition()
        {
            Teams.Where(team => team.Players.Count != 0).ToList().ForEach(team => team.Players.Sort());
        }

        public Team SelectTeam(TeamName teamName)
        {
            return Teams.Single(team => team.Name == teamName);
        }
    }
}
