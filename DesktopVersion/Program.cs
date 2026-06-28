using CoreGame;

namespace DesktopVersion;

class Program
{
    static void Main()
    {
        Game.Init(GameMode.Desktop);
        
        while (Game.ShouldRun) Game.Frame();

        Game.DesktopDeinit();
    }
}