using UnityEngine;

public class EnemyBrute : MonoBehaviour
{
    public float moveSpeed = 2f;

    public Transform groundCheck;
    public float groundCheckDistance = 0.3f;

    public Transform wallCheck;
    public float wallCheckDistance = 0.2f;

    public LayerMask groundLayer; 
    public LayerMask hazardLayer;

    private Rigidbody2D rb;
    private int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3;
        rb.freezeRotation = true;

        // Ignore hazard collisions
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Hazard"), true);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Check ground ahead (ignore hazards)
        bool noGroundAhead = !Physics2D.Raycast(
            groundCheck.position, 
            Vector2.down, 
            groundCheckDistance, 
            groundLayer    // hazards NOT included
        );

        // Check wall ahead (ignore hazards)
        bool wallAhead = Physics2D.Raycast(
            wallCheck.position, 
            new Vector2(direction, 0f), 
            wallCheckDistance, 
            groundLayer    // hazards NOT included
        );

        if (noGroundAhead || wallAhead)
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        direction *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PlayerMagnetController>() != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerMagnetController>() != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }
}
