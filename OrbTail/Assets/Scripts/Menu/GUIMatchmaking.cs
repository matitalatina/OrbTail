using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIMatchmaking : GUIMenuChoose {

    private GameObject ready_button;
    private GameObject not_ready_button;
    private NetworkPlayerBuilder network_builder;

    private IList<GameObject> player_icons;

    private const float kReadyScale = 1.33f;
    private const float kScalingTime = 0.5f;
    private Vector3 kLocalScale = 0.15f * Vector3.one;
    private static Color kDisabledColor = Color.grey;

	// Use this for initialization
	public override void Start ()
    {
		base.Start();

        //The icons (disabled by default)
        player_icons = (from icon in GameObject.FindGameObjectsWithTag(Tags.ShipSelector)
                        orderby icon.transform.position.x
                        select icon).ToList();
                               
        foreach (GameObject icon in player_icons)
        {

            icon.transform.localScale = Vector3.zero;

        }

        ready_button = GameObject.Find("ReadyButton");
        not_ready_button = GameObject.Find("NotReadyButton");

        ready_button.SetActive(false);
        not_ready_button.SetActive(false);

        //Listens to proper events

        network_builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>().NetworkBuilder;

        network_builder.EventIdAcquired += network_builder_EventIdAcquired;
        network_builder.EventPlayerRegistered += network_builder_EventPlayerRegistered;
        network_builder.EventPlayerUnregistered += network_builder_EventPlayerUnregistered;
        network_builder.EventPlayerReady += network_builder_EventPlayerReady;
        network_builder.EventNoGame += network_builder_EventNoGame;
        network_builder.EventErrorOccurred += network_builder_EventErrorOccurred;
        network_builder.EventDisconnected += network_builder_EventDisconnected;

        //Useful for the host only

        network_builder.MatchStartDelay = kScalingTime * 2.0f;

	}

    void network_builder_EventDisconnected(object sender, string message)
    {
        
    }

    void network_builder_EventErrorOccurred(object sender, string message)
    {
        
    }

    void network_builder_EventNoGame(object sender)
    {
        
    }

    void network_builder_EventPlayerReady(object sender, int id, bool value)
    {

        //Toggle the ready button
        if (network_builder.Id.HasValue &&
            id == network_builder.Id.Value )
        {
            
            ready_button.SetActive( !value );
            not_ready_button.SetActive( value );
            
        }

        //Update the interface showing ready players
        var icon = player_icons[id];

        icon.GetComponent<SmoothAnimation>().Color = value ? Color.white : kDisabledColor;
        icon.GetComponent<SmoothAnimation>().Scale = value ? kLocalScale * kReadyScale : kLocalScale;

    }

    void network_builder_EventPlayerUnregistered(object sender, int id)
    {

        var icon = player_icons[id];

        icon.GetComponent<SmoothAnimation>().Color = kDisabledColor;
        icon.GetComponent<SmoothAnimation>().Scale = Vector3.zero;

    }

    void network_builder_EventPlayerRegistered(object sender, int id, string name)
    {

        var icon = player_icons[id];

        var icon_resource = Resources.Load<Sprite>("ShipIcons/" + name);

        icon.GetComponent<SpriteRenderer>().sprite = icon_resource;

        icon.renderer.material.color = kDisabledColor;
        icon.transform.localScale = Vector3.zero;

        icon.GetComponent<SmoothAnimation>().Color = kDisabledColor;
        icon.GetComponent<SmoothAnimation>().Scale = kLocalScale;

    }

    void network_builder_EventIdAcquired(object sender, int id)
    {

        ready_button.SetActive(true);
        not_ready_button.SetActive(false);

    }


    void OnDestroy()
    {
        
        network_builder.EventIdAcquired -= network_builder_EventIdAcquired;
        network_builder.EventPlayerRegistered -= network_builder_EventPlayerRegistered;
        network_builder.EventPlayerUnregistered -= network_builder_EventPlayerUnregistered;
        network_builder.EventPlayerReady -= network_builder_EventPlayerReady;
        network_builder.EventNoGame -= network_builder_EventNoGame;
        network_builder.EventErrorOccurred -= network_builder_EventErrorOccurred;
        network_builder.EventDisconnected -= network_builder_EventDisconnected;
        
    }


	protected override void OnSelect (GameObject target)
	{
		base.OnSelect(target);

		if (target == ready_button)
		{
			
			network_builder.SetReady(true);
			
		}
		
		if (target == not_ready_button)
		{
			
			network_builder.SetReady(false);
			
		}

		if (target.tag == Tags.BackButton) {

            GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>().Restore();

			Application.LoadLevel("MenuChooseShip");

		}
	}


}
