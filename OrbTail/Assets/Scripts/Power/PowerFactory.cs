using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PowerFactory
{
        
    private static PowerFactory instance_ = null;

    private PowerFactory()
    {

        RegisterPower(new Boost(), 0);              //The boost is not a generated power
        RegisterPower(new Missile(), 1);
        RegisterPower(new Invincibility(), 1);
        RegisterPower(new OrbSteal(), 1);
        RegisterPower(new Magnet(), 1);
		RegisterPower(new Jam(), 1);

        PreloadPowers();

    }

    private void PreloadPowers()
    {

        //Preloads all the power's FXs
        GameObjectFactory.Instance.Preload(Power.power_prefab_path + "Boost", 4);
        GameObjectFactory.Instance.Preload(Power.power_prefab_path + "Missile", 4);
        GameObjectFactory.Instance.Preload(Power.power_prefab_path + "Invincibility", 4);
        GameObjectFactory.Instance.Preload(Power.power_prefab_path + "OrbSteal", 4);
        GameObjectFactory.Instance.Preload(Power.power_prefab_path + "Magnet", 4);
        GameObjectFactory.Instance.Preload(MissileBehavior.explosion_prefab_path, 4);

    }

    public static PowerFactory Instance{
    
        get{

            if (instance_ == null)
            {

                instance_ = new PowerFactory();

            }

            return instance_;

        }
        
    }

    /// <summary>
    /// Returns a random power
    /// </summary>
    public Power RandomPower
    {

        get{

            int value = random.Next(total_frequency);

            return power_table_.Values.SkipWhile((KeyValuePair<Power, int> p) =>
            {

                value -= p.Value;
                return value >= 0;

            }).First().Key.Generate();

        }
       
    }

    public Power PowerFromName(string name)
    {

        return power_table_[name].Key.Generate();

    }

    public int GroupFromName(string name)
    {

        return power_table_[name].Key.Group;

    }

    /// <summary>
    /// Register a new power
    /// </summary>
    private void RegisterPower(Power power, int frequency)
    {

        power_table_.Add(power.Name, new KeyValuePair<Power, int>(power, frequency));
        total_frequency += frequency;

    }

    /// <summary>
    /// Random number generator
    /// </summary>
    private Random random = new System.Random();
    
    /// <summary>
    /// The table containing all the powers (pair powers-relative frequency)
    /// </summary>
    private static IDictionary<string, KeyValuePair<Power, int>> power_table_ = new Dictionary<string, KeyValuePair<Power, int>>();

    private int total_frequency = 0;

}