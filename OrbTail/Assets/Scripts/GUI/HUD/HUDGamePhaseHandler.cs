using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDGamePhaseHandler : MonoBehaviour {

	private TextMesh textMeshCountdown;
	private Light mainLight;
	private float initialLightPower = 0.01f;
	private float blankOverlayFinalAlpha = 0.8f;
	private int fontBigSize = 130;
	private float standardLightPower;
	private GameBuilder builder;
	private Game game;
	private GameObject blankOverlay;
	
	// Use this for initialization
	void Start () {
		mainLight = GameObject.FindGameObjectWithTag(Tags.MainLight).GetComponent<Light>();
		standardLightPower = mainLight.intensity;
		mainLight.intensity = initialLightPower;

		textMeshCountdown = gameObject.GetComponent<TextMesh>();

		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
		blankOverlay = Instantiate(Resources.Load("Prefabs/HUD/BlankOverlay")) as GameObject;
		blankOverlay.renderer.material.color = new Color(0,0,0,0);
		blankOverlay.transform.parent = transform;
		blankOverlay.transform.localPosition = Vector3.forward * 40f;
		blankOverlay.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventStart += OnStart;
		game.EventEnd += OnGameOver;

		builder.EventGameBuilt -= OnGameBuilt;
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
			iTween.ValueTo(this.gameObject, iTween.Hash(
				"from", 1f,
				"to", 0f,
				"time", 2f,
				"onUpdate","ChangeAlphaColor"));
		}

	}

	private void ChangeLightIntensity(float intensity) {
		mainLight.intensity = intensity;
	}

	private void ChangeAlphaColor(float alpha) {
		Color color = textMeshCountdown.color;
		color.a = alpha;
		textMeshCountdown.color = color;
	}

	private void OnGameOver(object sender, GameObject winner, int info) {
		blankOverlay.SetActive(true);
		iTween.FadeTo(blankOverlay, blankOverlayFinalAlpha, 2f);
		textMeshCountdown.text = "Game Over";
		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", 0f,
			"to", 1f,
			"time", 2f,
			"onUpdate","ChangeAlphaColor"));
		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", standardLightPower,
			"to", initialLightPower,
			"onUpdate","ChangeLightIntensity"));

		CleanScript();

	}

	private void CleanScript() {
		game.EventStart -= OnStart;
		game.EventEnd -= OnGameOver;
	}
}
