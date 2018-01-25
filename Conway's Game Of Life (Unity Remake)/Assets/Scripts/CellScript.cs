﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
	// This script is added into the cell prefab!

	public enum CellState
	{
		Dead,
		Alive
	}

	public Material aliveMaterial;
	public Material deadMaterial;

	public GameOfLifeManager gameOfLife;

	public int x;
	public int y;

	// Array to check neighbours per time step.
	public CellScript[] neighbour;

	// Checks current state.
	public CellState state;
	// To change the state on next time step.
	public CellState nextState;

	public MeshRenderer mRender;

	void Awake ()
	{
		mRender = GetComponent<MeshRenderer> ();
	}

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	public void CellUpdate ()
	{
		nextState = state;

		int aliveCells = GetAliveCells ();

		if (state == CellState.Alive) {
			if (aliveCells != 2 && aliveCells != 3) {
				nextState = CellState.Dead;
			}
		} else {
			if (aliveCells == 3) {
				nextState = CellState.Alive;
			}
		}
	}

	public void ApplyCellUpdate ()
	{
		state = nextState;
		UpdateMaterial ();
	}

	public void InitCell (GameOfLifeManager GOS, int x, int y)
	{
		gameOfLife = GOS;
		transform.parent = GOS.transform;

		this.x = x;
		this.y = y;
	}

	// Randomize state for cell.
	public void SetRandomState ()
	{
		state = (Random.Range (0, 2) == 0) ? CellState.Alive : CellState.Dead;
		UpdateMaterial ();
	}

	public void ClearCell ()
	{
		state = CellState.Dead;
		UpdateMaterial ();
	}

	// Changing the material based on cell state.
	public void UpdateMaterial ()
	{
		if (state == CellState.Alive) {
			mRender.sharedMaterial = aliveMaterial;
		} else {
			mRender.sharedMaterial = deadMaterial;
		}
	}

	// Find neighbours that exists and are alive.
	public int GetAliveCells ()
	{
		int number = 0;

		for (int i = 0; i < neighbour.Length; i++) {
			if (neighbour [i] != null && neighbour [i].state == CellState.Alive) {
				number++;
			}
		}

		return number;
	}
}
