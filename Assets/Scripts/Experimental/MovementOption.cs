using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
	[InspectorName("Transform SetPosition()")]
	SetPosition,
	[InspectorName("Transform Translate()")]
	Translate,
	[InspectorName("Rigidbody AddForse()")]
	RigidbodyAddForse,
	[InspectorName("Rigidbody MovePosition()")]
	RigidbodyMovePosition,
	[InspectorName("Rigibody SetVelocity()")]
	RigidbodySetVelocity
}

[RequireComponent(typeof(Rigidbody))]
public class MovementOption : MonoBehaviour
{
	[SerializeField] private float speed = 5f;
	[SerializeField] private MovementType movementType;

	private Rigidbody rigidbodyComponent;

	private void Awake()
	{
		rigidbodyComponent = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		switch (movementType)
		{
			case MovementType.SetPosition:
				SetPosition();
				break;
			case MovementType.Translate:
				Translate();
				break;
			case MovementType.RigidbodyAddForse:
				AddForce();
				break;
			case MovementType.RigidbodySetVelocity:
				SetVelocity();
				break;
			case MovementType.RigidbodyMovePosition:
				MovePosition();
				break;
			default:
				break;
		}
	}
	private void SetPosition()
	{
		transform.position += transform.forward * speed * Time.fixedDeltaTime;
	}

	private void Translate()
	{
		transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
	}

	private void AddForce()
	{
		rigidbodyComponent.AddForce(transform.forward * speed);
	}

	private void SetVelocity()
	{
		rigidbodyComponent.velocity = transform.forward * speed;
	}

	private void MovePosition()
	{
		rigidbodyComponent.MovePosition(rigidbodyComponent.position + transform.forward * speed * Time.fixedDeltaTime);
	}
}
