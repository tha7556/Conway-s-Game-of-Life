using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_Life {
    class Board {
        private Square[][] squares;
        public int Width { get; }
        public int Height { get; }
        public Board(int height, int width) {
            squares = new Square[height][];
            for (int i = 0; i < height; i++)
                squares[i] = new Square[width];
            Height = height;
            Width = width;
        }
        public Board(Square[][] squares) {
            this.squares = squares;
            Height = squares.Length;
            Width = squares[0].Length;
        }
        public Square[] this[int index] {
            get {
                if (index < squares.Length && index > -1)
                    return squares[index];
                return null;
            }
            set {
                if (index < squares.Length && index > -1)
                    squares[index] = value;
            }
        }

    }
}
