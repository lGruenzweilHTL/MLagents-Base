using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flag : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private CaptureTheFlagGameManager gameManager;
    [SerializeField]
    private CaptureTheFlagTeam team;
    
    [Header("Ground Snap")]
    [SerializeField] private LayerMask snapLayers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            CaptureTheFlagAgent agent = other.GetComponent<CaptureTheFlagAgent>();
            if (agent.team != team)
            {
                gameManager.OnFlagCaptured(agent, team);
                
                transform.SetParent(agent.transform);
                transform.localPosition = Vector3.up; // Place the flag above the agent
            }
        }
    }

    public void Drop()
    {
        transform.SetParent(null, true);
        
        // Snap to ground
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10f, snapLayers))
        {
            transform.position = hit.point;
        }
    }
}