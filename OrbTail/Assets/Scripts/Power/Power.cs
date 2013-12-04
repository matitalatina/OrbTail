using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Power : PowerView
{
    /// <summary>
    /// Fired when the power has been destroyed and cannot be used anymore
    /// </summary>
    public event EventHandler<EventArgs> EventDestroyed;
    
    protected IGroup group { get; private set; }
    protected float? duration { get; set; }
    protected GameObject shipOwner { get; private set; }
    protected float activatedTime { get; private set; }

    private float time_accumulator = 0.0f;

    protected Power(IGroup group, float? duration)
    {
        this.group = group;
        this.duration = duration;
    }

    /// <summary>
    /// Get the group of this power
    /// </summary>
    public override IGroup Group
    {
        get
        {
            return group;
        }
    }

    public virtual void Activate(GameObject gameObj)
    {
        this.shipOwner = gameObj;
        this.activatedTime = Time.time;
        this.time_accumulator = 0.0f;
    }


    public virtual void Deactivate()
    {

        Destroy();

    }

    public virtual void Update()
    {

        time_accumulator += Time.deltaTime;

        Debug.Log(time_accumulator);

        if (time_accumulator > (duration ?? float.MaxValue))
        {
            
            time_accumulator = 0.0f;
            duration = null;

            Deactivate();

        }

    }


    public virtual void Fire() { }

}