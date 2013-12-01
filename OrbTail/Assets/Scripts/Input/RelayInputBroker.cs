using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Relays the input
/// </summary>
class RelayInputBroker: IInputBroker
{

    public RelayInputBroker()
    {

        fired_powers_ = new List<IGroup>();
        acceleration_ = 0;
        steering_ = 0;

    }

    public float Acceleration
    {

        get
        {
            return acceleration_;
        }

        set
        {
            acceleration_ = Mathf.Clamp(value, -1.0f, 1.0f);
        }

    }

    public float Steering
    {

        get
        {
            return steering_;
        }

        set
        {
            steering_ = Mathf.Clamp(value, -1.0f, 1.0f);
        }

    }

    public ICollection<IGroup> FiredPowerUps
    {
        get
        {

            return fired_powers_;

        }

    }

    public void Update() {}

    /// <summary>
    /// The acceleration
    /// </summary>
    private float acceleration_;

    /// <summary>
    /// The steering
    /// </summary>
    private float steering_;

    private IList<IGroup> fired_powers_;

}