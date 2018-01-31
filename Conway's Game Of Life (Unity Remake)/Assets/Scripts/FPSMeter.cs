using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSMeter : MonoBehaviour 
{
	public float targetFps = 60f;
	public float updateInterval = 1f;

	public GameObject fpsPanel;
	public Text fpsText;

	private Image fpsImage;
	private Canvas canvas;

	private int lastFrames = 0;
	private float timeAccu = 0f;

	void Awake () 
	{
		fpsImage = fpsPanel.GetComponent <Image> ();
		canvas = GetComponent <Canvas> ();
		fpsPanel.SetActive (false);

		MoveToFront ();
	}

	void Update () 
	{
		timeAccu += Time.deltaTime;
		if (timeAccu >= updateInterval) 
		{
			int frames = Time.frameCount - lastFrames;
			int fps = Mathf.RoundToInt (frames / timeAccu);
			UpdateControls (fps, System.GC.GetTotalMemory (false) / 1048576f);
			timeAccu = 0f;
			lastFrames = Time.frameCount;
			fpsPanel.SetActive (true);
		}
	}

	private void UpdateControls (int fps, float gcMemory) 
	{
		fpsText.text = string.Format ("FPS: {0}\nGC: {1:0.00}", fps, gcMemory);
		fpsImage.color = Color.Lerp (Color.red, Color.green, fps / targetFps);
	}

	private void MoveToFront () 
	{
		int maxSortOrder = 0;
		Canvas[] canvases = FindObjectsOfType <Canvas> ();
		for (int i = 0; i < canvases.Length; i++) 
		{
			if (canvases[i] != canvas && canvases [i].sortingOrder > maxSortOrder)
				maxSortOrder = canvases [i].sortingOrder;
		}
		canvas.sortingOrder = maxSortOrder + 1;
	}
}
