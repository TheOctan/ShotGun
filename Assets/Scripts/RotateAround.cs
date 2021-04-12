using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 5;
    [SerializeField] private bool isClockwise = true;

    private void Update()
    {
        transform.Rotate(Vector3.up * (isClockwise ? turnSpeed : -turnSpeed) * Time.deltaTime);
    }
}
