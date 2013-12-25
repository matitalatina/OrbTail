using UnityEngine;
using System.Collections;

public class HUDTimerHandler : MonoBehaviour {
	private TextMesh textMesh;
	private const float animationTime = 1f;
	private const float factorScale = 0.05f;

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

		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventTick += OnChangeTime;
		game.EventStart += OnStart;
		game.EventEnd += OnEnd;
	}

	private void OnStart(object sender, int countdown) {
		if (countdown <= 0) {
			textMesh.color = Color.white;
		}

	}

	private void OnChangeTime(object sender, int timeLeft) {
		var min = timeLeft / 60;
		var sec = timeLeft % 60;
		textMesh.text = string.Format("{0:00}:{1:00}", min, sec);

		if (timeLeft <= 10 && timeLeft > 0) {
			iTween.ScaleFrom(gameObject, Vector3.one * factorScale, animationTime);
			iTween.ColorFrom(gameObject, Color.red, animationTime);
		}
	}

	private void OnEnd(object sender, GameObject winner) {
		iTween.FadeTo(gameObject, 0f, animationTime);
	}
}
