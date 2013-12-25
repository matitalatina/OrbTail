using UnityEngine;
using System.Collections;

public class ShipSoundHandler : MonoBehaviour {
	private AudioClip heavyCrash;
	private AudioClip gatherOrbSound;
	private DriverStack<IEngineDriver> engineDriverStack;
	private Game game;

	private float volumeStd = 1f;
	private float smoothSound = 1f;
	private float pitchGap = 2f;
	private float actualPitchSound;
	private bool gameFinished = false;

	// Use this for initialization
	void Start () {
		heavyCrash = Resources.Load<AudioClip>("Sounds/Ship/Crash");
		gatherOrbSound = Resources.Load<AudioClip>("Sounds/Ship/AttachOrb");
		GameBuilder builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
		builder.EventGameBuilt += OnGameBuilt;
	}
	
	// Update is called once per frame
	void Update () {

		if (!gameFinished) {
			float engineForce = engineDriverStack.GetHead().GetForce();
			PlaySoundEngine(engineForce);
		}

	}



	void OnCollisionEnter(Collision collision) {

		if (gameObject == game.ActivePlayer && collision.collider.gameObject.tag != Tags.Orb) {
			AudioSource.PlayClipAtPoint(heavyCrash, collision.transform.position, volumeStd);
		}

	}

	private void OnGameBuilt(object sender) {

		game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();

		if (game.ActivePlayer == gameObject) {
			GetComponent<Tail>().OnEventOrbAttached += OnOrbAttached;
			game.EventEnd += OnEventEnd;
		}

		engineDriverStack = GetComponent<MovementController>().GetEngineDriverStack();

		actualPitchSound = audio.pitch;

	}

	private void OnOrbAttached(object sender, GameObject orb, GameObject ship) {
		audio.PlayOneShot(gatherOrbSound, volumeStd);
	}

	private void PlaySoundEngine(float engineForce) {
		audio.pitch = Mathf.Abs(engineForce) + pitchGap;
	}
	
	private void OnEventEnd(object sender, GameObject winner) {
		gameFinished = true;
		iTween.AudioTo(gameObject, 0f, 0f, 2f);
	}

}
