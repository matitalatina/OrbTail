using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrbSteal : Power
{
    private const float power_time = 10.0f;
    private EventLogger eventLogger;

    public OrbSteal() : base(PowerGroups.Main, float.MaxValue, "OrbSteal") { }

    protected override void ActivateServer()
    {

        eventLogger = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();

        eventLogger.EventFight += eventLogger_EventFight;
    }

    void eventLogger_EventFight(object sender, IList<GameObject> orbs, GameObject attacker, GameObject defender)
    {
		foreach (GameObject orb in orbs.Where((GameObject go) => { return !go.GetComponent<OrbController>().IsAttached();}))
        {
            attacker.GetComponent<TailController>().GetAttacherDriverStack().GetHead().AttachOrbs(orb, attacker.GetComponent<Tail>());
        }
    }

    public override void Deactivate()
    {

        base.Deactivate();

    }

    protected override float IsReady { get { return 1.0f; } }

    public override Power Generate()
    {

        return new OrbSteal();

    }

}
