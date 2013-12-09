using UnityEngine;
using System;
using System.Collections.Generic;

static class PowerFactory
{
    public static Power GetPower()
    {
        System.Random rng = new System.Random();

        switch (rng.Next(0, 5))
        {
            case 1:
            case 3:
            case 0:{ 
                return new Missle();
            }
            /*case 1: { 
                return new Magnet();
            }*/
            case 2: { 
                return new Invincibility();
            }
            /*case 3: { 
                return new TailSwap();
            }*/
            case 4: { 
                return new OrbSteal();
            }
            case 5: { 
                return new Jam();
            }
            default: {
                System.Diagnostics.Debug.Assert(false);
                return null;
            }
        }
    }
}

public class RandomPowerAttacher : MonoBehaviour
{
    public RandomPowerAttacher()
    { 
    }

    void OnCollisionEnter(Collision collision)
    {
        var collidedObj = collision.gameObject;

        if (collidedObj.tag == Tags.Ship)
        {
            Power randomPower = PowerFactory.GetPower();

            collidedObj.GetComponent<PowerController>().AddPower(randomPower);

            //Debug.Log("Ship captured a random power up!");

            randomPower.Activate(collidedObj);

            Debug.Log("Power activated on ship.");

            Destroy(this);
        }  
    }

    void Start()
    {
        // TODO: Special effects
        //Debug.Log("TODO: GFX Special effects for power up");

    }

    void Update()
    {
    }
}
