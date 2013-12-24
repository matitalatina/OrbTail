using UnityEngine;
using System.Collections;

public class HUDBoostIndicatorHandler : MonoBehaviour {


	private PowerView boostView;
	private TextMesh textMesh;
	private float refreshTime = 0.2f;

	// Use this for initialization
	void Start () {
		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;
		game.EventStart += OnEventStart;
		GameObject player = game.ActivePlayer;
		boostView = player.GetComponent<PowerController>().GetPowerView(PowerGroups.Passive);
		textMesh = GetComponent<TextMesh>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnEventEnd(object sender, GameObject winner) {
		StopCoroutine("RefreshIndicator");
	}

	private void OnEventStart(object sender, int countdown) {

		if (countdown <= 0) {
			textMesh.color = Color.green;
			StartCoroutine("RefreshIndicator");
		}

	}

	private IEnumerator RefreshIndicator() {

		while (true) {
			float percentage = boostView.IsReady * 100f;
			textMesh.text = string.Format("∝ {0:0}%", percentage);
			textMesh.color = Color.Lerp(Color.red, Color.green, boostView.IsReady);
			yield return new WaitForSeconds(refreshTime);
		}
	}




}
