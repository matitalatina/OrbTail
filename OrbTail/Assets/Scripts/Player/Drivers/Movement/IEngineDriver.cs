using UnityEngine;
using System.Collections;

public interface IEngineDriver {

	/// <summary>
	/// Gets the force of the engine.
	/// </summary>
	/// <returns>The force. Range [-1, 1]</returns>
	float GetForce();


	/// <summary>
	/// Gets the power of the ship prototype.
	/// </summary>
	/// <returns>The power. Range [1, 5]</returns>
	int GetPower();

	void Update(float inputAcceleratio);
}
