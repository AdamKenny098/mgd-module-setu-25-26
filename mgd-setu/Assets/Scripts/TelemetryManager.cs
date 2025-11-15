using UnityEngine;

public class TelemetryManager : MonoBehaviour
{
    private static TelemetryManager instance;
    public static TelemetryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<TelemetryManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("TelemetryManager");
                    instance = go.AddComponent<TelemetryManager>();
                }
            }
            return instance;
        }
    }


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnLevelStart(string levelName)
    {
        Debug.Log($"[Telemetry] level_start | level={levelName}");
    }

    public void OnLevelComplete(string levelName)
    {
        Debug.Log($"[Telemetry] level_complete | level={levelName}");
    }

    public void OnPlayerDied()
    {
        Debug.Log("[Telemetry] player_died");
    }

    public void OnEnemyDestroyed(string enemyType)
    {
        Debug.Log($"[Telemetry] enemy_destroyed | type={enemyType}");
    }

    public void OnQuit()
    {
        Debug.Log("[Telemetry] session_end");
    }
}
