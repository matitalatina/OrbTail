using UnityEngine;
using System.Collections;

public class DefaultDefenceDriver : IDefenceDriver {
	private int defenceShip;
	private float adjustedDefence;

	public DefaultDefenceDriver(int defence) {
		defenceShip = defence;
		adjustedDefence = defence / 5 + 1;
	}

	public int GetDefence() {
		return defenceShip;
	}
	

	public int DamageToOrbs(float damage) {
		return Mathf.FloorToInt(damage / adjustedDefence);
	}

	public void Update() {}
}
