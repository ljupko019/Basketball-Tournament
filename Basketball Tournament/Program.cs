using Basketball_Tournament;
using Newtonsoft.Json;


string jsonGroups = File.ReadAllText("F:\\project c#\\Basketball Tournament\\Basketball Tournament\\groups.json");
var groups = JsonConvert.DeserializeObject<Dictionary<string, List<Country>>>(jsonGroups);

//test1
Tournament tournament = new Tournament();

Group groupA = new Group { Teams = groups["A"], Name = "A" };
Group groupB = new Group { Teams = groups["B"], Name = "B" };
Group groupC = new Group { Teams = groups["C"], Name = "C" };

string jsonExibitions = "F:\\project c#\\Basketball Tournament\\Basketball Tournament\\exibitions.json";
string json2 = File.ReadAllText(jsonExibitions);

var matchesData = JsonConvert.DeserializeObject<Dictionary<string, List<FriendlyMatch>>>(json2);


void AddFriendlyMatchesToTeams(Group group)
{
    foreach (var team in group.Teams)
    {
        // Proveravamo da li u `matchesData` postoji prijateljski meč za ovaj tim
        if (matchesData.ContainsKey(team.ISOCode))
        {
            // Dodajemo mečeve timu
            team.FriendlyMatches = matchesData[team.ISOCode];
        }
    }
}
AddFriendlyMatchesToTeams(groupA);
AddFriendlyMatchesToTeams(groupB);
AddFriendlyMatchesToTeams(groupC);
    foreach (Country team in groupA.Teams)
    {
        team.FriendlyMatch();
    }

    foreach (var team in groupB.Teams)
    {
        team.FriendlyMatch();
    }

    foreach (var team in groupC.Teams)
    {
        team.FriendlyMatch();
    }


tournament.AddGroup(groupA);
tournament.AddGroup(groupB);
tournament.AddGroup(groupC);


tournament.PrintRoundMatches(3);

groupA.PrintGroupRanking();
groupB.PrintGroupRanking();
groupC.PrintGroupRanking();

tournament.PrintDrawPots();

tournament.QuarterFinals();
























