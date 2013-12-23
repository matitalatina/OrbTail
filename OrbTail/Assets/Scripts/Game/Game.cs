using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {

    #region Events

    public delegate void DelegateGameStart(object sender);

    public delegate void DelegateGameEnd(object sender);

    public delegate void DelegateGameTick(object sender, int time_left);

    public event DelegateGameStart EventStart;

    public event DelegateGameEnd EventEnd;

    public event DelegateGameTick EventTick;

    [RPC]
    private void RPCNotifyStart()
    {

        if (EventStart != null)
        {

            EventStart(this);

        }

        if (Network.isServer)
        {

            networkView.RPC("RPCNotifyStart", RPCMode.Others);

        }

    }
        
    [RPC]
    private void RPCNotifyEnd()
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

    private void NotifyTick()
    {

        if (EventTick != null)
        {

            EventTick(this, TimeLeft);

        }

    }

    #endregion


    /// <summary>
    /// Match duration in seconds
    /// </summary>
    public int MatchDuration;

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

    /// <summary>
    /// Time left
    /// </summary>
    public int TimeLeft { get; private set; }

	// Use this for initialization
	void Start () {

        start_time = Time.time;

        TimeLeft = MatchDuration;

        tick_accumulator = 0.0f;

        EventTick += Game_EventTick;

	}

    void Game_EventTick(object sender, int time_left)
    {

        Debug.Log(time_left);

    }
	
	// Update is called once per frame
	void Update () {

        TimeLeft = Mathf.Max(0, MatchDuration - Mathf.FloorToInt(Time.time - start_time));

        tick_accumulator += Time.deltaTime;

        if (tick_accumulator >= 1.0f)
        {

            NotifyTick();
            tick_accumulator = 0.0f;

        }

        if (TimeLeft <= 0 &&
            NetworkHelper.IsServerSide())
        {

            RPCNotifyEnd();

        }

	}

    private IEnumerable<GameObject> ships_ = null;

    private GameObject active_player_ = null;

    private float start_time = 0.0f;

    private float tick_accumulator = 0.0f;
    

}
