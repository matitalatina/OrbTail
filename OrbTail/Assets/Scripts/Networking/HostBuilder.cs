using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HostBuilder : NetworkPlayerBuilder
{

    #region Events

    public delegate void DelegateRegistrationSucceeded(object sender);

    public delegate void DelegateErrorOccurred(object sender, string message);

    public event DelegateRegistrationSucceeded EventRegistrationSucceeded;

    public event DelegateErrorOccurred EventErrorOccurred;

    private void NotifyRegistrationSucceeded()
    {

        if (EventRegistrationSucceeded != null)
        {

            EventRegistrationSucceeded(this);

        }

    }

    private void NotifyErrorOccurred(string message)
    {

        if (EventErrorOccurred != null)
        {

            EventErrorOccurred(this, message);

        }

    }

    #endregion

    public const int kMatchStartDelay = 5;

	// Use this for initialization
	void Start () {

        //Initialize the internal status
        available_ids_ = new Stack<int>();
        player_ids_ = new Dictionary<NetworkPlayer, int>();
        ready_players_ = new HashSet<int>();

        //Register to the proper events
        EventRegistrationSucceeded += HostBuilder_EventRegistrationSucceeded;
        EventErrorOccurred += HostBuilder_EventErrorOccurred;
        EventPlayerReady += HostBuilder_EventPlayerReady;
        EventIdAcquired += HostBuilder_EventIdAcquired;

        //Store the available ids
        for (int i = 0; i < GameBuilder.kMaxPlayerCount; i++)
        {

            available_ids_.Push(i);

        }

        //This prevents multiple registrations
        Registered = false;

        Debug.Log("Registering to master server...");

        //Register the match
        if (!LocalMasterServer)
        {

            //Asks the remote master server

            System.Random random = new System.Random();

            Network.InitializeServer(GameBuilder.kMaxPlayerCount - 1,
                                     GameBuilder.kServerPort,
                                     !Network.HavePublicAddress());

            MasterServer.RegisterHost(GameBuilder.kGameTypeName,
                                      GetComponent<GameBuilder>().ArenaName);

        }
        else
        {

            //Asks the local master server
            //TODO: not yet implemented

        }


	}

	// Update is called once per frame
	void Update () {
	
	}

    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        switch (server_event)
        {

            case MasterServerEvent.RegistrationSucceeded:

                if (!Registered)
                {

                    Debug.Log("Registration successful");

                    Registered = true;

                    NotifyRegistrationSucceeded();

                }

                break;
                
            case MasterServerEvent.RegistrationFailedNoServer:

                NotifyErrorOccurred("The server is unreachable.");
                break;

            case MasterServerEvent.RegistrationFailedGameType:
            case MasterServerEvent.RegistrationFailedGameName:

                NotifyErrorOccurred("Could not create the game.");
                break;

            default:

                //Nothing to do here
                break;

        }

    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {

        int id;

        if (player_ids_.TryGetValue(player, out id))
        {

            Debug.Log("Player " + id + " is now offline");


            available_ids_.Push(id);
            ready_players_.Remove(id);
            player_ids_.Remove(player);
        
            networkView.RPC("RPCPlayerUnregistered", RPCMode.AllBuffered, id);
        
        }
        else
        {

            System.Diagnostics.Debug.Assert(false, "The player is not registered");

        }

    }

    private void HostBuilder_EventErrorOccurred(object sender, string message)
    {

        //TODO: show this in a friendly way
        Debug.LogError(message);

    }

    private void HostBuilder_EventRegistrationSucceeded(object sender)
    {

        PlayerIdentity identity = GetComponents<PlayerIdentity>().SkipWhile((PlayerIdentity p) => { return !p.IsHuman; }).First();

        //Register the server identity
        RPCRegisterPlayer(Network.player, identity.Name);
        
    }

    private void HostBuilder_EventPlayerReady(object sender, int id, bool value)
    {

        if (value)
        {
            
            ready_players_.Add(id);
            Debug.Log("Player " + id + " is ready");

        }
        else
        {

            ready_players_.Remove(id);
            Debug.Log("Player " + id + " is not ready anymore");

        }
        

        if (ready_players_.Count == player_ids_.Count &&
            ready_players_.Count > 1)
        {

            Debug.Log("All players are ready");

            //Every device should load the proper arena
            networkView.RPC("RPCLoadArena", RPCMode.All, GetComponent<GameBuilder>().ArenaName);

        }

    }

    void HostBuilder_EventIdAcquired(object sender, int id)
    {

        SetReady(true);

    }

    protected override void RegisterPlayer(NetworkPlayer player, string name)
    {
 	    
        var id = available_ids_.Pop();

        Debug.Log("Player " + id + " is now online");

        player_ids_.Add(player, id);

        networkView.RPC("RPCPlayerRegistered", RPCMode.AllBuffered, id, name);

        if (Network.player.Equals(player))
        {

            RPCIdAcquired(id);

        }
        else
        {

            networkView.RPC("RPCIdAcquired", player, id);

        }
        
    }

    protected override void ArenaLoaded(NetworkPlayer player)
    {
        
        //Wait for every client then start the initialization of the ships

    }

    /// <summary>
    /// Available ids
    /// </summary>
    private Stack<int> available_ids_;

    /// <summary>
    /// The table associates the id to a networkplayer
    /// </summary>
    private IDictionary<NetworkPlayer, int > player_ids_;

    /// <summary>
    /// The set of all ready players
    /// </summary>
    private HashSet<int> ready_players_;

    /// <summary>
    /// Is the host registered?
    /// </summary>
    private bool Registered { get; set; }

}
