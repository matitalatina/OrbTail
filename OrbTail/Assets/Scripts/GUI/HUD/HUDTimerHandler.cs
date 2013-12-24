using UnityEngine;
using System.Collections;

public class HUDTimerHandler : MonoBehaviour {
	private TextMesh textMesh;

	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMesh>();
		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventTick += OnChangeTime;
		game.EventStart += OnStart;
	}
	
	// Update is called once per frame
	void Update () {
	
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
	}
}
