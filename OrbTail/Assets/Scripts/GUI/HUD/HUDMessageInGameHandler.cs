using UnityEngine;
using System.Collections;

public class HUDMessageInGameHandler : MonoBehaviour {
	private TextMesh textMesh;
	private GameBuilder builder;
	private PowerController powerController;
	private const float fadeTime = 4f;
	private Game game;

	// Use this for initialization
	void Start () {
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;

		textMesh = gameObject.GetComponent<TextMesh>();
	}

	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		GameObject activePlayer = game.ActivePlayer;
		powerController = activePlayer.GetComponent<PowerController>();
		powerController.EventPowerAttached += OnEventPowerAttached;

		ChangeAlphaColor(0f);
		builder.EventGameBuilt -= OnGameBuilt;

		game.EventEnd += OnEventEnd;
		game.EventShipEliminated += OnShipEliminated;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void ShowText(string text) {
		textMesh.text = text;
		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", 1f,
			"to", 0f,
			"time", fadeTime,
			"onUpdate","ChangeAlphaColor"));
	}

	private void OnEventPowerAttached(object sender, GameObject ship, Power power) {
		ShowText(power.Name);
	}

	private void OnEventEnd(object sender, GameObject winner, int info) {
		CleanScript();
	}

	private void OnShipEliminated(object sender, GameObject ship) {
		if (ship == game.ActivePlayer) {
			ShowText("You are eliminated!");
		}
	}

	private void ChangeAlphaColor(float alpha) {
		Color color = textMesh.color;
		color.a = alpha;
		textMesh.color = color;
	}

	private void CleanScript() {
		powerController.EventPowerAttached -= OnEventPowerAttached;
		game.EventEnd -= OnEventEnd;
	}
}
