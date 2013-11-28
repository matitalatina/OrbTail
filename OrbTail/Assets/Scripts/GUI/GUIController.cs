using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
     
    /// <summary>
    /// Is this controller relaying commands over the net?
    /// </summary>
    public bool remote_write = false;

    /// <summary>
    /// Add a new power to the GUI
    /// </summary>
    /// <param name="power_view">The power to add</param>
    [RPC]
    public void AddPower(IPowerView power_view)
    {

        if (GUIBroker == null)
        {

            //Remote call!
            NetworkInterface.RPC("AddPower", RPCMode.Others, power_view);

        }
        else
        {

            //Local call!
            GUIBroker.AddPower(power_view);
            
        }
                
    }

    /// <summary>
    /// Removes a power from the GUI
    /// </summary>
    /// <param name="power_view">The power view to remove</param>
    private void RemovePower(IPowerView power_view)
    {

        //if( GUIBroker ==)

    }

	// Use this for initialization
	void Start () {

        if (remote_write)
        {

            //This component just needs to relay the commands over the net
            GUIBroker = null;

        }
        else
        {

            //Do stuffs in local

            if (SystemInfo.supportsAccelerometer)
            {

                //Mobile platform
                GUIBroker = new MobileGUIBroker();

            }
            else
            {

                //Desktop platform
                GUIBroker = new DesktopGUIBroker();
            }

        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// The GUI broker used by the controller
    /// </summary>
    private IGUIBroker GUIBroker { get; set; }
    

    /// <summary>
    /// The network interface used to broadcast the RPC or used to receive them
    /// </summary>
    private NetworkView NetworkInterface { get; set; }

}
