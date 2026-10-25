using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Gerencia os botões do MainMenu: Play, Options e Exit.
/// Anexe este script num GameObject vazio (ex: "Menu_Manager")
/// e ligue os botões (OnClick) às funções correspondentes.
/// </summary>
public class MainMenuManager: MonoBehaviour
{
    [Header("Cenas")]
    [Tooltip("Nome exato da cena de gameplay a ser carregada pelo Button_Play.")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Painel de Options")]
    [Tooltip("Painel (GameObject) que representa a tela de Options. Deve come�ar desativado.")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
        // Garante que o painel de Options começa fechado
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    // Ligar ao Button_Play (OnClick)
    public void OnPlayButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Ligar ao Button_Options (OnClick) - abre o painel
    public void OnOptionsButton()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    // Ligar a um botão "Voltar"/"Fechar" dentro do próprio painel de Options
    public void OnCloseOptionsButton()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void OnCreditsButton()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void OnCloseCreditsButton()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    // Ligar ao Button_Exit (OnClick)
    public void OnExitButton()
    {
#if UNITY_EDITOR
        // Fecha o Play Mode quando testado dentro da Unity
        EditorApplication.isPlaying = false;
#else
        // Fecha o jogo de verdade na build
        Application.Quit();
#endif
    }
}