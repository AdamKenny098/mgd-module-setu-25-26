using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMagnetController>();
        if (player != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }
}
