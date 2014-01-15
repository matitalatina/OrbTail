using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HUDTutorialHandler : GUIMenuChoose
{
		private GameBuilder builder;
		private HUDPositionHandler hudPositionHandler;
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
				builder.EventGameBuilt += OnGameBuilt;

		}
	
		protected override void OnSelect (GameObject target)
		{
				base.OnSelect (target);

				if (target.tag == Tags.MenuSelector) {
						if (target.name == "Dismiss") {
								hudPositionHandler.enabled = true;
								builder.PlayerReady ();
								Destroy (gameObject);
						} else if (target.name == "PowerUpButton") {
								ActivateTab ("PowerTab");
						} else if (target.name == "GameModeButton") {
								ActivateTab ("GameModeTab");
						}
				}
		}

		private void OnGameBuilt(object sender) {
			hudPositionHandler = GameObject.FindGameObjectWithTag (Tags.HUD).GetComponent<HUDPositionHandler> ();
			FetchGameModeTutorial();
			tabList = new List<GameObject> (GameObject.FindGameObjectsWithTag (Tags.PageTutorial));
			ActivateTab ("GameModeTab");

			builder.EventGameBuilt -= OnGameBuilt;
		}

		private void FetchGameModeTutorial()
		{
				string prefabPath = pathTutorial;
				Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
				switch (game.GameMode) {

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
