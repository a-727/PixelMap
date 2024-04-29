namespace PixelMap.examples;

public class Startup
{
    static void Main(string[] args)
    {
        Console.WriteLine("Loading...");
        bool runCode = true;
        while (runCode)
        {
            ConsoleColor defaultColor = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = defaultColor;
            string input = PmGameUtilities.SelectMenu(new string[] {"Play Example Game: SpikeRunner",  "Exit PixelMap Demo"},"Please select an option: ", ConsoleColor.Green);
            if (input == "Play Example Game: SpikeRunner")
            {
                SpikeRunner.PlaySpikeRunner();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.BackgroundColor = defaultColor;
                Console.WriteLine("You can build a game like this using PixelMap.");
                Thread.Sleep(500);
            } 
            else if (input == "Exit PixelMap Demo")
            {
                Console.WriteLine("Exiting...");
                runCode = false;
            }
            else
            {
                Console.WriteLine("Invalid input. I'm not sure how you managed to do that. Maybe we offered more options than we should?");
            }
            Console.WriteLine("Press any key to continue:");
            Console.ReadKey();
        }
    }
}