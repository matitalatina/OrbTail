using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {

	private RelayInputBroker inputBroker = new RelayInputBroker();
	private GameObject target;
	private Vector3 desideredDirection;
	private bool alreadyCollided = false;
	private OrbController orbController;
	private float actualSteering;
	private FloatingObject floatingObject;

	public IInputBroker GetInputBroker() {
		return inputBroker;
	}

	// Use this for initialization
	void Start () {
		floatingObject = GetComponent<FloatingObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			if (target.tag == Tags.Ship) {
				if (!alreadyCollided) {
					ChasingPlayer();
				}
				else {
					GoAway();
				}
			}
			else {
				ChasingOrb();
			}
		}
		else {
			LookAround();
		}

		float steering = Vector3.Dot(-floatingObject.ArenaDown, Vector3.Cross(transform.forward, desideredDirection.normalized));

		//float steering = -Mathf.Sign(Vector3.Dot(-floatingObject.ArenaDown,
		//                                         Vector3.Cross(transform.forward, desideredDirection.normalized))) *
		//	(Vector3.Dot( transform.forward, desideredDirection.normalized ) - 1.0f);

		inputBroker.Steering = Mathf.Clamp(steering * 5f, -1f, 1f);
		inputBroker.Acceleration = 1f - Mathf.Clamp01(steering);
        
        Debug.Log(steering);

		AvoidOstacles();
	}

	void ChasingOrb() {
		if (!orbController.IsAttached()) {
			Vector3 relVector = target.transform.position - gameObject.transform.position;
			desideredDirection = relVector;
		}
		else {
			orbController = null;
			target = null;
		}
	}

	void GoAway() {
		Vector3 relVector = gameObject.transform.position - target.transform.position;
		desideredDirection = relVector;
	}

	void OnTriggerEnter(Collider other) {
		GameObject colObject = other.gameObject;

		if (target == null) {
			if (colObject.tag == Tags.Ship) {
				target = colObject;
			}
			//else if (IsFreeOrb(colObject)) {
			//	target = colObject;
			//	orbController = colObject.GetComponent<OrbController>();
			//}
		}
	}

	void ChasingPlayer() {
		Vector3 relVector = target.transform.position - gameObject.transform.position;
        
		desideredDirection = relVector;
	}

	void AvoidOstacles() {

	}

	void LookAround() {

	}


	private bool IsFreeOrb(GameObject orb) {
		return orb.tag == Tags.Orb && !orb.GetComponent<OrbController>().IsAttached();
	}

}
