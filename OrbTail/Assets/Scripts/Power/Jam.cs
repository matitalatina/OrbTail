using System;
using System.Collections.Generic;
using UnityEngine;

public class Jam : Power
{
    private const float power_time = 10.0f;

    static IGroup jam_group = new GroupID();

    public Jam() : base(jam_group, power_time, "Jam") { }

    private Deactivator deactivator;

    public override void ActivateClient()
    {

        Debug.Log("Ship: " + Owner + " Jammed!");

        var wheel_stack = Owner.GetComponent<MovementController>().GetWheelDriverStack();

        deactivator = wheel_stack.Push( new JammedWheelDriver( wheel_stack.GetPrototype().GetSteering() ));
        
    }

    public override void Deactivate()
    {
        Debug.Log("Ship no more Jammed!");

        base.Deactivate();

        if (deactivator != null)
        {
            deactivator.Deactivate();
        }
    }
    
    protected override float IsReady { get { return 0.0f; } }

    public override Power Generate()
    {

        return new Jam();

    }

}
