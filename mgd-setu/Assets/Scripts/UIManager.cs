using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenu;
    public GameObject hud;
    public GameObject pauseMenu;
    public GameObject infoPanel;

    [Header("HUD Text")]
    public TMP_Text distanceText;

    [Header("Buttons")]
    public Button playButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    [Header("Info Buttons")]
    public Button mainMenuInfoButton;
    public Button pauseMenuInfoButton;
    public Button infoBackButton;

    private bool isPaused;
    private bool infoOpenedFromPause;
    public TMP_Text infoText;

    [Header("Menu Stats")]
    public TMP_Text lastRunText;
    public TMP_Text bestRunText;

    async void Start()
    {
        if (infoText != null)
        {
            string version = Application.version;
            string device = SystemInfo.deviceModel;
            string buildDate = "2025-11-25";

            infoText.text =
                $"Version: {version}\n" +
                $"Device: {device}\n" +
                $"Build Date: {buildDate}";
        }

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        await ActivateAfterFrameAsync();

        AddEvents();
        ShowMainMenu();
    }

    private async Awaitable ActivateAfterFrameAsync()
    {
        await Awaitable.NextFrameAsync();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void ShowMainMenu()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (hud != null) hud.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);

        Time.timeScale = 0f;
        isPaused = false;
        infoOpenedFromPause = false;

        AudioManager.Instance.PlayMenuMusic();
    }

    public void StartGame()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (hud != null) hud.SetActive(true);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);

        Time.timeScale = 1f;

        bool tutorialShowing = false;

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.ShowTutorialIfNeeded();

            if (TutorialManager.Instance.tutorialPanel != null &&
                TutorialManager.Instance.tutorialPanel.activeSelf)
            {
                tutorialShowing = true;
            }
        }

        // If tutorial is showing — DO NOT START THE RUN.
        if (tutorialShowing)
            return;

        // Otherwise start immediately
        if (GameManager.Instance != null)
            GameManager.Instance.StartRun();
    }



    public void TogglePause()
    {
        if (infoPanel != null && infoPanel.activeSelf)
            return;

        isPaused = !isPaused;

        if (pauseMenu != null)
            pauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ShowMainMenu();

        if (GameManager.Instance != null)
            GameManager.Instance.RestartScene();

        AudioManager.Instance.PlayMenuMusic();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        TelemetryManager.Instance.OnQuit();
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenInfoFromMainMenu()
    {
        if (infoPanel == null) return;

        infoOpenedFromPause = false;

        if (mainMenu != null) mainMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);

        infoPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OpenInfoFromPause()
    {
        if (infoPanel == null) return;

        infoOpenedFromPause = true;

        if (hud != null) hud.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);

        infoPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseInfo()
    {
        if (infoPanel == null) return;

        infoPanel.SetActive(false);

        if (infoOpenedFromPause)
        {
            if (pauseMenu != null) pauseMenu.SetActive(true);
            if (hud != null) hud.SetActive(true);
        }
        else
        {
            if (mainMenu != null) mainMenu.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void AddEvents()
    {
        playButton?.onClick.AddListener(StartGame);
        pauseButton?.onClick.AddListener(TogglePause);
        resumeButton?.onClick.AddListener(TogglePause);
        restartButton?.onClick.AddListener(RestartGame);
        quitButton?.onClick.AddListener(QuitGame);

        mainMenuInfoButton?.onClick.AddListener(OpenInfoFromMainMenu);
        pauseMenuInfoButton?.onClick.AddListener(OpenInfoFromPause);
        infoBackButton?.onClick.AddListener(CloseInfo);
    }

    public void SetRunStats(float lastRun, float bestRun)
    {
        if (lastRunText != null)
            lastRunText.text = lastRun > 0.5f ? $"Last run: {lastRun:0} m" : "Last run: -";

        if (bestRunText != null)
            bestRunText.text = bestRun > 0.5f ? $"Best: {bestRun:0} m" : "Best: -";
    }

    public void ShowPauseFromSystem()
    {
        if (hud != null) hud.SetActive(true);
        if (pauseMenu != null) pauseMenu.SetActive(true);
        isPaused = true;
    }
}
