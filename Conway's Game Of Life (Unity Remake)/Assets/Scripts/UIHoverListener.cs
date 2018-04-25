using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverListener : MonoBehaviour
{
	public static bool isUIOverride { get; private set; }

	void Update ()
	{
		isUIOverride = EventSystem.current.IsPointerOverGameObject ();
	}
}
