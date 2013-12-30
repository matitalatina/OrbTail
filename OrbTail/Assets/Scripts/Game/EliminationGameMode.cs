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


    public override string Name
    {

        get
        {

            return "Elimination";

        }

    }

    public EliminationGameMode(Game game): base(game)
    {

        game.EventTick += game_EventTick;

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

                    i = ++i % game.ShipsInGame.Count();

                }
                
            }

        }

    }

    ~EliminationGameMode()
    {

        //Game.EventTick -= game_EventTick;

        foreach (GameObject ship in Game.ShipsInGame)
        {

            //ship.GetComponent<Tail>().OnEventOrbDetached -= EliminationGameMode_OnEventOrbDetached;

        }

    }

    public override GameObject Winner
    {
	 
        get
        {

            if (Game.ShipsInGame.Count() == 1)
            {

                return Game.ShipsInGame.First();

            }
            else
            {

                return null;

            }

        }

    }

    public override int Duration
    {

        get
        {

            return 180;

        }

    }
    
    private void game_EventTick(object sender, int time_left)
    {

        if( !end_of_match &&
            (time_left <= 0 ||
             Game.ShipsInGame.Count() <= 1))
        {

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
            Game.RemoveShip(ship);

            //TODO: Create the spectator camera, tell the ship that it has been eliminated
            //TODO: disable the ship properly!
            ship.SetActive(false);

        }

    }

    private bool end_of_match = false;

}

