using PixelMap;
namespace PixelMap.examples{
public class SpikeRunner
{
    private static Random rand = new Random();
        private static int[] GenerateCactusPatch(int traveled)
        {
            int[] chances = [15000 + 100*traveled, 15000 + 30*traveled, 15000+10*traveled, 15000, 12000+12*traveled, 6000+50*traveled, 3000+40*traveled, 800+30*traveled, 50*traveled];
            int[][] results = [new int[]{0}, new int[]{0,0}, new int[]{0,0,0}, new int[]{0,0,0,0,0}, new int[]{0,1,0}, new int[]{0,0,0,2,0}, new int[]{0,0,0,0,3,0}, new int[]{0,0,0,0,0,4,0,0}, new int[]{1}];
            int totalChance = 0;
            for (int i = 0; i < chances.Count(); i++)
            {
                totalChance += chances[i];
            }
            int currentSpot = 0;
            int realChance = rand.Next(totalChance+1);
            for (int i = 0; i < chances.Count(); i++)
            {
                currentSpot += chances[i];
                if (currentSpot >= realChance)
                {
                    return results[i];
                }
            }
            return new int[] {0};
        }
        public static void PlaySpikeRunner()
        {
            PixelMap map = new PixelMap(mapSizeY:10, mapSizeX:45, defaultColor:15);
            bool runCode = true;
            int current = 0;
            int height = 0;
            bool jumping = false;
            ConsoleKey? currentKey = null;
            List<int> cactusHeight= new List<int>(); //the height of cactuses along the path
            while (cactusHeight.Count < 12)
            {
                cactusHeight.Add(0);
            }
            while (runCode)
            {
                map.SetRectangleOutline(1,1,43,8,1,15,0);
                while (cactusHeight.Count < current + 45)
                {
                    foreach (int i in GenerateCactusPatch(current))
                    {
                        cactusHeight.Add(i);
                    }
                }
                if (Console.KeyAvailable)
                {
                    currentKey = Console.ReadKey().Key;
                }
                if (jumping)
                {
                    height += 1;
                    if (height == 5)
                    {
                        jumping = false;
                    }
                }
                else if (height > 0)
                {
                    height -= 1;
                }
                switch (currentKey)
                {
                    case ConsoleKey.Spacebar or ConsoleKey.UpArrow:
                        if (height == 0)
                        {
                            height = 1;
                            jumping = true;
                        }
                        currentKey = null;
                        break;
                    case ConsoleKey.DownArrow:
                        height = 0;
                        jumping = false;
                        currentKey = null;
                        break;
                }
                current += 1;
                if (height < cactusHeight[current])
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You loose!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Your final score is {current}");
                    break;
                }
                for (int i = 0; i < 43; i++)
                {
                    int g = cactusHeight[i+current];
                    map.SetRectangle(1+i,9-g,1,g,12);
                }
                map.SetPixel(1, 8-height, 10);
                map.DrawConsole();
                Thread.Sleep(250);
            }
        }
        
    }
}