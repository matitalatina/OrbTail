﻿using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour {
	private GameObject target;
	private IApproachListener listener;

	// Values used to create spring
	private float dampSpring = 1f;
	private float forceSpring = 2f;
	private float minDistance = 0.8f;
	private float maxDistance = 1f;



	// Use this for initialization
	void Start () {
	}



	/// <summary>
	/// Determines whether this instance is attached to something.
	/// </summary>
	/// <returns><c>true</c> if this instance is attached; otherwise, <c>false</c>.</returns>
	public bool IsAttached() {
		return this.gameObject.GetComponent<SpringJoint>() != null;
	}


	/// <summary>
	/// Links this instance to the passed parameter with a SpringJoint.
	/// </summary>
	/// <param name="destination">Target.</param>
	public void LinkTo(GameObject target) {
		SpringJoint joint = this.GetComponent<SpringJoint>();

		if (joint != null) {
			Object.Destroy(joint);
		}
		
		joint = this.gameObject.AddComponent<SpringJoint>();
		
		joint.connectedBody = target.rigidbody;
		joint.damper = dampSpring;
		joint.spring = forceSpring;
		joint.minDistance = minDistance;
		joint.maxDistance = maxDistance;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = Vector3.zero;
		joint.connectedAnchor = Vector3.zero;
		
	}
	

	/// <summary>
	/// Unlink this instance removing the joint.
	/// </summary>
	public void Unlink() {
		SpringJoint joint = this.GetComponent<SpringJoint>();
		
		if (joint != null) {
			Object.Destroy(joint);
		}
		
	}




}
