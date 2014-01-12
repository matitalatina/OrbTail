using UnityEngine;
using System.Collections;

public class ColliderDummy : MonoBehaviour {

    public delegate void DelegateCollision(object sender, Collision collision);

    public DelegateCollision EventCollision;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {

        if (EventCollision != null)
        {

            EventCollision(this, collision);

        }

    }

}
