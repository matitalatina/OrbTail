﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Power
{
    private const float power_time = 10.0f;

    public Invincibility() : base(MainPowerGroup.Instance.groupID, power_time, "Invincibility") { }

    private Deactivator deactivator;

    public override void Activate(UnityEngine.GameObject gameObj)
    {

        Debug.Log("Ship: "+ gameObj +" Invincible!");

        base.Activate(gameObj);

        var tail_stack = gameObj.GetComponent<TailController>().GetDetacherDriverStack();

        deactivator = tail_stack.Push( new InvincibleDetacherDriver());
        
    }

    public override void Deactivate()
    {

        Debug.Log("Ship no more Invincible!");

        base.Deactivate();

        deactivator.Deactivate();

    }
    
    protected override float IsReady { get { return 0.0f; } }

    public override Power Generate()
    {

        return new Invincibility();

    }

}
