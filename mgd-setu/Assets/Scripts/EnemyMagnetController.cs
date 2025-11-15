using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMagnetController : MonoBehaviour, IMagnetic
{
    [SerializeField]
    private IMagnetic.Polarity currentPolarity = IMagnetic.Polarity.Blue;

    public IMagnetic.Polarity CurrentPolarity
    {
        get => currentPolarity;
        set => currentPolarity = value;
    }


    [Header("Magnet Settings")]
    public float magneticStrength = 10f;
    public float massMultiplier = 1f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyMagneticForce(IMagnetic other)
    {
        if (other == null) return;

        //“Vector from this object to the other object
        Vector2 direction = (Vector2)((other as MonoBehaviour).transform.position - transform.position);

        // Same polarity = repel, opposite = attract
        float polarityMultiplier = (CurrentPolarity == other.CurrentPolarity) ? -1f : 1f;

        // Apply the magnetic force
        rb.AddForce(direction.normalized * magneticStrength * polarityMultiplier * massMultiplier, ForceMode2D.Force);
    }
}
