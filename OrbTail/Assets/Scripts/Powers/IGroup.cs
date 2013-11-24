using UnityEngine;
using System.Collections;

public interface IGroup {

    /// <summary>
    /// Returns the group's hashcode
    /// </summary>
    /// <returns>Returns the group's hashcode</returns>
    int GetHashCode();

    /// <summary>
    /// Returns true if this object is equal to 'other', false otherwise
    /// </summary>
    /// <param name="other">The other object to test against</param>
    /// <returns>Returns true if this object is equal to 'other', false otherwise</returns>
    bool Equals(object other);

    /// <summary>
    /// Serializes the group onto a stream
    /// </summary>
    /// <param name="stream">The stream to serialize to</param>
    void Serialize(BitStream stream);

    /// <summary>
    /// Deserialize a group from a stream
    /// </summary>
    /// <param name="stream">The stream to serialize from</param>
    void Deserialize(BitStream stream);

}
