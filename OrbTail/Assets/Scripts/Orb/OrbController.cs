using UnityEngine;
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


	public bool IsAttached() {
		return this.gameObject.GetComponent<SpringJoint>() != null;
	}

	public void LinkTo(GameObject destination) {
		SpringJoint joint = this.GetComponent<SpringJoint>();

		if (joint != null) {
			Object.Destroy(joint);
		}
		
		joint = this.gameObject.AddComponent<SpringJoint>();
		
		joint.connectedBody = destination.rigidbody;
		joint.damper = dampSpring;
		joint.spring = forceSpring;
		joint.minDistance = minDistance;
		joint.maxDistance = maxDistance;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = Vector3.zero;
		joint.connectedAnchor = Vector3.zero;
		
	}
	
	
	public void Unlink() {
		SpringJoint joint = this.GetComponent<SpringJoint>();
		
		if (joint != null) {
			Object.Destroy(joint);
		}
		
	}




}
