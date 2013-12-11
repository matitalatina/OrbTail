using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Power : PowerView
{
    protected IGroup group { get; private set; }
    protected float? duration { get; set; }
    protected GameObject shipOwner { get; private set; }
    protected float activatedTime { get; private set; }

    protected float time_accumulator = 0.0f;

    protected Power(IGroup group, float? duration, string name)
    {

        this.group = group;
        this.duration = duration;
        this.Name = name;

    }

    /// <summary>
    /// Get the power's group
    /// </summary>
    public override IGroup Group
    {
        get
        {
            return group;
        }
    }

    /// <summary>
    /// The power name
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
    /// Clone this power
    /// </summary>
    public abstract Power Generate();

    /// <summary>
    /// Activate the power up
    /// </summary>
    /// <param name="gameObj">Ship with activated power up</param>
    public virtual void Activate(GameObject gameObj)
    {
        this.shipOwner = gameObj;
        this.activatedTime = Time.time;
        this.time_accumulator = 0.0f;
    }

    /// <summary>
    /// Deactivate power up
    /// </summary>
    public virtual void Deactivate()
    {

        Destroy(group);

    }

    /// <summary>
    /// Counter to deactivate the active power up
    /// </summary>
    public virtual void Update()
    {

        time_accumulator += Time.deltaTime;

        // If power up time is expired, deactivate power up
        if (time_accumulator > (duration ?? float.MaxValue))
        {
            
            time_accumulator = 0.0f;
            duration = null;

            Deactivate();

        }

    }

    /// <summary>
    /// Fire avaiable power up
    /// </summary>
    public virtual void Fire() { }

}