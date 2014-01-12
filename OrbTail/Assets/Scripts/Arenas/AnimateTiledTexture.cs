using UnityEngine;
using System.Collections;

class AnimateTiledTexture : MonoBehaviour
{

    public Color FlashColor = Color.white;

    public Color CriticalColor = Color.red;

    public float FlashDecay = 1.0f;

    public Color RestColor = Color.black;

    public Color CriticalRestColor = Color.grey;

    private Color material_color;

    private Game game;

    void Start()
    {

        game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
            
        game.EventStart += AnimateTiledTexture_EventStart;
        
        material_color = RestColor;

        renderer.material.color = material_color;

    }

    void AnimateTiledTexture_EventStart(object sender, int countdown)
    {

        var player = game.ActivePlayer;

        player.GetComponent<TailController>().OnEventFight += AnimateTiledTexture_OnEventFight;
        player.GetComponent<Tail>().OnEventOrbAttached += AnimateTiledTexture_OnEventOrbAttached;

        game.EventTick += game_EventTick;
    }

    void game_EventTick(object sender, int time_left)
    {

        if (time_left <= 10)
        {

            FlashColor = CriticalColor;
            RestColor = CriticalRestColor;

            StopAllCoroutines();

            StartCoroutine(Flash(FlashColor, RestColor, FlashDecay));

        }

    }

    void AnimateTiledTexture_OnEventOrbAttached(object sender, GameObject orb, GameObject ship)
    {

        StopAllCoroutines();

        StartCoroutine(Flash(FlashColor, RestColor, FlashDecay));

    }

    void AnimateTiledTexture_OnEventFight(object sender, System.Collections.Generic.IList<GameObject> orbs, GameObject attacker, GameObject defender)
    {
        
        StopAllCoroutines();

        StartCoroutine(Flash(FlashColor, RestColor, FlashDecay));

    }

    private IEnumerator Flash(Color flash, Color rest, float decay)
    {

        var mat = renderer.material;

        material_color = flash;

        while (material_color != rest)
        {

            material_color = Color.Lerp(material_color, rest, Time.deltaTime * decay);

            Debug.Log(Time.deltaTime * decay);

            renderer.material.color = material_color;

            yield return new WaitForEndOfFrame();

        }
                
    }

}
