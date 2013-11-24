using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultDetacherDriver : IDetacherDriver {

	public List<GameObject> DetachOrbs(int nOrbs, Tail tail) {
		return tail.DetachOrbs(nOrbs);
	}

	public void Update() {}
}
