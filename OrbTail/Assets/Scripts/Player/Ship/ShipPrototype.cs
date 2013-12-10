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

        var boost = new Boost();

        boost.Activate(gameObject);

        var power_controller = gameObject.GetComponent<PowerController>();

        power_controller.AddPower(boost);

	}

    // Use this for initialization
    void Start()
    {

        //Everyone have a tail
        Tail tail = gameObject.AddComponent<Tail>();

        //Server side controls the collisions
        if (Network.peerType == NetworkPeerType.Disconnected ||
            Network.peerType == NetworkPeerType.Server)
        {

            TailController tail_controller = gameObject.AddComponent<TailController>();

            tail_controller.GetOffenceDriverStack().Push(new DefaultOffenceDriver(offence));
            tail_controller.GetDefenceDriverStack().Push(new DefaultDefenceDriver(defence));
            tail_controller.GetAttacherDriverStack().Push(new DefaultAttacherDriver());
            tail_controller.GetDetacherDriverStack().Push(new DefaultDetacherDriver());

        }

        //Client side controls the movement
        if (Network.peerType == NetworkPeerType.Disconnected ||
            networkView.isMine)
        {

            MovementController movement_controller = gameObject.AddComponent<MovementController>();

            movement_controller.GetEngineDriverStack().Push(new DefaultEngineDriver(speed));
            movement_controller.GetWheelDriverStack().Push(new DefaultWheelDriver(steering));

        }

        PowerController power_controller = gameObject.AddComponent<PowerController>();

    }

}
