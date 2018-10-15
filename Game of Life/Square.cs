using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_of_Life {
    class Square {
        public bool IsOn { get; set; }
        public Color Color { get { if (IsOn) return Color.White; else return Color.Black; } }
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public Texture2D Texture { get; }

        public Square(int x, int y, GraphicsDevice graphics) {
            X = x;
            Y = y;
            Size = 10;
            IsOn = false;
            Texture = new Texture2D(graphics, Size, Size);
            Color[] data = new Color[Size * Size];
            Texture2D rectTexture = new Texture2D(graphics, Size, Size);
            for (int i = 0; i < data.Length; i++) {
                data[i] = Color;
            }
            rectTexture.SetData(data);
        }
        public void Draw(GraphicsDevice graphics, ref SpriteBatch spriteBatch) {
            Vector2 position = new Vector2(X, Y);
            spriteBatch.Draw(Texture, position, Color);
        }
    }
}
