using System;

namespace Game_of_Life {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            using (GameOfLife game = new GameOfLife())
                game.Run();
        }
    }
#endif
}
