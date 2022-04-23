using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScript : MonoBehaviour
{
    public bool isLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        Debug.Log(other.name);
        if(other.tag != "Projectile" || isLoaded)return;
        else
        {
            isLoaded = true;
            Destroy(other.gameObject);    
        }

    }

}
