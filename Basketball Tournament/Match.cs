using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Basketball_Tournament
{
    public class Match
    {
        public Country Team1 { get; set; }
        public Country Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public Country Winner { get; set; }
        public Country Loser { get; set; }

        private static Random rand = new Random();

        public Match() { }
        public Match(Country team1, Country team2)
        {
            Team1 = team1;
            Team2 = team2;

            Team1.Matches.Add(this);
            Team2.Matches.Add(this);
        }

        public void SimulateMatch()
        {
            double probabilityTeam1Wins = CalculateWinProbability(Team1.FIBARanking, Team2.FIBARanking);
            double probabilityTeam2Wins = 1 - probabilityTeam1Wins;
            do
            {
                Team1Score = (int)(rand.Next(70, 101) + probabilityTeam1Wins * 10);
                Team2Score = (int)(rand.Next(70, 101) + probabilityTeam2Wins * 10);

                if (Team1Score > Team2Score)
                {
                    Winner = Team1;
                    Loser = Team2;
                    Team1.AddWin();
                    Team2.AddLoss();
                }
                else if (Team1Score < Team2Score)
                {
                    Winner = Team2;
                    Loser = Team1;
                    Team2.AddWin();
                    Team1.AddLoss();
                }
            } while (Team1Score == Team2Score);
            Team1.PointsScored += Team1Score;
            Team1.PointsReceived += Team2Score;
            Team2.PointsScored += Team2Score;
            Team2.PointsReceived += Team1Score;
            Console.WriteLine($"        {Team1.Team} - {Team2.Team} ({Team1Score}:{Team2Score})");
        }

        public void CreateMatch(int scoreTeam1, int scoreTeam2)
        {
            Team1Score = scoreTeam1;
            Team2Score = scoreTeam2;

            if (Team1Score > Team2Score)
            {
                Winner = Team1;
                Team1.AddWin();
                Team2.AddLoss();
            }
            else if (Team1Score < Team2Score)
            {
                Winner = Team2;
                Team2.AddWin();
                Team1.AddLoss();
            }
            Team1.PointsScored += Team1Score;
            Team1.PointsReceived += Team2Score;
            Team2.PointsScored += Team2Score;
            Team2.PointsReceived += Team1Score;
        }

        private double CalculateWinProbability(int rank1, int rank2)
        {
            double invertedRank1 = 1.0 / rank1;
            double invertedRank2 = 1.0 / rank2;
            return invertedRank1 / (invertedRank1 + invertedRank2);
        }
    }
}
