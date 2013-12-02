using UnityEngine;
using System.Collections;

public class NetworkMatchBooter : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (Network.isServer)
        {

            Debug.Log("Server match will be now initialize");

            ServerBuilder server_builder = GameObject.Find("ServerBuilder").GetComponent<ServerBuilder>();

            server_builder.InitializeServerMatch();

        }
        else if(Network.isClient)
        {

            Debug.Log("Client match will be now initialize");

            ClientBuilder client_builder = GameObject.Find("ClientBuilder").GetComponent<ClientBuilder>();

            client_builder.InitializeClientMatch();

        }

	}
	
	// Update is called once per frame
    void Update()
    {

    }



}
