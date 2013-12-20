using UnityEngine;
using System.Collections;

public class CameraParenting : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if( NetworkHelper.IsOwnerSide(networkView) &&
            GetComponent<PlayerIdentity>().IsHuman)
        {

            GameObject camera = GameObject.FindGameObjectWithTag(Tags.MainCamera);

            camera.GetComponent<CameraMovement>().LookAt(gameObject);

        }

        this.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
