using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelMap
{
    public class PixelMap
    {
        private int[][] _map;
        private int _defaultColor;
        private PmGame? game;
        private ConsoleColor[] _consoleMap = new ConsoleColor[] {
            ConsoleColor.Black, 
            ConsoleColor.DarkBlue, 
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkMagenta, 
            ConsoleColor.DarkYellow, 
            ConsoleColor.Gray, 
            ConsoleColor.DarkGray,
            ConsoleColor.Blue, 
            ConsoleColor.Green, 
            ConsoleColor.Cyan, 
            ConsoleColor.Red, 
            ConsoleColor.Magenta,
            ConsoleColor.Yellow, 
            ConsoleColor.White,
            ConsoleColor.Red,
            ConsoleColor.DarkMagenta,
        };
        private Color[] _monoMap = new Color[] {
            Color.Black,
            Color.DarkBlue,
            Color.DarkGreen,
            Color.DarkCyan,
            Color.DarkRed,
            Color.DarkMagenta,
            Color.DarkGoldenrod,
            Color.Gray,
            Color.DarkGray,
            Color.Blue,
            Color.Green,
            Color.Cyan,
            Color.Red,
            Color.Magenta,
            Color.Yellow,
            Color.White,
            Color.Orange,
            Color.Purple
        };
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
            _defaultColor = defaultColor;
        }

        public void SetupMono()
        {
            game = new PmGame();
        }
        
        public void ResetMap(int resetTo = -1)
        {
            if (resetTo is < 0 or >= 16)
            {
                resetTo = _defaultColor;
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
                    SetPixel(x + i, y + j, color);
                }
            }
        }

        public void ChanceDefaultColor(int changeTo)
        {
            if (changeTo < _consoleMap.Length)
            {
                _defaultColor = changeTo;
            }
        }
        
        public void SetRectangleOutline(int x, int y, int width, int height, int borderSize, int color, int borderColor)
        {
            SetRectangle(x, y, width, height, color); //Main Rectangle
            SetRectangle(x - borderSize, y - borderSize, width + borderSize * 2, borderSize,
                borderColor); //Top Border + top corners
            SetRectangle(x - borderSize, y + height, width + borderSize * 2, borderSize,
                borderColor); //Bottom Border + bottom corners
            SetRectangle(x - borderSize, y, borderSize, height, borderColor); //Left Border
            SetRectangle(x + width, y, borderSize, height, borderColor); //Right Border
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
            Console.BackgroundColor = _consoleMap[_defaultColor];
            Console.Clear();
            foreach (int[] line in _map)
            {
                ConsolePixelLine(line);
            }
        }

        public void DrawMono()
        {
            game.DrawFullMap(_map, _defaultColor, _monoMap);
        }

        public KeyboardState MonoKeyboard()
        {
            return game.GetKeyboardState();
        }

        public void QuitMono()
        {
            game.QuitGame();
            game = null;
        }
    }

    public class PmGameUtilities
    {
        public static string SelectMenu(string[]? options = null, string prompt = "Please select an option:",
            ConsoleColor highlightOption =
                ConsoleColor.Blue) //Use arrow keys to select an option from a menu. Console only.
        {
            if (options is null)
            {
                options = new string[] { "Yes", "No" };
            }

            int pos = 0;
            ConsoleColor defaultBackgroundColor = Console.BackgroundColor;
            ConsoleColor defaultForegroundColor = Console.ForegroundColor;
            bool selectedOption = false;
            while (!selectedOption)
            {
                Console.BackgroundColor = defaultBackgroundColor;
                Console.ForegroundColor = defaultForegroundColor;
                Console.Clear();
                Console.WriteLine(prompt);
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == pos)
                    {
                        Console.BackgroundColor = highlightOption;
                        Console.WriteLine($" > {options[i]}");
                    }
                    else
                    {
                        Console.BackgroundColor = defaultBackgroundColor;
                        Console.WriteLine($" • {options[i]}");
                    }
                }

                Console.BackgroundColor = defaultBackgroundColor;
                Console.WriteLine("Use arrow keys to move. Press space to select.");
                ConsoleKey currentKey = Console.ReadKey().Key;
                switch (currentKey)
                {
                    case ConsoleKey.DownArrow:
                        pos++;
                        if (pos >= options.Length)
                        {
                            pos = 0;
                        }

                        break;
                    case ConsoleKey.UpArrow:
                        pos--;
                        if (pos < 0)
                        {
                            pos = options.Length - 1;
                        }

                        break;
                    case ConsoleKey.Spacebar:
                        selectedOption = true;
                        break;
                }
            }

            return options[pos];
        }
    }

    public class PmGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect basicEffect;
        public PmGame(bool showMouse = true)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = showMouse;
        }
        public void DrawRectangle(float x, float y, float width, float height, Color color) //x and y are positions for the upper-left hand corner.
        {
            VertexPositionColor[] vertices_a = new VertexPositionColor[3];
            VertexPositionColor[] vertices_b = new VertexPositionColor[3];
            vertices_a[0] = new VertexPositionColor(new Vector3(x,y, 0), color);
            vertices_a[1] = new VertexPositionColor(new Vector3(x + width, y, 0), color);
            vertices_a[2] = new VertexPositionColor(new Vector3(x, y + height, 0), color);
            vertices_b[0] = new VertexPositionColor(new Vector3(x + width,y +height, 0), color);
            vertices_b[1] = new VertexPositionColor(new Vector3(x, y + height, 0), color);
            vertices_b[2] = new VertexPositionColor(new Vector3(x+width, y, 0), color);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices_a, 0, 1);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices_b, 0, 1);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            base.Update(gameTime);
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
        }

        public int PixelSize(int x_total, int y_total)
        {
            int a = Window.ClientBounds.Width / x_total;
            int b = Window.ClientBounds.Width / y_total;
            if (a < b)
            {
                return a;
            }
            return b;
        }

        private void DrawMapRow(int[] row, Color[] int_to_color, int pixelSize, int y, int x_offset)
        {
            for (int i = 0; i < row.Length; i++)
            {
                DrawRectangle(x_offset+i*pixelSize, y, pixelSize,pixelSize, int_to_color[row[i]]);
            }
        }
        
        public void DrawFullMap(int[][] map, int defaultColor, Color[] int_to_color)
        {
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
            GraphicsDevice.Clear(int_to_color[defaultColor]);
            int pixelSize = PixelSize(map[0].Length, map.Length);
            int x_offset = (int) Math.Floor((double) (Window.ClientBounds.Width-(map[0].Length*pixelSize))/2);
            int y_offset = (int) Math.Floor((double) (Window.ClientBounds.Height-(map.Length*pixelSize))/2);
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawRectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height,int_to_color[defaultColor]);
                for (int i = 0; i < map.Length; i++)
                {
                    DrawMapRow(map[i], int_to_color, pixelSize, y_offset+i*pixelSize, x_offset);
                }
            }
        }

        public KeyboardState GetKeyboardState()
        {
            return Keyboard.GetState();
        }

        public void QuitGame()
        {
            Exit();
        }
    }
    
}