using UnityEngine;

public class BossBombFactory : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 5;
    private int hp;

    public event System.Action DieEvent;

    [Header("Kami Spawning")]
    public GameObject kamikazeRedPrefab;
    public GameObject kamikazeBluePrefab;
    public Transform spawnPoint;
    public float baseInterval = 2.5f;
    private float nextSpawnTime;

    [Header("Damage Settings")]
    public float explosionDamageRadius = 2.5f;

    [Header("Rendering")]
    public SpriteRenderer sr; // assign in inspector

    private Transform player;
    
    public static BossBombFactory Instance;


    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);    // Another boss already exists → kill this one
            return;
        }

        Instance = this;

        hp = maxHP;
        player = FindFirstObjectByType<PlayerMagnetController>()?.transform;

        if (spawnPoint == null)
            spawnPoint = GameObject.Find("SpawnPoint").transform;

        if (sr != null)
            sr.sortingOrder = 999;
    }


    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + GetCurrentInterval();
            SpawnKami();
        }
    }

    float GetCurrentInterval()
    {
        float hpPercent = (float)hp / maxHP;

        if (hpPercent <= 0.2f) return 0.9f;
        if (hpPercent <= 0.4f) return 1.3f;
        if (hpPercent <= 0.6f) return 2.0f;

        return baseInterval;
    }

    void SpawnKami()
    {
        GameObject prefab;

        // Alternate between red and blue
        if ((maxHP - hp) % 2 == 0)
            prefab = kamikazeRedPrefab;
        else
            prefab = kamikazeBluePrefab;

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

    // This is called by explosions (modify your explosion script to ping the boss)
    public void NotifyExplosion(Vector3 pos)
    {
        if (Vector2.Distance(pos, transform.position) <= explosionDamageRadius)
        {
            hp--;
            if (hp <= 0)
                Die();
        }
    }

    public void Die()
    {
        DieEvent?.Invoke();

        if (Instance == this)
            Instance = null;

        Destroy(gameObject);
    }

}
