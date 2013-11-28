using System;
using System.Collections.Generic;

public class SpecialPowerGroup
{
    private static SpecialPowerGroup instance;
    public GroupID groupID { get; private set; }

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
}