using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIServerClient : MonoBehaviour {

    private GameObject server_button;
	private GameObject client_button;
	private GameObject start_button;
	private GameObject single_player_button;
	private GameObject master;

	// Use this for initialization
	void Start () {

        server_button = GameObject.Find("ServerButton");
        client_button = GameObject.Find("ClientButton");
        start_button = GameObject.Find("StartButton");

		single_player_button = GameObject.Find("SinglePlayerButton");

		master = GameObject.FindGameObjectWithTag(Tags.Master);

        //This should not be active at the beginning
        start_button.SetActive(false);

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
				else if (raycast_hit.collider.gameObject == single_player_button)
				{

					StartSinglePlayer();

				}

            }

        }

	}

    
	private void StartSinglePlayer() {

        this.enabled = false;

		var builder = master.GetComponent<GameBuilder>();

        builder.InitializeSinglePlayer();

	}

    private void StartHost()
    {

        this.enabled = false;

		var builder = master.GetComponent<GameBuilder>();

        builder.InitializeHost();

    }

    private void StartClient()
    {

        this.enabled = false;

		var builder = master.GetComponent<GameBuilder>();

        builder.InitializeClient();

    }

}
