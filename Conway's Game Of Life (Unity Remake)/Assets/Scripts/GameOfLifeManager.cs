using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameOfLifeManager : MonoBehaviour
{
	// This script is attached to an empty game object!

	public enum GameState
	{
		Start,
		Stop
	}

	public int mapSizeX = 50;
	public int mapSizeY = 50;

    [Header("===Determine Map Size Limit===")]
    public int minMapSizeX = 25;
    public int minMapSizeY = 25;
    public int maxMapSizeX = 150;
    public int maxMapSizeY = 150;
    
    [Header("===Cell Function===")]
    public CellScript cellPrefab;

	public float updateInterval = 0.1f;
    float counter;

    // Declaring a matrix for cells, allows neighbour checking!
    public CellScript[,] cells;

	public bool isPlaying = false;

	public int generation;
	public Text genText;

	//Singleton
    public static GameOfLifeManager instance = null;

    void Awake ()
	{
		//Run ();

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void Start ()
	{
		InitGrid (mapSizeX, mapSizeY);
    }

	void Update ()
	{
		if(isPlaying)
		{
			counter += Time.deltaTime;
			if(counter >= updateInterval)
			{
				counter = 0.0f;
				UpdateCells();
			}
		}
	}

	public void UpdateCells()
	{
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				cells[i,j].CellUpdate();
			}
		}
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				cells[i,j].ApplyCellUpdate();
			}
		}

		generation++;
		genText.text = generation.ToString("000");
	}

	public void RemoveGrid ()
	{
		// Checks if there are cells in the grid or not.
		if (cells == null) return;

		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
                SpawnPoolManager.instance.Despawn (cells [i, j].gameObject);
			}
		}
	}

	public void InitGrid (int newMapSizeX, int newMapSizeY)
	{
		generation = 0;
		genText.text = generation.ToString("000");

		mapSizeX = newMapSizeX;
		mapSizeY = newMapSizeY;

		//Create cells
		cells = new CellScript[mapSizeX, mapSizeY];

		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				// Creating a cell into the scene.
				//CellScript c = Instantiate (cellPrefab, new Vector3 ((float)i, (float)j, 0.0f), Quaternion.identity) as CellScript;

				CellScript c = SpawnPoolManager.instance.Spawn ("CellSprite", new Vector3 (i, j, 0.0f), Quaternion.identity);

				c.InitCell (i, j);

				cells [i, j] = c;
			}
		}
	}

	public void ResetCells ()
	{
		// Checks if there are cells in the grid or not.
		if (cells == null) return;

		generation = 0;
		genText.text = generation.ToString("000");

		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				cells [i, j].ClearCell ();
            }
		}
	}

	public void Run ()
	{
		if (!isPlaying)
		{
			isPlaying = true;
			counter = 0.0f;
        }
	}

	public void Stop ()
	{
		if (isPlaying)
		{
			isPlaying = false;
			counter = 0.0f;
		}
    }
}
