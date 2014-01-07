using UnityEngine;
using System.Collections;

public class GUIChooseGameMode : GUIMenuChoose {
	private GameBuilder builder;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
	}


	protected override void OnSelect (GameObject target)
	{
		if (target.tag == Tags.GameModeSelector)
		{
			
			builder.GameMode = int.Parse(target.name);
			
			Application.LoadLevel("MenuChooseArena");
			
		}
	}
}
