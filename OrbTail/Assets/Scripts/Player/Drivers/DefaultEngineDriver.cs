using UnityEngine;
using System.Collections;

public class DefaultEngineDriver : IEngineDriver {
	private int powerShip;

	public DefaultEngineDriver(int power) {
		powerShip = power;
	}

	public virtual float GetForce() {
		// TODO: implement
		return 2f;
	}

	public int GetPower() {
		return powerShip;
	}

	public void Update() {

	}
}
