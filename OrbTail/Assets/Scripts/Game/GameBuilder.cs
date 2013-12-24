using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// The script builds any game
/// </summary>
public class GameBuilder : MonoBehaviour {

    public const int kMaxPlayerCount = 4;
    public const int kServerPort = 6059;
    public const string kGameTypeName = "OrbTail";

    /// <summary>
    /// The current arena name
    /// </summary>
    public string ArenaName;

    /// <summary>
    /// The current game mode
    /// </summary>
    public int GameMode = GameModes.Any;

    /// <summary>
    /// Initialize a local match, only one human is allowed! (the identities are attached to the master game object)
    /// </summary>
    /// <param name="arena_name">The arena to load</param>
    public void InitializeSinglePlayer()
    {

        gameObject.AddComponent<SinglePlayerBuilder>();

        this.enabled = false;

    }

    /// <summary>
    /// Initializes the host
    /// </summary>
    public void InitializeHost()
    {

        gameObject.AddComponent<HostBuilder>();

        this.enabled = false;

    }

    /// <summary>
    /// Initializes a client
    /// </summary>
    public void InitializeClient()
    {

        gameObject.AddComponent<ClientBuilder>();

        this.enabled = false;

    }

    void Awake()
    {
		DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
