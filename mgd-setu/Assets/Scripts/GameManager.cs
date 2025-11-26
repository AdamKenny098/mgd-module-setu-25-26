using UnityEngine;
using UnityEngine.SceneManager;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerMagnetController Player;
    public UIManager UIManager;
    public TunnelSpawner tunnelSpawner;

    [Header("Run Settings")]
    public Vector3 startRunPosition = new Vector3(0, 1, 0);
    public float CurrentDistance { get; private set; }
    int runIndex = 0;

    [Header("Camera")]
    public CinemachineCamera virtualCamera;

    [Header("Persistence")]
    const string LastRunKey = "IH_LastRunDistance";
    const string BestRunKey = "IH_BestRunDistance";

    float lastRunDistance;
    float bestRunDistance;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = FindFirstObjectByType<PlayerMagnetController>();
        UIManager = FindFirstObjectByType<UIManager>();
        tunnelSpawner = FindFirstObjectByType<TunnelSpawner>();

        if (UIManager != null)
            UIManager.ShowMainMenu();
    }

    void Start()
    {
        if (TelemetryManager.Instance != null)
            TelemetryManager.Instance.OnLevelStart("Ironhollow");

        LoadPersistentData();
    }

    void Update()
    {
        if (UIManager != null && UIManager.hud != null && UIManager.hud.activeSelf)
            UpdateDistance();
    }

    void UpdateDistance()
    {
        if (tunnelSpawner == null)
            tunnelSpawner = FindFirstObjectByType<TunnelSpawner>();

        if (tunnelSpawner == null || UIManager == null || UIManager.distanceText == null)
            return;

        CurrentDistance += tunnelSpawner.scrollSpeed * Time.deltaTime;
        UIManager.distanceText.text = $"{CurrentDistance:0} m";
    }


    public void StartRun()
    {
        runIndex++;

        if (Player == null)
            Player = FindFirstObjectByType<PlayerMagnetController>();

        if (Player != null)
        {
            Player.transform.position = startRunPosition;
            Player.enabled = true;
        }

        if (virtualCamera != null)
            virtualCamera.PreviousStateIsValid = false;

        CurrentDistance = 0f;

        if (tunnelSpawner == null)
            tunnelSpawner = FindFirstObjectByType<TunnelSpawner>();

        if (tunnelSpawner != null)
            tunnelSpawner.BeginSpawning();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGameplayMusic();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerDied()
    {
        AudioManager.Instance?.PlayPlayerDeath();
        TelemetryManager.Instance?.OnPlayerDied();
        Time.timeScale = 0f;

        if (Player != null)
            Player.enabled = false;

        TelemetryManager.Instance.OnLevelComplete("Ironhollow");

        SaveRunProgress();
        RestartScene();
    }

    void SaveRunProgress()
    {
        if (CurrentDistance <= 0.5f) return;

        lastRunDistance = CurrentDistance;

        if (lastRunDistance > bestRunDistance)
            bestRunDistance = lastRunDistance;

        PlayerPrefs.SetFloat(LastRunKey, lastRunDistance);
        PlayerPrefs.SetFloat(BestRunKey, bestRunDistance);
        PlayerPrefs.Save();

        if (UIManager != null)
            UIManager.SetRunStats(lastRunDistance, bestRunDistance);
    }

    void LoadPersistentData()
    {
        lastRunDistance = PlayerPrefs.GetFloat(LastRunKey, 0f);
        bestRunDistance = PlayerPrefs.GetFloat(BestRunKey, 0f);

        if (UIManager != null)
            UIManager.SetRunStats(lastRunDistance, bestRunDistance);
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            HandleFocusLoss();
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
            HandleFocusLoss();
    }

    void HandleFocusLoss()
    {
        Time.timeScale = 0f;
        SaveRunProgress();

        if (UIManager != null && UIManager.hud != null && UIManager.hud.activeSelf)
            UIManager.ShowPauseFromSystem();
    }
}
