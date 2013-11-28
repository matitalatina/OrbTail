using UnityEngine;
using System;
using System.Collections.Generic;


public class FixedPowerContainer : MonoBehaviour
{
    public FixedPowerContainer()
    { 
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObj = collision.gameObject;

        if (collidedObj.tag == Tags.Ship)
        {

            // TODO; Do some stuff

        }  
    }

    void Update()
    {
    }
}
