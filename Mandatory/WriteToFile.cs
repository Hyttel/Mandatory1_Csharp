/*
class WriteToFile
{
    static void Main(string[] args)
    {
        string[,] teams = {
            {"FCN", "FC Nordsjælland", ""},
            {"FCK", "FC København", "W"},
            {"VFF", "Viborg FF", "P"},
            {"AGF", "AGF", ""},
            {"RFC", "Randers FC", "C"},
            {"BIF", "Brøndby IF", ""},
            {"SIF", "Silkeborg IF", "R"},
            {"FCM", "FC Midtjylland", ""},
            {"OB", "OB", ""},
            {"ACH", "AC Horsens", ""},
            {"LFC", "Lyngby", ""},
            {"AaB", "AaB", ""}
        };
        
        string folderPath = @"C:\Users\Mikkel\RiderProjects\Mandatory\Mandatory\WriteToFiles";
        string teamFilePath = Path.Combine(folderPath, "teams.csv");
        
        using (StreamWriter sw = new StreamWriter(teamFilePath))
        {
             for (int i = 0; i < teams.GetLength(0); i++)
            {
                sw.WriteLine($"{teams[i, 0]},{teams[i, 1]},{teams[i, 2]}");
            }
        }

        
        string setupFilePath = Path.Combine(folderPath, "setup.csv");

        using (StreamWriter writer = new StreamWriter(setupFilePath, false))
        {
            writer.WriteLine("Superligaen,1,2,3,0,2");
            writer.WriteLine("NordicBetLigaen,0,1,2,2,2");
        }
        
        Console.WriteLine("The individual team information was written to teams.csv");
        Console.WriteLine("The league setup information was written to setup.csv");
        
    }
} */

