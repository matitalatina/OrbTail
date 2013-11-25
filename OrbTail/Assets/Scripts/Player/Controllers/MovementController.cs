using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {
	private DriverStack<IEngineDriver> engineDriverStack;
	private DriverStack<IWheelDriver> wheelDriverStack;

	public float torqueForce = 40f;
	public float speedForce = 120f;
	public float maxRoll = 60f;
	public float rotationSmooth = 5f;
	
	public Vector3 Gravity {get; set;}

	// TODO: change with real initializer
	// Use this for initialization
	void Start () {
		engineDriverStack = new DriverStack<IEngineDriver>();
		wheelDriverStack = new DriverStack<IWheelDriver>();

		engineDriverStack.Push(new DefaultEngineDriver(3));
		wheelDriverStack.Push(new DefaultWheelDriver(3));
		Gravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
		engineDriverStack.GetHead().Update();
		wheelDriverStack.GetHead().Update();
	}

	void FixedUpdate () {
		float wheelSteer = wheelDriverStack.GetHead().GetDirection();
		Vector3 forwardProjected = Vector3.Cross(this.Gravity, Vector3.Cross(-this.Gravity, this.transform.forward)).normalized;
		Quaternion rollRotation = Quaternion.FromToRotation(this.transform.up, Quaternion.AngleAxis(wheelSteer * maxRoll, -this.transform.forward) * -this.Gravity);
		this.rigidbody.AddForce(forwardProjected * engineDriverStack.GetHead().GetForce() * speedForce, ForceMode.Acceleration);
		//this.rigidbody.AddRelativeForce(Vector3.forward * engineDriverStack.GetHead().GetForce() * speedForce);
		this.rigidbody.rotation = Quaternion.Lerp(this.transform.rotation, rollRotation * Quaternion.LookRotation(forwardProjected, -this.Gravity) * Quaternion.AngleAxis(wheelSteer * torqueForce, -this.Gravity), rotationSmooth * Time.deltaTime);
	}

	public DriverStack<IEngineDriver> GetEngineDriverStack() {
		return engineDriverStack;
	}

	public DriverStack<IWheelDriver> GetWheelDriverStack() {
		return wheelDriverStack;
	}


}
