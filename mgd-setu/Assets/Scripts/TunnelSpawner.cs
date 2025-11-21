using UnityEngine;
using System.Collections.Generic;

public class TunnelSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public List<GameObject> segmentPrefabs;
    public GameObject firstSegmentPrefab;

    [Header("Settings")]
    public int segmentsAhead = 6;
    public int maxSegments = 10;
    public float segmentWidth = 20f;

    private readonly Queue<GameObject> activeSegments = new();
    private float nextSpawnX = 0f;
    private bool isSpawning = false;
    private bool firstSpawnDone = false;

    void Update()
    {
        if (!isSpawning || !player) return;

        // Spawn ahead if needed
        while (player.position.x + segmentWidth * segmentsAhead > nextSpawnX)
        {
            SpawnSegment();
        }   

        // Recycle behind if too many
        while (activeSegments.Count > maxSegments)
        {
            var old = activeSegments.Dequeue();
            Destroy(old);
        }
    }

    public void BeginSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        nextSpawnX = 0f;
        firstSpawnDone = false;

        // Warm-up a few segments
        for (int i = 0; i < segmentsAhead; i++)
        {
            SpawnSegment();
        }
    }

    public void ClearAll()
    {
        foreach (var seg in activeSegments)
        {
            Destroy(seg);
        }
        activeSegments.Clear();
        nextSpawnX = 0f;
        isSpawning = false;
    }

    void SpawnSegment()
    {
        GameObject prefab;

        // First-ever segment spawn → use your chosen prefab
        if (!firstSpawnDone && firstSegmentPrefab != null)
        {
            prefab = firstSegmentPrefab;
            firstSpawnDone = true;
        }
        else
        {
            prefab = segmentPrefabs[Random.Range(0, segmentPrefabs.Count)];
        }

        var newSeg = Instantiate(prefab, new Vector3(nextSpawnX, 0, 0), Quaternion.identity, transform);
        activeSegments.Enqueue(newSeg);
        nextSpawnX += segmentWidth;
    }
}
