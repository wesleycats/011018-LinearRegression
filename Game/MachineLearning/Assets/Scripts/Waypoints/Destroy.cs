using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys waypoint on hit with player
/// </summary>
public class Destroy : MonoBehaviour {

	public delegate void Action(GameObject item);
	public Action Delete;

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			Delete(this.gameObject);
		}
	}
}
