using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// The script builds any game
/// </summary>
public class GameBuilder : MonoBehaviour {

    public enum BuildMode{

        SinglePlayer,
        RemoteHost,
        RemoteGuest,
        LocalHost,
        LocalGuest

    }

    public const int kMaxPlayerCount = 4;
    public const int kServerPort = 6059;
    public const string kGameTypeName = "OrbTail";

    /// <summary>
    /// The builder used for network matches
    /// </summary>
    public NetworkPlayerBuilder NetworkBuilder { get; private set; }

    public delegate void DelegateGameBuilt(object sender);

    /// <summary>
    /// Fired when the game has been properly built
    /// </summary>
    public event DelegateGameBuilt EventGameBuilt;
    
    public void NotifyGameBuilt()
    {

        if (EventGameBuilt != null)
        {

            EventGameBuilt(this);

        }

    }

    public delegate void DelegatePlayerLeft(object sender, int id);

    /// <summary>
    /// Fired when a client has left the match
    /// </summary>
    public event DelegatePlayerLeft EventPlayerLeft;

    public void NotifyPlayerLeft(int id)
    {

        if (EventPlayerLeft != null)
        {

            EventPlayerLeft(this, id);

        }

    }

    public delegate void DelegateServerLeft(object sender);

    /// <summary>
    /// Fired when the server has gone offline
    /// </summary>
    public event DelegateServerLeft EventServerLeft;

    public void NotifyServerLeft()
    {

        if (EventServerLeft != null)
        {

            EventServerLeft(this);

        }

    }

    public delegate void DelegateGameReady(object sender);

    /// <summary>
    /// Fired when the game is ready
    /// </summary>
    public event DelegateGameReady EventGameReady;

    [RPC]
    public void RPCNotifyGameReady()
    {

        if (EventGameReady != null)
        {

            EventGameReady(this);

        }

    }

    public void PlayerReady()
    {

        if (Action == BuildMode.SinglePlayer)
        {

            //The game is ready when the (only) player is ready
            RPCNotifyGameReady();

        }
        else
        {

            //The game is ready only when all players have dismissed the tutorial
            networkView.RPC("RPCTutorialDismissed", RPCMode.All, Network.player);
            
        }

    }

    /// <summary>
    /// The current arena name
    /// </summary>
    public string ArenaName;

    /// <summary>
    /// The current game mode
    /// </summary>
    public int GameMode = GameModes.Any;

    /// <summary>
    /// The build mode
    /// </summary>
    public BuildMode Action = BuildMode.SinglePlayer;

    /// <summary>
    /// Builds the game with the proper arena, game mode and modality
    /// </summary>
    public void BuildGame()
    {

        switch (Action)
        {
            case BuildMode.SinglePlayer:
                
                gameObject.AddComponent<SinglePlayerBuilder>();
                break;

            case BuildMode.RemoteHost:

                NetworkBuilder = gameObject.AddComponent<HostBuilder>();
                
                Application.LoadLevel("MenuMatchmaking");
				
				NetworkBuilder.EventDisconnected += NetworkBuilder_EventDisconnected;

                break;

            case BuildMode.RemoteGuest:

                NetworkBuilder = gameObject.AddComponent<ClientBuilder>();

                Application.LoadLevel("MenuMatchmaking");
				
				NetworkBuilder.EventDisconnected += NetworkBuilder_EventDisconnected;

                break;

        }

        
        
        this.enabled = false;
        
    }

    void OnDestroy()
    {

        if (Action != BuildMode.SinglePlayer)
        {

            NetworkBuilder.EventDisconnected -= NetworkBuilder_EventDisconnected;

        }
        
    }

    void NetworkBuilder_EventDisconnected(object sender, string message)
    {

        NotifyServerLeft();

    }
        
    void Awake()
    {
		DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {

        NetworkBuilder = null;

    }

    // Update is called once per frame
    void Update()
    {

    }

}
