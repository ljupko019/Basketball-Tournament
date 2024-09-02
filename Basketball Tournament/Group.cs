using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Basketball_Tournament
{
    public class Group
    {
        public List<Country> Teams { get; set; } = new List<Country>();
        public string Name { get; set; }

        public void SimulateGroupMatches()
        {
            for (int i = 0; i < Teams.Count; i++)
            {
                for (int j = i + 1; j < Teams.Count; j++)
                {
                    Match match = new Match(Teams[i], Teams[j]);
                    match.SimulateMatch();
                }
            }
        }

        public void RankTeams()
        {            
            List<Country> teamsWithSamePoints = new List<Country>();

            Dictionary<int, List<Country>> groupTeams = Teams
            .GroupBy(t => t.Points).ToDictionary(g => g.Key, g => g.ToList());

            List<List<Country>> listOfThreeTeams = groupTeams.Values
            .Where(list => list.Count == 3)
            .ToList();

            List<List<Country>> listOfTwoTeams = groupTeams.Values
           .Where(list => list.Count == 2)
           .ToList();

            if (listOfThreeTeams.Count > 0)
            {
                teamsWithSamePoints = listOfThreeTeams.First();

                for (int i = 0; i < teamsWithSamePoints.Count; i++)
                {

                    for (int j = 0; j < teamsWithSamePoints.Count; j++)
                    {
                        if (teamsWithSamePoints[i] != teamsWithSamePoints[j])
                        {
                            teamsWithSamePoints[i].SetPointsDifferenceAgainst(teamsWithSamePoints[j]);
                        }

                    }
                } 
                teamsWithSamePoints.Sort((x, y) => y.PointDifference3Teams.CompareTo(x.PointDifference3Teams));
                Country teamAlone = new Country();
                foreach (Country country in Teams)
                {
                    if (!teamsWithSamePoints.Contains(country))
                    {
                        teamAlone = country;
                    }
                }
                if (teamsWithSamePoints[0].Points > teamAlone.Points)
                {
                    List<Country> teams = teamsWithSamePoints;
                    teams.Add(teamAlone);
                    Teams = teams;
                }
                else
                {
                    List<Country> teams = new List<Country>();
                    teams.Add(teamAlone);
                    foreach (Country team in teamsWithSamePoints)
                    {
                        teams.Add(team);
                    }
                    Teams = teams;
                }
            }
            else if (listOfTwoTeams.Count == 2)
            {               
                Country winner1 = listOfTwoTeams.First()[0].IsMatchWinner(listOfTwoTeams.First()[1]);
                Country winner2 = listOfTwoTeams.Last()[0].IsMatchWinner(listOfTwoTeams.Last()[1]);

                if (winner1.Points > winner2.Points)
                {
                    Teams.Clear();
                    Teams.Add(winner1);
                    if (winner1 == listOfTwoTeams.First()[0])
                    {
                        Teams.Add(listOfTwoTeams.First()[1]);
                    }
                    else if (winner1 == listOfTwoTeams.First()[1])
                    {
                        Teams.Add(listOfTwoTeams.First()[0]);
                    }

                    Teams.Add(winner2);
                    if (winner2 == listOfTwoTeams.Last()[0])
                    {
                        Teams.Add(listOfTwoTeams.Last()[1]);
                    }
                    else if (winner2 == listOfTwoTeams.Last()[1])
                    {
                        Teams.Add(listOfTwoTeams.Last()[0]);
                    }
                }
                else
                {
                    Teams.Clear();
                    Teams.Add(winner2);
                    if (winner2 == listOfTwoTeams.Last()[0])
                    {
                        Teams.Add(listOfTwoTeams.Last()[1]);
                    }
                    else if (winner2 == listOfTwoTeams.Last()[1])
                    {
                        Teams.Add(listOfTwoTeams.Last()[0]);
                    }

                    Teams.Add(winner1);
                    if (winner1 == listOfTwoTeams.First()[0])
                    {
                        Teams.Add(listOfTwoTeams.First()[1]);
                    }
                    else if (winner1 == listOfTwoTeams.First()[1])
                    {
                        Teams.Add(listOfTwoTeams.First()[0]);
                    }
                }
            }
            else 
            {
                Teams.Sort((x, y) => y.Points.CompareTo(x.Points));
            }
        }

        public void PrintGroupRanking()
        {
            SetGroupToTeams();

            RankTeams();

            Console.WriteLine($"Grupa {Name}:");
            for (int i = 0; i < Teams.Count; i++)
            {
                var team = Teams[i];
                Console.WriteLine($"{i + 1}. {team.Team} {team.Wins} / {team.Losses} / {team.Points} / {team.PointsScored} / {team.PointsReceived} / {team.PointDifference}");
            }
        }

        public void SetGroupToTeams() 
        {
            foreach (Country team in Teams)
            {
                team.Group = this;
            }
        }

        public void CalculateTwoTeamsSamePoints(List<Country> teamsWithSamePoints)
        {
            Country otherBetterTeam = new Country();
            Country otherWorseTeam = new Country();
            List<Country> otherTeams = new List<Country>();
            foreach (Country country in Teams)
            {
                if (!teamsWithSamePoints.Contains(country))
                {
                    otherTeams.Add(country);
                }
            }

            if (otherTeams[0].Points > otherTeams[1].Points)
            {
                otherBetterTeam = otherTeams[0];
                otherWorseTeam = otherTeams[1];
            }
            else
            {
                otherBetterTeam = otherTeams[1];
                otherWorseTeam = otherTeams[0];
            }

            Teams.Clear();

            if (otherBetterTeam.Points > teamsWithSamePoints[0].Points)
            {
                Teams.Add(otherBetterTeam);
                if (otherWorseTeam.Points > teamsWithSamePoints[0].Points)
                {
                    Teams.Add(otherWorseTeam);
                    Teams.Add(teamsWithSamePoints[0]);
                    Teams.Add(teamsWithSamePoints[1]);
                }
                else
                {
                    Teams.Add(teamsWithSamePoints[0]);
                    Teams.Add(teamsWithSamePoints[1]);
                    Teams.Add(otherWorseTeam);
                }
            }
            else
            {
                Teams.Add(teamsWithSamePoints[0]);
                Teams.Add(teamsWithSamePoints[1]);
                Teams.Add(otherBetterTeam);
                Teams.Add(otherWorseTeam);
            }
        }
    }
}


