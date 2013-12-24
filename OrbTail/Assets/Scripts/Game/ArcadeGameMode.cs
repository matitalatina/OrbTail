using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The player with the higher score at the end of the match wins
/// </summary>
public class ArcadeGameMode: BaseGameMode
{
    #region Scores

    public const int kOrbAttachedScore = 10;

    #endregion

    public ArcadeGameMode(Game game)
    {

        game.EventTick += game_EventTick;

        foreach (GameObject ship in game.ShipsInGame)
        {

            ship.GetComponent<Tail>().OnEventOrbAttached += ArcadeGameMode_OnEventOrbAttached;
            ship.GetComponent<GameIdentity>().Score = 0;

        }

    }

    void ArcadeGameMode_OnEventOrbAttached(object sender, GameObject orb, GameObject ship)
    {

        var identity = ship.GetComponent<GameIdentity>();

        identity.Score += kOrbAttachedScore;

    }

    public override IList<GameObject> Rank
    {
	 
        get
        { 
            
            var ships = from s in GameObject.FindGameObjectsWithTag(Tags.Ship)
                        orderby s.GetComponent<GameIdentity>().Score descending
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

