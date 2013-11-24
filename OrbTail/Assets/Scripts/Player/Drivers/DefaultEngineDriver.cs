using UnityEngine;
using System.Collections;

public class DefaultEngineDriver : IEngineDriver {
	private int powerShip;
	private float actualForce;
	private float quickSmooth = 10f;
	private float smoothForce;
	private float lastInput;

	public DefaultEngineDriver(int power) {
		powerShip = power;
		smoothForce = Mathf.Pow(power / 5f, 2f) * quickSmooth;
	}

	public virtual float GetForce() {
		// TODO: implement
		return actualForce;
	}

	public int GetPower() {
		return powerShip;
	}

	public void Update() {
		float input = Input.GetAxis("Vertical");

		if (input >= actualForce) {
			actualForce = Mathf.Lerp(actualForce, input, smoothForce * Time.deltaTime);
		}
		else {
			actualForce = input;
		}

	}
}
