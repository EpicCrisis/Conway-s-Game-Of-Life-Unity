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
    public int generationNumber = 0;
    float counter;

    // Declaring a matrix for cells, allows neighbour checking!
    public CellScript[,] cells;

	public GameState state = GameState.Stop;

    // Action is used to update cells.
    private Action cellUpdate;
    private Action applyCellUpdate;

    private IEnumerator coroutine;

    public static GameOfLifeManager instance = null;

    [Header("===Text To Update Data===")]
    public Text GenerationText;

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
        GenerationText.text = "Generation: " + generationNumber;
    }

	void Update ()
	{
        //RunCellUpdate();
	}

	public void RemoveGrid ()
	{
		// Checks if there are cells in the grid or not.
		if (cells != null) {
			for (int i = 0; i < mapSizeX; i++) {
				for (int j = 0; j < mapSizeY; j++) {
                    SpawnPoolManager.instance.Despawn (cells [i, j].gameObject);
					//GameObject.Destroy (cells [i, j].gameObject);
				}
			}
		}
	}

	public void InitGrid (int x, int y)
	{
		cellUpdate = null;
		applyCellUpdate = null;

		coroutine = null;

		mapSizeX = x;
		mapSizeY = y;

		CreateCells (mapSizeX, mapSizeY);
	}

	public void CreateCells (int x, int y)
	{
		cells = new CellScript[x, y];

		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				// Creating a cell into the scene.
				//CellScript c = Instantiate (cellPrefab, new Vector3 ((float)i, (float)j, 0.0f), Quaternion.identity) as CellScript;

				CellScript c = SpawnPoolManager.instance.Spawn ("CellSprite", new Vector3 ((float)i, (float)j, 0.0f), Quaternion.identity);

				cells [i, j] = c;

				c.InitCell (this, i, j);
                //c.SetRandomState (); // Randomize cell state;

                // Referencing the actions to find the functions in the CellScript.
                cellUpdate += c.CellUpdate;
                applyCellUpdate += c.ApplyCellUpdate;
			}
		}

		// Find references for every neighbouring cell.
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				cells [i, j].neighbour = GetNeighbours (i, j);
			}
		}
	}

	public void ResetCells ()
	{
		if (cells != null) {
			for (int i = 0; i < mapSizeX; i++) {
				for (int j = 0; j < mapSizeY; j++) {
					cells [i, j].ClearCell ();

                    // Updates the generation step only when a cell update occurs.
                    generationNumber = 0;
                    GenerationText.text = "Generation: " + generationNumber;
                }
			}
		}
	}

	// Checking adjacent cells with array.
	public CellScript[] GetNeighbours (int x, int y)
	{
		CellScript[] result = new CellScript[8];

		result [0] = cells [x, (y + 1) % mapSizeY]; // top

		result [1] = cells [(x + 1) % mapSizeX, (y + 1) % mapSizeY]; // top right

		result [2] = cells [(x + 1) % mapSizeX, y % mapSizeY]; // right

		result [3] = cells [(x + 1) % mapSizeX, (mapSizeY + y - 1) % mapSizeY]; // bottom right

		result [4] = cells [x % mapSizeX, (mapSizeY + y - 1) % mapSizeY]; // bottom

		result [6] = cells [(mapSizeX + x - 1) % mapSizeX, (mapSizeY + y - 1) % mapSizeY]; // bottom left

		result [7] = cells [(mapSizeX + x - 1) % mapSizeX, y % mapSizeY]; // left

		result [5] = cells [(mapSizeX + x - 1) % mapSizeX, (y + 1) % mapSizeY]; // top left

		return result;
	}

	public void Run ()
	{
		if (state == GameState.Stop) {
			state = GameState.Start;

            if (coroutine != null) {
				StopCoroutine (coroutine);
			}

            coroutine = RunCoroutine ();
            StartCoroutine (coroutine);
        }
	}

	public void Stop ()
	{
		if (state == GameState.Start) {
			state = GameState.Stop;

            StopCoroutine (coroutine);
            //counter = 0.0f;
        }
    }

	public void UpdateCells ()
	{
		cellUpdate ();
		applyCellUpdate ();

        // Updates the generation step only when a cell update occurs.
        generationNumber++;
        GenerationText.text = "Generation: " + generationNumber;
	}

	private IEnumerator RunCoroutine ()
	{
		while (state == GameState.Start) {
			UpdateCells ();
			yield return new WaitForSeconds (updateInterval);
		}
	}

    /*public void RunCellUpdate()
    {
        if (state == GameState.Start)
        {
            counter += Time.deltaTime;
            if (counter > updateInterval)
            {
                counter = 0.0f;
                UpdateCells();
            }
        }
        else
        {
            return;
        }
    }*/
}
