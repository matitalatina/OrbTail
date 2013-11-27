using UnityEngine;
using System.Collections;

public interface IWheelDriver {

	/// <summary>
	/// Gets the direction.
	/// </summary>
	/// <returns>A float that represents the steering of the ship. Range [-1, 1].</returns>
	/// <param name="inputSteer">Steering from an input device</param>
	float GetDirection(float inputSteer);

	/// <summary>
	/// Gets the steering of the ship prototype. Range [1, 5].
	/// </summary>
	/// <returns>The steering of the ship prototype.</returns>
	int GetSteering();


	void Update();
}
