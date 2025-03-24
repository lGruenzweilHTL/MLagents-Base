/// <summary>
/// Enum representing the two teams in the Capture The Flag game.
/// </summary>
public enum CaptureTheFlagTeam
{
    Red,
    Blue
}

/// <summary>
/// Enum representing the different Flag states in the Capture The Flag game.
/// </summary>
public enum FlagState
{
    /// <summary>
    /// The flag is in its own territory
    /// </summary>
    Safe,
    
    /// <summary>
    /// The flag is in the enemy territory
    /// </summary>
    Danger,
    
    /// <summary>
    /// The flag is being carried by an enemy agent
    /// </summary>
    Captured
}