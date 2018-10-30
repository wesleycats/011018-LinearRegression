using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	GameObject player;

	void Update () {

		if (!player)
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}

		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}
