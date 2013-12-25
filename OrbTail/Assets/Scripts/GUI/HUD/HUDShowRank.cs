﻿using UnityEngine;
using System.Collections;

public class HUDShowRank : MonoBehaviour {

	private TextMesh textMesh;
	private Game game;

	private float fadeTime = 2f;

	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMesh>();

		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		textMesh.text = "Game mode: " + game.GameMode;
		iTween.FadeTo(gameObject, 0f, fadeTime);
	}

	private void OnEventEnd(object sender, GameObject winner) {

		if (winner == null) {
			textMesh.text = "Tie...";
			iTween.FadeTo(gameObject, 1f, fadeTime);
		}
		else if (winner == game.ActivePlayer) {
			textMesh.text = "You won!";
			iTween.ColorTo(gameObject, Color.green, fadeTime);
		}
		else {
			textMesh.text = "The winner is: " + winner.GetComponent<PlayerIdentity>().Name;
			iTween.FadeTo(gameObject, 1f, fadeTime);
		}

	}
}
