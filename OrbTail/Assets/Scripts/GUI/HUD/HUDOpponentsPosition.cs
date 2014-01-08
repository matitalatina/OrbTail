using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDOpponentsPosition : MonoBehaviour {
	private const string HUDIndicatorPrefabPath = "Prefabs/HUD/ShipIndicator";
	private const float boostAlpha = 0.9f;
	private const float distanceFromShip = 3f;
	private Transform myShipPosition;
	private bool gameStarted = false;
	private Dictionary<Transform, GameObject> positionShipAndIndicators;
	private Dictionary<GameObject, TextMesh> textMeshes;
	private Game game;

	// Use this for initialization
	void Start () {
		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
		positionShipAndIndicators = new Dictionary<Transform, GameObject>();
		textMeshes = new Dictionary<GameObject, TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameStarted) {
			foreach (KeyValuePair<Transform, GameObject> pair in positionShipAndIndicators) {
				GameObject indicator = pair.Value;
				Vector3 directionToOpponent = (pair.Key.position - myShipPosition.position).normalized;
				indicator.transform.localPosition = myShipPosition.position + directionToOpponent * distanceFromShip;
				indicator.transform.rotation = myShipPosition.rotation;

				float alpha = Mathf.Clamp(-Vector3.Dot(myShipPosition.forward, directionToOpponent) + boostAlpha, 0, 1);
				Color newColor = textMeshes[indicator].color;
				newColor.a = alpha;
				textMeshes[indicator].color = newColor;
			}
		}
	}

	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		game.EventStart += OnEventStart;
		game.EventShipEliminated += OnShipEliminated;
		GameObject myShip = game.ActivePlayer;
		myShipPosition = myShip.transform;

		foreach (GameObject ship in game.ShipsInGame) {
			if (ship != myShip) {
				addHUDIndicator(ship);
			}
		}
	}
	


	private void addHUDIndicator(GameObject ship) {
		GameObject indicator = (GameObject) Instantiate(Resources.Load(HUDIndicatorPrefabPath));
		TextMesh textMeshIndicator = indicator.GetComponent<TextMesh>();
		textMeshIndicator.color = ship.GetComponent<GameIdentity>().Color;
		textMeshes.Add(indicator, textMeshIndicator);
		positionShipAndIndicators.Add(ship.transform, indicator);
	}

	private void OnEventStart(object sender, int countdown) {
		if (countdown <= 0) {
			gameStarted = true;
		}
	}

	private void OnShipEliminated(object sender, GameObject ship) {
		if (ship == myShipPosition.gameObject) {

			foreach (KeyValuePair<Transform, GameObject> pair in positionShipAndIndicators) {
				Destroy(pair.Value);
			}

			positionShipAndIndicators = new Dictionary<Transform, GameObject>();
			textMeshes = new Dictionary<GameObject, TextMesh>();
			game.EventShipEliminated -= OnShipEliminated;
		}
		else {
			Transform transformShip = ship.transform;
			GameObject indicator = positionShipAndIndicators[transformShip];
			positionShipAndIndicators.Remove(transformShip);
			textMeshes.Remove(indicator);
			Destroy(indicator);
		}
	}

	private void OnEventEnd(object sender, GameObject winner, int info) {
		gameStarted = false;
	}
}
