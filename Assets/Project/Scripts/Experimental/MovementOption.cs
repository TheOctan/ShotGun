using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Experimental
{
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
		[SerializeField] private Space space = Space.Self;
		[SerializeField] private InterpolationType interpolation;
		[SerializeField] private UpdatingType updatingType;
		[SerializeField] private TimeScale timeScale;

		[Header("Debug")]
		[SerializeField] private bool isShowGizmos = true;

		private Rigidbody rigidbodyComponent;
		private Vector3 direction;

		private void Awake()
		{
			rigidbodyComponent = GetComponent<Rigidbody>();
			direction = Vector3.forward;
		}

		private void Update()
		{
			if (updatingType == UpdatingType.Update)
			{
				float deltaTime = timeScale == TimeScale.Scaled ? Time.deltaTime : Time.unscaledDeltaTime;
				HandleMovement(Time.deltaTime);
			}
		}

		private void FixedUpdate()
		{
			if (updatingType == UpdatingType.FixedUpdate)
			{
				float deltaTime = timeScale == TimeScale.Scaled ? Time.fixedDeltaTime : Time.fixedUnscaledDeltaTime;
				HandleMovement(deltaTime);
			}
		}

		void HandleMovement(float deltaTime)
		{
			switch (movementType)
			{
				case MovementType.SetPosition:
					SetPosition(deltaTime);
					break;
				case MovementType.Translate:
					Translate(deltaTime);
					break;
				case MovementType.RigidbodyAddForse:
					AddForce();
					break;
				case MovementType.RigidbodySetVelocity:
					SetVelocity();
					break;
				case MovementType.RigidbodyMovePosition:
					MovePosition(deltaTime);
					break;
				default:
					break;
			}
		}

		private void SetPosition(float deltaTime)
		{
			transform.position += GetSpaceDirection() * speed * deltaTime;
		}

		private void Translate(float deltaTime)
		{
			transform.Translate(direction * speed * deltaTime, space);
		}

		private void AddForce()
		{
			rigidbodyComponent.AddForce(GetSpaceDirection() * speed);
		}

		private void SetVelocity()
		{
			rigidbodyComponent.velocity = GetSpaceDirection() * speed;
		}

		private void MovePosition(float deltaTime)
		{
			rigidbodyComponent.MovePosition(rigidbodyComponent.position + GetSpaceDirection() * speed * deltaTime);
		}

		private Vector3 GetSpaceDirection()
		{
			if (space == Space.Self)
			{
				return transform.TransformDirection(direction);
			}
			else
			{
				return direction;
			}
		}

		private void OnDrawGizmos()
		{
			if (isShowGizmos)
			{

			}
		}
	}
}