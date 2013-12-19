using UnityEngine;
using System.Collections;

public class ProximityHandler : MonoBehaviour {

	public delegate void DelegateOnProximityEnter(object sender, Collider other);

	public event DelegateOnProximityEnter EventOnProximityEnter;

	private SphereCollider proximityCollider;

	// Use this for initialization
	void Start () {
		proximityCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/// <summary>
	/// Gets or sets the radius of the proximity field.
	/// </summary>
	/// <value>The radius.</value>
	public float Radius {
		get {
			return proximityCollider.radius;
		}

		set {
			proximityCollider.radius = value;
		}
	}

	void OnTriggerEnter(Collider other) {

        if (EventOnProximityEnter != null)
        {

            EventOnProximityEnter(this, other);

        }

    }

}
