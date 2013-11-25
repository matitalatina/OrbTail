using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : IApproachListener {
	private GameObject owner;
	private Stack<GameObject> orbStack = new Stack<GameObject>();
	private GameObject firstOrb;

	// Values used to create spring
	private float dampSpring = 1f;
	private float forceSpring = 2f;
	private float minDistance = 1f;
	private float maxDistance = 1.5f;

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
		
		joint.connectedBody = destination.rigidbody;
		joint.damper = dampSpring;
		joint.spring = forceSpring;
		joint.maxDistance = maxDistance;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = Vector3.zero;
		joint.connectedAnchor = Vector3.zero;

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
