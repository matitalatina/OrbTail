using UnityEngine;
using System.Collections;

public class DefaultOffenceDriver : IOffenceDriver {

	private int offenceShip;
	private float adjustedOffence;
	private float maxVelocity = 60f;
	private float maxNumberBalls = 14f;

	public DefaultOffenceDriver(int offence) {
		this.offenceShip = offence;
		this.adjustedOffence = offence / 5;
	}

	public int GetOffence() {
		return offenceShip;
	}

	// TODO: to check
	public float GetDamage(GameObject defender, Collision col) {
		float damage = col.relativeVelocity.magnitude / maxVelocity * maxNumberBalls * adjustedOffence;
		return damage;
	}

	public void Update() {}
}
