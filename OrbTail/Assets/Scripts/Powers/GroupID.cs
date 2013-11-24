using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GroupID: IGroup
{

    /// <summary>
    /// Returns the group's hashcode
    /// </summary>
    /// <returns>Returns the group's hashcode</returns>
    public override int GetHashCode()
    {

        //TODO: implement this
        throw new NotImplementedException();

    }

    /// <summary>
    /// Returns true if this object is equal to 'other', false otherwise
    /// </summary>
    /// <param name="other">The other object to test against</param>
    /// <returns>Returns true if this object is equal to 'other', false otherwise</returns>
    public override bool Equals(object other)
    {

        //TODO: implement this
        throw new NotImplementedException();

    }

    /// <summary>
    /// Serializes the group onto a stream
    /// </summary>
    /// <param name="stream">The stream to serialize to</param>
    public void Serialize(UnityEngine.BitStream stream)
    {

        //TODO: implement this
        throw new NotImplementedException();

    }

    /// <summary>
    /// Deserialize a group from a stream
    /// </summary>
    /// <param name="stream">The stream to serialize from</param>
    public void Deserialize(UnityEngine.BitStream stream)
    {

        //TODO: implement this
        throw new NotImplementedException();

    }

}

