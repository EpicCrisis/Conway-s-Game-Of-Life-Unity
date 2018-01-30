using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoolManager : MonoBehaviour
{
	// Setup for singleton.
	public static SpawnPoolManager instance = null;

	public List<GameObject> objectsToPool;
	public List<int> numberOfObjectsToPool;
	Dictionary <string, Stack<GameObject>> pool;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (this.gameObject);

		InitializePoolManager ();
	}

	void Start ()
	{
	}

	// Initialize pool manager.
	void InitializePoolManager ()
	{
		pool = new Dictionary<string, Stack<GameObject>> ();

		for (int i = 0; i < objectsToPool.Count; i++) {

			pool.Add (objectsToPool [i].name, new Stack<GameObject> ());

			for (int f = 0; f < numberOfObjectsToPool [i]; f++) {
				GameObject go = Instantiate (objectsToPool [i]);
				go.transform.SetParent (this.transform);
				go.name = objectsToPool [i].name;
				go.gameObject.SetActive (false);

				pool [objectsToPool [i].name].Push (go);
			}
		}
	}

	// Spawn, or displace an existing hidden object at a particular position or rotation.
	public CellScript Spawn (string objectName, Vector3 newPosition, Quaternion newRotation)
	{
		if (!pool.ContainsKey (objectName)) {
			Debug.LogWarning ("No Pool For " + objectName + " Exists");
			return null;
		}

		if (pool [objectName].Count > 0) {
			GameObject go = pool [objectName].Pop ();
			go.transform.position = newPosition;
			go.transform.rotation = newRotation;
			go.SetActive (true);
			return go.GetComponent<CellScript> ();
		} else {
			Debug.LogWarning ("Reached Pool Limit : " + objectName);
		}
		return null;
	}

	// Despawn.
	public void Despawn (GameObject objectToDespawn)
	{
		objectToDespawn.SetActive (false);
		pool [objectToDespawn.name].Push (objectToDespawn);
	}
}
