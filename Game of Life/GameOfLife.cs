using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game_of_Life {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameOfLife : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private int iteration = 0;
        private Texture2D whiteSquare, blackSquare;
        private readonly Random random = new Random();
        private int squareSize = 10;
        private bool[][] lastTick;

        public bool[][] Squares;
        public int Width, Height;
        public bool IsPaused = true;

        public GameOfLife() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            whiteSquare = new Texture2D(graphics.GraphicsDevice, squareSize, squareSize);
            Color[] data = new Color[squareSize * squareSize];
            for (int i = 0; i < data.Length; i++) {
                data[i] = Color.White;
            }
            whiteSquare.SetData(data);

            blackSquare = new Texture2D(graphics.GraphicsDevice, squareSize, squareSize);
            data = new Color[squareSize * squareSize];
            for (int i = 0; i < data.Length; i++) {
                data[i] = Color.Black;
            }
            blackSquare.SetData(data);
            Height = graphics.GraphicsDevice.Viewport.Height / squareSize;
            Width = graphics.GraphicsDevice.Viewport.Width / squareSize;
            Squares = new bool[graphics.GraphicsDevice.Viewport.Height / squareSize][];
            lastTick = new bool[graphics.GraphicsDevice.Viewport.Height / squareSize][];
            
            for (int i = 0; i < Squares.Length; i++) {
                Squares[i] = new bool[Width];
                lastTick[i] = new bool[Width];
                for(int j = 0; j < Width; j++) {
                    lastTick[i][j] = false;
                }
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
                IsPaused = true;    
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    IsPaused = false;
            HandleClick();
            if (!IsPaused && iteration++ == 3) {
                CalculateNextTick();
                iteration = 0;
            }
            base.Update(gameTime);
        }
        private void CalculateNextTick() {
            for(int i = 0; i < Squares.Length; i++) {
                for(int j = 0; j < Squares[0].Length; j++) {
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
            for(int i = 0; i < Height; i++)
                Squares[i].CopyTo(lastTick[i], 0);
        }
        private int GetNumberOfNeighbors(int x, int y) {
            int total = 0;
            if (y < 0 || y > Height)
                return total;
            if (x < 0 || x > Width)
                return total;
            //Horizontal
            if(x > 1) {
                if (lastTick[y][x - 1])
                    total++;
            }
            if(x < Width - 1) {
                if (lastTick[y][x + 1])
                    total++;
            }
            //Vertical
            if(y > 1) {
                if (lastTick[y - 1][x])
                    total++;
            }
            if(y < Height - 1) {
                if (lastTick[y + 1][x])
                    total++;
            }
            //Diagonal
            if(x > 1) {
                if(y > 1) {
                    if (lastTick[y - 1][x - 1])
                        total++;
                }
                if(y < Height - 1) {
                    if (lastTick[y + 1][x - 1])
                        total++;
                }
            }
            if(x < Width - 1) {
                if(y > 1) {
                    if (lastTick[y - 1][x + 1])
                        total++;
                }
                if(y < Height - 1) {
                    if (lastTick[y + 1][x + 1])
                        total++;
                }
            }
            return total;
        }
        private bool IsMouseInWindow() {
            Point pos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            return GraphicsDevice.Viewport.Bounds.Contains(pos);
        }
        private void HandleClick() {
            MouseState state = Mouse.GetState();
            if (!IsMouseInWindow())
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
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Bisque);
            spriteBatch.Begin();
            int x = 0;
            int y = 0;
            for(int i = 0; i < Squares.Length; i++) {
                for(int j = 0; j < Squares[0].Length; j++) {
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

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
