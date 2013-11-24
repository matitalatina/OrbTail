using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultAttacherDriver : IAttacherDriver {

	public void AttachOrbs(GameObject orb, Tail tail) {
		tail.AttachOrb(orb);
	}

	public void AttachOrbs(List<GameObject> orbs, Tail tail) {
		foreach(GameObject orb in orbs) {
			AttachOrbs(orb, tail);
		}
	}

	public void Update() {}
}
