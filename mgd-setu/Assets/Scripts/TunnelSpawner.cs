using UnityEngine;
using System.Collections.Generic;
using Unity.Profiling;

public class TunnelSpawner : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> segmentPrefabs;
    public GameObject firstSegmentPrefab;
    public int segmentsAhead = 6;
    public int maxSegments = 10;
    public float segmentWidth = 20f;
    public float scrollSpeed = 5f;

    readonly Queue<GameObject> activeSegments = new();
    readonly Queue<GameObject> pool = new();  // unified pool

    bool isSpawning = false;
    GameObject tailSegment;

    static readonly ProfilerMarker SpawnerUpdateMarker  = new ProfilerMarker("IH.TunnelSpawner.Update");
    static readonly ProfilerMarker SpawnerSpawnMarker   = new ProfilerMarker("IH.TunnelSpawner.Spawn");
    static readonly ProfilerMarker SpawnerRecycleMarker = new ProfilerMarker("IH.TunnelSpawner.Recycle");

    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) 
            return;
#endif

        if (!isSpawning)
            return;

        using (SpawnerUpdateMarker.Auto())
        {
            Debug.Log("Spawner running");
            float dt = Time.deltaTime;
            float camX = Camera.main ? Camera.main.transform.position.x : 0f;

            // Move active segments left
            foreach (var seg in activeSegments)
            {
                if (seg == null) continue;
                seg.transform.position += Vector3.left * scrollSpeed * dt;
            }

            using (SpawnerSpawnMarker.Auto())
            {
                // spawn ahead
                if (tailSegment != null)
                {
                    while (tailSegment.transform.position.x < camX + segmentsAhead * segmentWidth)
                    {
                        float spawnX = tailSegment.transform.position.x + segmentWidth;
                        SpawnRandomSegmentAt(spawnX);
                    }
                }
            }

            using (SpawnerRecycleMarker.Auto())
            {
                // recycle behind
                while (activeSegments.Count > 0)
                {
                    GameObject oldest = activeSegments.Peek();
                    if (oldest == null)
                    {
                        activeSegments.Dequeue();
                        continue;
                    }

                    if (oldest.transform.position.x < camX - segmentWidth * 2f)
                    {
                        activeSegments.Dequeue();
                        Recycle(oldest);
                    }
                    else break;
                }

                // safety limit
                while (activeSegments.Count > maxSegments)
                {
                    var old = activeSegments.Dequeue();
                    if (old != null) Recycle(old);
                }
            }
        }
    }

    public void BeginSpawning()
    {
        if (!Application.isPlaying)
            return;

        if (isSpawning)
            return;

        ClearAll();
        isSpawning = true;

        float camX = Camera.main ? Camera.main.transform.position.x : 0f;
        float firstX = camX;

        // guaranteed first segment
        SpawnFirstSegment(firstX);

        // fill ahead
        for (int i = 1; i <= segmentsAhead; i++)
        {
            float x = firstX + i * segmentWidth;
            SpawnRandomSegmentAt(x);
        }
    }

    public void ClearAll()
    {
        foreach (var seg in activeSegments)
            if (seg != null) Recycle(seg);

        activeSegments.Clear();
        tailSegment = null;
        isSpawning = false;
    }

    // --- Unified Pool Helpers ----

    GameObject GetFromPool(GameObject prefab, Vector3 pos)
    {
        GameObject inst;

        if (pool.Count > 0)
        {
            inst = pool.Dequeue();
            inst.transform.position = pos;
            inst.transform.rotation = Quaternion.identity;
            inst.SetActive(true);
        }
        else
        {
            inst = Instantiate(prefab, pos, Quaternion.identity, transform);
        }

        return inst;
    }

    void Recycle(GameObject seg)
    {
        seg.SetActive(false);
        pool.Enqueue(seg);
    }

    // --- Spawn Logic ----

    void SpawnFirstSegment(float x)
    {
        if (firstSegmentPrefab == null)
        {
            Debug.LogWarning("TunnelSpawner missing firstSegmentPrefab!");
            return;
        }

        var seg = GetFromPool(firstSegmentPrefab, new Vector3(x, 0f, 0f));
        activeSegments.Enqueue(seg);
        tailSegment = seg;
    }

    void SpawnRandomSegmentAt(float x)
    {
        if (segmentPrefabs == null || segmentPrefabs.Count == 0)
        {
            Debug.LogError("TunnelSpawner: segmentPrefabs is empty.");
            return;
        }

        GameObject prefab = segmentPrefabs[Random.Range(0, segmentPrefabs.Count)];

        var seg = GetFromPool(prefab, new Vector3(x, 0f, 0f));
        activeSegments.Enqueue(seg);
        tailSegment = seg;
    }
}
