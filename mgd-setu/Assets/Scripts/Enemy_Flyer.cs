using UnityEngine;

public class EnemyFlyer : MonoBehaviour
{
    public float hoverAmount = 0.25f;
    public float hoverSpeed = 3f;
    public float moveSpeed = 1.5f;

    public GameObject projectilePrefab;
    public float fireInterval = 1.6f;

    private Transform player;
    private Rigidbody2D rb;
    private float nextFireTime;
    private float hoverOffset;

    public float detectionRange = 10f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        hoverOffset = Random.value * Mathf.PI * 2f;

        player = FindFirstObjectByType<PlayerMagnetController>()?.transform;
    }

        void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Don't move or shoot if too far
        if (dist > detectionRange)
            return;
        
        // Hover movement
        float hover = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverAmount;

        Vector2 dir = (player.position - transform.position).normalized;
        Vector2 targetPos = (Vector2)transform.position - dir * 0.5f + new Vector2(0, hover);
        rb.MovePosition(Vector2.Lerp(transform.position, targetPos, 0.15f));

        // Fire bullets
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireInterval;
            Fire(dir);
        }
    }


    private void Fire(Vector2 dir)
    {
        // spawn position forward from the flyer body
        Vector3 baseSpawnPos = transform.position + (Vector3)dir * 0.5f;

        for (int i = -1; i <= 1; i++)
        {
            Vector2 spread = Quaternion.Euler(0, 0, i * 20f) * dir;

            // Each projectile spawns slightly forward, not inside the collider
            var go = Instantiate(projectilePrefab, baseSpawnPos, Quaternion.identity);

            var proj = go.GetComponent<MagneticProjectile>();
            proj.source = MagneticProjectile.SourceType.Enemy;
            proj.Fire(spread);

            // prevent immediate self-collision
            Physics2D.IgnoreCollision(
                go.GetComponent<Collider2D>(),
                GetComponent<Collider2D>()
            );
        }
    }

}
