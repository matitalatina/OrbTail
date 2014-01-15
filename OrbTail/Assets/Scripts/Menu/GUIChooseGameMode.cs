using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIChooseGameMode : GUIMenuChoose {
	private GameBuilder builder;
	private Dictionary<string, GameObject> gameModeButtons = new Dictionary<string, GameObject>();
	private HostFetcher hostFetcher;
	private GameObject fetchMessage;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();

		manageRandomButton();

		if (builder.Action == GameBuilder.BuildMode.Client) {
			fetchMessage = Instantiate(Resources.Load("Prefabs/Menu/MenuLabel")) as GameObject;
			fetchMessage.GetComponent<TextMesh>().text = "looking for matches...";
			fetchMessage.transform.position = new Vector3(0, -2, 0);
			FetchGameModeButtons();
			DisableAllGameModeButtons();
			hostFetcher = builder.SetupFetcher();
			hostFetcher.EventGameFound += OnEventGameFound;
		}
	}


	protected override void OnSelect (GameObject target)
	{
		if (target.tag == Tags.GameModeSelector)
		{
			
			builder.GameMode = int.Parse(target.name);
			
			Application.LoadLevel("MenuChooseArena");
			
		}
		else if (target.tag == Tags.BackButton) {
			Application.LoadLevel("MenuMain");
		}
	}

	private void manageRandomButton() {
		GameObject randomButton = GameObject.Find("-1");

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

	private void OnEventGameFound(object sender, IEnumerable<int> game_modes) {
		foreach (int gameMode in game_modes) {
			gameModeButtons[gameMode.ToString()].SetActive(true);
		}

		if (game_modes.Count() > 0) {
			gameModeButtons["-1"].SetActive(true);
			fetchMessage.SetActive(false);
		}
		else {
			fetchMessage.GetComponent<TextMesh>().text = "no matches are found...";
		}

		hostFetcher.EventGameFound -= OnEventGameFound;
	}

	private void FetchGameModeButtons() {
		foreach (GameObject arenaButton in GameObject.FindGameObjectsWithTag(Tags.GameModeSelector)) {
			gameModeButtons.Add(arenaButton.name, arenaButton);
		}
	}

	private void DisableAllGameModeButtons() {
		foreach (KeyValuePair<string, GameObject> pair in gameModeButtons) {
			pair.Value.SetActive(false);
		}
	}
	
}
