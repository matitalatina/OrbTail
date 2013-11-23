using UnityEngine;
using System.Collections;

public class MoveByGravity : MonoBehaviour {
	public float torqueForce = 1f;
	public float speedForce = 1f;
	public float maxRoll = 30f;
	public float rollSmooth = 30f;

	public Vector3 Gravity {get; set;}

	// Use this for initialization
	void Start () {
		Gravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.rigidbody.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * speedForce);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(Vector3.Cross(this.Gravity, this.transform.right), -this.Gravity) * Quaternion.AngleAxis(Input.GetAxis("Horizontal") * torqueForce, -this.Gravity), rollSmooth * Time.deltaTime);
	}

	void Update() {
		if (Input.GetKeyUp(KeyCode.H)) {
			Gravity += -Vector3.right; 
			Debug.Log("Gravità: sinistra" + Gravity.ToString());
		}
		if (Input.GetKeyUp(KeyCode.J)) {
			Gravity += Vector3.right; 
			Debug.Log("Gravità: destra" + Gravity.ToString());
		}

	}
}
