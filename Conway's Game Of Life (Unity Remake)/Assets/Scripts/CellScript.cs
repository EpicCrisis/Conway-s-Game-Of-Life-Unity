using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CellScript : MonoBehaviour
{
	// This script is added into the cell prefab!

	public enum CellState
	{
		Dead,
		Alive
	}

	public int x;
	public int y;

	// Checks current state.
	public CellState state;
	// To change the state on next time step.
	public CellState nextState;
    
	public SpriteRenderer sRender;

	public Color aliveColour;
	public Color deadColour;

	void Awake ()
	{
		sRender = GetComponent<SpriteRenderer> ();
	}

	[ContextMenu("Step")]
	public void CellUpdate ()
	{
		int aliveCells = GetAliveNeighbours ();

		nextState = state;

		if (state == CellState.Alive)
		{
			if (aliveCells != 2 && aliveCells != 3)
			{
				nextState = CellState.Dead;
			}
		}
		else
		{
			if (aliveCells == 3)
			{
				nextState = CellState.Alive;
			}
		}
	}

	public void ApplyCellUpdate ()
	{
		state = nextState;
		UpdateMaterial ();
	}

	public void InitCell (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void ClearCell ()
	{
		state = CellState.Dead;
		UpdateMaterial ();
	}

	// Changing the material based on cell state.
	private void UpdateMaterial ()
	{
		if (state == CellState.Alive) {
			sRender.color = aliveColour;
		} else {
			sRender.color = deadColour;
		}
	}

	// Find neighbours that exists and are alive.
	private int GetAliveNeighbours ()
	{
		int neighbours = 0;

		for(int i = x - 1; i <= x + 1; i++)
		{
			//Skips to the next if x is not in range
			if(i < 0 || i >= GameOfLifeManager.instance.mapSizeX)
				continue;
			
			for(int j = y - 1; j <= y + 1; j++)
			{
				//Skips to the next if y is not in range
				if(j < 0 || j >= GameOfLifeManager.instance.mapSizeY)
					continue;

				//Skips to the next when iterated to self
				if(i == x && j == y)
					continue;
				
				if (GameOfLifeManager.instance.cells [i,j].state == CellState.Alive)
				{
					neighbours++;
				}
			}
		}

		return neighbours;
	}

	void OnMouseOver ()
	{
		if (!UIHoverListener.isUIOverride)
		{
			//Debug.Log ("Mouse is over this cell");

			if (Input.GetButton ("Fire1"))
			{
				//Debug.Log ("LMB this object : " + this.transform);

				state = CellState.Alive;
				UpdateMaterial ();

			}
			else if (Input.GetButton ("Fire2"))
			{
				//Debug.Log ("RMB this object : " + this.transform);

				state = CellState.Dead;
				UpdateMaterial ();
			}
		}
	}
}
