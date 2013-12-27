using UnityEngine;
using System.Collections;

public class DefaultOffenceDriver : IOffenceDriver {

	private int offenceShip;
	private float adjustedOffence;
	private float maxVelocity = 50f;
	private float maxNumberBalls = 14f;

	public DefaultOffenceDriver(int offence) {
		this.offenceShip = offence;
		this.adjustedOffence = Mathf.Sqrt(offence / 5f);
	}

	public int GetOffence() {
		return offenceShip;
	}

	// TODO: to check
	public float GetDamage(GameObject defender, Collision col) {
		float damage = (col.relativeVelocity.magnitude / maxVelocity) * maxNumberBalls * adjustedOffence;
		Debug.Log("Damage: " + damage);
		return damage;
	}

	public void Update() {}
}
