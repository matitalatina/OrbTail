using UnityEngine;
using System.Collections;

public class AIFieldOfView : MonoBehaviour {
	PlayerAI playerAI;

	// Use this for initialization
	void Start () {
		playerAI = transform.parent.gameObject.GetComponent<PlayerAI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		playerAI.JustSeen(other);
	}
}
