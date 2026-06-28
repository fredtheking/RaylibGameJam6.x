import { dotnet } from './_framework/dotnet.js'

const { getAssemblyExports, getConfig, runMain } = await dotnet
    .withDiagnosticTracing(false)
    .create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

const buttonFullscreen = document.getElementById('btn-fullscreen')
const buttonMute = document.getElementById('btn-mute')
const buttonMusic = document.getElementById('btn-music')

dotnet.instance.Module['canvas'] = document.getElementById('canvas');

buttonFullscreen.addEventListener('click', () => {
    const canvas = document.getElementById('canvas');
    
    if (!document.fullscreenElement)
        canvas.requestFullscreen().catch(err =>
            console.error(`Unable to enter fullscreen: ${err.message}`)
        );
    else
        document.exitFullscreen().catch(err => 
            console.error(`Unable to exit fullscreen: ${err.message}`)
        );
});
document.addEventListener('fullscreenchange', () => {
    const isFullscreen = !!document.fullscreenElement

    exports.WasmVersion.Program.InformFullscreen(isFullscreen);
    buttonFullscreen.innerText = isFullscreen ? "[ON] Fullscreen" : "[OFF] Fullscreen";
});

buttonMute.addEventListener('click', () => {
    const isMuted = exports.WasmVersion.Program.SwitchMuted();
    buttonMute.innerText = isMuted ? "[OFF] Sound" : "[ON] Sound";
    
    buttonMusic.disabled = isMuted;
});

buttonMusic.addEventListener('click', () => {
    exports.WasmVersion.Program.RollNewMusic();
});

function mainLoop() {
    if (!document.hidden)
        exports.WasmVersion.Program.Frame();

    window.requestAnimationFrame(mainLoop);
}

if (!document.fullscreenEnabled) {
    buttonFullscreen.disabled = true;
    buttonFullscreen.innerText = "Fullscreen is Unavailable";
}
await runMain();
window.requestAnimationFrame(mainLoop);