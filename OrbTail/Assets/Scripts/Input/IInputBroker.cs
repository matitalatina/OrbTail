using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Brokes the input of various players
/// </summary>
public interface IInputBroker
{

    /// <summary>
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    float Acceleration { get; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    float Steering { get; }

    /// <summary>
    /// Returns a collection which indicates all the power ups the user wants to fire. The elements indicates just the group of the proper power
    /// </summary>
    ICollection<int> FiredPowerUps { get; }

    /// <summary>
    /// Updates the broker
    /// </summary>
    void Update();

}
