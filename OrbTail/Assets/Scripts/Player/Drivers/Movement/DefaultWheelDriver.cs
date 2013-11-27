using UnityEngine;
using System.Collections;

public class DefaultWheelDriver : IWheelDriver {
	private int steeringShip;
	private float adjustedSteer;
	

	/// <summary>
	/// Initializes a new instance of the <see cref="DefaultWheelDriver"/> class.
	/// </summary>
	/// <param name="steering">The steering of the ship prototype. Range [1, 5].</param>
	public DefaultWheelDriver(int steering) {
		steeringShip = steering;
		adjustedSteer = Mathf.Sqrt(steering / 5f);
	}

	
	public virtual float GetDirection(float inputSteer) {
		return inputSteer * adjustedSteer;
	}



	public int GetSteering() {
		return steeringShip;
	}

	public void Update() {

	}



}
