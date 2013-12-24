using System;
using System.Collections.Generic;
using UnityEngine;

public class OrbSteal : Power
{
    private const float power_time = 10.0f;
    private List<TailController> tailControllers;

	public OrbSteal() : base(PowerGroups.Main, power_time, "OrbSteal") { }

    protected override void ActivateServer()
    {
		tailControllers = new List<TailController>();

		foreach (GameObject ship in GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>().ShipsInGame) {
			TailController tailController = ship.GetComponent<TailController>();
			tailController.OnEventFight += eventLogger_EventFight;
			tailControllers.Add(tailController);
		}

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

		if (NetworkHelper.IsServerSide()) {

			foreach (TailController tailController in tailControllers) {
				tailController.OnEventFight -= eventLogger_EventFight;
			}

		}

        base.Deactivate();

    }

    public override float IsReady { get { return 1.0f; } }

    public override Power Generate()
    {

        return new OrbSteal();

    }

}
