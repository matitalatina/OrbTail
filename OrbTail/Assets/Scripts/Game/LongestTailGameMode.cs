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

    public LongestTailGameMode(Game game)
    {

        game.EventTick += game_EventTick;

    }

    public override GameObject Winner
    {
	 
        get
        {

            var ships = from s in GameObject.FindGameObjectsWithTag(Tags.Ship)
                        where s.activeSelf
                        select s;

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
    
    private void game_EventTick(object sender, int time_left)
    {

        //The game ends when the time's left
        if (time_left <= 0)
        {

            NotifyWin();

        }

    }

}

