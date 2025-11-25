using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (!Instance)
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
        Application.targetFrameRate = 60;
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

        if (UIManager) UIManager.ShowMainMenu();
    }

    void Start()
    {
        TelemetryManager.Instance.OnLevelStart("Ironhollow");
        LoadPersistentData();
    }

    void Update()
    {
        if (UIManager && UIManager.hud.activeSelf)
        {
            UpdateDistance();
        }
    }

    void UpdateDistance()
    {
        if (tunnelSpawner == null)
            tunnelSpawner = FindFirstObjectByType<TunnelSpawner>();

        if (tunnelSpawner == null) return;

        CurrentDistance += tunnelSpawner.scrollSpeed * Time.deltaTime;
        UIManager.distanceText.text = $"{CurrentDistance:0} m";
    }


    public void StartRun()
    {
        runIndex++;

        if (Player == null)
            Player = FindFirstObjectByType<PlayerMagnetController>();

        Player.transform.position = startRunPosition;
        Player.enabled = true;

        var vcam = virtualCamera;

        if (vcam)
        {
            vcam.PreviousStateIsValid = false; // forces a full recompute of camera state
        }

        // Reset everything else  
        CurrentDistance = 0f;

        if (tunnelSpawner == null)
            tunnelSpawner = FindFirstObjectByType<TunnelSpawner>();

        if (tunnelSpawner)
            tunnelSpawner.BeginSpawning();

        AudioManager.Instance.PlayGameplayMusic();
        //TutorialManager.Instance?.OnRunStarted(runIndex); To come
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerDied()
    {
        AudioManager.Instance?.PlayPlayerDeath();
        TelemetryManager.Instance.OnPlayerDied();
        Time.timeScale = 0f;

        if (Player) Player.enabled = false;

        TelemetryManager.Instance.OnLevelComplete("Ironhollow");

        SaveRunProgress();

        RestartScene();
    }

    void SaveRunProgress()
    {
        // Only bother if we actually ran somewhere
        if (CurrentDistance <= 0.5f) return;

        lastRunDistance = CurrentDistance;

        if (lastRunDistance > bestRunDistance)
            bestRunDistance = lastRunDistance;

        PlayerPrefs.SetFloat(LastRunKey, lastRunDistance);
        PlayerPrefs.SetFloat(BestRunKey, bestRunDistance);
        PlayerPrefs.Save();

        if (!UIManager)
            UIManager.SetRunStats(lastRunDistance, bestRunDistance);
    }


    

    void LoadPersistentData()
    {
        lastRunDistance = PlayerPrefs.GetFloat(LastRunKey, 0f);
        bestRunDistance = PlayerPrefs.GetFloat(BestRunKey, 0f);

        if (!UIManager)
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
        // Prevent weird resume behaviour
        Time.timeScale = 0f;

        // Save run progress if a run is active
        SaveRunProgress();

        // Show pause menu if UI exists
        if (UIManager != null && UIManager.hud.activeSelf)
        {
            UIManager.ShowPauseFromSystem();
        }
    }



}
