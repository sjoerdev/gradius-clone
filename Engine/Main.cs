using System;

namespace GameEngine
{
    public class EntryPoint
    {
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            GameEngine instance = GameEngine.GetInstance();

            if (instance == null)
                return;

            new AbstractGame();

            instance.Run();

            //Clean up unmanaged resources
            instance.Dispose();
        }
    }
}
