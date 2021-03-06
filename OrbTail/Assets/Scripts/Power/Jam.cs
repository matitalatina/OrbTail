﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Jam : Power
{
    private const float power_time = 7.0f;

    public Jam() : base(PowerGroups.Jam, power_time, "Jam") { }

    private Deactivator deactivator;

    protected override void ActivateClient()
    {


        var wheel_stack = Owner.GetComponent<MovementController>().GetWheelDriverStack();

        deactivator = wheel_stack.Push( new JammedWheelDriver( wheel_stack.GetPrototype().GetSteering() ));
        
    }

    public override void Deactivate()
    {

        base.Deactivate();

        if (deactivator != null)
        {
            deactivator.Deactivate();
        }
    }
    
    public override float IsReady { get { return 0.0f; } }

    public override Power Generate()
    {

        return new Jam();

    }

}
