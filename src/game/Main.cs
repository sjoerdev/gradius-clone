using System;

namespace Project;

public class EntryPoint
{
    static void Main()
    {
        GameEngine instance = GameEngine.Instance;
        var game = new AbstractGame();
        instance.Run();
    }
}