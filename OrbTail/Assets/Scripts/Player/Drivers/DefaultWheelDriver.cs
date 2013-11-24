using UnityEngine;
using System.Collections;

public class DefaultWheelDriver : IWheelDriver {
	private int steeringShip;

	public DefaultWheelDriver(int Steering) {
		steeringShip = Steering;
	}

	public virtual float GetDirection() {
		// TODO: to implement
		return Input.GetAxis("Horizontal") * steeringShip / 5f;
	}

	public int GetSteering() {
		return steeringShip;
	}

	public void Update() {

	}



}
