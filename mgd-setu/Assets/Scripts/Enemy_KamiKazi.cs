using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKamikaze : MonoBehaviour, IMagnetic
{
    [Header("Detection & Movement")]
    public float detectionRange = 8f;
    public float diveSpeed = 8f;

    [Header("Magnetism")]
    [SerializeField] private IMagnetic.Polarity currentPolarity = IMagnetic.Polarity.Blue;
    public IMagnetic.Polarity CurrentPolarity
    {
        get => currentPolarity;
        set => currentPolarity = value;
    }
    public float magneticStrength = 12f;

    [Header("Explosion")]
    public float explosionForce = 8f;
    public GameObject explosionPrefab;

    private Rigidbody2D rb;
    private Transform player;
    private bool diving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        player = FindFirstObjectByType<PlayerMagnetController>()?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (!diving && dist < detectionRange)
            diving = true; // Start dash when player enters range

        if (!diving)
        {
            // Stay nearly still (small damping)
            rb.linearVelocity *= 0.95f;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        var playerMag = player.GetComponent<IMagnetic>();
        if (playerMag == null) return;

        // Apply magnetism every frame
        ApplyMagneticForce(playerMag);

        // Handle dive movement (physics-based)
        if (diving)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            rb.AddForce(dirToPlayer * diveSpeed, ForceMode2D.Force);
        }
    }

    public void ApplyMagneticForce(IMagnetic other)
    {
        if (other == null) return;

        Vector2 direction = (Vector2)((other as MonoBehaviour).transform.position - transform.position);
        float polarityMultiplier = (CurrentPolarity == other.CurrentPolarity) ? -1f : 1f;

        rb.AddForce(direction.normalized * magneticStrength * polarityMultiplier, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();

        if (collision.gameObject.GetComponent<PlayerMagnetController>() != null)
            GameManager.Instance.PlayerDied();
    }

    private void Explode()
    {
        TelemetryManager.Instance.OnEnemyDestroyed("Kamikaze");
        if (explosionPrefab)
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1.5f);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (var hit in hits)
        {

            if (hit.CompareTag("Enemy") || hit.CompareTag("EnemyProjectile"))
            {
                // If it's a brute, ignore damage
                if (hit.GetComponent<EnemyBrute>() != null)
                {
                    // Brutes do not take projectile damage
                    Destroy(gameObject); // projectile dies but brute lives
                    return;
                }

                Destroy(hit.gameObject);
                continue;
            }

            Rigidbody2D body = hit.attachedRigidbody;
            if (body != null && body != rb)
                body.AddForce((body.position - (Vector2)transform.position).normalized * explosionForce, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}
