using UnityEngine;
using System.Collections;

public class Attach : MonoBehaviour
{
    public Transform ObjectGenerated;
    public Transform explosionPrefab;
    public string targetTag;
    

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == targetTag)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);
            Instantiate(ObjectGenerated, collision.transform.position, collision.transform.rotation);
        }
    }
}