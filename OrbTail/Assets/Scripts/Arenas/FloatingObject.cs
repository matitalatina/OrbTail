using UnityEngine;
using System.Collections;

public class FloatingObject : MonoBehaviour {

    public float hoverForce = 20.0f;//9.8f;
	public float hoverDistance = 5f;
	public float hoverDampen = 0f;
    private GravityField gravity_field;

	public Vector3 ArenaDown { get; set; } 

	private Rigidbody FloatingBody{ get; set; }

	// Use this for initialization
	void Start () {

        ArenaDown = Vector3.zero;
		FloatingBody = GetComponent<Rigidbody>();
        gravity_field = GameObject.FindGameObjectWithTag(Tags.Arena).GetComponent<GravityField>();

	}

	
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit hit;

		//TODO: Uncomment for spherical arena :D
		//ArenaDown = -transform.position.normalized;
		//

        gravity_field.SetGravity(this);

		if(Physics.Raycast(transform.position, ArenaDown, out hit, Layers.Field)) {

			if (hit.collider.gameObject.tag == Tags.Field) {

				rigidbody.AddForce(-ArenaDown * (hoverForce * (hoverDistance - hit.distance) - 
				                                 hoverDampen * (Vector3.Dot(FloatingBody.velocity, -ArenaDown))), 
				                   ForceMode.Acceleration);

			}

		}
		else {

			rigidbody.AddForce(ArenaDown * hoverForce, ForceMode.Acceleration);

		}
	}


}
