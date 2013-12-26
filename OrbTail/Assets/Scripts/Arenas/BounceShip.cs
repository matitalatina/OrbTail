using UnityEngine;
using System.Collections;

public class BounceShip : MonoBehaviour {
	private float bounceForce = 20f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collider = collision.gameObject;

		if (collider.tag == Tags.Ship) {
			Vector3 directionForce = -collision.contacts[0].normal;
			collider.rigidbody.AddForce(directionForce * bounceForce, ForceMode.Impulse);
		}
	}
}
