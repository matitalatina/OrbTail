using UnityEngine;
using System.Collections;

public class GUISplashScreen : MonoBehaviour {
	private float timeOfSplash = 1.5f;
	private float fadeTime = 0.3f;
	private GameObject radiance;
	private GameObject polimi;
	private GameObject menteZero;

	// Use this for initialization
	void Start () {
		radiance = GameObject.Find("LogoRadiance");
		polimi = GameObject.Find("PolimiGameCollective");
		menteZero = GameObject.Find("MenteZero");

		iTween.FadeTo(polimi, 0f, 0f);
		iTween.FadeTo(menteZero, 0f, 0f);
		StartCoroutine("changeSecondSplash");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator changeSecondSplash() {
		yield return new WaitForSeconds(timeOfSplash);
		polimi.renderer.enabled = true;
		menteZero.renderer.enabled = true;
		iTween.FadeTo(radiance, 0f, fadeTime);
		iTween.FadeTo(polimi, 1f, fadeTime);
		iTween.FadeTo(menteZero, 1f, fadeTime);
		yield return new WaitForSeconds(timeOfSplash + fadeTime);
		Application.LoadLevel("MenuMain");

	}
	
}
