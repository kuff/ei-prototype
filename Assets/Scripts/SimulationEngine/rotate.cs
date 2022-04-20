using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float rpmX;
    public float rpmY;
    public float rpmZ;
    public bool Rotating = true;

    void Start()
    {
        rpmX = Random.Range(0f, 7f);
        rpmY = Random.Range(0f, 7f);
        rpmZ = Random.Range(0f, 7f);
    }

    void Update()
    {
        if (Rotating == true)
        {
            transform.Rotate(6.0f * rpmX * Time.deltaTime, 6.0f * rpmY * Time.deltaTime, 6.0f * rpmZ * Time.deltaTime);
        }
    }
}
