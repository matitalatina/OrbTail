using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {

    /// <summary>
    /// Returns the active player
    /// </summary>
    public GameObject ActivePlayer
    {
        get
        {

            if (active_player_ == null)
            {

                active_player_ = GameObject.FindGameObjectsWithTag(Tags.Ship).Where((GameObject o) =>
                {

                    return o.GetComponent<PlayerIdentity>().IsHuman &&
                           NetworkHelper.IsOwnerSide(o.networkView);

                }).First();

            }

            return active_player_;

        }

    }
    
    /// <summary>
    /// List of all ships in game
    /// </summary>
    public IEnumerable<GameObject> ShipsInGame
    {

        get
        {

            if (ships_ == null)
            {

                ships_ = GameObject.FindGameObjectsWithTag(Tags.Ship);

            }

            return ships_;

        }

    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    private IEnumerable<GameObject> ships_ = null;

    private GameObject active_player_ = null;

}
