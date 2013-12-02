using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {

	private RelayInputBroker inputBroker = new RelayInputBroker();
	private EventLogger eventLogger;

	private GameObject target;
	private Vector3 desideredDirection;
	private bool alreadyCollided = false;
	private OrbController orbController;
	private float actualSteering;
	private FloatingObject floatingObject;

	private float maxTimeToGoAway = 4f;

	public IInputBroker GetInputBroker() {
		return inputBroker;
	}

	// Use this for initialization
	void Start () {
		floatingObject = GetComponent<FloatingObject>();
		eventLogger = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();
		eventLogger.EventFight += OnEventFight;
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

		AvoidOstacles();


		float steering = Vector3.Dot(-floatingObject.ArenaDown, Vector3.Cross(transform.forward, desideredDirection.normalized));

		//float steering = -Mathf.Sign(Vector3.Dot(-floatingObject.ArenaDown,
		//                                         Vector3.Cross(transform.forward, desideredDirection.normalized))) *
		//	(Vector3.Dot( transform.forward, desideredDirection.normalized ) - 1.0f);

		inputBroker.Steering = Mathf.Clamp(steering * 5f, -1f, 1f);
		inputBroker.Acceleration = 1f - Mathf.Clamp01(Mathf.Abs(steering));
        


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

	void OnEventFight(object sender, System.Collections.Generic.IList<GameObject> orbs, GameObject attacker, GameObject defender) {
		if (attacker == this.gameObject && defender == target) {
			alreadyCollided = true;
			StartCoroutine("stopGoAway");
		}
	}

	private IEnumerator stopGoAway() {
		float timeToWait = Random.value * maxTimeToGoAway;
		yield return new WaitForSeconds(timeToWait);
		alreadyCollided = false;
	}

	private bool IsFreeOrb(GameObject orb) {
		return orb.tag == Tags.Orb && !orb.GetComponent<OrbController>().IsAttached();
	}



}
