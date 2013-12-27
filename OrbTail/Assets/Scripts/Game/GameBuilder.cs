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

                break;

            case BuildMode.RemoteGuest:

                NetworkBuilder = gameObject.AddComponent<ClientBuilder>();

                Application.LoadLevel("MenuMatchmaking");

                break;

        }
        
        this.enabled = false;
        
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
