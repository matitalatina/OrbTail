using UnityEngine;
using System.Collections;

public class MenuSoundtrackHandler : MonoBehaviour {
	private const float battleSoundVolume = 0.3f;
	private const float menuSoundVolume = 0.4f;
	private const float fadeTime = 1f;
	private const float longFadeTime = 4f;

	private GameBuilder builder;
	private GameObject battleSound;
	private Game game;

	// Use this for initialization
	void Start () {
		builder = GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
		battleSound = transform.Find("BattleMusic").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGameBuilt(object sender) {
		iTween.AudioTo(gameObject, iTween.Hash(
			"volume", 0f,
			"time", fadeTime,
			"onComplete", "StartBattleMusic"
			));
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnMatchEnd;
	}

	private void OnMatchEnd(object sender, GameObject winner, int info) {
		iTween.AudioTo(battleSound, iTween.Hash(
			"volume", 0f,
			"time", longFadeTime
			));
	}

	private void StartBattleMusic() {
		battleSound.audio.volume = 0.3f;
		battleSound.audio.Play();
	}
}
