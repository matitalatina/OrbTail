using UnityEngine;
using System.Collections;

public class HUDPositionHandler : MonoBehaviour {

	private Transform camera;
	private const float smoothMovement = 1000000f;
	private const float smoothRotation = 1000000f;
	private const float distanceFromCamera = 10f;

	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag(Tags.MainCamera).transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, camera.position + camera.forward * distanceFromCamera, Time.deltaTime * smoothMovement);
		transform.rotation = Quaternion.Lerp(transform.rotation, camera.rotation, Time.deltaTime * smoothRotation);
	}
}
