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
		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", 1f,
			"to", 0f,
			"time", fadeTime,
			"onUpdate","ChangeAlphaColor"));
		//iTween.FadeTo(gameObject, 0f, fadeTime);

		builder.EventGameBuilt -= OnGameBuilt;
	}

	private void OnEventEnd(object sender, GameObject winner, int info) {

		if (winner == null) {

            if (info == Game.kInfoServerLeft)
            {

                textMesh.text = "server left...";

            }
            else
            {

                textMesh.text = "tie...";

            }
            			
		}
		else if (winner == game.ActivePlayer) {

			textMesh.text = "You won!";
			textMesh.color = new Color(0, 255, 0, 0);

		}
		else {
			textMesh.text = winner.GetComponent<PlayerIdentity>().ShipName + " wins";
			textMesh.color = winner.GetComponent<GameIdentity>().Color;
		}

		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", 0f,
			"to", 1f,
			"time", fadeTime,
			"onUpdate","ChangeAlphaColor"));

		game.EventEnd -= OnEventEnd;

	}

	private void ChangeAlphaColor(float alpha) {
		Color color = textMesh.color;
		color.a = alpha;
		textMesh.color = color;
	}
}
