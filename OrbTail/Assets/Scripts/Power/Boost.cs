using System;
using System.Collections.Generic;
using UnityEngine;

public class Boost : Power
{
    private const float reload_power_time = 5.0f;
    private float time_accumulator_to_reload = reload_power_time;
	private float boost_force = 60.0f;
	private AudioClip launchSound;
	private GameObject activePlayer;

    private Deactivator deactivator;

    public Boost() : base(PowerGroups.Passive, float.MaxValue, "Boost") {
		launchSound = Resources.Load<AudioClip>("Sounds/Powers/Boost");
		activePlayer = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>().ActivePlayer;
	}
    
    public override float IsReady
    {
        get
        {
            return time_accumulator_to_reload / reload_power_time;
        }
    }
    
    public override void Update()
    {
        base.Update();

        time_accumulator_to_reload = Mathf.Clamp(time_accumulator_to_reload + Time.deltaTime, 0.0f, reload_power_time);

    }

    /// <summary>
    /// Activate boost on ship if it's available
    /// </summary>
    public override bool Fire()
    {
        if (IsReady >= 1.0f)
        {
            time_accumulator_to_reload = 0.0f;

            Owner.GetComponent<Rigidbody>().AddForce(Owner.transform.forward * boost_force, ForceMode.Impulse);

			if (Owner == activePlayer) {
				AudioSource.PlayClipAtPoint(launchSound, Owner.gameObject.transform.position, 0.2f);


				#if UNITY_ANDROID || UNITY_IPHONE
					Handheld.Vibrate();
				#endif

			}

            return true;
        }
        else
        {

            return false;

        }

    }

    public override Power Generate()
    {
        
        return new Boost();

    }

}