using UnityEngine;
using System.Collections;

public interface IOffenceDriver {

	int GetOffence();
	float GetDamage(GameObject defender, Collision col);
	void Update();

}
