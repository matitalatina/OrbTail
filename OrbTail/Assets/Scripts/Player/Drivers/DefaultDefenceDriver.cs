using UnityEngine;
using System.Collections;

public class DefaultDefenceDriver : IDefenceDriver {
	private int defenceShip;

	public DefaultDefenceDriver(int defence) {
		defenceShip = defence;
	}

	public int GetDefence() {
		return defenceShip;
	}

	public int DamageToOrbs(float damage) {
		// TODO: to implement
		return 0;
	}

	public void Update() {}
}
