using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public string name;
        public Button button;
        public GameObject badEndingPanel;

        [Header("Ao acertar")]
        public ParticleSystem[] particlesToStop;
        public ParticleSystem[] particlesToPlay;
        public GameObject[] activate;
        public GameObject[] deactivate;
        public Material skybox;
        public AudioClip newMusic; // para a música do jogo e toca esta uma vez
    }

    [SerializeField] private Step[] steps;

    [Header("Áudio")]
    [SerializeField] private AudioSource gameMusicSource; // AudioManager (música principal)
    [FormerlySerializedAs("musicSource")]
    [SerializeField] private AudioSource endingSource;    // ABadEndingManager (Loop desligado)
    [SerializeField] private AudioClip badEndingMusic;

    [Header("Finais")]
    [FormerlySerializedAs("hideOnBadEnding")]
    [SerializeField] private GameObject[] hideOnEnding;   // Panel_Game, Panel_Pause
    [SerializeField] private GameObject goodEndingPanel;
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Cenas")]
    [SerializeField] private string menuSceneName = "MainMenuScene";

    private int currentStep;
    private bool finished;

    private void Start()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            int index = i;
            steps[i].button.onClick.AddListener(() => OnButtonClicked(index));

            if (steps[i].badEndingPanel != null)
                steps[i].badEndingPanel.SetActive(false);
        }

        if (goodEndingPanel != null) goodEndingPanel.SetActive(false);
    }

    private void OnButtonClicked(int index)
    {
        if (finished) return;

        if (index == currentStep) CompleteStep(steps[index]);
        else BadEnding(currentStep);
    }

    private void CompleteStep(Step step)
    {
        step.button.interactable = false;

        foreach (var ps in step.particlesToStop)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        foreach (var ps in step.particlesToPlay)
        {
            ps.gameObject.SetActive(true);
            ps.Play();
        }

        foreach (var go in step.activate) go.SetActive(true);
        foreach (var go in step.deactivate) go.SetActive(false);

        if (step.skybox != null)
        {
            RenderSettings.skybox = step.skybox;
            DynamicGI.UpdateEnvironment();
        }

        if (step.newMusic != null) PlayEndingMusic(step.newMusic);

        currentStep++;
        if (currentStep >= steps.Length) FinishGame();
    }

    private void BadEnding(int stepIndex)
    {
        EndGame();
        steps[stepIndex].badEndingPanel.SetActive(true);
        PlayEndingMusic(badEndingMusic);
    }

    private void FinishGame()
    {
        EndGame();
        if (goodEndingPanel != null) goodEndingPanel.SetActive(true);
    }

    // Comum aos dois finais: trava o jogo e esconde a interface
    private void EndGame()
    {
        finished = true;
        if (pauseMenu != null) pauseMenu.enabled = false;
        foreach (var go in hideOnEnding) go.SetActive(false);
    }

    // Para a música do jogo e toca a música do final uma única vez
    private void PlayEndingMusic(AudioClip clip)
    {
        if (gameMusicSource != null) gameMusicSource.Stop();

        endingSource.Stop();
        endingSource.loop = false;
        endingSource.clip = clip;
        endingSource.Play();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}