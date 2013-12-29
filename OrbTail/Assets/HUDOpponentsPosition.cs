using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDOpponentsPosition : MonoBehaviour {
	private const string HUDIndicator = "•";
	private Transform myShipPosition;
	private Dictionary<Transform, GameObject> positionsAndIndicators;

	// Use this for initialization
	void Start () {
		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGameBuilt(object sender) {
		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		GameObject myShip = game.ActivePlayer;
		myShipPosition = myShip.transform;

		foreach (GameObject ship in game.ShipsInGame) {
			if (ship != myShip) {
				prepareHUDIndicator(ship);
			}
		}
	}


	private void prepareHUDIndicator(GameObject ship) {

	}

	private void OnEventEnd(object sender, GameObject winner, int info) {
		

		
	}
}
