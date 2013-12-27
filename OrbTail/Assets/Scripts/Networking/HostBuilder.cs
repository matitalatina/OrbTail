using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HostBuilder : NetworkPlayerBuilder
{
    
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
        RejectAllConnections = false;

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

    void OnPlayerConnected(NetworkPlayer player)
    {

        if (!RejectAllConnections)
        {

            player_ids_.Add(player, 0);

        }
        else
        {

            Debug.Log("A player has been rejected");
            Network.CloseConnection(player, true);

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

            Network.RemoveRPCs(player);

            networkView.RPC("RPCPlayerUnregistered", RPCMode.All, id);
        
        }
        else
        {

            System.Diagnostics.Debug.Assert(false, "The player is not registered");

        }

    }
    
    // A new level has been loaded
    void OnLevelWasLoaded(int level)
    {

        ArenaLoaded(Id);

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
        player_ids_.Add(Network.player, 0);

        RPCRegisterPlayer(Network.player, identity.ShipName);
        
    }

    private void HostBuilder_EventPlayerReady(object sender, int id, bool value)
    {

        if (value)
        {
            
            ready_players_.Add(id);

        }
        else
        {

            ready_players_.Remove(id);

        }
        
        if (ready_players_.Count == player_ids_.Count &&
            ready_players_.Count > 1)
        {

            RejectAllConnections = true;

            ready_players_.Clear();

            Debug.Log("All players are ready");

            //Every device should load the proper arena
            networkView.RPC("RPCLoadArena", RPCMode.All, GetComponent<GameBuilder>().ArenaName);

        }

    }

    void HostBuilder_EventIdAcquired(object sender, int id)
    {


    }

    protected override void RegisterPlayer(NetworkPlayer player, string name)
    {

        if (player_ids_.ContainsKey(player))
        {

            var id = available_ids_.Pop();

            player_ids_[player] = id;

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
        
    }

    protected override void ArenaLoaded(int id)
    {

        if (IsArenaLoading)
        {

            ready_players_.Add(id);

            if (ready_players_.Count == player_ids_.Count)
            {

                //Everyone loaded the arena, start intializing
                ready_players_.Clear();

                //Create the game
                var game_resource = Resources.Load("Prefabs/Game");

                GameObject game = Network.Instantiate(game_resource, Vector3.zero, Quaternion.identity, 0) as GameObject;

                //Set the game mode and the arena
                game.networkView.RPC("RPCSetGame", RPCMode.All, GameModes.Resolve(GetComponent<GameBuilder>().GameMode));

                //Create the ships
                GameObject[] spawn_points = GameObject.FindGameObjectsWithTag(Tags.SpawnPoint);

                foreach (KeyValuePair<NetworkPlayer, int> player_id in player_ids_)
                {

                    if (!player_id.Key.Equals(Network.player))
                    {

                        networkView.RPC("RPCCreatePlayer", player_id.Key, spawn_points[player_id.Value].transform.position);

                    }
                    else
                    {

                        RPCCreatePlayer(spawn_points[player_id.Value].transform.position);

                    }

                }

            }

        }
     
    }

    protected override void PlayerCreated(int id, NetworkViewID view_id)
    {

        ready_players_.Add(id);

        Debug.Log("Player " + id + " instantiated");

        if (ready_players_.Count == player_ids_.Count)
        {

            //Everyone created its own ship, start the match!
            ready_players_.Clear();

            var game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();

            game.networkView.RPC("RPCGameEnable", RPCMode.All, true);

        }

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

    /// <summary>
    /// Indicate whether the server should reject all the incoming connection (when the match started, for example)
    /// </summary>
    private bool RejectAllConnections { get; set; }

}
