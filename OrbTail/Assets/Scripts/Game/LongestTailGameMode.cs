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

    public override IList<GameObject> Rank
    {
	 
        get
        { 
            
            var ships = from s in GameObject.FindGameObjectsWithTag(Tags.Ship)
                        where s.activeSelf
                        orderby s.GetComponent<Tail>().GetOrbCount() descending
                        select s;

            return new List<GameObject>( ships );

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

