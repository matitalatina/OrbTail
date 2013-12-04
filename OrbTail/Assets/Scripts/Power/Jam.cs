using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Jam: Power
{

    static IGroup jam_group = new GroupID();

    public Jam() : base(jam_group, 10.0f) { }

    private Deactivator deactivator;

    public override void Activate(UnityEngine.GameObject gameObj)
    {

        Debug.Log("Jammed!");

        base.Activate(gameObj);

        var wheel_stack = gameObj.GetComponent<MovementController>().GetWheelDriverStack();

        deactivator = wheel_stack.Push( new JammedWheelDriver( wheel_stack.GetPrototype().GetSteering() ));
        
    }

    public override void Deactivate()
    {

        Debug.Log("No more jammed!");

        base.Deactivate();

        deactivator.Deactivate();

    }
    
    protected override float IsReady { get { return 0.0f; } }
}
