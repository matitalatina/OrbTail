using System;
using System.Collections.Generic;

public class MainPowerGroup
{
    private static MainPowerGroup instance;
    private GroupID groupID;

    private MainPowerGroup()
    {
        groupID = new GroupID();
    }

    public static MainPowerGroup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MainPowerGroup();
            }
            return instance;
        }
    }

    /// <summary>
    /// Return PowerUp group ID
    /// </summary>
    /// <returns>Return PowerUp group ID</returns>
    public GroupID getGroupID(){
        return groupID;
    }
}