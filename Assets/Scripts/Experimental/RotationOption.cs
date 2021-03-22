using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationType
{
    Transform,
	TransformQuaternion,
    Rigidbody
}

[RequireComponent(typeof(Rigidbody))]
public class RotationOption : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private RotationType rotationType;

    private Rigidbody rigidbodyComponent;

    private void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    private void Update()
    {
		switch (rotationType)
		{
			case RotationType.Transform:
				TransformRotate();
				break;
			case RotationType.TransformQuaternion:
				TransformQuaternoinRotate();
				break;
			case RotationType.Rigidbody:
				RigigidbodyRotate();
				break;
			default:
				break;
		}
	}

    private void TransformRotate()
	{
		transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
	}

	private void TransformQuaternoinRotate()
	{

	}

	private void RigigidbodyRotate()
	{
		
	}
}
