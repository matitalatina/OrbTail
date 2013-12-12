using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ComponentHelper
{

    /// <summary>
    /// Returns the default RPC interface of a game object
    /// </summary>
    public static NetworkView GetRPCNetworkView(GameObject game_object){

        return game_object.GetComponents<NetworkView>().Where((NetworkView nv) => { return nv.observed = null; }).First();

    }

    /// <summary>
    /// Get a networkview watching a specific component type
    /// </summary>
    public static NetworkView GetNetworkView<T>(GameObject game_object) where T : Component
    {

        return game_object.GetComponents<NetworkView>().Where((NetworkView nv) => { return nv.observed is T; }).First();

    }

}
