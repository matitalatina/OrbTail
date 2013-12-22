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
    public void InitializeHost()
    {

        DontDestroyOnLoad(gameObject);

        GetComponent<HostBuilder>().enabled = true;

        this.enabled = false;

    }

    public void InitializeClient()
    {

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

}
