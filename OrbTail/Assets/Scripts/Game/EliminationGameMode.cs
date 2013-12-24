using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The last player standing wins the game. If the player has no more orbs it gets eliminated
/// </summary>
public class EliminationGameMode: BaseGameMode
{

    public EliminationGameMode(Game game)
    {

        game.EventTick += game_EventTick;

        ships_ = new List<GameObject>(game.ShipsInGame);

        Tail tail;
        var tails = new List<Tail>();

        foreach (GameObject ship in game.ShipsInGame)
        {

            tail = ship.GetComponent<Tail>();

            tail.OnEventOrbDetached += EliminationGameMode_OnEventOrbDetached;
            tails.Add( tail ) ;

        }

        //Give some orbs to all ships
        if (NetworkHelper.IsServerSide())
        {

            int i = 0;

            foreach (GameObject orb in GameObject.FindGameObjectsWithTag(Tags.Orb))
            {

                if (orb != null)
                {

                    tails[i].AttachOrb(orb);

                    i = ++i % ships_.Count;

                }
                
            }

        }

    }

    public override GameObject Winner
    {
	 
        get
        {

            if (ships_.Count == 1)
            {

                return ships_.First();

            }
            else
            {

                return null;

            }

        }

    }
    
    private void game_EventTick(object sender, int time_left)
    {

        if( !end_of_match &&
            (time_left <= 0 ||
             ships_.Count <= 1) ){

            //The time's up or there are less than one ships in game
            NotifyWin();

            end_of_match = true;

        }

        //TODO: remove an orb from time to time

    }

    private void EliminationGameMode_OnEventOrbDetached(object sender, GameObject ship)
    {

        if (ship.GetComponent<Tail>().GetOrbCount() == 0)
        {

            //The ship should be eliminated
            ships_.Remove(ship);

            //TODO: Create the spectator camera, tell the ship that it has been eliminated
            //TODO: disable the ship properly!
            ship.SetActive(false);

        }

    }

    private IList<GameObject> ships_;

    private bool end_of_match = false;

}

