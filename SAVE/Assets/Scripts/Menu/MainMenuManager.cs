using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private string gameSceneName = "GameScene";

    private void Start() => ShowMain();

    public void ShowTutorial() => Show(tutorialPanel);

    public void StartGame() => SceneManager.LoadScene(gameSceneName);

    public void ShowOptions() => Show(optionsPanel);
    public void ShowCredits() => Show(creditsPanel);
    public void ShowMain() => Show(mainPanel);

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void Show(GameObject panel)
    {
        mainPanel.SetActive(panel == mainPanel);
        optionsPanel.SetActive(panel == optionsPanel);
        creditsPanel.SetActive(panel == creditsPanel);
        tutorialPanel.SetActive(panel == tutorialPanel);
    }
}