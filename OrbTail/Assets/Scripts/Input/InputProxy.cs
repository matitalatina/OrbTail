using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to exchange and read input data over the network
/// </summary>
public class InputProxy : MonoBehaviour, IInputBroker{

    // Use this for initialization
	public void Start () {

        if (Network.isClient)
        {

            if(SystemInfo.supportsAccelerometer)
            {

                //Mobile platform
                input_broker_ = new MobileInputBroker();

            }
            else
            {
                
                //Desktop platform
                input_broker_ = new DesktopInputBroker();

            }

        }
        else
        {

            // The server has no broker as it just reads data
            input_broker_ = null;

        }

	}
	
	// Update is called once per frame
	public void Update () {

        if (input_broker_ != null)
        {

            input_broker_.Update();

        }

	}

    /// <summary>
    /// Called when the script needs to be serialized
    /// </summary>
    public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {

        //Stream: | Acceleration | Steering | #Powers | Group(1) | Group(2) | ...

        if (stream.isWriting)
        {
            
            if (Network.isClient)
            {

                // Only the client can serialize stuffs
                Serialize(stream);

            }
            
        }
        else
        {
            
            if (Network.isServer)
            {

                // Only the server can deserialize stuffs
                Deserialize(stream);

            }

        }

    }

    /// <summary>
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    public float Acceleration { get; private set; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    public float Steering { get; private set; }

    /// <summary>
    /// Returns a collection which indicates all the power ups the user wants to fire. The elements indicates just the group of the proper power
    /// </summary>
    public IEnumerable<IGroup> FiredPowerUps
    {

        get
        {
            return new List<IGroup>(fired_powers);
        }

    }

    /// <summary>
    /// Serializes the proxy over the net
    /// </summary>
    /// <param name="stream">The stream to write into</param>
    private void Serialize(BitStream stream){

        float acceleration = Acceleration;
        float steering = Steering;
        float powers_count = fired_powers.Count;

        stream.Serialize(ref acceleration);
        stream.Serialize(ref steering);
        stream.Serialize(ref powers_count);

        foreach (IGroup power_group in fired_powers)
        {

            power_group.Serialize(stream);

        }

    }

    /// <summary>
    /// Deserializes the proxy from the net
    /// </summary>
    /// <param name="stream">The stream to read from</param>
    private void Deserialize(BitStream stream){

        float acceleration = 0.0f;
        float steering = 0.0f;
        float powers_count = 0.0f;
        IGroup group;

        stream.Serialize(ref acceleration);
        stream.Serialize(ref steering);
        stream.Serialize(ref powers_count);

        Acceleration = acceleration;
        Steering = steering;
        fired_powers = new List<IGroup>();

        for (; powers_count > 0; powers_count--)
        {

            group = new GroupID();
            group.Deserialize(stream);
            fired_powers.Add(group);

        }

    }

    /// <summary>
    /// The input broker used to read user's input or to exechange data
    /// </summary>
    private IInputBroker input_broker_;

    /// <summary>
    /// The list of the powerups to be fired
    /// </summary>
    private IList<IGroup> fired_powers;

}
