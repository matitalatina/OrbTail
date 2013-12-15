using System;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Power
{
    private const float power_time = 10.0f;

    public Invincibility() : base(PowerGroups.Main, power_time, "Invincibility") { }

    private Deactivator deactivator;

    protected override void ActivateServer()
    {

        var tail_stack = Owner.GetComponent<TailController>().GetDetacherDriverStack();

        deactivator = tail_stack.Push( new InvincibleDetacherDriver());
    }

    public override void Deactivate()
    {

        base.Deactivate();

        if (deactivator != null)
        { 
            deactivator.Deactivate();
        }

    }
    
    protected override float IsReady { get { return 0.0f; } }

    public override Power Generate()
    {

        return new Invincibility();

    }

}
