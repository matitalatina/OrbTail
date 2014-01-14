using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour {
	private GameObject target;

	// Values used to create spring
	private float dampSpring = 0.4f;
	private float forceSpring = 3f;
	private float minDistance = 1.0f;
	private float maxDistance = 1.0f;
	private float shipDistance = 2.5f;
	private bool isLinked;



	// Use this for initialization
	void Start () {
		isLinked = this.gameObject.GetComponent<SpringJoint>() != null;
	}



	/// <summary>
	/// Determines whether this instance is attached to something.
	/// </summary>
	/// <returns><c>true</c> if this instance is attached; otherwise, <c>false</c>.</returns>
	public bool IsAttached() {
		return isLinked;
	}


	/// <summary>
	/// Links this instance to the passed parameter with a SpringJoint.
	/// </summary>
	/// <param name="destination">Target.</param>
	public void LinkTo(GameObject target) {
		Unlink();

		SpringJoint joint = this.gameObject.AddComponent<SpringJoint>();
		joint.connectedBody = target.rigidbody;
		joint.damper = dampSpring;
		joint.spring = forceSpring;
		joint.minDistance = minDistance;
		joint.maxDistance = maxDistance;

		if (target.tag == Tags.Ship) {
			joint.minDistance += shipDistance;
			joint.maxDistance += shipDistance;
		}

		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = Vector3.zero;
		joint.connectedAnchor = Vector3.zero;
		isLinked = true;
		
	}
	

	/// <summary>
	/// Unlink this instance removing the joint.
	/// </summary>
	public void Unlink() {

		foreach (SpringJoint jointToRemove in this.GetComponents<SpringJoint>()) {
            //TODO: remove this 

			Object.Destroy(jointToRemove);
		}

		isLinked = false;
		
	}




}
