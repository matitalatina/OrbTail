using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultDetacherDriver : IDetacherDriver {

//	private float force = 0.06f;

	public List<GameObject> DetachOrbs(int nOrbs, Tail tail) {
		List<GameObject> detachedOrbs = tail.DetachOrbs(nOrbs);

		//if (detachedOrbs.Count > 0) {

			//Vector3 upVector = -detachedOrbs[0].GetComponent<FloatingObject>().ArenaDown;
			//Vector3 lateralVector = detachedOrbs[0].transform.right;

			//foreach(GameObject orb in detachedOrbs) {
			//	orb.rigidbody.AddForce(Random.onUnitSphere * force, ForceMode.Impulse);
			//}

		//}

		return detachedOrbs;
	}

	public void Update() {}
	
}
