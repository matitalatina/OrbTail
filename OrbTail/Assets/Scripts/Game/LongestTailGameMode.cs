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

    void LongestTailGameMode_OnEventOrbDetached(object sender, GameObject ship)
    {

        var identity = ship.GetComponent<GameIdentity>();

        //One orb detached
        identity.AddScore(-1);

    }

    void LongestTailGameMode_OnEventOrbAttached(object sender, GameObject orb, GameObject ship)
    {

        var identity = ship.GetComponent<GameIdentity>();

        //One orb attached
        identity.AddScore(1);

    }

    ~LongestTailGameMode()
    {

       // Game.EventTick -= game_EventTick;

    }

    public override GameObject Winner
    {
	 
        get
        {

            var ships = Game.ShipsInGame;

            //Find the longest tail
            int longest_tail = ships.Max((GameObject go) => { return go.GetComponent<Tail>().GetOrbCount(); });

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

