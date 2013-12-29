using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameModes
{

    public const int Any = -1;
    public const int Arcade = 0;
    public const int LongestTail = 1;
    public const int Elimination = 2;

    public const int Min = Arcade;
    public const int Max = Elimination;

    /// <summary>
    /// Resolves a game mode. If Any was specified, a random game mode will be chosen
    /// </summary>
    public static int Resolve(int game_mode)
    {

        if (game_mode == GameModes.Any)
        {

            var rng = new System.Random();

            return rng.Next(GameModes.Min, GameModes.Max + 1);

        }
        else
        {

            return game_mode;

        }

    }
    
}

