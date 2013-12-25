using UnityEngine;
using System.Collections;

public class CollisionSoundHandler : MonoBehaviour {
	private AudioClip heavyCrash;
	private float volumeStd = 1f;

	// Use this for initialization
	void Start () {
		heavyCrash = Resources.Load<AudioClip>("Sounds/Ship/Crash");
		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {

		if (collision.collider.gameObject.tag != Tags.Orb) {
			AudioSource.PlayClipAtPoint(heavyCrash, collision.transform.position, volumeStd);
		}

	}

	private void OnGameBuilt(object sender) {

		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();

		if (game.ActivePlayer != gameObject) {
			this.enabled = false;
		}


	}

}
