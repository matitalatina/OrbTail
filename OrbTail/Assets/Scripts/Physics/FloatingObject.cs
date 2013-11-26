using UnityEngine;
using System.Collections;

public class FloatingObject : MonoBehaviour {
	
	public float hoverForce = 9.8f;
	public float hoverDistance = 5f;
	public float hoverDampen = 0f;

	public Vector3 ArenaDown { get; set; } 

	private Rigidbody FloatingBody{ get; set; }

	// Use this for initialization
	void Start () {
	
		ArenaDown = Vector3.down;
		FloatingBody = GetComponent<Rigidbody>();

	}


	// TODO: change Vector3.up with gravity
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit hit;

		//TODO: Uncomment for spherical arena :D
		//ArenaDown = -transform.position.normalized;
		//

		if(Physics.Raycast(transform.position, ArenaDown, out hit)) {

			if (hit.collider.gameObject.tag == Tags.Field) {

				rigidbody.AddForce(-ArenaDown * (hoverForce * (hoverDistance - hit.distance) - 
				                                 hoverDampen * (Vector3.Dot(FloatingBody.velocity, -ArenaDown))), 
				                   ForceMode.Acceleration);

			}

		}
	}


}
