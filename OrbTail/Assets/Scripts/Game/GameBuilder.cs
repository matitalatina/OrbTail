using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// The script builds any game
/// </summary>
public class GameBuilder : MonoBehaviour {

    public const int kMaxPlayerCount = 4;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Initialize a local match, only one human is allowed! (the identities are attached to the master game object)
    /// </summary>
    /// <param name="arena_name">The arena to load</param>
    public void InitializeSinglePlayerMatch(string arena_name)
    {

        //This object should NOT be destroyed
        DontDestroyOnLoad(gameObject);

        //Loads the proper arena
        Application.LoadLevel(arena_name);

        GetComponent<SinglePlayerBuilder>().enabled = true;

        this.enabled = false;

    }

}
