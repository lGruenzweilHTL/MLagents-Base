using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class TestingAgent : Agent
{
    [SerializeField] private Vector3 minPos;
    [SerializeField] private Vector3 maxPos;

    [SerializeField] private Transform target;
    
    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect target and own position
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        // Place somewhere random
        transform.localPosition = new Vector3(
            Random.Range(minPos.x, maxPos.x),
            Random.Range(minPos.y, maxPos.y),
            Random.Range(minPos.z, maxPos.z)
        );
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Move the agent
        var dirToGo = Vector3.zero;
        var action = actions.DiscreteActions;
        dirToGo.x = action[0] - 1; // Convert from range 0-2 to -1, 0, 1
        dirToGo.z = action[1] - 1;
        transform.localPosition += dirToGo * Time.deltaTime;

        // Try to get the agent to move closer to the target
        var distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);
        SetReward(1.0f / (distanceToTarget + 1.0f)); // Adding 1 to avoid division by zero

        // If the agent is close enough to the target, end the episode
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();

        // Example heuristic: use arrow keys to control the agent
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActionsOut[0] = 0; // Move left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            discreteActionsOut[0] = 2; // Move right
        }
        else
        {
            discreteActionsOut[0] = 1; // No movement in x direction
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[1] = 2; // Move up
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            discreteActionsOut[1] = 0; // Move down
        }
        else
        {
            discreteActionsOut[1] = 1; // No movement in z direction
        }
    }
}
