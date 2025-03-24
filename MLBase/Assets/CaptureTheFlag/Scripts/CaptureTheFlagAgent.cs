using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CaptureTheFlagAgent : Agent
{
    [Header("References")]
    [SerializeField]
    private CaptureTheFlagMovement movement;
    [SerializeField]
    private CaptureTheFlagGameManager gameManager;
    public CaptureTheFlagTeam team;
    private CaptureTheFlagTeam enemyTeam => team == CaptureTheFlagTeam.Red 
        ? CaptureTheFlagTeam.Blue 
        : CaptureTheFlagTeam.Red;

    public override void Initialize()
    {
        
    }

    public override void OnEpisodeBegin()
    {
        transform.position = gameManager.GetPosition(team);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Own flag: State, direction and distance
        sensor.AddObservation((int)GetFlagState(team));
        sensor.AddObservation(DirectionDist(transform.position, GetFlag(team).transform.position));
        
        // Enemy flag: State, direction and distance
        sensor.AddObservation((int)GetFlagState(enemyTeam));
        sensor.AddObservation(DirectionDist(transform.position, GetFlag(enemyTeam).transform.position));
        
        sensor.AddObservation(movement.IsGrounded);
    }

    public override void OnActionReceived(ActionBuffers buffers)
    {
        var vectorAction = buffers.DiscreteActions;
        
        var movementVector = new Vector3(vectorAction[0] - 1, 0, vectorAction[1] - 1);
        movement.Move(movementVector);

        if (vectorAction[2] > 0)
        {
            movement.Jump();
        }
        
        // Reward for moving towards the flag
        var flagDirection = DirectionDist(transform.position, GetFlag(enemyTeam).transform.position);
        var movementDirection = movementVector.normalized;
        var reward = Vector3.Dot(flagDirection.normalized, movementDirection);
        AddReward(reward);
        
        // Reward for being near the flag
        if (flagDirection.magnitude < 2f)
        {
            AddReward(10f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var vectorAction = actionsOut.DiscreteActions;
        
        vectorAction[0] = (int)Input.GetAxis("Horizontal") + 1;
        vectorAction[1] = (int)Input.GetAxis("Vertical") + 1;
        vectorAction[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
    
    private GameObject GetFlag(CaptureTheFlagTeam team) => team == CaptureTheFlagTeam.Red 
        ? gameManager.redFlag.gameObject 
        : gameManager.blueFlag.gameObject;
    
    private FlagState GetFlagState(CaptureTheFlagTeam team) => team == CaptureTheFlagTeam.Red 
        ? gameManager.redFlagState 
        : gameManager.blueFlagState;
    
    private Vector3 DirectionDist(Vector3 src, Vector3 dest) => dest - src;
}