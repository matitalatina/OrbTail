using UnityEngine;
using System.Collections;

public class JammedWheelDriver : DefaultWheelDriver {

	public JammedWheelDriver(int steering) : base(steering) {}

	public override float GetDirection()
	{
		return -base.GetDirection();
	}

}
