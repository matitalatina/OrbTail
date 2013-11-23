using UnityEngine;
using System.Collections;

public class DefaultWheelDriver : IWheelDriver {
	private int steeringShip;

	public DefaultWheelDriver(int Steering) {
		steeringShip = Steering;
	}

	public virtual float GetDirection() {
		// TODO: to implement
		return 0f;
	}

	public int GetSteering() {
		return steeringShip;
	}

	public void Update() {

	}



}
