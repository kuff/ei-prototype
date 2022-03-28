using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]

public class Target : MonoBehaviour
{
    public int targetType = 1;
    public UnityEvent OnHit;

    private GunScript gun;
    private void Awake()
    {
        gun = GameObject.Find("Gun").GetComponent<GunScript>();
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoxCollider>().size = new Vector3(5,5,5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile" && targetType == gun.gunType)
        {
            OnHit.Invoke();
            StartCoroutine(collision.gameObject.GetComponent<ProjectileScript>().kill(2));
        }
    }

    public void HitTest()
    {
        Debug.Log("Hit!");
    }

}
