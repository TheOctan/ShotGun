using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotationOption : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 100f;
	[SerializeField] private InterpolationType interpolation;

    private Rigidbody rigidbodyComponent;

    private void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    private void Update()
    {
		transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
	}
}