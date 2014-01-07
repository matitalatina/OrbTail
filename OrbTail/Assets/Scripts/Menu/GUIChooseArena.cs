using UnityEngine;
using System.Collections;

public class GUIChooseArena : GUIMenuChoose {
	private GameBuilder builder;

	// Use this for initialization
	public override void Start () {
		base.Start();

		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
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
}
