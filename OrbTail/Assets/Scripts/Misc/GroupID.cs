using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GroupID: IGroup
{

    private int uid;
    public GroupID()
    {
        SetHashCode(UIDGenerator.Instance.GetNewUID());
    }

    /// <summary>
    /// Returns the group's hashcode
    /// </summary>
    /// <returns>Returns the group's hashcode</returns>
    public override int GetHashCode()
    {
        return uid;
    }

    /// <summary>
    /// Set the group's hashcode
    /// </summary>
    /// <param name="uid">Group's UID</param>
    private void SetHashCode(int uid)
    {
        this.uid = uid;
    }

    /// <summary>
    /// Returns true if this object is equal to 'other', false otherwise
    /// </summary>
    /// <param name="other">The other object to test against</param>
    /// <returns>Returns true if this object is equal to 'other', false otherwise</returns>
    public override bool Equals(object obj)
    {
        // Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            GroupID other = (GroupID)obj;
            return (GetHashCode() == other.GetHashCode());
        }
    }

    /// <summary>
    /// Serializes the group onto a stream
    /// </summary>
    /// <param name="stream">The stream to serialize to</param>
    public void Serialize(UnityEngine.BitStream stream)
    {
        stream.Serialize(ref uid);
    }

    /// <summary>
    /// Deserialize a group from a stream
    /// </summary>
    /// <param name="stream">The stream to serialize from</param>
    public void Deserialize(UnityEngine.BitStream stream)
    {
        SetHashCode(stream.GetHashCode());
    }

}

