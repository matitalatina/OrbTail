using UnityEngine;
using System.Collections;

public class GUIMainMenu : GUIMenuChoose {
	
	private GameObject multiplayer_button;
	private GameObject single_player_button;
	private GameObject master;
	
	// Use this for initialization
	public override void Start () {
		base.Start();

		master = GameObject.FindGameObjectWithTag(Tags.Master);

		if (master == null) {
			master = GameObject.Instantiate(Resources.Load("Prefabs/Master")) as GameObject;
		}

		multiplayer_button = GameObject.Find("MultiPlayerButton");
		
		single_player_button = GameObject.Find("SinglePlayerButton");

		if (SystemInfo.deviceType == DeviceType.Handheld) {
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
		}


		
	}


	protected override void OnSelect(GameObject target) {
		base.OnSelect(target);

		if (target == single_player_button)
		{
			StartSinglePlayer();
			
		}
		else if (target == multiplayer_button)
		{
			StartMultiPlayer();
			
		}
		else if (target.name == "ExitButton") {
			Application.Quit();
		}
		else if (target.name == "CreditsButton") {
			Application.LoadLevel("MenuCredits");
		}
	}
	
	
	private void StartSinglePlayer() {
		
		var builder = master.GetComponent<GameBuilder>();
		builder.Action = GameBuilder.BuildMode.SinglePlayer;
		
        Application.LoadLevel("MenuChooseGameMode");
		
	}
	
	private void StartMultiPlayer()
	{
		
		Application.LoadLevel("MenuServerClient");
		
	}


}
