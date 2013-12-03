using UnityEngine;
using System.Collections;

public class CameraParenting : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if( ( networkView.isMine ||
              (!Network.isServer &&
               !Network.isClient ) ) &&
            GetComponent<InputProxy>().proxy_type == InputProxy.InputProxyType.Human)
        {

            GameObject camera = GameObject.FindGameObjectWithTag(Tags.MainCamera);

            camera.GetComponent<CameraMovement>().LookAt(gameObject);

        }

        Destroy(this);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
