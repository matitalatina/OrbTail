using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class AnimateTiledTexture : MonoBehaviour
{

    public int ColorizeInterval = 10;

    public float ColorizeDecay = 1.0f;

    public Color CriticalColor = Color.red;

    public Color CriticalRestColor = Color.grey;

    public Queue<Color> Colors = new Queue<Color>();

    private Color flash_color;

    private Color rest_color;

    private Game game;
        
    void Start()
    {

        var builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();

        builder.EventGameBuilt += builder_EventGameBuilt;
        
    }

    void builder_EventGameBuilt(object sender)
    {

        //Fills the color stack
        Colors.Enqueue(Color.red);
        Colors.Enqueue(Color.yellow);
        Colors.Enqueue(Color.green);
        Colors.Enqueue(Color.cyan);
        Colors.Enqueue(Color.blue);
        Colors.Enqueue(Color.magenta);

        renderer.material.color = Colors.Peek();

        game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();

        game.EventStart += AnimateTiledTexture_EventStart;
        
    }

    void AnimateTiledTexture_EventStart(object sender, int countdown)
    {

        var player = game.ActivePlayer;

        player.GetComponent<Tail>().OnEventOrbAttached += AnimateTiledTexture_OnEventOrbAttached;

        game.EventTick += game_EventTick;

        StartCoroutine(Colorize());

    }

    void AnimateTiledTexture_OnEventOrbAttached(object sender, GameObject orb, GameObject ship)
    {

        Flash(flash_color);

    }

    void game_EventTick(object sender, int time_left)
    {

        if (time_left <= 10)
        {

            flash_color = CriticalColor;
            rest_color = CriticalRestColor;

            Flash(CriticalColor);

        }
        else if (time_left % ColorizeInterval == 0)
        {

            flash_color = Colors.Peek();
            rest_color = Colors.Peek() * 0.2f;
            rest_color.a = 1.0f;

            Colors.Enqueue(Colors.Dequeue());

        }

    }


    private void Flash(Color flash)
    {

        renderer.material.color = flash;

    }

    private IEnumerator Colorize()
    {

        var mat = renderer.material;

        while (true)
        {

            mat.color = Color.Lerp(mat.color, rest_color, Time.deltaTime * ColorizeDecay);
            
            yield return new WaitForEndOfFrame();

        }
                
    }

}
