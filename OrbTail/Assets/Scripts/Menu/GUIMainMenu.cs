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

		multiplayer_button = GameObject.Find("MultiPlayerButton");
		
		single_player_button = GameObject.Find("SinglePlayerButton");

		
	}


	protected override void OnSelect(GameObject target) {
		if (target == single_player_button)
		{
			StartSinglePlayer();
			
		}
		else if (target == multiplayer_button)
		{
			StartMultiPlayer();
			
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
