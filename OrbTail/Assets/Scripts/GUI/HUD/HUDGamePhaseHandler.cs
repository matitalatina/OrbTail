﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDGamePhaseHandler : MonoBehaviour {

	private TextMesh textMeshCountdown;
	private Light mainLight;
	private float initialLightPower = 0.01f;
	private int fontBigSize = 150;
	private float standardLightPower;
	
	// Use this for initialization
	void Start () {
		mainLight = GameObject.FindGameObjectWithTag(Tags.MainLight).GetComponent<Light>();
		standardLightPower = mainLight.intensity;
		mainLight.intensity = initialLightPower;

		textMeshCountdown = GetComponent<TextMesh>();

		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGameBuilt(object sender) {
		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventStart += OnStart;
		game.EventEnd += OnGameOver;
	}
	
	private void OnStart(object sender, int countdown) {

		if (countdown > 0) {
			textMeshCountdown.text = countdown.ToString();
		}
		else {
			iTween.ValueTo(this.gameObject, iTween.Hash(
				"from", initialLightPower,
				"to", standardLightPower,
				"onUpdate","ChangeLightIntensity"));
			textMeshCountdown.color = Color.red;
			textMeshCountdown.fontSize = fontBigSize;
			textMeshCountdown.text = "GO!";
			iTween.FadeTo(this.gameObject, 0f, 2f);
		}

	}

	private void ChangeLightIntensity(float intensity) {
		mainLight.intensity = intensity;
	}

	private void OnGameOver(object sender, GameObject winner) {
		textMeshCountdown.text = "Game Over";
		iTween.FadeTo(this.gameObject, 1f, 2f);
		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", standardLightPower,
			"to", initialLightPower,
			"onUpdate","ChangeLightIntensity"));

	}
}