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

    public override int Duration
    {

        get
        {

            return 30;

        }

    }


    public override string Name
    {

        get
        {

            return "Arcade";

        }

    }

    public ArcadeGameMode(Game game): base(game)
    {

        game.EventTick += game_EventTick;

        foreach (GameObject ship in game.ShipsInGame)
        {

            ship.GetComponent<Tail>().OnEventOrbAttached += ArcadeGameMode_OnEventOrbAttached;

            ship.GetComponent<GameIdentity>().ResetScore();

        }

    }

    ~ArcadeGameMode()
    {

        /*
        Game.EventTick -= game_EventTick;

        foreach (GameObject ship in Game.ShipsInGame)
        {

            ship.GetComponent<Tail>().OnEventOrbAttached -= ArcadeGameMode_OnEventOrbAttached;

        }
        */
    }

    void ArcadeGameMode_OnEventOrbAttached(object sender, GameObject orb, GameObject ship)
    {

        var identity = ship.GetComponent<GameIdentity>();

        identity.AddScore(kOrbAttachedScore);

    }

    public override GameObject Winner
    {
	 
        get
        {

            var ships = Game.ShipsInGame;

            Debug.Log("Found " + ships.Count() + " ships");

            //Find the highest score
            int highest_score = ships.Max( (GameObject go) => { return go.GetComponent<GameIdentity>().Score; } );

            var winners = from s in ships
                          where s.GetComponent<GameIdentity>().Score == highest_score
                          select s;

            Debug.Log("Winners " + winners.Count());

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

