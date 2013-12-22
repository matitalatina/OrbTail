using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// The script builds any game
/// </summary>
public class GameBuilder : MonoBehaviour {

    public const int kMaxPlayerCount = 4;
    public const int kServerPort = 6059;
    public const string kGameName = "OrbTail";

    /// <summary>
    /// The current arena name
    /// </summary>
    public string ArenaName { get; set; }

    #region Events

    public delegate void DelegateRegistrationSucceeded(object sender);

    public delegate void DelegatePlayerConnected(object sender, NetworkPlayer network_player, PlayerIdentity player_identity);

    public delegate void DelegatePlayerReady(object sender, NetworkPlayer network_player);

    public delegate void DelegatePlayerDisconnected(object sender, NetworkPlayer network_player);

    public delegate void DelegateErrorOccurred(object sender, string message);

    public event DelegateRegistrationSucceeded EventRegistrationSucceeded;

    public event DelegatePlayerConnected EventPlayerConnected;

    public event DelegatePlayerReady EventPlayerReady;

    public event DelegatePlayerDisconnected EventPlayerDisconnected;

    public event DelegateErrorOccurred EventErrorOccurred;

    private void NotifyRegistrationSucceeded()
    {

        if (EventRegistrationSucceeded != null)
        {

            EventRegistrationSucceeded(this);

        }

    }

    private void NotifyPlayerConnected(NetworkPlayer network_player, PlayerIdentity player_identity)
    {

        if (EventPlayerConnected != null)
        {

            EventPlayerConnected(this, network_player, player_identity);

        }

    }

    private void NotifyPlayerReady(NetworkPlayer network_player)
    {

        if (EventPlayerReady != null)
        {

            EventPlayerReady(this, network_player);

        }

    }

    private void NotifyPlayerDisconnected(NetworkPlayer network_player)
    {

        if (EventPlayerDisconnected != null)
        {

            EventPlayerDisconnected(this, network_player);

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

    /// <summary>
    /// Initialize a local match, only one human is allowed! (the identities are attached to the master game object)
    /// </summary>
    /// <param name="arena_name">The arena to load</param>
    public void InitializeSinglePlayer()
    {

        DontDestroyOnLoad(gameObject);

        GetComponent<SinglePlayerBuilder>().enabled = true;

        this.enabled = false;

    }

    /// <summary>
    /// Initializes the host
    /// </summary>
    public void InitializeHost(string arena_name, bool local_master_server = false)
    {

        EventRegistrationSucceeded += GameBuilder_EventRegistrationSucceeded;
        EventErrorOccurred += GameBuilder_EventErrorOccurred;

        if (!local_master_server)
        {

            //Asks the remote master server

            System.Random random = new System.Random();

            Network.InitializeServer(kMaxPlayerCount - 1,
                                     kServerPort,
                                     !Network.HavePublicAddress());

            MasterServer.RegisterHost(kGameName,
                                      random.Next().ToString(),
                                      ArenaName);

        }
        else
        {

            //Asks the local master server
            //TODO: not yet implemented

        }

    }

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        switch (server_event)
        {

            case MasterServerEvent.RegistrationSucceeded:

                NotifyRegistrationSucceeded();
                break;

            case MasterServerEvent.RegistrationFailedNoServer:

                NotifyErrorOccurred("The server is unreachable.");
                break;

            case MasterServerEvent.RegistrationFailedGameType:
            case MasterServerEvent.RegistrationFailedGameName:

                NotifyErrorOccurred("Could not create the game.");
                break;  

            case MasterServerEvent.HostListReceived:

                //TODO: fill with client-code
                break;

        }

    }

    void OnPlayerConnected(NetworkPlayer player)
    {

        
        Debug.Log("A player has been connected!");

    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {

        Debug.Log("A player has been disconnected!");
        
    }


    void GameBuilder_EventErrorOccurred(object sender, string message)
    {

        //TODO: show the actual message in a friendly way
        Debug.LogError(message);

    }

    private void GameBuilder_EventRegistrationSucceeded(object sender)
    {

        //TODO: the server is waiting
        Debug.Log("Waiting for players");

    }
  
}
