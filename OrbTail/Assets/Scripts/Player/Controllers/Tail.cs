using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : IApproachListener {
	private GameObject owner;
	private Stack<GameObject> orbStack = new Stack<GameObject>();
	private GameObject firstOrb;
	private Vector3 distanceBetweenOrbs = Vector3.forward;

	// Values used to create spring
	public float dampSpring = 10f;
	public float forceSpring = 10f;

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
		orb.GetComponent<OrbController>().ApproachTo(target, this);


	}


	public List<GameObject> DetachOrbs(int nOrbs) {
		List<GameObject> detachedOrbs = new List<GameObject>();

		int i = 0;

		while (i < nOrbs && orbStack.Count <= 0) {
			GameObject orbToDetach = orbStack.Pop();
			CleanOrb(orbToDetach);
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

	public void ApproachedTo(GameObject destination, GameObject caller) {
		SpringJoint joint = caller.GetComponent<SpringJoint>();
		if (joint != null) {
			Object.Destroy(joint);
		}

		joint = caller.AddComponent<SpringJoint>();
		joint.damper = dampSpring;
		joint.spring = forceSpring;
		joint.connectedBody = destination.rigidbody;
	}


	private void CleanOrb(GameObject orb) {
		SpringJoint joint = orb.GetComponent<SpringJoint>();

		if (joint != null) {
			Object.Destroy(joint);
		}

		OrbController orbController = orb.GetComponent<OrbController>();
		if (orbController.IsApproaching()) {
			orbController.InterruptApproaching();
		}

	}


}
