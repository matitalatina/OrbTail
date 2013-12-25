using UnityEngine;
using System.Collections;

public class CollisionSoundHandler : MonoBehaviour {
	private AudioClip heavyCrash;
	private float volumeStd = 1f;

	// Use this for initialization
	void Start () {
		heavyCrash = Resources.Load<AudioClip>("Sounds/Ship/Crash");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {

		if (collision.collider.gameObject.tag != Tags.Orb) {
			AudioSource.PlayClipAtPoint(heavyCrash, collision.transform.position, volumeStd);
		}

	}

}
