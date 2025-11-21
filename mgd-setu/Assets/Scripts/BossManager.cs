using UnityEngine;

public class BossManager : MonoBehaviour
{
    public Transform player;
    public float nextBossDistance = 250f;  // first spawn at 250m
    public float bossInterval = 250f;      // subsequent spawns at 250m intervals
    public float minimumGap = 100f;        // skip spawns if too close

    public GameObject bossPrefab;

    private BossBombFactory currentBoss;

    void Update()
    {
        if (!player) return;

        float dist = player.position.x;

        // If a boss is active, do nothing
        if (currentBoss != null)
            return;

        // Skip if player is not far enough yet
        if (dist < nextBossDistance)
            return;

        // Check if the NEXT interval is too close
        float nextInterval = nextBossDistance + bossInterval;
        float distanceToNextInterval = nextInterval - dist;

        if (distanceToNextInterval < minimumGap)
        {
            // Skip this spawn, move target to the next interval
            nextBossDistance += bossInterval;
            return;
        }

        // Otherwise spawn the boss normally
        SpawnBoss();
    }

    void SpawnBoss()
    {
        if (BossBombFactory.Instance != null)
        return;


        Transform anchor = Camera.main.transform.Find("BossAnchor");
        var go = Instantiate(bossPrefab, anchor.position, Quaternion.identity, anchor);
        currentBoss = BossBombFactory.Instance;
        currentBoss.DieEvent += HandleBossDeath;

    }

    void HandleBossDeath()
    {
        if (currentBoss != null)
            currentBoss.DieEvent -= HandleBossDeath;

        currentBoss = null;
        nextBossDistance += bossInterval;
    }

}
