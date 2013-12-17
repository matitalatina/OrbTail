using UnityEngine;
using System.Collections;

public class ProximityHandler : MonoBehaviour {
	
    TailController tailController;
    PowerController powerController;

	// Use this for initialization
	void Start () {

        if (NetworkHelper.IsServerSide()) {

            tailController = transform.parent.gameObject.GetComponent<TailController>();
            powerController = transform.parent.gameObject.GetComponent<PowerController>();

        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		tailController.OnProximityEnter(other);
	}
}
