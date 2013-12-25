using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {

	private float restartTime = 6f;

    #region Events

    public delegate void DelegateGameStart(object sender, int countdown);

    public delegate void DelegateGameEnd(object sender, GameObject winner);

    public delegate void DelegateGameTick(object sender, int time_left);

    public event DelegateGameStart EventStart;

    public event DelegateGameEnd EventEnd;

    public event DelegateGameTick EventTick;
    
    private void NotifyStart(int countdown)
    {

        if (EventStart != null)
        {

            EventStart(this, countdown);

        }

    }

    [RPC]
    private void NotifyEnd()
    {

        //TODO: Remove this
        GameObject winner = game_mode_.Winner;

        if( winner == null ){

            Debug.Log("Tie");

        }else{

            GameIdentity gi = winner.GetComponent<GameIdentity>();

            Debug.Log("Player " + gi.Id + " wins");
    
        }
        //

        if (EventEnd != null)
        {

            EventEnd(this, game_mode_.Winner);

        }
        
    }

    private void NotifyTick(int time_left)
    {

        if (EventTick != null)
        {

            EventTick(this, time_left);

        }

    }

    #endregion

    /// <summary>
    /// Countdown duration in seconds
    /// </summary>
    public int CountdownDuration = 3;

    /// <summary>
    /// The current game mode
    /// </summary>
    public int GameMode = -1;

    /// <summary>
    /// The current duration
    /// </summary>
    public int Duration = 30;

    [RPC]
    public void RPCSetGame(int game_mode)
    {

        GameMode = game_mode;

    }

    /// <summary>
    /// Returns the active player
    /// </summary>
    public GameObject ActivePlayer
    {
        get
        {

            if (active_player_ == null)
            {

                active_player_ = GameObject.FindGameObjectsWithTag(Tags.Ship).Where((GameObject o) =>
                {

                    return o.GetComponent<PlayerIdentity>().IsHuman &&
                           NetworkHelper.IsOwnerSide(o.networkView);

                }).First();

            }

            return active_player_;

        }

    }
    
    /// <summary>
    /// List of all ships in game
    /// </summary>
    public IEnumerable<GameObject> ShipsInGame
    {

        get
        {

            if (ships_ == null)
            {

                ships_ = GameObject.FindGameObjectsWithTag(Tags.Ship);

            }

            return ships_;

        }

    }

	// Use this for initialization
	void Start () {

        //Create the proper game mode
        switch (GameMode)
        {
            case GameModes.Arcade:

                game_mode_ = new ArcadeGameMode(this);
                break;

            case GameModes.LongestTail:

                game_mode_ = new LongestTailGameMode(this);
                break;

            case GameModes.Elimination:

                game_mode_ = new EliminationGameMode(this);
                break;

            default:

                System.Diagnostics.Debug.Assert(false, "No game mode specified");
                break;

        }

        game_mode_.EventWin += GameMode_EventEnd;

        EnableControls(false);
        
        //Ok the game is ready

        var master = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();

        master.NotifyGameBuilt();

        StartCoroutine("UpdateCountdown");

	}
	
	// Update is called once per frame
	void Update () {


     

	}

    /// <summary>
    /// Enable or disable the controls for all players
    /// </summary>
    private void EnableControls(bool value)
    {

        MovementController movement;

        foreach (GameObject ship in ShipsInGame)
        {

            movement = ship.GetComponent<MovementController>();

            if (movement != null)
            {

                movement.enabled = value;

            }

        }

    }

    private void GameMode_EventEnd(BaseGameMode sender)
    {

        EnableControls(false);

        //End of the game (only the server can declare the end of a match)
        if (Network.isServer)
        {

            networkView.RPC("NotifyEnd", RPCMode.All);

        }
        else
        {

            NotifyEnd();

        }

		StartCoroutine("RestartGame");

    }

	/// <summary>
	/// Restarts the game. Temporary method
	/// </summary>
	private IEnumerator RestartGame() {
		yield return new WaitForSeconds(restartTime);
		Destroy(GameObject.FindGameObjectWithTag(Tags.Master));
		Application.LoadLevel("MenuMain");
	}

    /// <summary>
    /// Used to update the countdown timer
    /// </summary>
    private IEnumerator UpdateCountdown()
    {

        int counter = CountdownDuration;

        do
        {

            NotifyStart(counter);

            yield return new WaitForSeconds(1);

            counter--;

        } while (counter > 0);

        //End of the countdown
        NotifyStart(0);

        EnableControls(true);

        StartCoroutine("UpdateGameTime");
        
    }

    /// <summary>
    /// Used to update the game time
    /// </summary>
    private IEnumerator UpdateGameTime()
    {

        int counter = Duration;

        do
        {

            NotifyTick(counter);

            yield return new WaitForSeconds(1);

            counter--;

        } while (counter > 0);

        NotifyTick(counter);

    }

    [RPC]
    private void RPCSetGameMode(int game_mode)
    {

        GameMode = game_mode;

    }
    
    [RPC]
    private void RPCSetDuration(int duration)
    {

        Duration = duration;

    }

    [RPC]
    private void RPCGameEnable(bool value)
    {

        Debug.Log("Enabling the game");
        this.enabled = value;

    }

    private IList<GameObject> ships_ = null;

    private GameObject active_player_ = null;
    
    private BaseGameMode game_mode_;

}
