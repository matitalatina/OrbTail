using System;
using System.Collections;
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
        game.EventStart += game_EventStart;

    }

    void game_EventStart(object sender, int countdown)
    {

        Tail tail;
        var tails = new List<Tail>();

        var orbs = new Queue<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Orb));
        int orbs_per_player = orbs.Count / Game.ShipsInGame.Count();

        foreach (GameObject ship in new List<GameObject>( Game.ShipsInGame ))
        {

            tail = ship.GetComponent<Tail>();

            if (NetworkHelper.IsServerSide())
            {

                tail.DetachOrbs(int.MaxValue);

            }
            
            tail.OnEventOrbDetached += EliminationGameMode_OnEventOrbDetached;
            tail.OnEventOrbAttached += tail_OnEventOrbAttached;

            if (NetworkHelper.IsServerSide())
            {

                for (int i = 0; i < orbs_per_player; i++)
                {

                    tail.AttachOrb(orbs.Dequeue());

                }

            }

            ship.GetComponent<GameIdentity>().ResetScore();
            
        }

        //We just need the first event
        Game.EventStart -= game_EventStart;       

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
        
    }

    private void EliminationGameMode_OnEventOrbDetached(object sender, GameObject ship, int count)
    {

        var identity = ship.GetComponent<GameIdentity>();

        identity.SetScore(ship.GetComponent<Tail>().GetOrbCount());

        if (ship.GetComponent<Tail>().GetOrbCount() == 0)
        {

            //The ship should be eliminated
            Game.RemoveShip(ship);

        }

    }

    void tail_OnEventOrbAttached(object sender, GameObject orb, GameObject ship)
    {
        
        var identity = ship.GetComponent<GameIdentity>();

        identity.SetScore(ship.GetComponent<Tail>().GetOrbCount());

    }

    private bool end_of_match = false;

}

