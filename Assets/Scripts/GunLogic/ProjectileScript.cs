using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private GameObject impactPrefab;
    void Awake()
    {
        StartCoroutine(kill(8));
    }

    public IEnumerator kill(float T)
    {     
        yield return new WaitForSeconds(T);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);

        GameObject impact;
        impact = Instantiate(impactPrefab,contact.point, Quaternion.LookRotation(contact.normal));
        
    }
}
