using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {

	private RelayInputBroker inputBroker = new RelayInputBroker();
	private GameObject target;
	private Vector3 desideredDirection;
	private bool alreadyCollided;

	public IInputBroker GetInputBroker() {
		return inputBroker;
	}

	// Use this for initialization
	void Start () {
		inputBroker.Acceleration = 1f;
		inputBroker.Steering = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			if (target.tag == Tags.Ship) {
				if (!alreadyCollided) {
					Chasing();
				}
				else {
					GoAway();
				}
			}
			else {
				Chasing();
			}
		}
		else {
			LookAround();
		}

		AvoidOstacles();

		inputBroker.Acceleration = desideredDirection.z;
		inputBroker.Steering = desideredDirection.x;
	}

	void Chasing() {
		Vector3 relVector = target.transform.position - gameObject.transform.position;
		desideredDirection = relVector.normalized;
	}

	void GoAway() {
		Vector3 relVector = gameObject.transform.position - target.transform.position;
		desideredDirection = relVector.normalized;
	}

	void OnTriggerEnter(Collider other) {
		GameObject colObject = other.gameObject;

		if (target == null) {
			if (colObject.tag == Tags.Ship || IsFreeOrb(colObject)) {
				target = colObject;
			}
		}
	}

	void AvoidOstacles() {

	}

	void LookAround() {

	}


	private bool IsFreeOrb(GameObject orb) {
		return orb.tag == Tags.Orb && !orb.GetComponent<OrbController>().IsAttached();
	}

}
