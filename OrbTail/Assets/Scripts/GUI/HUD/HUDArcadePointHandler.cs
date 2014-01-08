using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDArcadePointHandler : MonoBehaviour {

	private GameBuilder builder;
	private Game game;
	private const float xOrigin = -6f;
	private const float yOrigin = 5f;
	private const float distanceBetweenScoresY = 0.5f;
	private const float distanceBetweenScoresX = 2.5f;
	private const string pathPointsPrefab = "Prefabs/HUD/PointText";
	private Dictionary<GameIdentity, TextMesh> gameIdentityTextMeshes;



	// Use this for initialization
	void Start () {
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
		gameIdentityTextMeshes = new Dictionary<GameIdentity, TextMesh>();
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		game.EventStart += OnEventStart;
		game.EventShipEliminated += OnShipEliminated;
		
		foreach (GameObject ship in game.ShipsInGame) {
			AddScoreIndicator(ship);
		}
	}

	private void AddScoreIndicator(GameObject ship) {
		// Instantiate and place in the correct position
		GameObject textPoint = (GameObject) GameObject.Instantiate(Resources.Load(pathPointsPrefab));
		textPoint.transform.parent = gameObject.transform;
		textPoint.transform.localPosition = new Vector3(xOrigin + distanceBetweenScoresX * gameIdentityTextMeshes.Count, yOrigin, 0f);

		// Change color
		TextMesh textMesh = textPoint.GetComponent<TextMesh>();
		GameIdentity gameIdentity = ship.GetComponent<GameIdentity>();
		textMesh.color = gameIdentity.Color;

		// Attach event
		gameIdentity.EventScore += OnEventScore; 
		gameIdentityTextMeshes.Add(gameIdentity, textMesh);
	}

	private void OnEventStart(object sender, int countdown) {

	}
	
	private void OnShipEliminated(object sender, GameObject ship) {

	}
	
	private void OnEventEnd(object sender, GameObject winner, int info) {

	}

	private void OnEventScore(object sender, int delta_score, int total_score) {
		gameIdentityTextMeshes[(GameIdentity) sender].text = string.Format("{0:0000}",total_score);
	}
}
