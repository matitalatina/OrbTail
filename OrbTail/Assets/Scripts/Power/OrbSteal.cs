using System;
using System.Collections.Generic;
using UnityEngine;

public class OrbSteal : Power
{
    private const float power_time = 10.0f;
    private EventLogger eventLogger;

	public OrbSteal() : base(PowerGroups.Main, power_time, "OrbSteal") { }

    protected override void ActivateServer()
    {

        eventLogger = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();

        eventLogger.EventFight += eventLogger_EventFight;

    }

    void eventLogger_EventFight(object sender, IList<GameObject> orbs, GameObject attacker, GameObject defender)
    {
		if (attacker == Owner) {

			foreach (GameObject orb in orbs)
        	{
				if (!orb.GetComponent<OrbController>().IsAttached()) {
            		attacker.GetComponent<TailController>().GetAttacherDriverStack().GetHead().AttachOrbs(orb, attacker.GetComponent<Tail>());
				}
        	}

		}
    }

    public override void Deactivate()
    {
		eventLogger.EventFight -= eventLogger_EventFight;
        base.Deactivate();

    }

    protected override float IsReady { get { return 1.0f; } }

    public override Power Generate()
    {

        return new OrbSteal();

    }

}
