using UnityEngine;
using System.Collections;

public class JammedWheelDriver : DefaultWheelDriver {

	public JammedWheelDriver(int steering) : base(steering) {}

	public override float GetDirection(float inputSteer)
	{
		return -base.GetDirection(inputSteer);
	}

}
