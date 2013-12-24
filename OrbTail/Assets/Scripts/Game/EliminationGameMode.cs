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

        foreach (GameObject ship in game.ShipsInGame)
        {

            

        }

    }
    public override IList<GameObject> Rank
    {
	 
        get
        {

            return null;

        }

    }
    
    private void game_EventTick(object sender, int time_left)
    {

        //The game ends when the time's left, anyway
        if (time_left <= 0)
        {

            NotifyWin();

        }

    }

}

