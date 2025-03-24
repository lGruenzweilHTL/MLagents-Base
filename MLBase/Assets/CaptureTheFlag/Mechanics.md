# Capture the Flag - Game Mechanics

## Rules

### Winning/Losing

A team wins, when a member captures the enemy flag and brings it back to their flag area.

### Out of bounds rules

When an agent goes out of bounds, it will respawn at a random position within its half of the arena.
If it is holding the flag, it will return back to its own area.

### Flag dropping rules

When an agent has the flag an another agent collides with said agent, the flag will be dropped on the spot with a little bit of random offset.
If the flag goes out of bounds, it will return bck to its own area.

## Agents

### Rewards

- **Team Winning/Losing**: The agent gets a reward when its team has won and a penalty when lost.
- **Winning**: The agent gets a larger reward when it brought back the flag by itself.
- **Taking the flag**:  The agent gets a reward when it captures the enemy flag.
- **Out of bounds**: The agent gets a penalty when going out of bounds.


### Movement Options
- XAxis: -1, 0, 1
- YAxis: -1, 0, 1
- Jump: 0, 1

All movement options are represented as discrete branches in the action space.

### Observations

Each agent is able to see the following values in addition to a FOV using Raycasts.

- The [state](#flag-states) of its own flag, allowing for a switch in playstyle when the flag is in danger.
- The direction to its own flag, allowing the agent to defend the flag.
- The distance to its own flag.
- The direction to the enemy flag.
- The distance to the enemy flag.
- The [state](#flag-states) of the enemy flag.
- Is the agent currently grounded?

Total observation space: **9**, as distance and direction can be represented as a single vector.

#### Raycast Obervations

The agent will have a FOV using Raycasts. These raycasts are **pointing down** slighty, so it can see the ground, but also forward.
The Raycasts will be able to identify the following objects (all objects need to have collision to be seen by the raycast):
- **Ground**
- **Elevation**, like stairs, ramps, etc.
- **Flag Podium**

#### Flag States

Each flag can be in one of the following states:

- **Safe**: The flag is not being carried by anyone and is in its own territory.
- **Danger**: The flag is not being carried by anyone and is in the enemy territory.
- **Carried**: An enemy is currently carrying the flag.
