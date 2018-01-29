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
	public SpriteRenderer sRender;

	public Color aliveColour;
	public Color deadColour;

	UIHoverListener UIListener;

	void Awake ()
	{
		mRender = GetComponent<MeshRenderer> ();
		sRender = GetComponent<SpriteRenderer> ();
		UIListener = FindObjectOfType<UIHoverListener> ();
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
			//mRender.sharedMaterial = aliveMaterial;
			sRender.color = aliveColour;
		} else {
			//mRender.sharedMaterial = deadMaterial;
			sRender.color = deadColour;
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

	void OnMouseOver ()
	{
		if (!UIListener.isUIOverride) {

			//Debug.Log ("Mouse is over this cell");

			if (Input.GetButton ("Fire1")) {
				//Debug.Log ("LMB this object : " + this.transform);

				state = CellState.Alive;
				UpdateMaterial ();

			} else if (Input.GetButton ("Fire2")) {
				//Debug.Log ("RMB this object : " + this.transform);

				state = CellState.Dead;
				UpdateMaterial ();
			}
		}
	}
}
