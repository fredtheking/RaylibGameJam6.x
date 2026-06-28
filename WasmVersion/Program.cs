using System.Runtime.InteropServices.JavaScript;
using CoreGame;

namespace WasmVersion;

public partial class Program
{
    public static void Main() => Game.Init(GameMode.WASM);

    [JSExport] public static void Frame() => Game.Frame();
    [JSExport] public static bool SwitchMuted() => Game.SwitchMuted();
    [JSExport] public static void InformFullscreen(bool _is_fullscreen) => Game.InformFullscreen(_is_fullscreen);
    [JSExport] public static void RollNewMusic() => MusicPlayer.RollNew();
}
