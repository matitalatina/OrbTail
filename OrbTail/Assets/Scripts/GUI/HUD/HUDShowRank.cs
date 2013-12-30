using UnityEngine;
using System.Collections;

public class HUDShowRank : MonoBehaviour {

	private TextMesh textMesh;
	private Game game;
	private GameBuilder builder;

	private float fadeTime = 2f;

	// Use this for initialization
	void Start () {
		textMesh = gameObject.GetComponent<TextMesh>();

		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		textMesh.text = "Game mode: " + game.GameModeName;
		iTween.FadeTo(gameObject, 0f, fadeTime);

		builder.EventGameBuilt -= OnGameBuilt;
	}

	private void OnEventEnd(object sender, GameObject winner, int info) {

		if (winner == null) {
			textMesh.text = "Tie...";
			iTween.FadeTo(gameObject, 1f, fadeTime);
		}
		else if (winner == game.ActivePlayer) {
			textMesh.text = "You won!";
			iTween.ColorTo(gameObject, Color.green, fadeTime);
		}
		else {
			textMesh.text = winner.GetComponent<PlayerIdentity>().ShipName + " wins";
			iTween.FadeTo(gameObject, 1f, fadeTime);
		}

		game.EventEnd -= OnEventEnd;

	}
}
