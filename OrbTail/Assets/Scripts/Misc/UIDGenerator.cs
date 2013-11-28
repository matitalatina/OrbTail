using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UIDGenerator
{
    private static UIDGenerator instance;

    private UIDGenerator() { }

    public static UIDGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIDGenerator();
            }
            return instance;
        }
    }

    private int uid = 0;

    /// <summary>
    /// Return a new univocal group id
    /// </summary>
    /// <returns>Return a new univocal group id</returns>
    public int GetNewUID(){
        return uid++;
    }
}