public class Stuff
{
    public string GetFormattedClubInfo(List<Club> clubs)
    {
        // Sort the clubs according to the specified order
        clubs = clubs.OrderByDescending(c => c.Points)
            .ThenByDescending(c => c.GoalDifference)
            .ThenByDescending(c => c.GoalsFor)
            .ThenBy(c => c.GoalsAgainst)
            .ThenBy(c => c.FullClubName)
            .ToList();

        // Create a formatted string for each club
        var clubStrings = new List<string>();
        for (var i = 0; i < clubs.Count; i++)
        {
            var club = clubs[i];

            // Get the current winning streak, up to the last 5 games
            var streak = "";
            for (var j = Math.Max(0, club.LastFiveGames.Count - 5); j < club.LastFiveGames.Count; j++)
            {
                var result = club.LastFiveGames[j];
                streak += result switch
                {
                    Club.GameResult.Win => "W|",
                    Club.GameResult.Draw => "D|",
                    Club.GameResult.Loss => "L|",
                    _ => "-"
                };
            }

            if (streak.EndsWith("|")) // remove the last separator if present
                streak = streak.Substring(0, streak.Length - 1);

            // Create the formatted string for the club
            var position = i + 1;
            var specialMarking = "";
            if (position <= 6) // top 6 clubs get special markings
                specialMarking = "*";
            else if (position >= clubs.Count - 2) // bottom 2 clubs get special markings
                specialMarking = "#";
            var clubString =
                $"{position,-2} {specialMarking,-1} {club.FullClubName,-20} {club.GamesPlayed,-2} {club.GamesWon,-2} {club.GamesDrawn,-2} {club.GamesLost,-2} {club.GoalsFor,-2} {club.GoalsAgainst,-2} {club.GoalDifference,-2} {club.Points,-2} {streak,-5}";

            clubStrings.Add(clubString);
        }

        Console.WriteLine(clubs);

        // Join the club strings together with newlines
        return string.Join(Environment.NewLine, clubStrings);
    }

    public class Club
    {
        public enum GameResult
        {
            Win,
            Draw,
            Loss
        }

        public string FullClubName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesDrawn { get; set; }
        public int GamesLost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public int Points => GamesWon * 3 + GamesDrawn;
        public List<GameResult> LastFiveGames { get; set; }
    }
}