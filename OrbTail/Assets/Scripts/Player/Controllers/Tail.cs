using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail {
	private GameObject owner;
	private Stack<GameObject> orbStack = new Stack<GameObject>();
	private GameObject firstOrb;
	

	public Tail(GameObject owner) {
		this.owner = owner;
	}

	// TODO: to implement
	public void AttachToPlayer(GameObject player) {

	}


	public void AttachOrb(GameObject orb) {
		GameObject target;

		if (orbStack.Count <= 0) {
			target = owner;
			firstOrb = orb;
		} 
		else {
			target = orbStack.Peek();
		}

		orbStack.Push(orb);
		//orb.GetComponent<OrbController>().ApproachTo(target, this);
		orb.GetComponent<OrbController>().LinkTo(target);


	}


	public List<GameObject> DetachOrbs(int nOrbs) {
		List<GameObject> detachedOrbs = new List<GameObject>();

		int i = 0;

		while (i < nOrbs && orbStack.Count <= 0) {
			GameObject orbToDetach = orbStack.Pop();
			orbToDetach.GetComponent<OrbController>().Unlink();
			detachedOrbs.Add(orbToDetach);
		}

		if (orbStack.Count <= 0) {
			firstOrb = null;
		}

		return detachedOrbs;
	}

	public int GetOrbCount() {
		return orbStack.Count;
	}

	// TODO: to implement
	public void Update() {

	}




}
