public class LeagueTable
{
    static void Main(string[] args)
    {
        readFiles();
    }

    public static void readFiles()
    {
        // Her indlæser jeg setup.csv
        List<LeagueTable> leagueTables = new List<LeagueTable>();
        using (var reader = new StreamReader("setup.csv"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                leagueTables.Add(new LeagueTable
                {
                    Name = values[0],
                    ChampionsLeaguePositions = int.Parse(values[1]),
                    EuropaLeaguePositions = int.Parse(values[2]),
                    ConferenceLeaguePositions = int.Parse(values[3]),
                    PromotionPositions = int.Parse(values[4]),
                    RelegationPositions = int.Parse(values[5])
                });
            }
        }

        // Her indlæser jeg teams.csv
        List<Team> teams = new List<Team>();
        using (StreamReader reader = new StreamReader("teams.csv"))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                if (values.Length >= 3)
                {
                    int specialRanking;
                    if (int.TryParse(values[2], out specialRanking))
                    {
                        teams.Add(new Team
                        {
                            Abbreviation = values[0],
                            Name = values[1],
                            SpecialRanking = values[2]
                        });
                    }
                }
            }
        }
    }


    public string Name { get; set; }

    public int RelegationPositions { get; set; }

    public int PromotionPositions { get; set; }

    public int ConferenceLeaguePositions { get; set; }

    public int EuropaLeaguePositions { get; set; }

    public int ChampionsLeaguePositions { get; set; }
}


public enum Result
{
    Win,
    Draw,
    Loss
}

public class Team
{
    public string Abbreviation { get; set; }
    public string Name { get; set; }
    public string SpecialRanking { get; set; }
    public int GamesPlayed { get; private set; }
    public int GamesWon { get; private set; }
    public int GamesDrawn { get; private set; }
    public int GamesLost { get; private set; }
    public int GoalsFor { get; private set; }
    public int GoalsAgainst { get; private set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points => GamesWon * 3 + GamesDrawn;
    public List<Result> CurrentStreak { get; private set; }
    public List<string> LatestResults { get; set; }

    public string GetWinningStreakString()
    {
        var streak = "";
        var maxStreakLength = Math.Min(LatestResults.Count, 5);
        for (int i = 0; i < maxStreakLength; i++)
        {
            var result = LatestResults[i];
            if (result == "W")
            {
                streak += "W|";
            }
            else if (result == "D")
            {
                streak += "D|";
            }
            else
            {
                streak += "L|";
            }
        }

        if (streak == "")
        {
            streak = "-";
        }

        return streak.TrimEnd('|');
    }

    public class LeagueSetup
    {
        public string LeagueName { get; set; }
        public int PositionsToChampionsLeague { get; set; }
        public int PositionsToEuropaLeague { get; set; }
        public int PositionsToConferenceLeague { get; set; }
        public int PositionsToUpperLeague { get; set; }
        public int PositionsToRelegation { get; set; }
    }

    public class LeagueTable
    {
        private List<Team> teams;
        private LeagueSetup leagueSetup;

        public LeagueTable(List<Team> teams, LeagueSetup leagueSetup)
        {
            this.teams = teams;
            this.leagueSetup = leagueSetup;
        }

        public void PrintTable()
        {
            // Calculate and assign all necessary values for each team
            foreach (var team in teams)
            {
                team.Points = team.Wins * 3 + team.Draws;
                team.GoalDifference = team.GoalsFor - team.GoalsAgainst;
                team.LatestResults.Reverse(); // Reverse to get the latest results first
                team.LatestResults = team.LatestResults.Take(5).ToList(); // Only keep the last 5 results
            }

            // Sort the teams based on points and goal difference
            teams = teams.OrderByDescending(t => t.Points).ThenByDescending(t => t.GoalDifference).ToList();

            // Assign positions to the teams
            int position = 1;
            for (int i = 0; i < teams.Count; i++)
            {
                if (i > 0 && teams[i].Points != teams[i - 1].Points ||
                    teams[i].GoalDifference != teams[i - 1].GoalDifference)
                {
                    position = i + 1;
                }

                teams[i].Position = position;
            }

            // Print the table header
            Console.WriteLine("{0,-4}{1,-6}{2,-30}{3,-4}{4,-4}{5,-4}{6,-4}{7,-4}{8,-4}{9,-4}{10,-6}", "Pos", "Rank",
                "Club", "P", "W", "D", "L", "GF", "GA", "GD", "Pts");

            // Print the table body
            foreach (var team in teams)
            {
                string positionString = team.Position.ToString();
                if (team.Position > 1 && teams[team.Position - 2].Points == team.Points &&
                    teams[team.Position - 2].GoalDifference == team.GoalDifference)
                {
                    positionString = "-";
                }

                string specialRankingString = "";
                switch (team.SpecialRanking)
                {
                    case "W":
                        specialRankingString = "(C)";
                        break;
                    case "C":
                        specialRankingString = "(C)";
                        break;
                    case "P":
                        specialRankingString = "(P)";
                        break;
                    case "R":
                        specialRankingString = "(R)";
                        break;
                }

                Console.WriteLine("{0,-4}{1,-6}{2,-30}{3,-4}{4,-4}{5,-4}{6,-4}{7,-4}{8,-4}{9,-4}{10,-6}",
                    positionString,
                    specialRankingString,
                    team.FullName,
                    team.GamesPlayed,
                    team.Wins,
                    team.Draws,
                    team.Losses,
                    team.GoalsFor,
                    team.GoalsAgainst,
                    team.GoalDifference,
                    team.Points);
            }
        }
    }

    public Team(string abbreviation, string name, string specialRanking)
    {
        Abbreviation = abbreviation;
        Name = name;
        SpecialRanking = specialRanking;
        GamesPlayed = 0;
        GamesWon = 0;
        GamesDrawn = 0;
        GamesLost = 0;
        GoalsFor = 0;
        GoalsAgainst = 0;
        CurrentStreak = new List<Result>();
    }

    public Team()
    {
        throw new NotImplementedException();
    }


    public void PlayMatch(int goalsFor, int goalsAgainst)
    {
        GamesPlayed++;
        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;

        if (goalsFor > goalsAgainst)
        {
            GamesWon++;
            CurrentStreak.Add(Result.Win);
        }
        else if (goalsFor == goalsAgainst)
        {
            GamesDrawn++;
            CurrentStreak.Add(Result.Draw);
        }
        else
        {
            GamesLost++;
            CurrentStreak.Add(Result.Loss);
        }
    }
}