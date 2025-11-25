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

    void Start()
    {
        string version = Application.version;
        string device = SystemInfo.deviceModel;
        string buildDate = "2025-11-25";

        infoText.text =
            $"Version: {version}\n" +
            $"Device: {device}\n" +
            $"Build Date: {buildDate}";

        if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);

        _ = ActivateAfterFrameAsync();

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
        mainMenu.SetActive(true);
        hud.SetActive(false);
        pauseMenu.SetActive(false);
        if (infoPanel) infoPanel.SetActive(false);

        Time.timeScale = 0f;
        isPaused = false;
        infoOpenedFromPause = false;

        AudioManager.Instance.PlayMenuMusic();
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        hud.SetActive(true);
        pauseMenu.SetActive(false);
        if (infoPanel) infoPanel.SetActive(false);

        Time.timeScale = 1f;

        GameManager.Instance.StartRun();
    }

    public void TogglePause()
    {
        if (infoPanel && infoPanel.activeSelf)
            return;

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ShowMainMenu();
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
        if (!infoPanel) return;

        infoOpenedFromPause = false;

        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        infoPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void OpenInfoFromPause()
    {
        if (!infoPanel) return;

        infoOpenedFromPause = true;

        hud.SetActive(false);
        pauseMenu.SetActive(false);
        infoPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseInfo()
    {
        if (!infoPanel) return;

        infoPanel.SetActive(false);

        if (infoOpenedFromPause)
        {
            pauseMenu.SetActive(true);
            hud.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            mainMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // ===== BUTTON WIRING =====

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
}
