using UnityEngine;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Represents a game identity, this script shold be attached to every ship
/// </summary>
public class GameIdentity : MonoBehaviour {

    public delegate void DelegateScore(object sender, int delta_score, int total_score);

    public DelegateScore EventScore;

    private void NotifyScore(int delta_score)
    {

        if (EventScore != null)
        {

            EventScore(this, delta_score, Score);

        }

    }

    public delegate void DelegateIdSet(object sender, int id);

    public DelegateIdSet EventIdSet;

    private void NotifyIdSet(int id)
    {

        if (EventIdSet != null)
        {

            EventIdSet(this, id);

        }

    }

    /// <summary>
    /// The ordinal number of the player
    /// </summary>
    public int Id;

    public Color Color
    {
        get
        {

            switch (Id)
            {
                case 0:

                    return Color.red;
                    
                case 1:

                    return Color.blue;

                case 2:

                    return Color.green;

                default:

                    return Color.yellow;

            }

        }
    }

    /// <summary>
    /// Is the player local?
    /// </summary>
    public bool IsLocal
    {
        get
        {
            return NetworkHelper.IsOwnerSide(gameObject.networkView);
        }
    }

    /// <summary>
    /// The score of the player
    /// </summary>
    public int Score;

    /// <summary>
    /// Returns the tail length
    /// </summary>
    public int TailLength
    {

        get
        {

            return tail_.GetOrbCount();

        }

    }

    void Start()
    {

        tail_ = GetComponent<Tail>();

    }

    /// <summary>
    /// The tail controller
    /// </summary>
    private Tail tail_;

    /// <summary>
    /// Add some points to the score
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {

        Score += score;

        NotifyScore(score);

        if (Network.isServer)
        {

            networkView.RPC("SetScore", RPCMode.Others, Score);

        }

    }

    [RPC]
    public void SetScore(int score)
    {

        int delta = score - Score;

        Score = score;

        NotifyScore(delta);

        if (Network.isServer)
        {

            networkView.RPC("SetScore", RPCMode.Others, Score);

        }

    }

    public void ResetScore()
    {

        var delta = Score;

        Score = 0;

        NotifyScore(-delta);

    }

    [RPC]
    public void RPCSetGameId(int id)
    {

        Id = id;

        NotifyIdSet(id);

    }

}
