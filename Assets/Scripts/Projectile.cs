using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 10f;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
