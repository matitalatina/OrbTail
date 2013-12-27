using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIServerClient : MonoBehaviour {

    private GameObject server_button;
	private GameObject client_button;
	private GameObject ready_button;
	private GameObject master;

	// Use this for initialization
	void Start () {

        server_button = GameObject.Find("ServerButton");
        client_button = GameObject.Find("ClientButton");
        ready_button = GameObject.Find("ReadyButton");

		master = GameObject.FindGameObjectWithTag(Tags.Master);

        //This should not be active at the beginning
        ready_button.SetActive(false);

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

                    StartHost();
					
                }
                else if (raycast_hit.collider.gameObject == client_button)
                {

                    StartClient();

                }

            }

        }

	}
	

    private void StartHost()
    {

        this.enabled = false;

		var builder = master.GetComponent<GameBuilder>();

		builder.Action = GameBuilder.BuildMode.RemoteHost;

        Application.LoadLevel("MenuChooseShip");

    }

    private void StartClient()
    {

        this.enabled = false;

		var builder = master.GetComponent<GameBuilder>();

		builder.Action = GameBuilder.BuildMode.RemoteGuest;

        Application.LoadLevel("MenuChooseShip");

    }

}
