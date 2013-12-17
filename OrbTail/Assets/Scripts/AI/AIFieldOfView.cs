using UnityEngine;
using System.Collections;

public class AIFieldOfView : MonoBehaviour {

	public delegate void DelegateOnFieldOfViewEnter(object sender, Collider other);
	
	public event DelegateOnFieldOfViewEnter EventOnFieldOfViewEnter;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
        
        if (EventOnFieldOfViewEnter != null)
        {

            EventOnFieldOfViewEnter(this, other);

        }
		
	}
}
