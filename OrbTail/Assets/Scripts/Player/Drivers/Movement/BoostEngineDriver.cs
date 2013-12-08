using UnityEngine;
using System.Collections;

public class BoostEngineDriver : DefaultEngineDriver {

    public BoostEngineDriver(int power) : base(power) { }

	public override float GetForce() {
		return base.GetForce() * 4;
	}
    
}
