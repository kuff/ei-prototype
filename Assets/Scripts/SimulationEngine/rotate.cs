using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float rpmX = 10f;
    public float rpmY = 10f;
    public float rpmZ = 10f;
    public bool Rotating = true;

    void Update()
    {
        if (Rotating == true)
        {
            transform.Rotate(6.0f * rpmX * Time.deltaTime, 6.0f * rpmY * Time.deltaTime, 6.0f * rpmZ * Time.deltaTime);
        }
    }
}
