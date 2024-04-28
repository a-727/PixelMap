namespace PixelMap
{
    public class PixelMap
    {
        private int[][] _map;
        public int DefaultColor;
        readonly private ConsoleColor[] _consoleMap = new ConsoleColor[]{ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.White};
        public PixelMap(int defaultColor = 1, int[][]? startingMap = null, int mapSizeX = 100, int mapSizeY = 50)
        {
            if (startingMap is null)
            {
                _map = new int[mapSizeY][];
                for (int i = 0; i < mapSizeY; i++)
                {
                    _map[i] = new int[mapSizeX];
                    for (int j = 0; j < mapSizeX; j++)
                    {
                        _map[i][j] = defaultColor;
                    }
                }
            }
            else
            {
                _map = startingMap;
            }
            DefaultColor = defaultColor;
        }
        public void ResetMap(int resetTo = -1)
        {
            if (resetTo is < 0 or >= 16)
            {
                resetTo = DefaultColor;
            }
            for (var index = 0; index < _map.Length; index++)
            {
                var t = _map[index];
                for (int j = 0; j < t.Length; j++)
                {
                    t[j] = resetTo;
                }
            }
        }
        public void SetPixel(int x, int y, int to)
        {
            _map[y][x] = to;
        }
        public void SetRectangle(int x, int y, int width, int height, int color)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                   SetPixel(x+i, y+j, color);
                }
            }
        }
        public void SetRectangleOutline(int x, int y, int width, int height, int borderSize, int color, int borderColor)
        {
            SetRectangle(x, y, width, height, color); //Main Rectangle
            SetRectangle(x-borderSize, y-borderSize, width+borderSize*2, borderSize, borderColor); //Top Border + top corners
            SetRectangle(x-borderSize, y+height, width+borderSize*2, borderSize, borderColor); //Bottom Border + bottom corners
            SetRectangle(x-borderSize, y, borderSize, height, borderColor); //Left Border
            SetRectangle(x+width, y, borderSize, height, borderColor);//Right Border
        }
        private void ConsoleDrawPixel(int color)
        {
            ConsoleColor c_color = _consoleMap[color];
            Console.ForegroundColor = c_color;
            Console.BackgroundColor = c_color;
            Console.Write("++");
        }
        private void ConsolePixelLine(int[] colors)
        {
            Console.WriteLine();
            foreach (int color in colors)
            {
                ConsoleDrawPixel(color);
            }
        }
        public void DrawConsole()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = _consoleMap[DefaultColor];
            Console.Clear();
            foreach (int[] line in _map)
            {
                ConsolePixelLine(line);
            }
        }
    }
}