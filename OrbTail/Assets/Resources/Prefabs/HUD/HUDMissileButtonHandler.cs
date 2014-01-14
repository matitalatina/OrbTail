using UnityEngine;
using System.Collections;

public class HUDMissileButtonHandler : MonoBehaviour {
	private GameBuilder builder;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGameBuilt(object sender) {
		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		GameObject player = game.ActivePlayer;
		player.GetComponent<PowerController>().EventPowerAttached += EventPowerAttached;
		
		builder.EventGameBuilt -= OnGameBuilt;
	}

	private void EventPowerAttached(object sender, GameObject ship, Power power) {
		if (power.Group == PowerGroups.Main && power.Name == "Missile") {
			renderer.enabled = true;
			power.EventDestroyed += PowerDestroyed;
		}
	}

	private void PowerDestroyed(object sender, int group) {
		((Power) sender).EventDestroyed -= PowerDestroyed;

		if (group == PowerGroups.Main) {
			renderer.enabled = false;
		}
	}
}
