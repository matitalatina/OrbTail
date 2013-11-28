using UnityEngine;
using System.Collections;

public interface IOffenceDriver {


	/// <summary>
	/// Gets the offence of the ship prototype.
	/// </summary>
	/// <returns>The offence of the ship prototype.</returns>
	int GetOffence();


	/// <summary>
	/// Gets the damage calculated using the defender ship and the collision.
	/// </summary>
	/// <returns>The damage calculated which is a qualitative number orbs to detach
	/// without considering the defence of the defender.</returns>
	/// <param name="defender">The defender GameObject.</param>
	/// <param name="col">The collision.</param>
	float GetDamage(GameObject defender, Collision col);


	void Update();

}
