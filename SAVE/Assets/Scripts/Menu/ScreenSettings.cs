using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;

    private const int WindowedWidth = 1280;
    private const int WindowedHeight = 720;

    private void Start()
    {
        // Reflete o estado real da tela (começa ligado se o jogo abriu em tela cheia)
        fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void OnDestroy()
    {
        fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);
    }

    private void SetFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            // Volta para a resolução nativa do monitor
            int width = Display.main.systemWidth;
            int height = Display.main.systemHeight;
            Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(WindowedWidth, WindowedHeight, FullScreenMode.Windowed);
        }
    }
}