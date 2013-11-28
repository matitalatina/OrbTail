using UnityEngine;
using System.Collections;

public interface IDefenceDriver {

	/// <summary>
	/// Given a damage that is a qualitative value about how much orbs have to be detached
	/// it returns the numbers of orbs to detach from the ship.
	/// </summary>
	/// <returns>The real number of orbs to detach considering the defence of the ship.</returns>
	/// <param name="damage">Damage: qualitative value about how much orbs have to be detached
	/// without considering the defence of the ship.</param>
	int DamageToOrbs(float damage);


	/// <summary>
	/// Gets the defence of the ship prototype.
	/// </summary>
	/// <returns>The defence of the ship prototype.</returns>
	int GetDefence();


	void Update();
}
