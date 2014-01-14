using UnityEngine;
using System.Collections;

public class GUIChooseGameMode : GUIMenuChoose {
	private GameBuilder builder;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();

		manageRandomButton();
	}


	protected override void OnSelect (GameObject target)
	{
		if (target.tag == Tags.GameModeSelector)
		{
			
			builder.GameMode = int.Parse(target.name);
			
			Application.LoadLevel("MenuChooseArena");
			
		}
		else if (target.tag == Tags.BackButton) {
			Destroy(GameObject.FindGameObjectWithTag(Tags.Master));
			Application.LoadLevel("MenuMain");
		}
	}

	private void manageRandomButton() {
		GameObject randomButton = GameObject.Find("-1");

		switch (builder.Action) {
		case GameBuilder.BuildMode.RemoteGuest:
				randomButton.GetComponent<TextMesh>().text = "Any";
			break;
		case GameBuilder.BuildMode.RemoteHost:
			randomButton.renderer.enabled = false;
			randomButton.collider.enabled = false;
			break;
		}
	}
}
