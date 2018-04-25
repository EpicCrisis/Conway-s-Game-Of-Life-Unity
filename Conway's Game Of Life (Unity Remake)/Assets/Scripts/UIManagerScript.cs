using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{

	[Header ("===GameObject Adjust===")]
	//public GameObject[] UIButtons;
	public GameObject PlayButton;
	public GameObject StopButton;
	public GameObject StepButton;

	public GameObject MoreSizeButton;
	public GameObject LessSizeButton;

	[Header ("===Slider Adjust===")]
	//public Slider[] UISliders;
	public Slider mapSlider;
	public Slider timeSlider;

    [Header("===Text Adjust===")]
    public Text mapSizeText;
    public Text updateIntervalText;

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

    void Start()
    {
        mapSizeText.text = "Map Size: " + GameOfLifeManager.instance.mapSizeX + "x" + GameOfLifeManager.instance.mapSizeY;
        updateIntervalText.text = "Time Step: " + Mathf.Round(GameOfLifeManager.instance.updateInterval * 1000.0f) + "ms";

		if (GameOfLifeManager.instance.mapSizeX >= GameOfLifeManager.instance.maxMapSizeX
			&& GameOfLifeManager.instance.mapSizeY >= GameOfLifeManager.instance.maxMapSizeY) {

			MoreSizeButton.SetActive (false);
		}  else if (GameOfLifeManager.instance.mapSizeX <= GameOfLifeManager.instance.minMapSizeX
			&& GameOfLifeManager.instance.mapSizeY <= GameOfLifeManager.instance.minMapSizeY) {

			LessSizeButton.SetActive (false);
		} 
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

	public void NextStep()
	{
		GameOfLifeManager.instance.UpdateCells();
	}

    // Clear map now automatically stops the simulation.
	public void ClearMap ()
	{
        StopSim();
		GameOfLifeManager.instance.ResetCells ();
	}

	public void ChangeMapSize (Slider slider)
	{
		ClearMap ();
		GameOfLifeManager.instance.RemoveGrid ();

		GameOfLifeManager.instance.mapSizeX = (int)slider.value * 25;
		GameOfLifeManager.instance.mapSizeY = (int)slider.value * 25;

		GameOfLifeManager.instance.InitGrid (GameOfLifeManager.instance.mapSizeX, GameOfLifeManager.instance.mapSizeY);
	}

	public void IncreaseMapSize ()
	{
		if (GameOfLifeManager.instance.mapSizeX < GameOfLifeManager.instance.maxMapSizeX
            && GameOfLifeManager.instance.mapSizeY < GameOfLifeManager.instance.maxMapSizeY) {

			ClearMap ();
			GameOfLifeManager.instance.RemoveGrid ();

			GameOfLifeManager.instance.mapSizeX += 25;
			GameOfLifeManager.instance.mapSizeY += 25;

			GameOfLifeManager.instance.InitGrid (GameOfLifeManager.instance.mapSizeX, GameOfLifeManager.instance.mapSizeY);

            mapSizeText.text = "Map Size: " + GameOfLifeManager.instance.mapSizeX + "x" + GameOfLifeManager.instance.mapSizeY;
        }

		if (GameOfLifeManager.instance.mapSizeX >= GameOfLifeManager.instance.maxMapSizeX
            && GameOfLifeManager.instance.mapSizeY >= GameOfLifeManager.instance.maxMapSizeY) {

			MoreSizeButton.SetActive (false);
		} else {
			MoreSizeButton.SetActive (true);
			LessSizeButton.SetActive (true);
		}
	}

	public void DecreaseMapSize ()
	{
		if (GameOfLifeManager.instance.mapSizeX > GameOfLifeManager.instance.minMapSizeX
            && GameOfLifeManager.instance.mapSizeY > GameOfLifeManager.instance.minMapSizeY) {

			ClearMap ();
			GameOfLifeManager.instance.RemoveGrid ();

			GameOfLifeManager.instance.mapSizeX -= 25;
			GameOfLifeManager.instance.mapSizeY -= 25;

			GameOfLifeManager.instance.InitGrid (GameOfLifeManager.instance.mapSizeX, GameOfLifeManager.instance.mapSizeY);

            mapSizeText.text = "Map Size: " + GameOfLifeManager.instance.mapSizeX + "x" + GameOfLifeManager.instance.mapSizeY;
        }

        if (GameOfLifeManager.instance.mapSizeX <= GameOfLifeManager.instance.minMapSizeX
            && GameOfLifeManager.instance.mapSizeY <= GameOfLifeManager.instance.minMapSizeY) {

			LessSizeButton.SetActive (false);
		} else {
			MoreSizeButton.SetActive (true);
			LessSizeButton.SetActive (true);
		}
	}

	public void ChangeUpdateInterval (Slider slider)
	{
		GameOfLifeManager.instance.updateInterval = slider.value;

        updateIntervalText.text = "Time Step: " + Mathf.Round(GameOfLifeManager.instance.updateInterval * 1000.0f) + "ms";
    }
}
