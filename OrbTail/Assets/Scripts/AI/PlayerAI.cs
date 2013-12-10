using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAI : MonoBehaviour {

	private RelayInputBroker inputBroker = new RelayInputBroker();
	private EventLogger eventLogger;

	private GameObject target = null;
	private Vector3 desideredDirection;
	private bool alreadyCollided = false;

	private OrbController orbController;
	private FloatingObject floatingObject;

	private float maxTimeToGoAway = 4f;
	private float maxTimeToFirePowerUp = 5f;
	private float maxVisibility = 60f;
	private float maxAcceleration = 0.8f;
	private float thresholdToGiveUp = 0.4f;
	private float sqrCheckpointDistanceThreshold = 15f;

	private HashSet<GameObject> checkpoints;


	public IInputBroker GetInputBroker() {
		return inputBroker;
	}

	// Use this for initialization
	void Start () {
		floatingObject = GetComponent<FloatingObject>();
		eventLogger = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();
		eventLogger.EventFight += OnEventFight;
		eventLogger.EventOrbAttached += OnEventOrbAttached;
		eventLogger.EventPowerAttached += OnEventPowerAttached;
		checkpoints = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag(Tags.AICheckpoint));
	}
	
	// Update is called once per frame
	void Update () {
		CheckVisibility();

		if (target != null) {

			if (target.tag == Tags.Ship) {

				if (!alreadyCollided) {
					ChasingPlayer();
				}
				else {
					GoAway();
				}

			}
			else if (IsPatrolling()) {
				Patrolling();
			}
			else {
				ChasingOrb();
			}
		}
		else {
			ChangeCheckpoint();
		}

		AvoidOstacles();


		float steering = Vector3.Dot(-floatingObject.ArenaDown, Vector3.Cross(transform.forward, desideredDirection.normalized));

		//float steering = -Mathf.Sign(Vector3.Dot(-floatingObject.ArenaDown,
		//                                         Vector3.Cross(transform.forward, desideredDirection.normalized))) *
		//	(Vector3.Dot( transform.forward, desideredDirection.normalized ) - 1.0f);

		inputBroker.Steering = Mathf.Clamp(steering * 5f, -1f, 1f);
		inputBroker.Acceleration = Mathf.Min (maxAcceleration, 1f - Mathf.Clamp01(Mathf.Abs(steering)));
        


	}

	void ChasingOrb() {
		if (!orbController.IsAttached()) {
			Vector3 relVector = target.transform.position - gameObject.transform.position;
			desideredDirection = relVector;
		}
		else {
			ResetTarget();
		}
	}

	void GoAway() {
		Vector3 relVector = gameObject.transform.position - target.transform.position;
		desideredDirection = relVector;
	}



	void ChasingPlayer() {
		Vector3 relVector = target.transform.position - gameObject.transform.position;
        
		desideredDirection = relVector;
	}

	void AvoidOstacles() {

	}

	void Patrolling() {
		Vector3 relVector = target.transform.position - gameObject.transform.position;
		desideredDirection = relVector;

		if (desideredDirection.sqrMagnitude < sqrCheckpointDistanceThreshold) {
			ChangeCheckpoint();
		}
	}

	void OnEventFight(object sender, System.Collections.Generic.IList<GameObject> orbs, GameObject attacker, GameObject defender) {

		if (attacker == this.gameObject && defender == target) {
			alreadyCollided = true;
			StartCoroutine("decideWhetherContinue");
		}

	}

	void OnEventOrbAttached(object sender, GameObject orb, GameObject ship) {

		if (target == orb) {
			ResetTarget();
		}

	}

	void OnTriggerStay(Collider other) {
		GameObject colObject = other.gameObject;
		
		if (target == null || IsPatrolling()) {
			if (colObject.tag == Tags.Ship) {
				target = colObject;
			}
			else if (IsFreeOrb(colObject)) {
				target = colObject;
				orbController = colObject.GetComponent<OrbController>();
			}
		}
	}

	private void CheckVisibility() {

		if (target != null && !IsPatrolling() && !IsVisible()) {
			ResetTarget();
		}

	}

	private IEnumerator decideWhetherContinue() {
		float timeToWait = Random.value * maxTimeToGoAway;
		yield return new WaitForSeconds(timeToWait);
		alreadyCollided = false;

		if (Random.value <= thresholdToGiveUp) {
			ResetTarget();
		}
	}

	private void OnEventPowerAttached(object sender, Power power, GameObject ship) {
		Debug.Log("power attached");
		StartCoroutine("FirePowerUp");
	}

	private IEnumerator FirePowerUp() {
		inputBroker.FiredPowerUps.Clear();
		float timeToWait = Random.value * maxTimeToFirePowerUp;
		yield return new WaitForSeconds(timeToWait);
		inputBroker.FiredPowerUps.Add(MainPowerGroup.Instance.groupID);
	}

	private bool IsFreeOrb(GameObject orb) {
		return orb.tag == Tags.Orb && !orb.GetComponent<OrbController>().IsAttached();
	}

	private bool IsVisible() {
		if (target != null) {
			Vector3 relVector = target.transform.position - transform.position;
			float distance = relVector.magnitude;
			// If raycast collide something -> not visible
			return distance < maxVisibility && !Physics.Raycast(transform.position, relVector, distance, Layers.FieldAndObstacles);
		}

		return false;
	}

	private bool IsPatrolling() {
		return checkpoints.Contains(target);
	}

	private void ChangeCheckpoint() {
		var enumerator = checkpoints.GetEnumerator();

		int i = 0;
		int chosenNumber = Random.Range(0, checkpoints.Count);
		GameObject newTarget = null;

		while (i < chosenNumber) {
			newTarget = enumerator.Current;
			enumerator.MoveNext();
			i++;
		}

		target = newTarget;
	}

	private void ResetTarget() {
		target = null;
		orbController = null;
	}
	



}
