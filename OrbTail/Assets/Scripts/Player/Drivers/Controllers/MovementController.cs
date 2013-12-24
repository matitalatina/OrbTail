using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementController : MonoBehaviour {

	public float maxTorqueForce = 17f;
	public float maxSpeedForce = 100.0f;
	public float maxRoll = 80f;
	public float rotationSmooth = 10f;

	private float smoothSound = 7f;

	private float pitchGap = 2f;
	
	public FloatingObject FloatingBody {get; private set;}


	private DriverStack<IEngineDriver> engineDriverStack;
	private DriverStack<IWheelDriver> wheelDriverStack;

	private InputProxy inputProxy;

	private float actualPitchSound;


	void Awake() {
		engineDriverStack = new DriverStack<IEngineDriver>();
		wheelDriverStack = new DriverStack<IWheelDriver>();
	}

	// Use this for initialization
	void Start () {
		FloatingBody = this.GetComponent<FloatingObject>();
		inputProxy = this.GetComponent<InputProxy>();

		Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
		game.EventEnd += OnEventEnd;

		actualPitchSound = audio.pitch;
	}
	
	// Update is called once per frame
	void Update () {
		engineDriverStack.GetHead().Update(inputProxy.Acceleration);
		wheelDriverStack.GetHead().Update();
	}

	void FixedUpdate () {
		Vector3 arenaDown = FloatingBody.ArenaDown;

		float wheelSteer = wheelDriverStack.GetHead().GetDirection(inputProxy.Steering);
		float engineForce = engineDriverStack.GetHead().GetForce();

		PlaySoundEngine(engineForce);

		Vector3 forwardProjected = Vector3.Cross(arenaDown,
		                                         Vector3.Cross(-arenaDown, this.transform.forward)
		                                         ).normalized;

		Quaternion rollRotation = Quaternion.FromToRotation(this.transform.up,
		                                                    Quaternion.AngleAxis(	wheelSteer * maxRoll, 
		                     														-this.transform.forward
		                     													) * -arenaDown);
		Quaternion yawRotation = Quaternion.AngleAxis(wheelSteer * maxTorqueForce, -arenaDown);
		Quaternion pitchStabilization = Quaternion.LookRotation(forwardProjected, -arenaDown);

		this.rigidbody.AddForce(forwardProjected * engineForce * maxSpeedForce, ForceMode.Acceleration);
		this.rigidbody.rotation = Quaternion.Lerp(this.transform.rotation, rollRotation * yawRotation * pitchStabilization, rotationSmooth * Time.deltaTime);
	}


	/// <summary>
	/// Gets the engine driver stack.
	/// </summary>
	/// <returns>The engine driver stack.</returns>
	public DriverStack<IEngineDriver> GetEngineDriverStack() {
		return engineDriverStack;
	}


	/// <summary>
	/// Gets the wheel driver stack.
	/// </summary>
	/// <returns>The wheel driver stack.</returns>
	public DriverStack<IWheelDriver> GetWheelDriverStack() {
		return wheelDriverStack;
	}

	private void PlaySoundEngine(float engineForce) {
		audio.pitch = Mathf.Lerp(Mathf.Abs(engineForce) + pitchGap, actualPitchSound, smoothSound * Time.deltaTime);
	}

	private void OnEventEnd(object sender, IList<GameObject> rank) {
		iTween.AudioTo(gameObject, 0f, 0f, 2f);
	}


}
