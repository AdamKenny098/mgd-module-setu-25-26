using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float flickerSpeed = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        sr = GetComponent<SpriteRenderer>();

        // Wisp moves through everything
        GetComponent<Collider2D>().isTrigger = true;

        player = FindFirstObjectByType<PlayerMagnetController>()?.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Float toward player
        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        // Flicker effect
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed));
        sr.color = new Color(1f, 1f, 1f, alpha);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Only killable by projectiles or explosions
        if (col.CompareTag("PlayerProjectile") || col.CompareTag("Explosion") || col.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        var playerMag = col.GetComponent<PlayerMagnetController>();
        if (playerMag != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }
}
