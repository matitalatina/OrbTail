using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Boost : Power
{
    private bool ready = true;

    public Boost(IGroup group, float duration)
        : base(group, duration)
    {

    }
    
    protected override float IsReady
    {
        get
        {
            return Mathf.Clamp01(activatedTime / duration);
        }
    }
    
    public override void Update()
    {
        // If it's active or on recharge
        if (!ready)
        {
            float deltaActivatedTime = Time.time - activatedTime;
            duration -= deltaActivatedTime;

            if (duration <= 0f)
            {
                ready = false;
                //Deactivate();
            }
        }
    }

    public override void Fire()
    {
        ready = false;
    }

    protected override GameObject GetShip()
    {
        return shipOwner;
    }
}