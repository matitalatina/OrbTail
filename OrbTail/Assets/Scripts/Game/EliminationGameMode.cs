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

        foreach (GameObject ship in game.ShipsInGame)
        {

            ship.GetComponent<Tail>().OnEventOrbDetached += EliminationGameMode_OnEventOrbDetached;

        }

        //TODO: Attach some orbs to begin!

    }

    public override IList<GameObject> Rank
    {
	 
        get
        {

            return new List<GameObject>(ships_);

        }

    }
    
    private void game_EventTick(object sender, int time_left)
    {

        if( !end_of_match &&
            (time_left <= 0 ||
             ships_.Count <= 1) ){

            //The time's up or there are less than one ships in game
            NotifyWin();

        }

        //TODO: Remove an orb

    }

    private void EliminationGameMode_OnEventOrbDetached(object sender, GameObject ship)
    {

        if (ship.GetComponent<Tail>().GetOrbCount() == 0)
        {

            //The ship should be eliminated
            ships_.Remove(ship);

            //TODO: Create the spectator camera, tell the ship that it has been eliminated
            ship.SetActive(false);

        }

    }

    private IList<GameObject> ships_;

    private bool end_of_match = false;

}

