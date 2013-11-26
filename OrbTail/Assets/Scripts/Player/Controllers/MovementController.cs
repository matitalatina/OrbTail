using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public float torqueForce = 17f;
	public float speedForce = 120f;
	public float maxRoll = 60f;
	public float rotationSmooth = 5f;
	
	public FloatingObject FloatingBody {get; private set;}


	private DriverStack<IEngineDriver> engineDriverStack;
	private DriverStack<IWheelDriver> wheelDriverStack;

	private InputProxy inputProxy;
	


	void Awake() {
		engineDriverStack = new DriverStack<IEngineDriver>();
		wheelDriverStack = new DriverStack<IWheelDriver>();
	}

	// Use this for initialization
	void Start () {

		FloatingBody = this.GetComponent<FloatingObject>();
		inputProxy = this.GetComponent<InputProxy>();

	}
	
	// Update is called once per frame
	void Update () {
		engineDriverStack.GetHead().Update(inputProxy.Acceleration);
		wheelDriverStack.GetHead().Update();
	}

	void FixedUpdate () {
		Vector3 arenaDown = FloatingBody.ArenaDown;


		float wheelSteer = wheelDriverStack.GetHead().GetDirection(inputProxy.Steering);
		Vector3 forwardProjected = Vector3.Cross(arenaDown, Vector3.Cross(-arenaDown, this.transform.forward)).normalized;
		Quaternion rollRotation = Quaternion.FromToRotation(this.transform.up, Quaternion.AngleAxis(wheelSteer * maxRoll, -this.transform.forward) * -arenaDown);
		this.rigidbody.AddForce(forwardProjected * engineDriverStack.GetHead().GetForce() * speedForce, ForceMode.Acceleration);
		//this.rigidbody.AddRelativeForce(Vector3.forward * engineDriverStack.GetHead().GetForce() * speedForce);
		this.rigidbody.rotation = Quaternion.Lerp(this.transform.rotation, rollRotation * Quaternion.AngleAxis(wheelSteer * torqueForce, -arenaDown) * Quaternion.LookRotation(forwardProjected, -arenaDown), rotationSmooth * Time.deltaTime);
	}

	public DriverStack<IEngineDriver> GetEngineDriverStack() {
		return engineDriverStack;
	}

	public DriverStack<IWheelDriver> GetWheelDriverStack() {
		return wheelDriverStack;
	}


}
