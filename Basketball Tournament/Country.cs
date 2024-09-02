using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketball_Tournament
{
    public class Country
    {
        public string Team { get; set; }
        public string ISOCode { get; set; }
        public int FIBARanking { get; set; }

        public List<Match> Matches { get; set; } = new List<Match>();

        public int Points { get; set; }
        public int PointsScored { get; set; }
        public int PointsReceived { get; set; }
        public int PointDifference => PointsScored - PointsReceived;
        public int PointDifference3Teams { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double Form { get; set; }
        public int GroupRang { get; set; }

        public int WinRating { get; set; }

        public int AllWins { get; set; }
        public int AllLoses { get; set; }

        public Group Group { get; set; }

        public List<FriendlyMatch> FriendlyMatches { get; set; } = new List<FriendlyMatch>();

        public void AddWin()
        {
            Wins++;
            AllWins++;
            Points += 2;
        }

        public void AddLoss()
        {
            Losses++;
            AllLoses++;
            Points += 1;
        }
        public void SetPointsDifferenceAgainst(Country opponent)
        {
            var match = Matches.FirstOrDefault(m => (m.Team1 == this && m.Team2 == opponent) || (m.Team1 == opponent && m.Team2 == this));

            if (match == null)
            {
            }

            int thisTeamScore, opponentScore;
            if (match.Team1 == this)
            {
                thisTeamScore = match.Team1Score;
                opponentScore = match.Team2Score;
            }
            else
            {
                thisTeamScore = match.Team2Score;
                opponentScore = match.Team1Score;
            }
            PointDifference3Teams += thisTeamScore - opponentScore;
        }

        public Country IsMatchWinner(Country opponent)
        {
            var match = Matches.FirstOrDefault(m => (m.Team1 == this && m.Team2 == opponent) || (m.Team1 == opponent && m.Team2 == this));
            if (match != null)
            {
                return match.Winner;
            }
            return null;
        }

        public void FriendlyMatch()
        {
            foreach (var match in FriendlyMatches)
            {
                var resultParts = match.Result.Split('-');
                int teamScore = int.Parse(resultParts[0]);
                int opponentScore = int.Parse(resultParts[1]);

                if (teamScore > opponentScore)
                {
                    AllWins++;
                }
                else
                {
                    AllLoses++;
                }
            }
        }
    }
}
