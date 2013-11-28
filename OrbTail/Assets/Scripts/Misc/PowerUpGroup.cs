using System;
using System.Collections.Generic;

public class PowerUpGroup
{
    private static PowerUpGroup instance;

    private PowerUpGroup() { }

    public static PowerUpGroup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerUpGroup();
            }
            return instance;
        }
    }

    // TODO: Needs Danny check....
    /*public GroupID getPowerUpGroup(){
        return new GroupID();
    }*/
}
