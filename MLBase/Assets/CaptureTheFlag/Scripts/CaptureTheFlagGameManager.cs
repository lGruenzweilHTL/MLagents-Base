using System;
using Unity.MLAgents;
using UnityEngine;

/// <summary>
/// This class is responsible for controlling the Capture The Flag game.
/// It will manage the game state, player scores, and basic rules.
/// It will also be responsible for rewarding and penalizing the agents.
/// </summary>
public class CaptureTheFlagGameManager : MonoBehaviour
{
    [Header("References")] 
    public Flag redFlag;
    public Flag blueFlag;
    [SerializeField]
    private Agent[] redAgents, blueAgents;
    
    [Header("Spawn Settings")]
    [SerializeField] private Bounds redSpawnArea;
    [SerializeField] private Bounds blueSpawnArea;

    private Vector3 redFlagInitialPosition, blueFlagInitialPosition;
    
    [NonSerialized]
    public FlagState redFlagState = FlagState.Safe, blueFlagState = FlagState.Safe;

    private void Start()
    {
        redFlagInitialPosition = redFlag.transform.position;
        blueFlagInitialPosition = blueFlag.transform.position;
        
        Academy.Instance.OnEnvironmentReset += ResetGame;
    }

    /// <summary>
    /// Reset the flag positions and randomize the agent positions.
    /// </summary>
    public void ResetGame()
    {
        /*redFlag.transform.position = redFlagInitialPosition;
        blueFlag.transform.position = blueFlagInitialPosition;

        foreach (var agent in redAgents)
        {
            //agent.EndEpisode();
            agent.transform.position = GetRandomPosition(redSpawnArea);
        }
        
        foreach (var agent in blueAgents)
        {
            //agent.EndEpisode();
            agent.transform.position = GetRandomPosition(blueSpawnArea);
        }*/
    }

    public Vector3 GetPosition(CaptureTheFlagTeam team) => team == CaptureTheFlagTeam.Red
        ? GetRandomPosition(redSpawnArea)
        : GetRandomPosition(blueSpawnArea);

    private Vector3 GetRandomPosition(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    /// <summary>
    /// Call this when an object has entered the out-of-bounds area.
    /// The method will reset the object to an appropriate position.
    /// </summary>
    /// <param name="obj">The object that has entere the out-of-bounds area</param>
    public void OnObjectOutOfBounds(GameObject obj)
    {
        if (obj.TryGetComponent<Agent>(out var a))
        {
            a.SetReward(-1f);
            a.EndEpisode();
        }
    }
    
    public void OnFlagCaptured(CaptureTheFlagAgent agent, CaptureTheFlagTeam team)
    {
        agent.AddReward(100f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(redSpawnArea.center, redSpawnArea.size);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(blueSpawnArea.center, blueSpawnArea.size);
    }
}