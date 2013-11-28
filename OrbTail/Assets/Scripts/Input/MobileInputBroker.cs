using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Reads the data from a mobile platform
/// </summary>
public class MobileInputBroker: IInputBroker
{

    private const float kAccelerationExponent = 0.2f;

    private const float kSteeringExponent = 0.5f;

    public MobileInputBroker()
    {

        //Standard position, with the phone in landscape position and the bottom on the right.
        AccelerometerOffset = Vector3.zero;
		Calibrate();

    }

    /// <summary>
    /// Returns the accelerometers' offset
    /// </summary>
    public Vector3 AccelerometerOffset { get; private set; }

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
        get { return fired_power_ups_; }
    }

    /// <summary>
    /// Set the actual offset used for the accelerometer's calculation
    /// </summary>
    public void Calibrate()
    {

        AccelerometerOffset = Input.acceleration.normalized;

    }

    public void Update()
    {

        var direction = Input.acceleration.normalized - AccelerometerOffset;

        //From 0.0f to 1.0f. The power here is useful to avoid extreme angles
        Acceleration = Mathf.Pow( Mathf.Clamp01(-direction.z), kAccelerationExponent );

        //From -1.0f to 1.0f.
        Steering = Mathf.Pow( Mathf.Clamp01( Mathf.Abs( direction.x ) ), 
                                              kSteeringExponent) * Mathf.Sign(direction.x);

    }

    private IList<IGroup> fired_power_ups_ = new List<IGroup>();

}
