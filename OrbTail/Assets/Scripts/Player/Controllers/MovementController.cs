using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {
	private DriverStack<IEngineDriver> engineDriverStack;
	private DriverStack<IWheelDriver> wheelDriverStack;

	public float torqueForce = 17f;
	public float speedForce = 120f;
	public float maxRoll = 60f;
	public float rotationSmooth = 5f;
	
	public FloatingObject FloatingBody {get; private set;}

	// TODO: change with real initializer
	// Use this for initialization
	void Start () {
		engineDriverStack = new DriverStack<IEngineDriver>();
		wheelDriverStack = new DriverStack<IWheelDriver>();

		engineDriverStack.Push(new DefaultEngineDriver(3));
		wheelDriverStack.Push(new DefaultWheelDriver(3));

		FloatingBody = this.GetComponent<FloatingObject>();

	}
	
	// Update is called once per frame
	void Update () {
		engineDriverStack.GetHead().Update();
		wheelDriverStack.GetHead().Update();
	}

	void FixedUpdate () {

		Vector3 arenaDown = FloatingBody.ArenaDown;


		float wheelSteer = wheelDriverStack.GetHead().GetDirection();
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
