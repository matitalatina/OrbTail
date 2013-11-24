using UnityEngine;
using System.Collections;

public interface IWheelDriver {
	float GetDirection();
	int GetSteering();
	void Update();
}
