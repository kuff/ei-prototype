using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(kill(8));
    }

    public IEnumerator kill(float T)
    {     
        yield return new WaitForSeconds(T);
        Destroy(gameObject);
    }
}
