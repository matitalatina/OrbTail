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
    protected float duration { get; set; }
    protected GameObject shipOwner { get; set; }
    protected float activatedTime { get; private set; }
    
    protected Power(IGroup group, float duration)
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

    protected void Activate(GameObject gameObj)
    {
        this.shipOwner = gameObj;
        this.activatedTime = Time.time;
    }

    public abstract void Update();
    public abstract void Fire();
    protected abstract GameObject GetShip();
}