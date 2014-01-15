using UnityEngine;
using System.Collections;

public class GUIChooseArena : GUIMenuChoose {
	private GameBuilder builder;

	// Use this for initialization
	public override void Start () {
		base.Start();

		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		manageRandomButton();
	}


	protected override void OnSelect (GameObject target)
	{
		//The touch or the mouse collided with something
		if (target.tag == Tags.ArenaSelector)
		{
			
			builder.ArenaName = target.name;
			
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
}
