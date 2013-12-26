using UnityEngine;
using System.Collections;

public class HUDMessageInGameHandler : MonoBehaviour {
	private TextMesh textMesh;


	// Use this for initialization
	void Start () {
		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;

		textMesh = GetComponent<TextMesh>();
	}

	private void OnGameBuilt(object sender) {
		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		GameObject activePlayer = game.ActivePlayer;
		PowerController powerController = activePlayer.GetComponent<PowerController>();
		powerController.EventPowerAttached += OnEventPowerAttached;

		iTween.FadeTo(gameObject, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnEventPowerAttached(object sender, GameObject ship, Power power) {
		textMesh.text = power.Name;
		iTween.FadeUpdate(gameObject, 0f, 0f);
		iTween.FadeFrom(gameObject, 1f, 4f);
	}
}
