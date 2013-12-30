using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used in spectator mode to cycle among different players
/// </summary>
public class SpectatorMode : MonoBehaviour {

	// Use this for initialization
	void Start () {

        camera_movement_ = GetComponent<CameraMovement>();

        game_ = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();

        ships_ = new List<GameObject>(game_.ShipsInGame);

        game_.EventShipEliminated += game__EventShipEliminated;

        LookNext();

	}

    /// <summary>
    /// Removes the eliminated ship and look at the next ship
    /// </summary>
    void game__EventShipEliminated(object sender, GameObject ship)
    {

        ships_.Remove(ship);

        if (camera_movement_.Target == ship){

            LookNext();

        }       

    }

    /// <summary>
    /// Looks to the next ship
    /// </summary>
    private void LookNext()
    {

        if (ships_.Count > 0)
        {

            var element = ships_[0];

            ships_.RemoveAt(0);

            ships_.Add(element);

            camera_movement_.LookAt(ships_[0]);

        }

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis(DesktopInputBroker.fire_main_axis_name) > 0.0f ||
            Input.touchCount > 0)
        {

            if (change_target)
            {

                change_target = false;

                LookNext();
                
            }

        }
        else
        {

            //No key or no touch, reset
            change_target = true;

        }


	}
    
    private bool change_target = true;
    
    private Game game_;

    private IList<GameObject> ships_;

    private CameraMovement camera_movement_;

}
