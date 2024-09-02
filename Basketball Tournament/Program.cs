using Basketball_Tournament;
using Newtonsoft.Json;


string json = File.ReadAllText("F:\\project c#\\Basketball Tournament\\Basketball Tournament\\groups.json");
var groups = JsonConvert.DeserializeObject<Dictionary<string, List<Country>>>(json);

//test1
Tournament tournament = new Tournament();

Group groupA = new Group { Teams = groups["A"], Name = "A" };
Group groupB = new Group { Teams = groups["B"], Name = "B" };
Group groupC = new Group { Teams = groups["C"], Name = "C" };

tournament.AddGroup(groupA);
tournament.AddGroup(groupB);
tournament.AddGroup(groupC);


tournament.PrintRoundMatches(3);

groupA.PrintGroupRanking();
groupB.PrintGroupRanking();
groupC.PrintGroupRanking();

tournament.PrintDrawPots();

tournament.QuarterFinals();
























