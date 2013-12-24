using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {

    #region Events

    public delegate void DelegateGameStart(object sender, int countdown);

    public delegate void DelegateGameEnd(object sender, IList<GameObject> rank);

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

        if (EventEnd != null)
        {

            EventEnd(this, game_mode_.Rank);

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
    public int Duration = 300;

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


        StartCoroutine("UpdateCountdown");

	}
	
	// Update is called once per frame
	void Update () {


     

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

        //End of the game (only the server can declare the end of a match)
        if (Network.isServer)
        {

            networkView.RPC("NotifyEnd", RPCMode.All);

        }
        else
        {

            NotifyEnd();

        }
        
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

        this.enabled = value;

    }

    private IEnumerable<GameObject> ships_ = null;

    private GameObject active_player_ = null;
    
    private IGameMode game_mode_;

}
