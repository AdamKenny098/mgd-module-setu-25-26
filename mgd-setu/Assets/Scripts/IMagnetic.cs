using UnityEngine;

public interface IMagnetic
{
    enum Polarity
    {
        Red,
        Blue
    }

    public Polarity CurrentPolarity { get; set; }

    public Transform transform { get; }

    void ApplyMagneticForce(IMagnetic other);
}
