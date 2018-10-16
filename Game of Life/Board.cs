using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game_of_Life {
    /// <summary>
    /// A board for displaying Conway's Game of Life
    /// </summary>
    public class Board {
        private int iteration = 0;
        private Texture2D whiteSquare, blackSquare;
        private readonly Random random = new Random();
        private int squareSize = 10;
        private bool[][] lastTick;
        /// <summary>
        /// The matrix of squares being displayed.
        /// </summary>
        public bool[][] Squares;
        /// <summary>
        /// The dimensions of the matrix
        /// </summary>
        public int Width, Height;
        /// <summary>
        /// Determines if the game should keep updating
        /// </summary>
        public bool IsPaused = true;
        /// <summary>
        /// Initializes the textures and the matrix
        /// </summary>
        /// <param name="device">The Graphics device of the game</param>
        public Board(GraphicsDevice device) {
            //White texture
            whiteSquare = new Texture2D(device, squareSize, squareSize);
            Color[] data = new Color[squareSize * squareSize];
            for (int i = 0; i < data.Length; i++) {
                data[i] = Color.White;
            }
            whiteSquare.SetData(data);
            //Black texture
            blackSquare = new Texture2D(device, squareSize, squareSize);
            data = new Color[squareSize * squareSize];
            for (int i = 0; i < data.Length; i++) {
                data[i] = Color.Black;
            }
            blackSquare.SetData(data);
            //Finding size of the matrix
            Height = device.Viewport.Height / squareSize;
            Width = device.Viewport.Width / squareSize;
            //Initializing the matrices
            Squares = new bool[device.Viewport.Height / squareSize][];
            lastTick = new bool[device.Viewport.Height / squareSize][];
            for (int i = 0; i < Squares.Length; i++) {
                Squares[i] = new bool[Width];
                lastTick[i] = new bool[Width];
                for (int j = 0; j < Width; j++) {
                    lastTick[i][j] = false;
                    Squares[i][j] = false;
                }
            }
        }
        /// <summary>
        /// Updates the game and handles button/mouse clicks
        /// </summary>
        /// <param name="viewport">The viewport of the game</param>
        public void Update(Viewport viewport) {
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
                IsPaused = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                IsPaused = false;
            HandleClick(viewport);
            if (!IsPaused && iteration++ == 3) {
                CalculateNextTick();
                iteration = 0;
            }
        }
        /// <summary>
        /// Calculates the next tick based on the rules of the Game of Life
        /// </summary>
        private void CalculateNextTick() {
            for (int i = 0; i < Squares.Length; i++) {
                for (int j = 0; j < Squares[0].Length; j++) {
                    int number = GetNumberOfNeighbors(j, i);
                    if (lastTick[i][j]) {
                        if (number < 2)
                            Squares[i][j] = false;
                        else if (number > 3)
                            Squares[i][j] = false;
                    }
                    else if (number == 3)
                        Squares[i][j] = true;
                }
            }
            for (int i = 0; i < Height; i++)
                Squares[i].CopyTo(lastTick[i], 0);
        }
        /// <summary>
        /// Counts the number of on squares surrounding the coordinates
        /// </summary>
        /// <param name="x">The x coordinate of the square</param>
        /// <param name="y">The y coordinate of the square</param>
        /// <returns>The number of neighbors surronding the square</returns>
        private int GetNumberOfNeighbors(int x, int y) {
            int total = 0;
            if (y < 0 || y > Height)
                return total;
            if (x < 0 || x > Width)
                return total;
            //Horizontal
            if (x > 1) {
                if (lastTick[y][x - 1])
                    total++;
            }
            if (x < Width - 1) {
                if (lastTick[y][x + 1])
                    total++;
            }
            //Vertical
            if (y > 1) {
                if (lastTick[y - 1][x])
                    total++;
            }
            if (y < Height - 1) {
                if (lastTick[y + 1][x])
                    total++;
            }
            //Diagonal
            if (x > 1) {
                if (y > 1) {
                    if (lastTick[y - 1][x - 1])
                        total++;
                }
                if (y < Height - 1) {
                    if (lastTick[y + 1][x - 1])
                        total++;
                }
            }
            if (x < Width - 1) {
                if (y > 1) {
                    if (lastTick[y - 1][x + 1])
                        total++;
                }
                if (y < Height - 1) {
                    if (lastTick[y + 1][x + 1])
                        total++;
                }
            }
            return total;
        }
        /// <summary>
        /// Determines if the Mouse is inside of the viewport
        /// </summary>
        /// <param name="viewport">The viewport of the game</param>
        /// <returns>True if the mouse is within the viewport</returns>
        private bool IsMouseInWindow(Viewport viewport) {
            Point pos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            return viewport.Bounds.Contains(pos);
        }
        /// <summary>
        /// Assigns left click to turning the squares on, right click to turning them off
        /// </summary>
        /// <param name="viewport">The viewport of the game</param>
        private void HandleClick(Viewport viewport) {
            MouseState state = Mouse.GetState();
            if (!IsMouseInWindow(viewport))
                return;
            int x = state.X / squareSize;
            int y = state.Y / squareSize;
            if (state.LeftButton == ButtonState.Pressed) {
                lastTick[y][x] = true;
                Squares[y][x] = true;
            }
            if (state.RightButton == ButtonState.Pressed) {
                lastTick[y][x] = false;
                Squares[y][x] = false;
            }
        }
        /// <summary>
        /// Saves board to a file 
        /// </summary>
        /// <param name="fileName">The name of the file to save to</param>
        public void Save(string fileName) {
            string[] lines = new string[Height + 1];
            lines[0] = string.Format("#squareSize: {0}, #width: {1}, #height: {2}", squareSize, Width, Height);
            int fileIndex = 1;
            for (int i = 0; i < Height; i++) {
                string line = "";
                for (int j = 0; j < Width; j++) {
                    line += Squares[i][j] + ", ";
                }
                lines[fileIndex] = line;
            }
            System.IO.File.WriteAllLines(fileName, lines);
        }
        /// <summary>
        /// Draws the Board onto the screen
        /// </summary>
        /// <param name="gameTime">The gameTime used during draw</param>
        /// <param name="device">The graphicsDevice of the game</param>
        /// <param name="spriteBatch">The spriteBatch of the game</param>
        public void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch) {
            int x = 0;
            int y = 0;
            for (int i = 0; i < Squares.Length; i++) {
                for (int j = 0; j < Squares[0].Length; j++) {
                    Vector2 position = new Vector2(x, y);
                    if (Squares[i][j]) {
                        spriteBatch.Draw(whiteSquare, position, Color.White);
                    }
                    else {
                        spriteBatch.Draw(blackSquare, position, Color.Black);
                    }
                    x += squareSize;
                }
                x = 0;
                y += squareSize;
            }
        }
    }
}
