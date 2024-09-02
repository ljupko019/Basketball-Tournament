using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Basketball_Tournament
{
    public class Tournament
    {
        List<Group> allGroups = new List<Group>();
        List<Country> allTeams = new List<Country>();
        List<Pot> allPots = new List<Pot>();

        List<Country> firstPlaceTeams = new List<Country>();
        List<Country> secondPlaceTeams = new List<Country>();
        List<Country> thirdPlaceTeams = new List<Country>();


        public Tournament() { }
        public void AddGroup(Group group)
        {
            allGroups.Add(group);
        }

        public void PrintRoundMatches(int roundNumber)
        {
            string[] groupNames = { "A", "B", "C" };

            for (int i = 0; i < roundNumber; i++)
            {
                Console.WriteLine($"Grupna faza - {i + 1}. kolo:");

                for (int j = 0; j < allGroups.Count; j++)
                {
                    var group = allGroups[j];
                    string groupName = groupNames[j];
                    Console.WriteLine($"    Grupa {groupName}:");

                    switch (i)
                    {
                        case 0:
                            Match match1 = new Match(group.Teams[0], group.Teams[1]);
                            Match match2 = new Match(group.Teams[2], group.Teams[3]);
                            match1.SimulateMatch();
                            match2.SimulateMatch();
                            break;
                        case 1:
                            Match match3 = new Match(group.Teams[0], group.Teams[2]);
                            Match match4 = new Match(group.Teams[1], group.Teams[3]);
                            match3.SimulateMatch();
                            match4.SimulateMatch();
                            break;
                        case 2:
                            Match match5 = new Match(group.Teams[0], group.Teams[3]);
                            Match match6 = new Match(group.Teams[1], group.Teams[2]);
                            match5.SimulateMatch();
                            match6.SimulateMatch();
                            break;
                    }
                                                           
                }
            }
        }

        public void AddTeamsToPlaceGroups()
        {
            foreach (Group group in allGroups)
            {
                for (int i = 0; i < group.Teams.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            firstPlaceTeams.Add(group.Teams[i]);
                            break;
                        case 1:
                            secondPlaceTeams.Add(group.Teams[i]);
                            break;
                        case 2:
                            thirdPlaceTeams.Add(group.Teams[i]);
                            break;
                    }
                }

            }
        }

        public void SortTeamInGroup(List<Country> teams)
        {
            Dictionary<int, List<Country>> groupTeamsPerPoints = teams
           .GroupBy(t => t.Points).ToDictionary(g => g.Key, g => g.ToList());

            List<List<Country>> listOfThreeTeams = groupTeamsPerPoints.Values
           .Where(list => list.Count == 3)
           .ToList();

            List<List<Country>> listOfTwoTeams = groupTeamsPerPoints.Values
           .Where(list => list.Count == 2)
           .ToList();

            //Ukoliko sva 3 tima imaju isti broj bodova
            if (listOfThreeTeams.Count == 3)
            {

                Dictionary<int, List<Country>> groupTeamsPerPointDifference = teams
           .GroupBy(t => t.PointDifference).ToDictionary(g => g.Key, g => g.ToList());

                List<List<Country>> listOfThreeTeamsPointsDifferece = groupTeamsPerPointDifference.Values
           .Where(list => list.Count == 3)
           .ToList();

                List<List<Country>> listOfTwoTeamsPointsDifferece = groupTeamsPerPointDifference.Values
        .Where(list => list.Count == 2)
        .ToList();

                //Ukoliko sva 3 tima imaju istu medjusobnu razliku u poenima
                if (listOfThreeTeamsPointsDifferece.Count == 3)
                {
                    teams.Sort((x, y) => y.PointsScored.CompareTo(x.PointsScored));
                }
                // Ukoliko 2 tima imaju istu medjusobnu razliku u poenima
                else if (listOfTwoTeamsPointsDifferece.Count == 2)
                {
                    Country aloneTeam = new Country();
                    foreach (Country team in teams)
                    {
                        if (!listOfTwoTeamsPointsDifferece.First().Contains(team))
                        {
                            aloneTeam = team;
                        }

                    }

                    
                    teams.Clear();
                    listOfTwoTeamsPointsDifferece.First().Sort((x, y) => y.PointsScored.CompareTo(x.PointsScored));
                    //provera da li 2 tima, koji imaju istu razliku u poenima, imaju vecu razliku u poenima
                    //od treceg tima
                    if (listOfTwoTeamsPointsDifferece.First()[0].PointDifference > aloneTeam.PointDifference)
                    {
                        teams.Add(listOfTwoTeamsPointsDifferece.First()[0]);
                        teams.Add(listOfTwoTeamsPointsDifferece.First()[1]);
                        teams.Add(aloneTeam);
                    }
                    else
                    {
                        teams.Add(aloneTeam);
                        teams.Add(listOfTwoTeamsPointsDifferece.First()[0]);
                        teams.Add(listOfTwoTeamsPointsDifferece.First()[1]);
                    }

                }
                else
                {
                    teams.Sort((x, y) => y.PointDifference.CompareTo(x.PointDifference));
                }


            }
            //ukoliko 2 tima imaju isti broj poena
            else if (listOfTwoTeams.Count == 2)
            {
                Country aloneTeam = new Country();
                foreach (Country team in teams)
                {
                    if (!listOfTwoTeams.First().Contains(team))
                    {
                        aloneTeam = team;
                    }

                }
                teams.Clear();
                //provera da li 2 tima, koji imaju isti broj poenima, imaju veci broj poena
                //od treceg tima
                if (listOfTwoTeams.First()[0].Points > aloneTeam.Points)
                {

                    if (listOfTwoTeams.First()[0].PointDifference > listOfTwoTeams.First()[1].PointDifference)
                    {
                        teams.Add(listOfTwoTeams.First()[0]);
                        teams.Add(listOfTwoTeams.First()[1]);
                    }
                    else if (listOfTwoTeams.First()[1].PointDifference > listOfTwoTeams.First()[0].PointDifference)
                    {
                        teams.Add(listOfTwoTeams.First()[1]);
                        teams.Add(listOfTwoTeams.First()[0]);
                    }
                    else
                    {
                        listOfTwoTeams.First().Sort((x, y) => y.PointsScored.CompareTo(x.PointsScored));
                        teams.Add(listOfTwoTeams.First()[0]);
                        teams.Add(listOfTwoTeams.First()[1]);
                    }
                    teams.Add(aloneTeam);
                }
                else
                {
                    teams.Add(aloneTeam);
                    if (listOfTwoTeams.First()[0].PointDifference > listOfTwoTeams.First()[1].PointDifference)
                    {
                        teams.Add(listOfTwoTeams.First()[0]);
                        teams.Add(listOfTwoTeams.First()[1]);
                    }
                    else if (listOfTwoTeams.First()[1].PointDifference > listOfTwoTeams.First()[0].PointDifference)
                    {
                        teams.Add(listOfTwoTeams.First()[1]);
                        teams.Add(listOfTwoTeams.First()[0]);
                    }
                    else
                    {
                        listOfTwoTeams.First().Sort((x, y) => y.PointsScored.CompareTo(x.PointsScored));
                        teams.Add(listOfTwoTeams.First()[0]);
                        teams.Add(listOfTwoTeams.First()[1]);
                    }
                    teams.Add(aloneTeam);
                }

            }
            else
            {
                teams.Sort((x, y) => y.Points.CompareTo(x.Points));
            }
        }

        public void SortTeamsPerGroupRang()
        {
            allTeams.Sort((x, y) => y.GroupRang.CompareTo(x.GroupRang));
            for (int i = 0; i < firstPlaceTeams.Count; i++)
            {
                firstPlaceTeams[i].GroupRang = i + 1;
                allTeams.Add(firstPlaceTeams[i]);
            }

            for (int i = 0; i < secondPlaceTeams.Count; i++)
            {
                secondPlaceTeams[i].GroupRang = i + 4;
                allTeams.Add(secondPlaceTeams[i]);
            }

            for (int i = 0; i < thirdPlaceTeams.Count; i++)
            {
                thirdPlaceTeams[i].GroupRang = i + 7;
                allTeams.Add(thirdPlaceTeams[i]);
            }

        }

        public void CreatePots()
        {

            Pot potD = new Pot("Sesir D");
            Pot potE = new Pot("Sesir E");
            Pot potF = new Pot("Sesir F");
            Pot potG = new Pot("Sesir G");
            allPots.Add(potD);
            allPots.Add(potE);
            allPots.Add(potF);
            allPots.Add(potG);

            potD.Teams.Add(allTeams[0]);
            potD.Teams.Add(allTeams[1]);

            potE.Teams.Add(allTeams[2]);
            potE.Teams.Add(allTeams[3]);

            potF.Teams.Add(allTeams[4]);
            potF.Teams.Add(allTeams[5]);

            potG.Teams.Add(allTeams[6]);
            potG.Teams.Add(allTeams[7]);
        }
        public void PrintDrawPots()
        {
            AddTeamsToPlaceGroups();

            SortTeamInGroup(firstPlaceTeams);
            SortTeamInGroup(secondPlaceTeams);
            SortTeamInGroup(thirdPlaceTeams);

            SortTeamsPerGroupRang();

            CreatePots();
            Console.WriteLine("\nSesiri :");

            for (int i = 0; i < allPots.Count; i++)
            {
                Console.WriteLine(allPots[i].ToString() + "\n");
            }
        }

        public void QuarterFinals()
        {
            List<Country> potDTeams = new List<Country>(allPots[0].Teams); 
            List<Country> potETeams = new List<Country>(allPots[1].Teams); 
            List<Country> potFTeams = new List<Country>(allPots[2].Teams); 
            List<Country> potGTeams = new List<Country>(allPots[3].Teams); 

            List<Match> quarterFinalMatches = new List<Match>();
            Random random = new Random();

            
            List<(Country, Country)> possiblePairs1 = new List<(Country, Country)>();
            List<(Country, Country)> possiblePairs2 = new List<(Country, Country)>();


            if (potDTeams.Count > 1 && potGTeams.Count > 1)
            {
                possiblePairs1 = SetPossiblePairs(potDTeams, potGTeams);
                if (possiblePairs1.Count > 0)
                {
                    int randomIndex = random.Next(possiblePairs1.Count);
                    Country teamFromD = possiblePairs1[randomIndex].Item1;
                    Country teamFromG = possiblePairs1[randomIndex].Item2;

                    quarterFinalMatches.Add(new Match(teamFromD, teamFromG));
                    potGTeams.Remove(teamFromG); // Ukloni tim iz liste da ne bi bio ponovo uparen
                    potDTeams.Remove(teamFromD); // Ukloni tim iz liste da ne bi bio ponovo uparen
                }
                quarterFinalMatches.Add(new Match(potDTeams[0], potGTeams[0]));
            }

            
            if (potETeams.Count > 1 && potFTeams.Count > 1)
            {
                possiblePairs2 = SetPossiblePairs(potETeams, potFTeams);
                if (possiblePairs2.Count > 0)
                {
                    int randomIndex = random.Next(possiblePairs2.Count);
                    Country teamFromE = possiblePairs2[randomIndex].Item1;
                    Country teamFromF = possiblePairs2[randomIndex].Item2;

                    quarterFinalMatches.Add(new Match(teamFromE, teamFromF));
                    potFTeams.Remove(teamFromF); // Ukloni tim iz liste da ne bi bio ponovo uparen
                    potETeams.Remove(teamFromE); // Ukloni tim iz liste da ne bi bio ponovo uparen
                }
                quarterFinalMatches.Add(new Match(potETeams[0], potFTeams[0]));
            }

                      
            Console.WriteLine("\n Cetvrtfinale :");
            foreach (var match in quarterFinalMatches)
            {
                 match.SimulateMatch();
            }

            SemiFinals(quarterFinalMatches);
        }

        public List<(Country, Country)> SetPossiblePairs(List<Country> pot1, List<Country> pot2)
        {
            List<(Country, Country)> possiblePairs = new List<(Country, Country)>();
            foreach (Country team1 in pot1)
            {
                foreach (Country team2 in pot2)
                {
                    if (team1.Group != team2.Group)
                    {
                        possiblePairs.Add((team1, team2));
                    }
                }
            }
            return possiblePairs;
        }

        public void SemiFinals(List<Match> quarterFinalMatches)
        {
            List<Match> semiFinalMatches = new List<Match>();
            Random random = new Random();

            // Podeli parove cetvrtfinala u dva dela
            
            List<Match> matchesDG = quarterFinalMatches.Where(m => (allPots[0].Teams.Contains(m.Team1) && allPots[3].Teams.Contains(m.Team2)) ||
                                                                    (allPots[3].Teams.Contains(m.Team1) && allPots[0].Teams.Contains(m.Team2))).ToList();
            List<Match> matchesEF = quarterFinalMatches.Where(m => (allPots[2].Teams.Contains(m.Team1) && allPots[1].Teams.Contains(m.Team2)) ||
                                                                    (allPots[1].Teams.Contains(m.Team1) && allPots[2].Teams.Contains(m.Team2))).ToList();
            
            // Nasumicno ukrstanje parova          
            int randomIndexDG = random.Next(matchesDG.Count);
            Country matchDG1Winner = matchesDG[randomIndexDG].Winner;
            matchesDG.RemoveAt(randomIndexDG);
            Country matchDG2Winner = matchesDG[0].Winner;
            matchesDG.RemoveAt(0);

            int randomIndexEF = random.Next(matchesEF.Count);
            Country matchEF1Winner = matchesEF[randomIndexEF].Winner;
            matchesEF.RemoveAt(randomIndexEF);
            Country matchEF2Winner = matchesEF[0].Winner;
            matchesEF.RemoveAt(0);

            semiFinalMatches.Add(new Match(matchDG1Winner, matchEF1Winner));
            semiFinalMatches.Add(new Match(matchDG2Winner, matchEF2Winner));

            Console.WriteLine("\nPolufinale : ");
            foreach (var match in semiFinalMatches)
            {           
                match.SimulateMatch();
            }

            Final(semiFinalMatches);
        }

        public void Final(List<Match> semiFinalMatches) 
        {
            Country winnerTeam1 = semiFinalMatches[0].Winner;
            Country winnerTeam2 = semiFinalMatches[1].Winner;

            Match finalMatch = new Match(winnerTeam1, winnerTeam2);
            Console.WriteLine("\n Finale : ");
            finalMatch.SimulateMatch();
        }
    }
}
