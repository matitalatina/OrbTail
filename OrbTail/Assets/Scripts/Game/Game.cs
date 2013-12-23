using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {

    #region Events

    public delegate void DelegateGameStart(object sender, int countdown);

    public delegate void DelegateGameEnd(object sender);

    public delegate void DelegateGameTick(object sender, int time_left);

    public event DelegateGameStart EventStart;

    public event DelegateGameEnd EventEnd;

    public event DelegateGameTick EventTick;

    private void NotifyStart(int countdown)
    {

        if (NetworkHelper.IsServerSide())
        {

            if (EventStart != null)
            {

                EventStart(this, countdown);

            }

            if (Network.isServer)
            {

                networkView.RPC("RPCNotifyStart", RPCMode.Others);

            }

        }
       
    }

    private void NotifyEnd()
    {

        if (NetworkHelper.IsServerSide())
        {

            if (EventEnd != null)
            {

                EventEnd(this);

            }

            if (Network.isServer)
            {

                networkView.RPC("RPCNotifyEnd", RPCMode.Others);

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

    #endregion

    /// <summary>
    /// Match duration in seconds
    /// </summary>
    public int MatchDuration;

    /// <summary>
    /// Countdown duration in seconds
    /// </summary>
    public int CountdownDuration;

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

        int counter = MatchDuration;

        do
        {

            NotifyTick(counter);

            yield return new WaitForSeconds(1);

            counter--;

        } while (counter > 0);

        //End of the game
        NotifyEnd();

    }

    private IEnumerable<GameObject> ships_ = null;

    private GameObject active_player_ = null;

    

}
