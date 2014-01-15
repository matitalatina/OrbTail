using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAI : MonoBehaviour {
	
	private RelayInputBroker inputBroker = new RelayInputBroker();
	//private EventLogger eventLogger;
	private PowerController powerController;
	
	private bool gameBuilt = false;
	private GameObject target = null;
	private Vector3 desideredDirection;
	private bool alreadyCollided = false;
	
	private OrbController orbController;
	private FloatingObject floatingObject;
	private Game game;
	
	private float maxTimeToGoAway = 4f;
	private float maxTimeToFirePowerUp = 5f;
	private float maxVisibility = 60f;
	private float maxAcceleration = 0.9f;
	private int minOrbsToStartFight = 5;
	private float thresholdToGiveUp = 0.4f;
	private float sqrCheckpointDistanceThreshold = 15f;
	private float minTimeToGiveUp = 20f;
	private float maxTimeToGiveUp = 40f;

	private HashSet<GameObject> checkpoints;
	
	
	public IInputBroker GetInputBroker() {
		return inputBroker;
	}
	
	private void OnFieldOfViewEnter(object sender, Collider other) {
		GameObject colObject = other.gameObject;
		
		if (target == null || IsPatrolling()) {
			StopCoroutine("GiveUpHandler");

			if (colObject.tag == Tags.Ship && colObject.GetComponent<Tail>().GetOrbCount() >= minOrbsToStartFight) {
				target = colObject;
				StartCoroutine("GiveUpHandler");
			}
			else if (IsFreeOrb(colObject)) {
				target = colObject;
				orbController = colObject.GetComponent<OrbController>();
			}
		}
	}

	private IEnumerator GiveUpHandler() {
		yield return new WaitForSeconds(Random.Range(minTimeToGiveUp, maxTimeToGiveUp));
		ResetTarget();
	}
	
	// Use this for initialization
	void Start () {
		
		GameBuilder gameBuilder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		gameBuilder.EventGameBuilt += OnGameBuilt;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameBuilt) {
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

		/* Now there's OnShipEliminated
		if (target.activeSelf == false) {
			ResetTarget();
			return;
		} */

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
			StartCoroutine("decideWhetherContinueFight");
		}
		
	}
	
	void OnEventOrbAttached(object sender, GameObject orb, GameObject ship) {
		
		if (target == orb) {
			ResetTarget();
		}
		
	}
	
	private void OnGameBuilt(object sender) {
		floatingObject = GetComponent<FloatingObject>();	
		
		powerController = GetComponent<PowerController>();
		powerController.EventPowerAttached += OnEventPowerAttached;

		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventShipEliminated += OnShipEliminated;
		game.EventEnd += OnEventEnd;
		
		// Listen all event fights
		foreach (GameObject ship in game.ShipsInGame) {
			TailController tailController = ship.GetComponent<TailController>();
			tailController.OnEventFight += OnEventFight;
		}
		
		gameObject.GetComponent<Tail>().OnEventOrbAttached += OnEventOrbAttached;
		// Attaching field of view notification
		GetComponentInChildren<AIFieldOfView>().EventOnFieldOfViewEnter += OnFieldOfViewEnter;
		checkpoints = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag(Tags.AICheckpoint));

		
		gameBuilt = true;
		
	}

	private void OnEventEnd(object sender, GameObject winner, int info) {
		CleanAI();
	}

	private void OnShipEliminated(object sender, GameObject ship) {
		ship.GetComponent<TailController>().OnEventFight -= OnEventFight;

		if (ship == target) {
			ResetTarget();
		}
		else if (ship == gameObject) {
			CleanAI();
		}
	}
	
	private void CheckVisibility() {
		
		if (target != null && !IsPatrolling() && !IsVisible()) {
			ResetTarget();
		}
		
	}
	
	private IEnumerator decideWhetherContinueFight() {
		float timeToWait = Random.value * maxTimeToGoAway;
		yield return new WaitForSeconds(timeToWait);
		alreadyCollided = false;
		
		if (Random.value <= thresholdToGiveUp) {
			ResetTarget();
		}
	}
	
	private void OnEventPowerAttached(object sender, GameObject ship, Power power) {
		if (ship == gameObject) {
			StartCoroutine("FirePowerUp");
		}
	}
	
	private IEnumerator FirePowerUp() {
		inputBroker.FiredPowerUps.Clear();
		float timeToWait = Random.value * maxTimeToFirePowerUp;
		yield return new WaitForSeconds(timeToWait);
		inputBroker.FiredPowerUps.Add(PowerGroups.Main);
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

	private void CleanAI() {
		game.EventShipEliminated -= OnShipEliminated;
		game.EventEnd -= OnEventEnd;

		// Remove all event fights
		foreach (GameObject ship in game.ShipsInGame) {
			TailController tailController = ship.GetComponent<TailController>();
			tailController.OnEventFight -= OnEventFight;
		}
		
		gameObject.GetComponent<Tail>().OnEventOrbAttached -= OnEventOrbAttached;
		// Attaching field of view notification
		//GetComponentInChildren<AIFieldOfView>().EventOnFieldOfViewEnter -= OnFieldOfViewEnter;
	}
	
	
	
	
}
