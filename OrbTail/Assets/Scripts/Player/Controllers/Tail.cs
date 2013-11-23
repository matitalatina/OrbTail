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

	// TODO: add logic to attach orbs to each other
	public void AttachOrb(GameObject orb) {
		if (orbStack.Count <= 0) {
			firstOrb = orb;
		}

		orbStack.Push(orb);
	}

	// TODO: add logic to detach orbs to each other
	public List<GameObject> DetachOrbs(int nOrbs) {
		List<GameObject> detachedOrbs = new List<GameObject>();

		int i = 0;

		while (i < nOrbs && orbStack.Count <= 0) {
			detachedOrbs.Add(orbStack.Pop());
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
