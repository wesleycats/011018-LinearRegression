using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Info guiManager;
	public GameObject enemyPre;
	public GameObject playerPre;
	public GameObject waypoint;
	public float playerWeight = 15f;
	public float playerMaxSpeed = 2.5f;
	public bool playerTurns = true;
	public Vector3 playerStartPos;
	public List<GameObject> waypoints = new List<GameObject>();
	public bool debug = false;
	public float resetTime = 10f;
	public float timer = 0;

	GameObject player;
	Character playerInfo;

	GameObject enemy;
	Character enemyInfo;
	
	private bool isTraining = false;
	private int populationSize = 50;
	public int generationNumber = 0;
	private int[] layers = new int[] { 1, 10, 10, 1 }; //1 input and 1 output
	private List<NeuralNetwork> nets;
	private bool leftMouseDown = false;
	public List<Enemy> enemyList = null;


	// Use this for initialization
	void Start () {
		Initialize();

		debug = false;
	}

	// Update is called once per frame
	void Update () {
		if (debug)
		{
			playerInfo.Weight = playerWeight;
			playerInfo.MaxSpeed = playerMaxSpeed;
		}

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				GameObject item = Instantiate(waypoint, hit.point, Quaternion.identity);
				waypoints.Add(item);
			}
		}

		if (Input.GetKey(KeyCode.Space))
		{
			Time.timeScale = 3;
		}
		else
		{
			Time.timeScale = 1;
		}

		if (waypoints.Count > 0)
		{
			playerInfo.Target = waypoints[0];

			foreach (GameObject w in waypoints)
			{
				w.GetComponent<Destroy>().Delete += OnDelete;
			}

			if (Input.GetMouseButtonDown(1))
			{
				Destroy(waypoints[waypoints.Count - 1]);
				waypoints.RemoveAt(waypoints.Count - 1);
			}
		}

		if (isTraining == false)
		{
			if (generationNumber == 0)
			{
				InitEnemyNeuralNetworks();
			}
			else
			{
				nets.Sort();
				for (int i = 0; i < populationSize / 2; i++)
				{
					nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
					nets[i].Mutate();

					nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
				}

				for (int i = 0; i < populationSize; i++)
				{
					nets[i].SetFitness(0f);
				}
			}


			generationNumber++;
			guiManager.allFitness.Clear();
			timer = resetTime;

			isTraining = true;
			Invoke("Timer", resetTime);
			CreateEnemyBodies();
		}
		else
		{
			timer = timer - Time.deltaTime;
		}
	}

	void Timer()
	{
		isTraining = false;
	}

	private void CreateEnemyBodies()
	{
		if (enemyList != null)
		{
			for (int i = 0; i < enemyList.Count; i++)
			{
				GameObject.Destroy(enemyList[i].gameObject);
			}

		}

		enemyList = new List<Enemy>();

		for (int i = 0; i < populationSize; i++)
		{
			Enemy enemy = ((GameObject)Instantiate(enemyPre, new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0), enemyPre.transform.rotation)).GetComponent<Enemy>();
			enemy.Init(nets[i], player.transform);
			enemyList.Add(enemy);
		}

	}

	void InitEnemyNeuralNetworks()
	{
		//population must be even, just setting it to 20 incase it's not
		if (populationSize % 2 != 0)
		{
			populationSize = 2;
		}

		nets = new List<NeuralNetwork>();


		for (int i = 0; i < populationSize; i++)
		{
			NeuralNetwork net = new NeuralNetwork(layers);
			net.Mutate();
			nets.Add(net);
		}
	}


	/// <summary>
	/// Destroy target waypoint and removes from the waypoint list
	/// </summary>
	/// <param name="item"></param>
	void OnDelete(GameObject item)
	{
		if (waypoints.Count <= 0 || item != waypoints[0]) return;
		waypoints.RemoveAt(0);
		Destroy(item);
	}

	/// <summary>
	/// Initializes all objects in the scene
	/// </summary>
	void Initialize()
	{
		// Player
		player = Instantiate(playerPre, playerStartPos, Quaternion.identity);
		playerInfo = player.GetComponent<Character>();
		playerInfo.Weight = playerWeight;
		playerInfo.MaxSpeed = playerMaxSpeed;
		playerInfo.CurrentPosition = playerStartPos;
		playerInfo.Turn = playerTurns;

		float enemyWeight = playerWeight;
		float enemyMaxSpeed = playerMaxSpeed;
		Vector3 enemyStartPos = playerStartPos + new Vector3(10, 0, 0);
		bool enemyTurns = playerTurns;

		// Enemy
		/*enemy = Instantiate(enemyPre, enemyStartPos, Quaternion.identity);
		enemyInfo = enemy.GetComponent<Character>();
		enemyInfo.Weight = enemyWeight;
		enemyInfo.MaxSpeed = enemyMaxSpeed;
		enemyInfo.CurrentPosition = enemyStartPos;
		enemyInfo.Turn = enemyTurns;*/
	}
}
