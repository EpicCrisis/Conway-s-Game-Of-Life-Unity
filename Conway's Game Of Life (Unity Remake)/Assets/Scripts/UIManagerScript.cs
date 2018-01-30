using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{

	[Header ("GameObject Adjust")]
	//public GameObject[] UIButtons;
	public GameObject PlayButton;
	public GameObject StopButton;
	public GameObject StepButton;

	public GameObject MoreSizeButton;
	public GameObject LessSizeButton;

	[Header ("Slider Adjust")]
	//public Slider[] UISliders;
	public Slider mapSlider;
	public Slider timeSlider;

	public static UIManagerScript instance = null;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public void StartSim ()
	{
		GameOfLifeManager.instance.Run ();

		PlayButton.SetActive (false);
		StopButton.SetActive (true);
		StepButton.SetActive (false);
	}

	public void StopSim ()
	{
		GameOfLifeManager.instance.Stop ();

		PlayButton.SetActive (true);
		StopButton.SetActive (false);
		StepButton.SetActive (true);
	}

	public void NextStep ()
	{
		GameOfLifeManager.instance.UpdateCells ();
	}

	public void ClearMap ()
	{
		GameOfLifeManager.instance.ResetCells ();
	}

	public void ChangeMapSize (Slider slider)
	{
		StopSim ();
		ClearMap ();
		GameOfLifeManager.instance.RemoveGrid ();

		GameOfLifeManager.instance.mapSizeX = (int)slider.value * 25;
		GameOfLifeManager.instance.mapSizeY = (int)slider.value * 25;

		GameOfLifeManager.instance.InitGrid (GameOfLifeManager.instance.mapSizeX, GameOfLifeManager.instance.mapSizeY);
	}

	public void IncreaseMapSize ()
	{
		if (GameOfLifeManager.instance.mapSizeX != 75 && GameOfLifeManager.instance.mapSizeY != 75) {
			StopSim ();
			ClearMap ();
			GameOfLifeManager.instance.RemoveGrid ();

			GameOfLifeManager.instance.mapSizeX += 25;
			GameOfLifeManager.instance.mapSizeY += 25;

			GameOfLifeManager.instance.InitGrid (GameOfLifeManager.instance.mapSizeX, GameOfLifeManager.instance.mapSizeY);
		}

		if (GameOfLifeManager.instance.mapSizeX == 75 && GameOfLifeManager.instance.mapSizeY == 75) {
			MoreSizeButton.SetActive (false);
		} else {
			MoreSizeButton.SetActive (true);
			LessSizeButton.SetActive (true);
		}
	}

	public void DecreaseMapSize ()
	{
		if (GameOfLifeManager.instance.mapSizeX != 25 && GameOfLifeManager.instance.mapSizeY != 25) {
			StopSim ();
			ClearMap ();
			GameOfLifeManager.instance.RemoveGrid ();

			GameOfLifeManager.instance.mapSizeX -= 25;
			GameOfLifeManager.instance.mapSizeY -= 25;

			GameOfLifeManager.instance.InitGrid (GameOfLifeManager.instance.mapSizeX, GameOfLifeManager.instance.mapSizeY);
		}

		if (GameOfLifeManager.instance.mapSizeX == 25 && GameOfLifeManager.instance.mapSizeY == 25) {
			LessSizeButton.SetActive (false);
		} else {
			MoreSizeButton.SetActive (true);
			LessSizeButton.SetActive (true);
		}
	}

	public void ChangeUpdateInterval (Slider slider)
	{
		GameOfLifeManager.instance.updateInterval = slider.value;
	}
}
