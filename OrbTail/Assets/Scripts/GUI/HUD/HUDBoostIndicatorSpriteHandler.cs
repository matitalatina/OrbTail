using UnityEngine;
using System.Collections;

public class HUDBoostIndicatorSpriteHandler : MonoBehaviour {

	private PowerView boostView;
	private TextMesh textMesh;
	private float refreshTime = 0.2f;
	private const float animationTime = 0.2f;
	private const float scaleBig = 0.2f;
	private bool charged;
	private Game game;
	private GameBuilder gameBuilder;
	
	// Use this for initialization
	void Start () {
		gameBuilder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		gameBuilder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnGameBuilt(object sender) {
		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		game.EventStart += OnEventStart;
		game.EventEnd += OnEnd;
		game.EventShipEliminated += OnShipEliminated;
		
		GameObject player = game.ActivePlayer;
		player.GetComponent<PowerController>().EventPowerAttached += HUDBoostIndicatorHandler_EventPowerAttached;
		textMesh = GetComponent<TextMesh>();
		
		gameBuilder.EventGameBuilt -= OnGameBuilt;
	}
	
	private void HUDBoostIndicatorHandler_EventPowerAttached(object sender, GameObject ship, Power power)
	{
		
		if (power.Group == PowerGroups.Passive)
		{
			
			boostView = power;
			charged = true;
			StartCoroutine("RefreshIndicator");
			
		}
		
	}
	
	
	private void OnEventEnd(object sender, GameObject winner, int info) {
		prepareToDisable();
	}
	
	private void OnEventStart(object sender, int countdown) {
		
		if (countdown <= 0) {
			
			iTween.FadeTo(gameObject, 1f, animationTime);
			
		}
		
	}
	
	private void OnShipEliminated(object sender, GameObject ship) {
		if (ship == game.ActivePlayer) {
			prepareToDisable();
		}
	}
	
	private IEnumerator RefreshIndicator() {
		
		while (true) {
			// Ready
			if (boostView.IsReady >= 1 && charged == false) {
				iTween.FadeTo(gameObject, 1f, animationTime);
				iTween.ScaleFrom(gameObject, Vector3.one * scaleBig, animationTime);
				charged = true;
			}
			// Not ready
			else if (boostView.IsReady < 1 && charged == true) {
				iTween.FadeTo(gameObject, 0.1f, animationTime);
				charged = false;
			}

			yield return new WaitForSeconds(refreshTime);
		}
		
	}
	
	private void OnEnd(object sender, GameObject winner, int info) {
		iTween.FadeTo(gameObject, 0f, animationTime);
	}
	
	private void prepareToDisable() {
		StopCoroutine("RefreshIndicator");
		
		game.EventEnd -= OnEventEnd;
		game.EventStart -= OnEventStart;
		game.EventEnd -= OnEnd;
	}

}
