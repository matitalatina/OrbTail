using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The player with the longest tail at the end of the match wins
/// </summary>
public class LongestTailGameMode: BaseGameMode
{


    public override string Name
    {

        get
        {

            return "Longest tail";

        }

    }

    public LongestTailGameMode(Game game): base(game)
    {

        game.EventTick += game_EventTick;

        foreach (GameObject ship in game.ShipsInGame)
        {

            ship.GetComponent<Tail>().OnEventOrbAttached += LongestTailGameMode_OnEventOrbAttached;
            ship.GetComponent<Tail>().OnEventOrbDetached += LongestTailGameMode_OnEventOrbDetached;

            ship.GetComponent<GameIdentity>().ResetScore();

        }

    }

    void LongestTailGameMode_OnEventOrbDetached(object sender, GameObject ship, int count)
    {

        var identity = ship.GetComponent<GameIdentity>();

        identity.SetScore(ship.GetComponent<Tail>().GetOrbCount());

    }

    void LongestTailGameMode_OnEventOrbAttached(object sender, GameObject orb, GameObject ship)
    {

        var identity = ship.GetComponent<GameIdentity>();

        identity.SetScore(ship.GetComponent<Tail>().GetOrbCount());

    }
    
    public override GameObject Winner
    {
	 
        get
        {

            var ships = Game.ShipsInGame;

            //Find the longest tail (Can't use linq.max because iOS sucks)
            int longest_tail = 0;
            int current_tail = 0;

            foreach (GameObject go in ships)
            {

                current_tail = go.GetComponent<Tail>().GetOrbCount();

                if (longest_tail < current_tail)
                {

                    longest_tail = current_tail;

                }

            }

            var winners = from s in ships
                          where s.GetComponent<Tail>().GetOrbCount() == longest_tail
                          select s;

            if (winners.Count() == 1)
            {

                return winners.First();

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

        //The game ends when the time's left
        if (time_left <= 0)
        {

            NotifyWin();

        }

    }

}

