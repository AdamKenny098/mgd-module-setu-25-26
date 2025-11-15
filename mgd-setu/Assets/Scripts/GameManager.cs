using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerMagnetController Player;
    public UIManager UIManager;

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

        if (UIManager) UIManager.ShowMainMenu();
            
    }

    void Start()
    {
        Player = FindFirstObjectByType<PlayerMagnetController>();
        UIManager = FindFirstObjectByType<UIManager>();
        TelemetryManager.Instance.OnLevelStart("The Game");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerDied()
    {
        TelemetryManager.Instance.OnPlayerDied();
        Time.timeScale = 0f;

        if (Player) Player.enabled = false;
        TelemetryManager.Instance.OnLevelComplete("The Game");

        RestartScene();
    }

}
