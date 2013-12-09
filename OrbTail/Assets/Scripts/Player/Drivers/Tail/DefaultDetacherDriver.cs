using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultDetacherDriver : IDetacherDriver {

//	private float force = 0.06f;

	public List<GameObject> DetachOrbs(int nOrbs, Tail tail) {
		List<GameObject> detachedOrbs = tail.DetachOrbs(nOrbs);

		return detachedOrbs;
	}

	public void Update() {}
	
}
