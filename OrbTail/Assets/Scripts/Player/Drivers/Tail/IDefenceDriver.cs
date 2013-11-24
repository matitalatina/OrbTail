using UnityEngine;
using System.Collections;

public interface IDefenceDriver {
	int DamageToOrbs(float damage);
	int GetDefence();
	void Update();
}
