using UnityEngine;
using System.Collections;

public class GUIChooseArena : GUIMenuChoose {
	private GameBuilder builder;
	private float disabledAlpha = 0.1f;

	// Use this for initialization
	public override void Start () {
		base.Start();

		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		manageRandomButton();

		if (builder.Action == GameBuilder.BuildMode.Client) {
			ManageClientArenaButtons();
		}
	}


	protected override void OnSelect (GameObject target)
	{
		//The touch or the mouse collided with something
		if (target.tag == Tags.ArenaSelector)
		{
			
			builder.ArenaName = target.name;
			
			Application.LoadLevel("MenuChooseShip");
			
		}
		else if (target.name == "Any") {
			GameObject[] gameModesButtons = GameObject.FindGameObjectsWithTag(Tags.ArenaSelector);
			int randomArenaNumber = Random.Range(0, gameModesButtons.Length - 1);
			builder.ArenaName = gameModesButtons[randomArenaNumber].name;
			Application.LoadLevel("MenuChooseShip");
		}
		else if (target.tag == Tags.BackButton) {
			Application.LoadLevel("MenuChooseGameMode");
		}
	}


	private void manageRandomButton() {
		GameObject randomButton = GameObject.Find("Arenas/Any");
		
		switch (builder.Action) {
		case GameBuilder.BuildMode.Client:
			randomButton.GetComponent<TextMesh>().text = "Any";
			break;
		case GameBuilder.BuildMode.Host:
			randomButton.renderer.enabled = false;
			randomButton.collider.enabled = false;
			break;
		}
	}

	private void ManageClientArenaButtons() {
		HostFetcher hostFetcher = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<HostFetcher>();
		bool oneIsActive = false;

		foreach (GameObject arenaButton in GameObject.FindGameObjectsWithTag(Tags.ArenaSelector)) {
			if (!hostFetcher.HasArena(arenaButton.name)) {
				arenaButton.collider.enabled = false;
				arenaButton.renderer.material.color = new Color(255, 255, 255, disabledAlpha);
			}
			else {
				oneIsActive = true;
			}
		}

		if (!oneIsActive) {
			GameObject.Find("Arenas/Any").SetActive(false);
		}
	}
}
