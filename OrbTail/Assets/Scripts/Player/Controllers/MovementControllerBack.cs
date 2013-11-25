using UnityEngine;
using System.Collections;

public class MovementControllerBack : MonoBehaviour {
	private DriverStack<IEngineDriver> engineDriverStack;
	private DriverStack<IWheelDriver> wheelDriverStack;
	
	public float torqueForce = 40f;
	public float speedForce = 120f;
	public float rotationSmooth = 5f;
	
	public Vector3 Gravity {get; set;}
	
	// TODO: change with real initializer
	// Use this for initialization
	void Start () {
		engineDriverStack = new DriverStack<IEngineDriver>();
		wheelDriverStack = new DriverStack<IWheelDriver>();
		
		engineDriverStack.Push(new DefaultEngineDriver(1));
		wheelDriverStack.Push(new DefaultWheelDriver(5));
		Gravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
		engineDriverStack.GetHead().Update();
		wheelDriverStack.GetHead().Update();
	}
	
	void FixedUpdate () {
		this.rigidbody.AddRelativeForce(Vector3.forward * engineDriverStack.GetHead().GetForce() * speedForce);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(Vector3.Cross(this.Gravity, this.transform.right), -this.Gravity) * Quaternion.AngleAxis(wheelDriverStack.GetHead().GetDirection() * torqueForce, -this.Gravity), rotationSmooth * Time.deltaTime);
		
	}
	
	public DriverStack<IEngineDriver> GetEngineDriverStack() {
		return engineDriverStack;
	}
	
	public DriverStack<IWheelDriver> GetWheelDriverStack() {
		return wheelDriverStack;
	}

}
