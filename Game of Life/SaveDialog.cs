using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game_of_Life {
    class SaveDialog {
        private string path;
        private Texture2D fileImage;
        private Texture2D folderImage;
        private int width;
        public SaveDialog() {
            path = Directory.GetCurrentDirectory();
        }
        public void LoadContent(ContentManager manager) {
            fileImage = manager.Load<Texture2D>("file.png");
            folderImage = manager.Load<Texture2D>("folder.png");
        }
        public void Update() {

        }
        public string[] GetAllFolders() {
            if (path == null)
                return null;
            if (!Directory.Exists(path))
                return null;
            return Directory.GetDirectories(path);
        }
        public string[] GetAllFiles() {
            if (path == null)
                return null;
            if (!Directory.Exists(path))
                return null;
            return Directory.GetFiles(path);
        }
        public void MoveUp() {
            if (path == null)
                return;
            if (!Directory.Exists(path))
                return;
            if (!Directory.GetParent(path).Exists)
                return;
            path = Directory.GetParent(path).FullName;
        }
        public void Draw() {

        }
        public void Save(Board board) {
            board.Save(path);
        }
    }
}
