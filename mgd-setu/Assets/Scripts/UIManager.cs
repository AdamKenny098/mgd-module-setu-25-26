using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Threading;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenu;
    public GameObject hud;
    public GameObject pauseMenu;

    [Header("HUD Text")]
    public TMP_Text distanceText;

    [Header("Buttons")]
    public Button playButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    private bool isPaused;
    private float startPoint;

    public TunnelSpawner tunnelSpawner;

    void Start()
    {
        // Give focus immediately when play starts
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

        // Force InputSystem to update mouse state safely after frame init
        UnityEngine.InputSystem.InputSystem.QueueStateEvent(
            UnityEngine.InputSystem.Mouse.current,
            new UnityEngine.InputSystem.LowLevel.MouseState()
        );
    }

    void Update()
    {
        if (hud.activeSelf)
        {
            // Ensure player reference exists
            if (GameManager.Instance.Player == null)
            {
                GameManager.Instance.Player = FindFirstObjectByType<PlayerMagnetController>();
                if (GameManager.Instance.Player == null)
                    return; // Still null, skip this frame
            }

            // Safe distance calculation
            float dist = GameManager.Instance.Player.transform.position.x - startPoint;
            distanceText.text = $"{dist:0} m";
        }

    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        hud.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;
    }


    public void StartGame()
    {
        mainMenu.SetActive(false);
        hud.SetActive(true);
        pauseMenu.SetActive(false);

        if (GameManager.Instance.Player is null)
        GameManager.Instance.Player = FindFirstObjectByType<PlayerMagnetController>();

        startPoint = GameManager.Instance.Player.transform.position.x;
        Time.timeScale = 1f;

        if (tunnelSpawner == null)
        tunnelSpawner = FindFirstObjectByType<TunnelSpawner>();

        tunnelSpawner.BeginSpawning();
        GameManager.Instance.UIManager = this;
    }


    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ShowMainMenu();
        GameManager.Instance.RestartScene();
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

    public void AddEvents()
    {
        if (playButton != null)
        playButton.onClick.AddListener(StartGame);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(TogglePause);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }
}
