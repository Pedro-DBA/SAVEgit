using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public ParticleSystem[] particlesToStop;   // para de emitir (as partículas somem aos poucos)
        public ParticleSystem[] particlesToPlay;   // ativa o objeto e toca
        public GameObject[] activate;              // ex.: modelo da represa
        public GameObject[] deactivate;            // ex.: Panel_Game, Panel_Pause
        public Material skybox;                    // opcional
        public AudioClip newMusic;                 // opcional
    }

    [SerializeField] private Step[] steps;

    [Header("Áudio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip badEndingMusic;

    [Header("Finais")]
    [SerializeField] private GameObject[] hideOnBadEnding;   // Panel_Game, Panel_Pause
    [SerializeField] private GameObject goodEndingPanel;     // opcional
    [SerializeField] private PauseMenu pauseMenu;            // para desligar o Esc no final

    [Header("Cenas")]
    [SerializeField] private string menuSceneName = "MainMenuScene";

    private int currentStep;
    private bool finished;

    private void Start()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            int index = i; // necessário para o clique lembrar o número certo
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
        else BadEnding(currentStep); // erro: o final depende da etapa atual
    }

    private void CompleteStep(Step step)
    {
        step.button.interactable = false; // não dá para clicar de novo

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

        if (step.newMusic != null) PlayMusic(step.newMusic, true);

        currentStep++;
        if (currentStep >= steps.Length) FinishGame();
    }

    private void BadEnding(int stepIndex)
    {
        finished = true;
        if (pauseMenu != null) pauseMenu.enabled = false;

        foreach (var go in hideOnBadEnding) go.SetActive(false);

        steps[stepIndex].badEndingPanel.SetActive(true);
        PlayMusic(badEndingMusic, false);
    }

    private void FinishGame()
    {
        finished = true;
        if (pauseMenu != null) pauseMenu.enabled = false;
        if (goodEndingPanel != null) goodEndingPanel.SetActive(true);
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    // Ligue aos botões dos painéis de bad ending
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