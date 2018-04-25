using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCheck : MonoBehaviour
{
	public Camera cam;
	RaycastHit hit;
	Ray ray;

	void OnMouseClick ()
	{
		if (Input.GetButton ("Fire1")) {

			ray = cam.ScreenPointToRay (Input.mousePosition);

			Debug.Log ("Press the left mouse button!");

			if (Physics.Raycast (ray, out hit)) {

				GameObject objectHit = hit.transform.gameObject;

				Debug.Log ("Ray hit : " + objectHit);
			}

		} else if (Input.GetButton ("Fire2")) {

			Debug.Log ("Press the right mouse button!");

		}
	}
}
