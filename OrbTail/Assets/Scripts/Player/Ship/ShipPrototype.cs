using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake(){

        MovementController movement_controller = GetComponent<MovementController>();

        movement_controller.GetEngineDriverStack().Push( new DefaultEngineDriver(speed) );
        movement_controller.GetWheelDriverStack().Push( new DefaultWheelDriver(steering) );

        TailController tail_controller = GetComponent<TailController>();

        tail_controller.GetOffenceDriverStack().Push( new DefaultOffenceDriver(offence) );
        tail_controller.GetDefenceDriverStack().Push( new DefaultDefenceDriver(defence) );
        tail_controller.GetAttacherDriverStack().Push( new DefaultAttacherDriver() );
    
        
    
    }

}
