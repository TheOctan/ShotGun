using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	private Vector3 velocity;
	private Rigidbody rigidbodyComponent;

	void Start()
	{
		rigidbodyComponent = GetComponent<Rigidbody>();
		
	}

	void FixedUpdate()
	{
		rigidbodyComponent.MovePosition(rigidbodyComponent.position + velocity * Time.fixedDeltaTime);
	}

	public void Move(Vector3 velocity)
	{
		this.velocity = velocity;
	}

	public void LookAt(Vector3 lookPoint)
	{
		Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt(heightCorrectedPoint);
	}
}
