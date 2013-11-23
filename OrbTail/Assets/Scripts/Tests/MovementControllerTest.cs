using UnityEngine;
using System.Collections;

public class MovementControllerTest : MonoBehaviour {
	public float torqueForce = 1f;
	public float speedForce = 1f;
	public float maxRoll = 30f;
	public float rollSmooth = 30f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//this.rigidbody.AddTorque(Vector3.up * Input.GetAxis("Horizontal") * torqueForce);
		this.rigidbody.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * speedForce);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + Input.GetAxis("Horizontal") * torqueForce, -Input.GetAxis("Horizontal") * maxRoll), rollSmooth * Time.deltaTime);


	}


}
