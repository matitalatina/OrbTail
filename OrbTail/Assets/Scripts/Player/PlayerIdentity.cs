using UnityEngine;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Represents a player identity, this script shold be attached to every ship
/// </summary>
public class PlayerIdentity : MonoBehaviour {

    /// <summary>
    /// The ordinal number of the player
    /// </summary>
    public int Id;

    /// <summary>
    /// The nickname of the player
    /// </summary>
    public string Name;

    /// <summary>
    /// Is the player human?
    /// </summary>
    public bool IsHuman;

    /// <summary>
    /// Is the player local?
    /// </summary>
    public bool IsLocal
    {
        get
        {
            return NetworkHelper.IsOwnerSide(gameObject.networkView);
        }
    }

    /// <summary>
    /// The score of the player
    /// </summary>
    public int Score;

    /// <summary>
    /// Returns the color of the ship
    /// </summary>
    public Color Color
    {

        get
        {

            switch (Id)
            {
                case 0:

                    return Color.red;
                    
                case 1:

                    return Color.blue;

                case 2:

                    return Color.green;

                case 3:

                    return Color.yellow;

                default:

                    System.Diagnostics.Debug.Assert(false, "The id is invalid!");
                    break;

            }

            return Color.black;

        }

    }
    
    /// <summary>
    /// Returns the tail length
    /// </summary>
    public int TailLength
    {

        get
        {

            return tail_.GetOrbCount();

        }

    }

	void Awake () {

        Score = 0;
        Id = 0;
        Name = "";

	}

    void Start()
    {

        tail_ = GetComponent<Tail>();

    }

    /// <summary>
    /// The tail controller
    /// </summary>
    private Tail tail_;

}
