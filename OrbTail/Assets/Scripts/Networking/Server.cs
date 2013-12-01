using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {

    public int max_players = 4;

    NetworkPlayer player;

    bool online = false;

    void Awake()
    {

        Network.InitializeServer(max_players - 1, 60590, !Network.HavePublicAddress());

        System.Random rng = new System.Random();

        MasterServer.RegisterHost("OrbTail",
                                  "Game #" + rng.Next().ToString(),
                                  "Server test!");

        Debug.Log("Creating a server a server...");

    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {

        if (msEvent == MasterServerEvent.RegistrationSucceeded) {

            Debug.Log("Server is now online");

        }

    }

    void OnPlayerConnected(NetworkPlayer player)
    {

        this.player = player;

        Debug.Log("A player has been connected, let the game begin!");

        //Instantiate a normal orb
        var orb = Resources.Load("Prefabs/Ship");

        Network.Instantiate(orb, Vector3.zero, Quaternion.identity, 0);

        online = true;

    }

	// Use this for initialization
	void Start () {
        
        
        
	}
	
	// Update is called once per frame
	void Update () {

        if (online)
        {

            Debug.Log("The player pinged: " + Network.GetLastPing(player));

        }

	}

}
