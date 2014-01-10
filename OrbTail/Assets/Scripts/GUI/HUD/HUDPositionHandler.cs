using UnityEngine;
using System.Collections;

public class HUDPositionHandler : MonoBehaviour {

	private Transform cameraPos;
	private const float smoothMovement = 30f;
	private const float smoothRotation = 30f;
	private const float distanceFromCamera = 10f;

	// Use this for initialization
	void Start () {
		cameraPos = GameObject.FindGameObjectWithTag(Tags.MainCamera).transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, cameraPos.position + cameraPos.forward * distanceFromCamera, Time.deltaTime * smoothMovement);
		transform.rotation = Quaternion.Lerp(transform.rotation, cameraPos.rotation, Time.deltaTime * smoothRotation);
	}
}
