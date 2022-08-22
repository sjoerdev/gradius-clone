using System;

namespace Project
{
    public class EntryPoint
    {
        static void Main(string[] args)
        {
            GameEngine instance = GameEngine.GetInstance();
            if (instance == null) return;
            new AbstractGame();
            instance.Run();
            instance.Dispose();
        }
    }
}
