using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIChooseGameMode : GUIMenuChoose {
	private Color enabledButtonColor = new Color(0, 0, 0, 1f);
	private Color disabledButtonColor = new Color(0, 0, 0, 0.1f);
	private GameBuilder builder;
	private Dictionary<string, GameObject> gameModeButtons = new Dictionary<string, GameObject>();
	private HostFetcher hostFetcher;
	private GameObject fetchMessage;
	private const float fetchDelay = 3.0f;

	// Use this for initialization
	public override void Start () {
		base.Start();
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();

		manageRandomButton();

		if (builder.Action == GameBuilder.BuildMode.Client) {

			fetchMessage = Instantiate(Resources.Load("Prefabs/Menu/MenuLabel")) as GameObject;
		
			fetchMessage.transform.position = new Vector3(0, -2, 0);
			FetchGameModeButtons();
			DisableAllGameModeButtons();
            hostFetcher = builder.GetFetcher();
			hostFetcher.EventGameFound += OnEventGameFound;
            hostFetcher.EventNoMaster += hostFetcher_EventNoMaster;

            FetchHosts();

		}
	}

    void hostFetcher_EventNoMaster(object sender)
    {

        fetchMessage.GetComponent<TextMesh>().text = "no master server...";

    }

    private void FetchHosts()
    {

        fetchMessage.GetComponent<TextMesh>().text = "looking for matches...";

        builder.SetupFetcher();

    }

    private IEnumerator FetchHostDelayed()
    {

        yield return new WaitForSeconds(fetchDelay);

        fetchMessage.GetComponent<TextMesh>().text = "retrying...";

        yield return new WaitForSeconds(fetchDelay);

        FetchHosts();

    }


	protected override void OnSelect (GameObject target)
	{
		if (target.tag == Tags.GameModeSelector)
		{
			
			builder.GameMode = int.Parse(target.name);

            StopCoroutine("FetchHostDelayed");

            if (hostFetcher != null)
            {

                hostFetcher.EventGameFound -= OnEventGameFound;

            }
            
			Application.LoadLevel("MenuChooseArena");
			
		}
		else if (target.tag == Tags.BackButton) {

            if (hostFetcher != null)
            {

                hostFetcher.enabled = false;
                Destroy(hostFetcher);

            }

            StopCoroutine("FetchHostDelayed");

            if (hostFetcher != null)
            {

                hostFetcher.EventGameFound -= OnEventGameFound;

            }

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
			setActiveButton(gameModeButtons[gameMode.ToString()], true);
		}

		if (game_modes.Count() > 0) {
			
            setActiveButton(gameModeButtons["-1"], true);
			fetchMessage.SetActive(false);
            hostFetcher.EventGameFound -= OnEventGameFound;

		}
		else {
			
            fetchMessage.GetComponent<TextMesh>().text = "no matches found...";

            StartCoroutine(FetchHostDelayed());

		}
       	
	}

	private void FetchGameModeButtons() {
		foreach (GameObject arenaButton in GameObject.FindGameObjectsWithTag(Tags.GameModeSelector)) {
			gameModeButtons.Add(arenaButton.name, arenaButton);
		}
	}

	private void DisableAllGameModeButtons() {
		foreach (KeyValuePair<string, GameObject> pair in gameModeButtons) {
			setActiveButton(pair.Value, false);
		}
	}

	private void setActiveButton(GameObject button, bool activated) {
		button.collider.enabled = activated;
		button.GetComponent<TextMesh>().color = (activated ? enabledButtonColor :  disabledButtonColor);
	}
	
}
