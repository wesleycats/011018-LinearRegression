using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject enemyPre;
	public GameObject waypoint;
	public GameObject playerPre;
	public float playerWeight;
	public float playerMaxSpeed;
	public bool playerTurns = true;
	public Vector3 playerStartPos;
	public List<GameObject> waypoints = new List<GameObject>();

	GameObject player;
	Character playerInfo;

	GameObject enemy;
	Character enemyInfo;

	// Use this for initialization
	void Start () {
		Initialize();
	}

	// Update is called once per frame
	void Update () {
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

		if (waypoints.Count <= 0) return;

		playerInfo.Target = waypoints[0];

		foreach (GameObject w in waypoints)
		{
			w.GetComponent<Destroy>().Delete += OnDelete;
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
		enemy = Instantiate(enemyPre, enemyStartPos, Quaternion.identity);
		enemyInfo = enemy.GetComponent<Character>();
		enemyInfo.Weight = enemyWeight;
		enemyInfo.MaxSpeed = enemyMaxSpeed;
		enemyInfo.CurrentPosition = enemyStartPos;
		enemyInfo.Turn = enemyTurns;
	}
}
