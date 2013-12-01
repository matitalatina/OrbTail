using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIServerClient : MonoBehaviour {

    GameObject server_button;
    GameObject client_button;

	// Use this for initialization
	void Start () {

        server_button = GameObject.Find("ServerButton");
        client_button = GameObject.Find("ClientButton");

	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonUp(0) ||
           Input.touchCount > 0)
        {

            Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycast_hit;

            if (Physics.Raycast(mouse_ray, out raycast_hit) ||
                Input.touchCount > 0 &&
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out raycast_hit))
            {

                //The touch or the mouse collided with something

                if (raycast_hit.collider.gameObject == server_button)
                {

                    //Loads the server builder
                    var server_builder = Resources.Load("Prefabs/ServerBuilder");
                    Instantiate(server_builder, Vector3.zero, Quaternion.identity);

                    server_button.gameObject.SetActive(false);
                    client_button.gameObject.SetActive(false);

                }
                else if (raycast_hit.collider.gameObject == client_button)
                {

                    //Loads the client builder
                    var client_builder = Resources.Load("Prefabs/ClientBuilder");
                    Instantiate(client_builder, Vector3.zero, Quaternion.identity);

                    server_button.gameObject.SetActive(false);
                    client_button.gameObject.SetActive(false);

                }

            }

        }

	}

}
