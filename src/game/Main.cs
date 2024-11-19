using System;

namespace Spork;

public class EntryPoint
{
    static void Main()
    {
        Engine instance = Engine.Instance;
        var game = new AbstractGame();
        instance.Run();
    }
}