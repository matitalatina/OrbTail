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
        GameObject collidedObj = collision.gameObject;

        if (collidedObj.tag == Tags.Ship)
        {
            
            Power randomPower = new Jam(); //TODO random power up gen. Factory?
            Debug.Log("Random ass power");

            collidedObj.GetComponent<PowerController>().AddPower(randomPower);

            ///Activate the power
            randomPower.Activate(collidedObj);

            Destroy(this);
        }  
    }

    void Start()
    {
        // TODO: Special effects
        Debug.Log("Special effects");

    }

    void Update()
    {
    }
}
