using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	#region Members
	public float Weight
	{
		get;
		set;
	}

	public float MaxSpeed
	{
		get;
		set;
	}

	public GameObject Target
	{
		get;
		set;
	}

	public Vector3 CurrentVelocity
	{
		get;
		set;
	}

	public Vector3 CurrentPosition
	{
		get;
		set;
	}

	public bool Turn
	{
		get;
		set;
	}

	#endregion

	#region Constructors

	public Character(float weigth, float speed)
	{
		this.Weight = weigth;
		this.MaxSpeed = speed;
	}

	#endregion

	#region Methods

	void Update()
	{
		if (Target == null) return;

		MoveTo(Target);
	}

	/// <summary>
	/// Moves to target while calculating the turning radius
	/// </summary>
	/// <param name="target"></param>
	public void MoveTo(GameObject target)
	{
		Vector3 step = target.transform.position - this.transform.position;
		step.Normalize();
		Vector3 velocity = step * MaxSpeed;
		Vector3 steeringForce = velocity - CurrentVelocity;
		CurrentVelocity += steeringForce / Weight;
		CurrentPosition += CurrentVelocity * Time.deltaTime;
		transform.position = CurrentPosition;

		if (Turn)
		{
			float angle = Mathf.Atan2(CurrentVelocity.y, CurrentVelocity.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

	#endregion
}
