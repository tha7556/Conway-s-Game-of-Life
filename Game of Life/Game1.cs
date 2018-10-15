using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game_of_Life {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int squareSize = 10;
        bool[][] squares;
        bool[][] lastTick;
        Texture2D whiteSquare, blackSquare;
        Random random = new Random();
        int width, height;
        bool paused = true;
        int iteration = 0;
        public Game1() {
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
            height = graphics.GraphicsDevice.Viewport.Height / squareSize;
            width = graphics.GraphicsDevice.Viewport.Width / squareSize;
            squares = new bool[graphics.GraphicsDevice.Viewport.Height / squareSize][];
            lastTick = new bool[graphics.GraphicsDevice.Viewport.Height / squareSize][];
            
            for (int i = 0; i < squares.Length; i++) {
                squares[i] = new bool[width];
                lastTick[i] = new bool[width];
                for(int j = 0; j < width; j++) {
                    //lastTick[i][j] = random.NextDouble() > .5;
                    lastTick[i][j] = false;
                }
            }
            //lastTick[10][10] = true;
            squares[11][10] = true;
            //lastTick[12][10] = true;

            //lastTick[11][12] = true;
            //lastTick[10][12] = true;
           // lastTick[9][12] = true;

            //lastTick[13][8] = true;
            //lastTick[14][8] = true;

            //lastTick[14][7] = true;
            Console.WriteLine(string.Format("width: {0}, height: {1}", width, graphics.GraphicsDevice.Viewport.Height / squareSize));
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
                paused = true;    
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    paused = false;
            HandleClick();
            if (!paused && iteration++ == 3) {
                CalculateNextTick();
                iteration = 0;
            }
            base.Update(gameTime);
        }
        private void CalculateNextTick() {
            for(int i = 0; i < squares.Length; i++) {
                for(int j = 0; j < squares[0].Length; j++) {
                    int number = GetNumberOfNeighbors(j, i);
                    if (lastTick[i][j]) {
                        if (number < 2)
                            squares[i][j] = false;
                        else if (number > 3)
                            squares[i][j] = false;
                    }
                    else if (number == 3)
                        squares[i][j] = true;
                }
            }
            for(int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++)
                    lastTick[i][j] = squares[i][j];
            }
        }
        private int GetNumberOfNeighbors(int x, int y) {
            int total = 0;
            if (y < 0 || y > height)
                return total;
            if (x < 0 || x > width)
                return total;
            //Horizontal
            if(x > 1) {
                if (lastTick[y][x - 1])
                    total++;
            }
            if(x < width - 1) {
                if (lastTick[y][x + 1])
                    total++;
            }
            //Vertical
            if(y > 1) {
                if (lastTick[y - 1][x])
                    total++;
            }
            if(y < height - 1) {
                if (lastTick[y + 1][x])
                    total++;
            }
            //Diagonal
            if(x > 1) {
                if(y > 1) {
                    if (lastTick[y - 1][x - 1])
                        total++;
                }
                if(y < height - 1) {
                    if (lastTick[y + 1][x - 1])
                        total++;
                }
            }
            if(x < width - 1) {
                //Console.WriteLine(string.Format("x: {0}, y: {1}, width: {2}, height: {3}", x, y, width, height));
                if(y > 1) {
                    if (lastTick[y - 1][x + 1])
                        total++;
                }
                if(y < height - 1) {
                    if (lastTick[y + 1][x + 1])
                        total++;
                }
            }
            return total;
        }
        private bool IsMouseInWindow(int x, int y) {
            Point pos = new Point(x, y);
            return GraphicsDevice.Viewport.Bounds.Contains(pos);
        }
        private void HandleClick() {
            MouseState state = Mouse.GetState();
            if (!IsMouseInWindow(state.X, state.Y))
                return;
            int x = state.X / squareSize;
            int y = state.Y / squareSize;
            if (state.LeftButton == ButtonState.Pressed) {
                lastTick[y][x] = true;
                squares[y][x] = true;
            }
            if (state.RightButton == ButtonState.Pressed) {
                lastTick[y][x] = false;
                squares[y][x] = false;
            }

        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            //System.Console.WriteLine("what");
            GraphicsDevice.Clear(Color.Bisque);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            int x = 0;
            int y = 0;
            for(int i = 0; i < squares.Length; i++) {
                for(int j = 0; j < squares[0].Length; j++) {
                    Vector2 position = new Vector2(x, y);
                    if (squares[i][j]) {
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
