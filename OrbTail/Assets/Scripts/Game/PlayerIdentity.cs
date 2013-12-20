using UnityEngine;
using System.Collections;

public class PlayerIdentity : MonoBehaviour {

    /// <summary>
    /// The player name
    /// </summary>
    public string Name;

    /// <summary>
    /// The player ship's name
    /// </summary>
    public string ShipName;

    /// <summary>
    /// Is this player human?
    /// </summary>
    public bool IsHuman;

    /// <summary>
    /// Copies this identity to another identity
    /// </summary>
    public void CopyTo(PlayerIdentity other)
    {

        other.Name = Name;
        other.ShipName = ShipName;
        other.IsHuman = IsHuman;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
