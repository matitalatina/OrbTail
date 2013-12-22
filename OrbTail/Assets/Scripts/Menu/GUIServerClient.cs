using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIServerClient : MonoBehaviour {

    GameObject server_button;
    GameObject client_button;
    GameObject start_button;
	GameObject single_player_button;

	// Use this for initialization
	void Start () {

        server_button = GameObject.Find("ServerButton");
        client_button = GameObject.Find("ClientButton");
        start_button = GameObject.Find("StartButton");

		single_player_button = GameObject.Find("SinglePlayerButton");

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

                    //Loads the server builder
                    gameObject.AddComponent<ServerBuilder_d>().EventMatchCreated += GUIServerClient_EventMatchCreated;
                    
					single_player_button.SetActive(false);
                    server_button.gameObject.SetActive(false);
                    client_button.gameObject.SetActive(false);

                }
                else if (raycast_hit.collider.gameObject == client_button)
                {

                    //Loads the client builder
                    gameObject.AddComponent<ClientBuilder_d>().EventServerReady += GUIServerClient_EventServerReady;

					single_player_button.SetActive(false);
                    server_button.gameObject.SetActive(false);
                    client_button.gameObject.SetActive(false);

                }
				else if (raycast_hit.collider.gameObject == single_player_button)
				{
					StartSinglePlayer();
				}
                else if (raycast_hit.collider.gameObject == start_button)
                {

                    MasterServer.UnregisterHost();      //No more connections are allowed from now on

                    NextLevel();

                }

            }

        }

	}

    

    private void NextLevel()
    {
        
        this.enabled = false;

        DontDestroyOnLoad(gameObject);      //The Game should be preserved

        Application.LoadLevel("NetworkTest");

    }

	private void StartSinglePlayer() {

        this.enabled = false;

        var builder = GetComponent<GameBuilder>();

        builder.ArenaName = "Flat";
        builder.InitializeSinglePlayer();

	}

    void GUIServerClient_EventMatchCreated(object sender)
    {

        start_button.gameObject.SetActive(true);

    }

    void GUIServerClient_EventServerReady(object sender)
    {

        NextLevel();

    }


}
