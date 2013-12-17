using UnityEngine;
using System.Collections;

public class ProximityHandler : MonoBehaviour {

	public delegate void DelegateOnProximityEnter(object sender, Collider other);

	public event DelegateOnProximityEnter EventOnProximityEnter;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		EventOnProximityEnter(this, other);
	}
}
