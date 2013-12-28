﻿using UnityEngine;
using System.Collections;

public class HUDBoostIndicatorHandler : MonoBehaviour {


	private PowerView boostView;
	private TextMesh textMesh;
	private float refreshTime = 0.2f;
	private const float animationTime = 1f;
    private Game game;
	// Use this for initialization
	void Start () {
		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		game.EventStart += OnEventStart;
		game.EventEnd += OnEnd;

        GameObject player = game.ActivePlayer;
        player.GetComponent<PowerController>().EventPowerAttached += HUDBoostIndicatorHandler_EventPowerAttached;
		textMesh = GetComponent<TextMesh>();
	}

    void HUDBoostIndicatorHandler_EventPowerAttached(object sender, GameObject ship, Power power)
    {

        if (power.Group == PowerGroups.Passive)
        {

            boostView = power;
            StartCoroutine("RefreshIndicator");

        }

    }


	private void OnEventEnd(object sender, GameObject winner) {
		StopCoroutine("RefreshIndicator");
	}

	private void OnEventStart(object sender, int countdown) {

		if (countdown <= 0) {

            textMesh.color = Color.green;
					
        }

	}

	private IEnumerator RefreshIndicator() {

		while (true) {
			float percentage = boostView.IsReady * 100f;
			textMesh.text = string.Format("∝ {0:0}%", percentage);
			textMesh.color = Color.Lerp(Color.red, Color.green, boostView.IsReady);
			yield return new WaitForSeconds(refreshTime);
		}

	}

	private void OnEnd(object sender, GameObject winner) {
		iTween.FadeTo(gameObject, 0f, animationTime);
	}




}
