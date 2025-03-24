using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField]
    private CaptureTheFlagGameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        gameManager.OnObjectOutOfBounds(other.gameObject);
    }
}