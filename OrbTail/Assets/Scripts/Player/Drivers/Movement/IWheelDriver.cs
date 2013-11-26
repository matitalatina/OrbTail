using UnityEngine;
using System.Collections;

public interface IWheelDriver {
	float GetDirection(float inputSteer);
	int GetSteering();
	void Update();
}
