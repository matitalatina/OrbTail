using UnityEngine;
using System.Collections;

public class DefaultEngineDriver : IEngineDriver {
	private int powerShip;
	private float actualForce;
	private float quickSmooth = 10f;
	private float smoothForce;
	private float lastInput;

	/// <summary>
	/// Initializes a new instance of the <see cref="DefaultEngineDriver"/> class.
	/// </summary>
	/// <param name="power">The power of the ship's engine. Range [1, 5].</param>
	public DefaultEngineDriver(int power) {
		powerShip = power;
		smoothForce = Mathf.Pow(power / 5f, 2f) * quickSmooth;
	}


	public virtual float GetForce() {
		return actualForce;
	}


	public int GetPower() {
		return powerShip;
	}

	public void Update(float inputAcceleration) {

		if (inputAcceleration >= 0 && inputAcceleration >= actualForce) {
			actualForce = Mathf.Lerp(actualForce, inputAcceleration, smoothForce * Time.deltaTime);
		}
		else if (inputAcceleration < 0 && inputAcceleration <= actualForce) {
			actualForce = Mathf.Lerp(actualForce, inputAcceleration, smoothForce * Time.deltaTime);
		}
		else {
			actualForce = inputAcceleration;
		}

	}
}
