using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Boost : Power
{
    private bool ready = true;

    public Boost()
        : base(SpecialPowerGroup.Instance.groupID, float.MaxValue)
    {

    }
    
    protected override float IsReady
    {
        get
        {
            return 0.0f; //TODO; this is fucking retared
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

}