﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HUDTutorialHandler : GUIMenuChoose
{
		private GameBuilder builder;
		private HUDPositionHandler HUDPositionHandler;
		private List<GameObject> tabList;
		private const string pathTutorial = "Prefabs/HUD/Tutorial/";
		private const string arcadeTutorialPrefab = "TutorialArcade";
		private const string eliminationTutorialPrefab = "TutorialElimination";
		private const string longestTailTutorialPrefab = "TutorialLongestTail";
	

		// Use this for initialization
		public override void Start ()
		{
				base.Start ();
		
				builder = GameObject.FindGameObjectWithTag (Tags.Master).GetComponent<GameBuilder> ();
				HUDPositionHandler = GameObject.FindGameObjectWithTag (Tags.HUD).GetComponent<HUDPositionHandler> ();
				FetchGameModeTutorial();
				tabList = new List<GameObject> (GameObject.FindGameObjectsWithTag (Tags.PageTutorial));
				ActivateTab ("GameModeTab");

		}
	
		protected override void OnSelect (GameObject target)
		{
				base.OnSelect (target);

				if (target.tag == Tags.MenuSelector) {
						if (target.name == "Dismiss") {
								HUDPositionHandler.enabled = true;
								builder.PlayerReady ();
								Destroy (gameObject);
						} else if (target.name == "PowerUpButton") {
								ActivateTab ("PowerTab");
						} else if (target.name == "GameModeButton") {
								ActivateTab ("GameModeTab");
						}
				}
		}

		private void FetchGameModeTutorial()
		{
				string prefabPath = pathTutorial;

				switch (builder.GameMode) {

				case GameModes.Arcade:
						prefabPath += arcadeTutorialPrefab;
						break;
				case GameModes.Elimination:
						prefabPath += eliminationTutorialPrefab;
						break;
				case GameModes.LongestTail:
						prefabPath += longestTailTutorialPrefab;
						break;
				
				}

			GameObject tutorial = (GameObject) GameObject.Instantiate(Resources.Load(prefabPath));
			tutorial.transform.parent = gameObject.transform;
			tutorial.transform.localPosition = Vector3.zero;
		}

		private void ActivateTab (string tabName)
		{
				foreach (GameObject tab in tabList) {
						tab.SetActive (tab.name == tabName);
				}
		}
}