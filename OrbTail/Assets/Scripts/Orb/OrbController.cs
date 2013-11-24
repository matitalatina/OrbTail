using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour, IApproachable {
	private GameObject target;
	private IApproachListener listener;

	// Value used in approaching
	public float distanceThreshold = 30f;
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

		if ( Vector3.Distance(this.transform.position, target.transform.position) < distanceThreshold ) {
			GameObject destination = target;
			InterruptApproaching();
			listener.ApproachedTo(destination, this.gameObject);
			return;
		}

		Vector3 directionForce = (target.transform.position - this.transform.position).normalized;
		this.rigidbody.AddForce(directionForce * attachForce);
	}
}
