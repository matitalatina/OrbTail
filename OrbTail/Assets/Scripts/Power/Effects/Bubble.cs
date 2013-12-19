using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// Too heavy
        // float bubblingFactor = Mathf.Cos(Time.time * 2.0f);
        this.transform.localScale = Vector3.one * 2f;
	}
}
