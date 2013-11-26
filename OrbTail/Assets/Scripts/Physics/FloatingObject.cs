using UnityEngine;
using System.Collections;

public class FloatingObject : MonoBehaviour {
	
	public float hoverForce = 9.8f;
	public float hoverDistance = 5f;
	// public float smoothPitch = 5f;
	

	// Use this for initialization
	void Start () {
		ArenaDown = Vector3.down;
	}

	// TODO: change Vector3.up with gravity
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit hit;

		if(Physics.Raycast(transform.position, ArenaDown, out hit, hoverDistance * 2)) {
			//rigidbody.AddForce(Vector3.up * realHoverForce * (hoverDistance - hit.distance) - Physics.gravity);
			//this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(Vector3.Cross(hit.normal, -Vector3.Cross(hit.normal, this.transform.forward)), this.transform.forward), smoothPitch * Time.deltaTime);

			//this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.transform.rotation * Quaternion.AngleAxis(Vector3.Angle(hit.normal, this.transform.up), this.transform.right), smoothPitch * Time.deltaTime);
			if (hit.collider.gameObject.tag == Tags.Field) {
				rigidbody.AddForce(-ArenaDown * hoverForce * (hoverDistance - hit.distance) / hoverDistance, ForceMode.Acceleration);
			}
		}
	}

	public Vector3 ArenaDown { get; set; } 
}
