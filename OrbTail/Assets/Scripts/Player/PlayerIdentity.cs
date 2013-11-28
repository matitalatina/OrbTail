using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a player identity
/// </summary>
public class PlayerIdentity : MonoBehaviour {

    /// <summary>
    /// The nickname of the player
    /// </summary>
    public string player_name = "";

    /// <summary>
    /// The color associated to the player
    /// </summary>
    public Color color = Color.red;

    /// <summary>
    /// The ordinal number of the player
    /// </summary>
    public int player_number = 0;

    /// <summary>
    /// The name of the ship prefab used by the player
    /// </summary>
    public string ship_name = "ship";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
