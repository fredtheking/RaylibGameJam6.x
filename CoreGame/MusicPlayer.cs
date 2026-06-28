using Raylib_cs;

namespace CoreGame;

public static class MusicPlayer
{
    private static Music CurrentlyPlaying = default;
    private static int LastPlayedMusic = 0;
  
    public static void RollNew()
    {
        if (Raylib.IsMusicValid(CurrentlyPlaying))
        {
            Raylib.StopMusicStream(CurrentlyPlaying);
            Unload();
        }

        int currentId;
        do
        {
            currentId = Raylib.GetRandomValue(1, 5);
        } while (LastPlayedMusic == currentId);
        LastPlayedMusic = currentId;
    
        CurrentlyPlaying = Raylib.LoadMusicStream($"Resources/Music/{currentId}.ogg");
        CurrentlyPlaying.Looping = false;
        Raylib.PlayMusicStream(CurrentlyPlaying);
    }

    public static void Process()
    {
        var valid = Raylib.IsMusicValid(CurrentlyPlaying); 
        
        if (!valid || !Raylib.IsMusicStreamPlaying(CurrentlyPlaying)) RollNew();
        if (valid && (!Game.State?.IsMuted ?? false)) Raylib.UpdateMusicStream(CurrentlyPlaying);
    }
    
    public static void Unload() =>
        Raylib.UnloadMusicStream(CurrentlyPlaying);
}