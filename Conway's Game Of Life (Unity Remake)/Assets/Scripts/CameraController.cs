using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	// This script is attached to the camera object!

	public GameOfLifeManager game;

	public float cameraSpeed = 10.0f;
	public float zoomSpeed = 8.0f;

	void Start ()
	{
		
	}

	void Update ()
	{
		CameraMove ();
	}

	void CameraMove ()
	{
		float xMove = Input.GetAxis ("Horizontal");
		float yMove = Input.GetAxis ("Vertical");

		Vector3 newPos = transform.position;
		newPos.x += xMove * cameraSpeed * Time.deltaTime;
		newPos.y += yMove * cameraSpeed * Time.deltaTime;

		// Camera clamp to prevent over-offset.
		newPos.x = Mathf.Clamp (newPos.x, 0.0f, (float)(game.mapSizeX - 1.0f));
		newPos.y = Mathf.Clamp (newPos.y, 0.0f, (float)(game.mapSizeY - 1.0f));

		transform.position = newPos;

		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			CameraZoom (zoomSpeed);
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			CameraZoom (-zoomSpeed);
		}
	}

	void CameraZoom (float zMove)
	{
		Vector3 newPos = transform.position;
		newPos.z += zMove * Time.deltaTime;

		newPos.z = Mathf.Clamp (newPos.z, (float)(-game.mapSizeX - game.mapSizeY), (float)((-game.mapSizeX - game.mapSizeY) / 10.0f));

		transform.position = newPos;
	}
}
