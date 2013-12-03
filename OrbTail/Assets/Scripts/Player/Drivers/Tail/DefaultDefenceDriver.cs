using UnityEngine;
using System.Collections;

public class DefaultDefenceDriver : IDefenceDriver {
	private int defenceShip;
	private float adjustedDefence;

	public DefaultDefenceDriver(int defence) {
		defenceShip = defence;
		adjustedDefence = defence / 5f + 1f;
	}

	public int GetDefence() {
		return defenceShip;
	}
	

	public int DamageToOrbs(float damage) {
		return Mathf.FloorToInt(damage / adjustedDefence);
	}

	public void Update() {}
}
