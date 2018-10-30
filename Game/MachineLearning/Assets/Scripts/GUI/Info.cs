using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour {

	public Text generation;
	public Text topFitness;
	public Text timer;
	public GameManager gameManager;
	public List<float> allFitness = null;

	void Start () {
		allFitness = new List<float>();
	}
	
	void Update () {
		generation.text = "GENERATION: " + gameManager.generationNumber;
		
		// Itterates over all enemies and adds their fitness to the list
		for (int i = 0; i < gameManager.enemyList.Count; i++)
		{
			allFitness.Add(gameManager.enemyList[i].net.fitness);
		}
		
		// Changes text to top fitness
		if (allFitness.Count > 0)
		{
			allFitness.Sort();
			topFitness.text = "TOP FITNESS: " + allFitness[allFitness.Count - 1];
		}

		timer.text = "TIMER: " + (Mathf.Round(gameManager.timer * 10f) / 10f) + " seconds";
	}
}
