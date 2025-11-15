using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MagneticSurface : MonoBehaviour, IMagnetic
{
    [SerializeField]
    private IMagnetic.Polarity currentPolarity = IMagnetic.Polarity.Red;

    public IMagnetic.Polarity CurrentPolarity
    {
        get => currentPolarity;
        set => currentPolarity = value;
    }

    //Optional extra pull/repel multiplier for this surface
    public float surfaceStrengthMultiplier = 1f;

    public void ApplyMagneticForce(IMagnetic other)
    {
        // Dummy implementation as surfaces don't move
    }
}
