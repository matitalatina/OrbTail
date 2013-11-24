using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour, IApproachable {
	private GameObject target;
	private IApproachListener listener;

	// Value used in approaching
	public float distanceToReach = 3f;
	public float distanceThreshold = 0.5f;
	public float attachForce = 10f;
	public float attachDrag = 10f;


	// Backup values of rigidbody before approaching
	private float prevDrag;

	// Use this for initialization
	void Start () {
		prevDrag = this.rigidbody.drag;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (IsApproaching()) {
			performApproach();
		}

	}

	public void ApproachTo(GameObject targetObject, IApproachListener listener) {
		this.target = targetObject;
		this.listener = listener;
		this.prevDrag = this.rigidbody.drag;
		this.rigidbody.drag = attachDrag;
	}

	public bool IsApproaching() {
		return target != null;
	}

	public void InterruptApproaching() {
			this.rigidbody.drag = prevDrag;
			listener = null;
			target = null;
	}

	private void performApproach() {

		Vector3 pointToReach = target.transform.position - target.transform.forward * distanceToReach;
		float distance = Vector3.Distance(this.transform.position, pointToReach);
		Vector3 directionForce;

		if (distance > distanceThreshold) {
			directionForce = (pointToReach - this.transform.position).normalized;
		}
		else {
			// TODO: see if better to call InterruptApproaching()
			listener.ApproachedTo(target, this.gameObject);
			InterruptApproaching();
			return;
		}

		this.rigidbody.AddForce(directionForce * attachForce, ForceMode.Acceleration);
	}
}
