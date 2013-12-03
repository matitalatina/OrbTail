using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float smooth = 1.5f;         // The relative speed at which the camera will catch up.
	public float relDistancePos = -5f;
	public float relHighPos = 3f;
	
	private Transform player;           // Reference to the player's transform.
	private Vector3 relCameraPos;       // The relative position of the camera from the player.
	private float relCameraPosMag;      // The distance of the camera from the player.
	private Vector3 newPos;             // The position the camera is trying to reach.


	private FloatingObject FloatingComponent{ get;set; }

    public void LookAt(GameObject target)
    {

        // Setting up the reference.
        player = target.transform;
        FloatingComponent = player.GetComponent<FloatingObject>();

        // Setting the relative position as the initial relative position of the camera in the scene.
        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;
	
    }

	void Awake ()
	{
	}
	
	void FixedUpdate ()
	{
		// The standard position of the camera is the relative position of the camera from the player.
		//Vector3 standardPos = player.position + relCameraPos;
		Vector3 arenaDown = FloatingComponent.ArenaDown;

		Vector3 standardPos = player.position + player.forward * relDistancePos + relHighPos * -arenaDown;
		
		newPos = standardPos;
		// Lerp the camera's position between it's current position and it's new position.
		transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
		
		// Make sure the camera is looking at the player.
		SmoothLookAt(arenaDown);
	}
	
	
	bool ViewingPosCheck (Vector3 checkPos)
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something...
		if(Physics.Raycast(checkPos, player.position - checkPos, out hit, relCameraPosMag))
			// ... if it is not the player...
			if(hit.transform != player)
				// This position isn't appropriate.
				return false;
		
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		newPos = checkPos;
		return true;
	}
	
	
	void SmoothLookAt (Vector3 arenaDown)
	{
		// Create a vector from the camera towards the player.
		Vector3 relPlayerPosition = player.position - transform.position;

		// Create a rotation based on the relative position of the player being the forward vector.

		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, -arenaDown);
		
		// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	}
}
