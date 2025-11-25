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

        RestartScene();
    }
}
