using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {

	private float restart_time = 6f;

    public const string explosion_prefab_path = "Prefabs/Power/Explosion";

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

        //Deactivate the ship
        ship.SetActive(false);

        StartCoroutine(ShipExplosion(ship.transform.position));

        NotifyShipEliminated(ship);
        
    }

    private IEnumerator ShipExplosion(Vector3 position)
    {

        GameObject explosion = GameObject.Instantiate(explosion_resource_, position, Quaternion.identity) as GameObject;

        AudioSource.PlayClipAtPoint(explosion_sound_, position);

        // Delayed for GFX
        yield return new WaitForSeconds(1.0f);

        Destroy(explosion);

    }
    
    private IEnumerator ActivateSpectatorMode()
    {

        yield return new WaitForSeconds(kSpectatorModeDelay);

        Camera.GetComponent<SpectatorMode>().enabled = true;

    }


	// Use this for initialization
	void Start () {

        //Graphics stuff
        explosion_sound_ = Resources.Load<AudioClip>("Sounds/Powers/Explosion");
        explosion_resource_ = Resources.Load<GameObject>(explosion_prefab_path);

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
        if (NetworkHelper.IsServerSide())
        {

            //Clients will rely on the server
            game_mode_.EventWin += GameMode_EventEnd;

        }
        
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
        //If the end was fired we don't care about the server, we are going to terminate the match anyways
        if (!event_end_fired_)
        {

            GetComponent<PowerGenerator>().enabled = false;

            //Stops the coroutine
            StopCoroutine("UpdateGameTime");
            StopCoroutine("UpdateCountdown");

            EnableControls(false);

            NotifyEnd(kInfoServerLeft);

            StartCoroutine("RestartGame");   

        }
        
    }

    void master_EventPlayerLeft(object sender, int id)
    {

        if (!event_end_fired_)
        {

            //Restores the orbs
            var disconnected_player = (from player in ShipsInGame
                                       where player.GetComponent<GameIdentity>().Id == id
                                       select player).First();

            //Detaches all orbs from the player's tail
            var orbs = (from orb in disconnected_player.GetComponent<Tail>().DetachOrbs(int.MaxValue)
                        select orb);

            if (Network.isServer)
            {

                var ownership_mgr = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<OwnershipMgr>();

                foreach (GameObject orb in orbs)
                {

                    networkView.RPC("RPCChangeOwnership", RPCMode.All, orb.networkView.viewID, ownership_mgr.FetchViewID(Network.player));

                }

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

        if (Network.isServer)
        {

            networkView.RPC("EndMatch", RPCMode.All);

        }
        else if(!Network.isClient)
        {

            EndMatch();

        }

    }

    [RPC]
    private void EndMatch(){

        GetComponent<PowerGenerator>().enabled = false;

        //Stops the coroutine
        StopCoroutine("UpdateGameTime");
        StopCoroutine("UpdateCountdown");

        EnableControls(false);

        NotifyEnd(kInfoNone);

        StartCoroutine("RestartGame");

    }

	/// <summary>
	/// Restarts the game. Temporary method
	/// </summary>
	private IEnumerator RestartGame() {
		
        yield return new WaitForSeconds(restart_time);

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

        float counter = CountdownDuration;

        float beg, end;

        do
        {

            NotifyStart((int)counter);

            beg = Time.realtimeSinceStartup;

            yield return new WaitForSeconds(1);

            end = Time.realtimeSinceStartup;

            counter -= (end - beg);

        } while (counter > 0);

        //End of the countdown
        NotifyStart(0);

        EnableControls(true);

        StartCoroutine("UpdateGameTime");
        
    }

    float game_time_counter;

    /// <summary>
    /// Used to update the game time
    /// </summary>
    private IEnumerator UpdateGameTime()
    {

        game_time_counter = game_mode_.Duration;

        float beg, end;

        do
        {

            NotifyTick((int)game_time_counter);

            beg = Time.realtimeSinceStartup;

            yield return new WaitForSeconds(1);

            end = Time.realtimeSinceStartup;

            game_time_counter -= (end - beg);

            if (Network.isServer)
            {

                networkView.RPC("RPCSyncTime", RPCMode.Others, game_time_counter);

            }
            else if (Network.isClient)
            {

                //This should prevent the end of the match to be fired from clients
                game_time_counter = Mathf.Max(1.0f, game_time_counter);

            }

        } while (!(NetworkHelper.IsServerSide() && game_time_counter <= 0));

        NotifyTick(0);

    }

    [RPC]
    private void RPCSyncTime(float time)
    {

        game_time_counter = time;

    }

    [RPC]
    private void RPCChangeOwnership(NetworkViewID old_view_id, NetworkViewID new_view_id)
    {

        NetworkView.Find(old_view_id).networkView.viewID = new_view_id;

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

    AudioClip explosion_sound_;
    GameObject explosion_resource_;

}
