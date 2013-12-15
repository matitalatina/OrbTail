using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Utility class for networking
/// </summary>
public class NetworkHelper
{

    /// <summary>
    /// Returns true if the network view is mine or the device is disconnected
    /// </summary>
    public static bool IsOwnerSide(NetworkView network_view)
    {

        return network_view.isMine ||
               Network.peerType == NetworkPeerType.Disconnected;

    }

    /// <summary>
    /// Returns true if this is the server or the device is disconnected
    /// </summary>
    /// <returns></returns>
    public static bool IsServerSide()
    {

        return Network.isServer ||
               Network.peerType == NetworkPeerType.Disconnected;

    }

}

