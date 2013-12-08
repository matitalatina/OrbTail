using UnityEngine;
using System;
using System.Collections.Generic;


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
            
            Power randomPower = new Boost(); //TODO random power up gen. Factory?

            Debug.Log("Ship captured a random power up!");

            collidedObj.GetComponent<PowerController>().AddPower(randomPower);

            randomPower.Activate(collidedObj);

            Debug.Log("Power activated on ship.");

            Destroy(this);
        }  
    }

    void Start()
    {
        // TODO: Special effects
        Debug.Log("TODO: GFX Special effects for power up");

    }

    void Update()
    {
    }
}
