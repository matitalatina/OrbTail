using UnityEngine;
using System.Collections;

/// <summary>
/// Push some default drivers to the ship and destroy shortly after
/// </summary>
public class ShipPrototype : MonoBehaviour {

    /// <summary>
    /// The defence power of the ship (in the range 1..5)
    /// </summary>
    public int defence;

    /// <summary>
    /// The offence power of the ship (in the range 1..5)
    /// </summary>
    public int offence;

    /// <summary>
    /// The steering of the ship (in the range 1..5)
    /// </summary>
    public int steering;

    /// <summary>
    /// The speed of the ship (in the range 1..5)
    /// </summary>
    public int speed;


	// Update is called once per frame
	void Update () {
	
        this.enabled = false;

	}

    void Awake()
    {

        //Everyone have a tail
        gameObject.AddComponent<Tail>();
        PowerController powerController = gameObject.AddComponent<PowerController>();

		if (NetworkHelper.IsServerSide())
		{
			
			//Adds a boost to the ship, only it this is the server
			powerController.AddPower(new Boost());
			
		}

        //Server side controls the collisions
        if (NetworkHelper.IsServerSide())
        {

            TailController tail_controller = gameObject.AddComponent<TailController>();

            tail_controller.GetOffenceDriverStack().Push(new DefaultOffenceDriver(offence));
            tail_controller.GetDefenceDriverStack().Push(new DefaultDefenceDriver(defence));
            tail_controller.GetAttacherDriverStack().Push(new DefaultAttacherDriver());
            tail_controller.GetDetacherDriverStack().Push(new DefaultDetacherDriver());

        }

        //Client side controls the movement
        if (NetworkHelper.IsOwnerSide(networkView))
        {

            MovementController movement_controller = gameObject.AddComponent<MovementController>();

            movement_controller.enabled = false;

            movement_controller.GetEngineDriverStack().Push(new DefaultEngineDriver(speed));
            movement_controller.GetWheelDriverStack().Push(new DefaultWheelDriver(steering));

			gameObject.AddComponent<CollisionSoundHandler>();

        }

    }

    // Use this for initialization
    void Start()
    {



    }

}
