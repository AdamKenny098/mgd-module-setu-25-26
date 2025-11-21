using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyTurret : MonoBehaviour
{
    public float detectionRange = 7f;
    public float fireRate = 1.25f;
    public GameObject projectilePrefab;

    private Transform player;
    private Rigidbody2D rb;
    private float nextShootTime;

    private Vector3 turretTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        player = FindFirstObjectByType<PlayerMagnetController>().transform;

        turretTransform = transform.localScale;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > detectionRange) return;

        // Rotate to face player
        Vector2 dir = (player.position - transform.position).normalized;
        
        if(player.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // Shoot
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + fireRate;
            Shoot(dir);
        }
    }

    private void Shoot(Vector2 dir)
    {
        Vector3 spawnPos = transform.position + (Vector3)dir * 0.5f;

        var go = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        var proj = go.GetComponent<MagneticProjectile>();

        // Mark projectile as enemy
        proj.source = MagneticProjectile.SourceType.Enemy;
        proj.Fire(dir);

        // Prevent self-collision
        Physics2D.IgnoreCollision(go.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

}
