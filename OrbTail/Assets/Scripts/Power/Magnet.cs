using UnityEngine;
using System.Collections;

public class Magnet : Power {
	private const float newProximityRadius = 10f;
	private const float powerTime = 10f;
	private float oldProximityRadius;
	private ProximityHandler proximityHandler;

	public Magnet() : base(PowerGroups.Main, powerTime, "Magnet") { }


	protected override void ActivateServer()
	{
		proximityHandler = Owner.GetComponentInChildren<ProximityHandler>();
		oldProximityRadius = proximityHandler.Radius;
		proximityHandler.Radius = newProximityRadius;
	}


	public override void Deactivate()
	{
		proximityHandler.Radius = oldProximityRadius;
		base.Deactivate();
		
	}
	
	protected override float IsReady { get { return 1.0f; } }
	
	public override Power Generate()
	{
		
		return new Magnet();
		
	}
}
