using System;
using System.Collections.Generic;

public class MainPowerGroup
{
    private static MainPowerGroup instance;
    public GroupID groupID { get; private set; }

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
}