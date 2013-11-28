using System;
using System.Collections.Generic;

public class SpecialPowerGroup
{
    private static SpecialPowerGroup instance;
    private GroupID groupID;

    private SpecialPowerGroup()
    {
        groupID = new GroupID();
    }

    public static SpecialPowerGroup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SpecialPowerGroup();
            }
            return instance;
        }
    }

    /// <summary>
    /// Return PowerUp group ID
    /// </summary>
    /// <returns>Return PowerUp group ID</returns>
    public GroupID getGroupID()
    {
        return groupID;
    }
}