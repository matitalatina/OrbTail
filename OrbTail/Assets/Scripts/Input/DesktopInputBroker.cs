using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Reads the input data from a desktop platform
/// </summary>
class DesktopInputBroker: IInputBroker
{

    /// <summary>
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    public float Acceleration { get; private set; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    public float Steering { get; private set; }

    /// <summary>
    /// Returns a collection which indicates all the power ups the user wants to fire. The elements indicates just the group of the proper power
    /// </summary>
    public ICollection<IGroup> FiredPowerUps
    {
        get { return fired_power_ups; }
    }

    public void Update()
    {

        Acceleration = Input.GetAxis("Vertical");
        Steering = Input.GetAxis("Horizontal");

        //Debug.Log(Acceleration + " & " + Steering);

        //TODO: Add the actual code for powerup firing
        fired_power_ups = new List<IGroup>();

    }

    private IList<IGroup> fired_power_ups;

}

