using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {

	private float restartTime = 6f;

    #region Events

    public delegate void DelegateGameStart(object sender, int countdown);

    public delegate void DelegateGameEnd(object sender, GameObject winner, int info);

    public delegate void DelegateGameTick(object sender, int time_left);

    public delegate void DelegateShipEliminated(object sender, GameObject ship);

    public event DelegateGameStart EventStart;

    public event DelegateGameEnd EventEnd;

    public event DelegateGameTick EventTick;

    public event DelegateShipEliminated EventShipEliminated;
    
    private void NotifyStart(int countdown)
    {

        if (EventStart != null)
        {

            EventStart(this, countdown);

        }

    }

    [RPC]
    private void NotifyEnd(int info)
    {

        if (EventEnd != null &&
            !event_end_fired_)
        {

            event_end_fired_ = true;

            if ((info & kInfoNoWinner) != 0)
            {

                EventEnd(this, null, info);

            }
            else {

                var winner = game_mode_.Winner;

                if (winner == null)
                {

                    EventEnd(this, null, info | kInfoNoWinner);

                }
                else
                {

                    EventEnd(this, winner, info);

                }
                
            }

        }
        
    }

    private void NotifyTick(int time_left)
    {

        if (EventTick != null)
        {

            EventTick(this, time_left);

        }

    }

    private void NotifyShipEliminated(GameObject ship)
    {
        
        if (EventShipEliminated != null)
        {

            EventShipEliminated(this, ship);
            
        }

    }

    /// <summary>
    /// This flags avoid multiple ends...
    /// </summary>
    private bool event_end_fired_ = false;

    /// <summary>
    /// Nothing to signal so far
    /// </summary>
    public const int kInfoNone = 0;

    /// <summary>
    /// The game ended because the server left the match!
    /// </summary>
    public const int kInfoServerLeft = 1 | kInfoNoWinner;

    /// <summary>
    /// The game ended with no winner
    /// </summary>
    public const int kInfoNoWinner = 2;

    #endregion

    /// <summary>
    /// Countdown duration in seconds
    /// </summary>
    public int CountdownDuration = 3;

    /// <summary>
    /// Seconds the user have to wait when it has been destroyed
    /// </summary>
    public float kSpectatorModeDelay = 3f;

    /// <summary>
    /// The current game mode
    /// </summary>
    public int GameMode = -1;

    /// <summary>
    /// Return the name of the game mode
    /// </summary>
    public string GameModeName
    {
        get
        {

            return game_mode_.Name;

        }

    }

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
    /// Returns the active camera
    /// </summary>
    public GameObject Camera
    {
        
        get
        {

            if (active_camera_ == null)
            {

                active_camera_ = GameObject.FindGameObjectWithTag(Tags.MainCamera);

            }

            return active_camera_;

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

                ships_ = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Ship));

            }

            return ships_;

        }

    }

    public void RemoveShip(GameObject ship)
    {

        ships_.Remove(ship);

        //Enables the spectator mode
        if (ship == ActivePlayer)
        {

            StartCoroutine(ActivateSpectatorMode());

        }

        ship.SetActive(false);

        //TODO: Add an explosion??

        NotifyShipEliminated(ship);
        
    }
    
    private IEnumerator ActivateSpectatorMode()
    {

        yield return new WaitForSeconds(kSpectatorModeDelay);

        Camera.GetComponent<SpectatorMode>().enabled = true;

    }


	// Use this for initialization
	void Start () {


        var master = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();

        master.EventPlayerLeft += master_EventPlayerLeft;
        master.EventServerLeft += master_EventServerLeft;
        master.EventGameReady += master_EventGameReady;

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
        
        //Ok the game has been built

        game_mode_.EventWin += GameMode_EventEnd;
        EventStart += Game_EventStart;

        EnableControls(false);

		master.NotifyGameBuilt();
        
	}

    void master_EventGameReady(object sender)
    {

        StartCoroutine("UpdateCountdown");

    }

    void master_EventServerLeft(object sender)
    {

        //This is called only on client-side

        //Stops the coroutine
        StopCoroutine("UpdateGameTime");

        NotifyEnd(kInfoServerLeft);

        //Purges the instantiated objects
        GameObjectFactory.Instance.Purge();

        StartCoroutine("RestartGame");      

    }

    void master_EventPlayerLeft(object sender, int id)
    {

        //Restores the orbs
        var disconnected_player = (from player in ShipsInGame
                                   where player.GetComponent<GameIdentity>().Id == id
                                   select player).First();

        //Detaches all orbs from the player's tail
        var orbs = (from orb in disconnected_player.GetComponent<Tail>().DetachOrbs(int.MaxValue)
                    select orb);

        //Restores the orbs ownership
        var ownership_mgr = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<OwnershipManager>();

        foreach (GameObject orb in orbs)
        {

            ownership_mgr.RestoreOwnership(orb);

        }

        //Removes the disconnected player
        RemoveShip(disconnected_player);

        //Destroy the player who left
        Network.RemoveRPCs(disconnected_player.networkView.owner);
        Network.DestroyPlayerObjects(disconnected_player.networkView.owner);
        
        //There's only one player, he must have won
        if (ShipsInGame.Count() <= 1)
        {

            game_mode_.NotifyWin();

        }

    }

    void Game_EventStart(object sender, int countdown)
    {
        
        //Routines when the game starts

        if (countdown == 0)
        {


            GetComponent<PowerGenerator>().enabled = true;

            if (NetworkHelper.IsServerSide())
            {

                var power_controllers = from player in ShipsInGame
                                        select player.GetComponent<PowerController>();

                foreach (var power_controller in power_controllers)
                {
                        
                    //Adds a boost to the ship, only it this is the server
                    power_controller.AddPower(new Boost());

                }

            }

        }
        
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

        //Stops the coroutine
        StopCoroutine("UpdateGameTime");

        EnableControls(false);

        //End of the game (only the server can declare the end of a match)
        if (Network.isServer)
        {

            networkView.RPC("NotifyEnd", RPCMode.All, kInfoNone);

        }
        else
        {

            NotifyEnd(kInfoNone);

        }

        //Purges the instantiated objects
        GameObjectFactory.Instance.Purge();

		StartCoroutine("RestartGame");

    }

	/// <summary>
	/// Restarts the game. Temporary method
	/// </summary>
	private IEnumerator RestartGame() {
		yield return new WaitForSeconds(restartTime);
		Destroy(GameObject.FindGameObjectWithTag(Tags.Master));

        GameObjectFactory.Instance.Purge();

        //Okay, good game, let's go home...
        Network.Disconnect();

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

        int counter = game_mode_.Duration;

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
    private void RPCGameEnable(bool value)
    {

        Debug.Log("Enabling the game");
        this.enabled = value;

    }

    private IList<GameObject> ships_ = null;

    private GameObject active_player_ = null;

    private GameObject active_camera_ = null;
    
    private BaseGameMode game_mode_;

}
