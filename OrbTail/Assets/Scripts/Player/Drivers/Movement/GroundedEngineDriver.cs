using UnityEngine;
using System.Collections;

public class GroundedEngineDriver : DefaultEngineDriver {

	public GroundedEngineDriver(int power) : base(power) {
	}

	public override float GetForce() {
		return base.GetForce() / 2;
	}

}
