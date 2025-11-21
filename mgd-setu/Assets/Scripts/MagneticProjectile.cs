using UnityEngine;

public class MagneticProjectile : MonoBehaviour, IMagnetic
{
    public enum SourceType
    {
        Player,
        Enemy
    }

    [Header("Projectile Settings")]
    public SourceType source = SourceType.Enemy;
    public float speed = 6f;
    public float damage = 10f;
    public float lifeTime = 4f;
    public bool canBeReflected = true;

    [Header("Magnetism")]
    public float magneticStrength = 8f;   // base force
    public float maxMagneticDistance = 8f; // beyond this, magnetism is very weak

    [SerializeField]
    private IMagnetic.Polarity currentPolarity = IMagnetic.Polarity.Red;
    public IMagnetic.Polarity CurrentPolarity
    {
        get => currentPolarity;
        set => currentPolarity = value;
    }

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Transform player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerMagnetController>()?.transform;
        Destroy(gameObject, lifeTime);
    }

    public void Fire(Vector2 direction)
    {
        moveDir = direction.normalized;
        rb.linearVelocity = moveDir * speed;
    }

    public void Deflect(Vector2 newDir)
    {
        if (!canBeReflected) return;

        source = SourceType.Player;
        Fire(newDir);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        var playerMag = player.GetComponent<IMagnetic>();
        if (playerMag == null) return;

        ApplyMagneticForce(playerMag);
    }

    public void ApplyMagneticForce(IMagnetic other)
    {
        Vector2 toPlayer = (Vector2)((other as MonoBehaviour).transform.position - transform.position);
        float distance = toPlayer.magnitude;

        if (distance > maxMagneticDistance)
            return; // too far for meaningful magnetism

        float polarityMultiplier = (CurrentPolarity == other.CurrentPolarity) ? -1f : 1f;

        // Strength falls off smoothly with distance
        float falloff = 1f - Mathf.Clamp01(distance / maxMagneticDistance);

        rb.AddForce(toPlayer.normalized * magneticStrength * polarityMultiplier * falloff,
                    ForceMode2D.Force);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Player gets hit
        var player = col.GetComponent<PlayerMagnetController>();
        if (player != null)
        {
            GameManager.Instance.PlayerDied();
            Destroy(gameObject);
            return;
        }

        // Enemy gets hit (always valid now, per your choice B)
        if (col.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
            return;
        }

        // Walls kill projectiles
        if (col.CompareTag("Wall") || col.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }
    }
}
