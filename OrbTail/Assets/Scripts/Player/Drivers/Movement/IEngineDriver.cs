using UnityEngine;
using System.Collections;

public interface IEngineDriver {
	float GetForce();
	int GetPower();
	void Update(float inputAcceleratio);
}
