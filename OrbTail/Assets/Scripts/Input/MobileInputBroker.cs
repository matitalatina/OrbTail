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

    private const float kAccelerationExponent = 4f;

    private const float kSteeringExponent = 2f;

	private const float kBoostThreshold = 3.0f;

	private const float kMaxOffsetAcceleration = 0.6f;

    public MobileInputBroker()
    {

        //Standard position, with the phone in landscape position and the bottom on the right.
		zOffsetAcceleration = 0f;
		Calibrate();
		HUDButtonsHandler hudButtonsHandler = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<HUDButtonsHandler>();
		hudButtonsHandler.EventOnMissileButtonSelect += OnPowerButtonSelect;

    }
	

	/// <summary>
	/// Returns the accelerometers' offset on z Axis (Acceleration)
	/// </summary>
	public float zOffsetAcceleration { get; private set; }

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
    public ICollection<int> FiredPowerUps
    {
        get { return fired_power_ups_; }
    }

    /// <summary>
    /// Set the actual offset used for the accelerometer's calculation
    /// </summary>
    public void Calibrate()
    {

        //AccelerometerOffset = Input.acceleration.normalized;
		zOffsetAcceleration = Mathf.Clamp(-Input.acceleration.z, -kMaxOffsetAcceleration, kMaxOffsetAcceleration);

    }

    public void Update()
    {

		//var delta = Vector3.Cross(AccelerometerOffset, Input.acceleration.normalized);
		//Acceleration = Mathf.Clamp(delta.x * kAccelerationExponent, -1f, 1f);
		Acceleration = Mathf.Clamp((-Input.acceleration.z - zOffsetAcceleration) * kAccelerationExponent, -1f, 1f);
		Steering = Mathf.Clamp(Input.acceleration.x * kSteeringExponent, -1f, 1f);
        //From -1.0f to 1.0f.
        //Steering = Mathf.Pow( Mathf.Clamp01( Mathf.Abs( direction.x ) ), 
        //                                      kSteeringExponent) * Mathf.Sign(direction.x);
		//Steering = Mathf.Clamp(delta.z * kSteeringExponent, -1f, 1f);

		if( fired_power_ups_.Count > 0){

			fired_power_ups_.Clear();

		}


		// TODO: to enhance 
		if (Input.acceleration.sqrMagnitude > kBoostThreshold) {

			fired_power_ups_.Add(PowerGroups.Passive);

		}


    }

	private void OnPowerButtonSelect(object sender, GameObject button) {
		fired_power_ups_.Add(PowerGroups.Main);
	}

    private IList<int> fired_power_ups_ = new List<int>();

}
