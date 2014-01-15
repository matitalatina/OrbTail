using UnityEngine;
using System.Collections;

public class SmoothAnimation : MonoBehaviour {

    public float kAnimationSmooth = 1f;

    public Vector3 Scale;

    public Color Color;

	// Use this for initialization
	void Start () {

        StartCoroutine(UpdateAnimation());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator UpdateAnimation()
    {

        while (true)
        {

            gameObject.renderer.material.color = Color.Lerp(gameObject.renderer.material.color, Color, Time.deltaTime * kAnimationSmooth);
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Scale, Time.deltaTime * kAnimationSmooth);

            yield return new WaitForEndOfFrame();

        }

    }


    void OnDestroy()
    {

        StopAllCoroutines();

    }

}
