using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMagnetController : MonoBehaviour, IMagnetic
{
    public IMagnetic.Polarity CurrentPolarity { get; set; } = IMagnetic.Polarity.Red;

    [Header("Movement / Magnet Settings")]
    public float magneticStrength = 60f;
    public float detectionRadius = 8f;
    public LayerMask magneticLayer;
    public float maxSpeed = 15f;

    [Header("Bounds")]
    public float minY = -6f;
    public float maxY = 6f;
    public float leftKillX = -2f; // if pushed too far left, die

    Rigidbody2D rb;
    PlayerInput playerInput;
    InputAction polarityAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        polarityAction = playerInput.actions["Polarity"];

        if (polarityAction != null)
        {
            polarityAction.performed += OnPolarityPerformed;
            polarityAction.Enable();
        }
    }

    void OnDisable()
    {
        if (polarityAction != null)
        {
            polarityAction.performed -= OnPolarityPerformed;
            polarityAction.Disable();
        }
    }

    void OnPolarityPerformed(InputAction.CallbackContext ctx)
    {
        if (IsPointerOverUI()) return;
        FlipPolarity();
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        Vector2 pos;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            pos = Touchscreen.current.primaryTouch.position.ReadValue();
        else if (Mouse.current != null)
            pos = Mouse.current.position.ReadValue();
        else
            return false;

        var data = new PointerEventData(EventSystem.current) { position = pos };
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, hits);
        return hits.Count > 0;
    }

    void FixedUpdate()
    {
        var pos = rb.position;

        // Kill if swept too far left by terrain
        if (pos.x < leftKillX)
        {
            GameManager.Instance.PlayerDied();
            return;
        }

        // Kill if out of vertical bounds
        if (pos.y < minY - 0.1f || pos.y > maxY + 0.1f)
        {
            GameManager.Instance.PlayerDied();
            return;
        }

        // Magnetic interaction
        var hits = Physics2D.OverlapCircleAll(pos, detectionRadius, magneticLayer);
        foreach (var hit in hits)
        {
            if (!hit) continue;

            IMagnetic other = hit.GetComponent<IMagnetic>();
            if (other != null && other != this)
                other.ApplyMagneticForce(this);
        }

        // Clamp overall speed (mainly affects vertical)
        var vel = rb.linearVelocity;
        if (vel.magnitude > maxSpeed)
            vel = vel.normalized * maxSpeed;

        // We do NOT lock X here – let physics handle bumps, leftKillX handles crushes
        rb.linearVelocity = vel;
    }

    public void ApplyMagneticForce(IMagnetic other)
    {
        Vector2 direction = (Vector2)(other.transform.position - transform.position);
        direction.x = 0f; //vertical only

        float polarityMultiplier = (CurrentPolarity == other.CurrentPolarity) ? -0.5f : 1f;
        rb.AddForce(direction.normalized * magneticStrength * polarityMultiplier, ForceMode2D.Force);
    }

    public void FlipPolarity()
    {
        CurrentPolarity = (CurrentPolarity == IMagnetic.Polarity.Red)
            ? IMagnetic.Polarity.Blue
            : IMagnetic.Polarity.Red;

        AudioManager.Instance.PlayMagnetFlip();

        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.color = (CurrentPolarity == IMagnetic.Polarity.Red) ? Color.red : Color.blue;

        float nudge = (CurrentPolarity == IMagnetic.Polarity.Red) ? -1f : 1f;
        rb.AddForce(Vector2.up * nudge * 5f, ForceMode2D.Impulse);
    }
}
