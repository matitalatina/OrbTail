using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {

	private RelayInputBroker inputBroker = new RelayInputBroker();

	public IInputBroker GetInputBroker() {
		return inputBroker;
	}

	// Use this for initialization
	void Start () {
		inputBroker.Acceleration = 1f;
		inputBroker.Steering = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		inputBroker.Acceleration = 1f;
		inputBroker.Steering = 1f;
	}

	void Chasing() {

	}

	void GoAway() {

	}
	

}
