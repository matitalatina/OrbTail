using UnityEngine;
using System.Collections;

public class ProximityHandler : MonoBehaviour {
	TailController tailController;

	// Use this for initialization
	void Start () {
		tailController = transform.parent.gameObject.GetComponent<TailController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		tailController.OnProximityEnter(other);
	}
}
