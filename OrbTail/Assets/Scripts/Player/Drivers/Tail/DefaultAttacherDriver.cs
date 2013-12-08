using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultAttacherDriver : IAttacherDriver {
	//private float forceUp = 0.03f;

	public void AttachOrbs(GameObject orb, Tail tail) {
		//orb.rigidbody.AddForce(-orb.GetComponent<FloatingObject>().ArenaDown * forceUp, ForceMode.Impulse);
		tail.AttachOrb(orb);
	}

	public void AttachOrbs(List<GameObject> orbs, Tail tail) {
		foreach(GameObject orb in orbs) {
			AttachOrbs(orb, tail);
		}
	}

	public void Update() {}
}
