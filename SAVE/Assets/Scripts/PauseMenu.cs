using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string menuSceneName = "MainMenuScene";

    private bool isPaused;

    private void Start()
    {
        SetPaused(false);
    }

    private void Update()
    {
        if (EscapePressed())
            TogglePause();
    }

    private bool EscapePressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    // Ligue o botão do canto da tela a este método
    public void TogglePause() => SetPaused(!isPaused);

    // Ligue o botão "Continuar" a este método
    public void Resume() => SetPaused(false);

    // Ligue o botão "Menu" a este método
    public void GoToMenu()
    {
        Time.timeScale = 1f; // sem isso o menu carregaria congelado
        SceneManager.LoadScene(menuSceneName);
    }

    private void SetPaused(bool paused)
    {
        isPaused = paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f; // segurança ao trocar de cena
    }
}