using UnityEngine;
using System.Collections.Generic;

public class TunnelSpawner : MonoBehaviour
{
    [Header("References")]
    public List<GameObject> segmentPrefabs;
    public GameObject firstSegmentPrefab;

    [Header("Settings")]
    public int segmentsAhead = 6;
    public int maxSegments = 10;
    public float segmentWidth = 20f;
    public float scrollSpeed = 5f;

    readonly Queue<GameObject> activeSegments = new();
    bool isSpawning = false;
    GameObject tailSegment;

    void Update()
    {
        if (!isSpawning) return;

        float dt = Time.deltaTime;
        float camX = Camera.main ? Camera.main.transform.position.x : 0f;

        // Move all segments left
        foreach (var seg in activeSegments)
        {
            if (seg == null) continue;
            seg.transform.position += Vector3.left * scrollSpeed * dt;
        }

        // Spawn ahead when needed
        if (tailSegment != null)
        {
            while (tailSegment.transform.position.x < camX + segmentsAhead * segmentWidth)
            {
                float spawnX = tailSegment.transform.position.x + segmentWidth;
                SpawnRandomSegmentAt(spawnX);
            }
        }

        // Recycle segments that fall behind camera
        while (activeSegments.Count > 0)
        {
            var oldest = activeSegments.Peek();
            if (oldest == null)
            {
                activeSegments.Dequeue();
                continue;
            }

            if (oldest.transform.position.x < camX - segmentWidth * 2f)
            {
                activeSegments.Dequeue();
                Destroy(oldest);
            }
            else break;
        }

        // Safety limit
        while (activeSegments.Count > maxSegments)
        {
            var old = activeSegments.Dequeue();
            if (old != null) Destroy(old);
        }
    }

    public void BeginSpawning()
    {
        if (isSpawning) return;

        ClearAll();
        isSpawning = true;

        float camX = Camera.main ? Camera.main.transform.position.x : 0f;

        // Spawn the first segment UNDER the camera
        float firstX = camX;
        SpawnFirstSegment(firstX);

        // Spawn ahead segments
        for (int i = 1; i <= segmentsAhead; i++)
        {
            float x = firstX + i * segmentWidth;
            SpawnRandomSegmentAt(x);
        }
    }


    public void ClearAll()
    {
        foreach (var seg in activeSegments)
        {
            if (seg != null) Destroy(seg);
        }

        activeSegments.Clear();
        tailSegment = null;
        isSpawning = false;
    }


    void SpawnFirstSegment(float x)
    {
        if (firstSegmentPrefab == null)
        {
            Debug.LogWarning("TunnelSpawner: No firstSegmentPrefab assigned!");
            return;
        }

        var seg = Instantiate(firstSegmentPrefab, new Vector3(x, 0f, 0f), Quaternion.identity, transform);
        activeSegments.Enqueue(seg);
        tailSegment = seg;
    }

    void SpawnRandomSegmentAt(float x)
    {
        if (segmentPrefabs == null || segmentPrefabs.Count == 0)
        {
            Debug.LogError("TunnelSpawner: No segmentPrefabs assigned.");
            return;
        }

        var prefab = segmentPrefabs[Random.Range(0, segmentPrefabs.Count)];
        var seg = Instantiate(prefab, new Vector3(x, 0f, 0f), Quaternion.identity, transform);

        activeSegments.Enqueue(seg);
        tailSegment = seg;
    }
}
