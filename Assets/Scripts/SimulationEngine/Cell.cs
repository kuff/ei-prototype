using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    WhiteBloodCell,
    Pathogen,
    PathogenNeutralized,
    Antibody,
    Filler, // one filler type despite several different models, because the system does not need to distinguish between different models of filler Cells
}
[RequireComponent(typeof(AudioSource))]
public class Cell : MonoBehaviour
{
    private Simulation simulation;
    private Cell collisionCell;
    private AudioSource audioSource;

    public CellType type;
    public double creationTimestamp;
    public Cell targetCell;
    public Transform sparklesPrefab;
    public Transform explosionPrefab;
    public AudioClip explosionSound;
    public AudioClip sparklesSound;
    public Transform ObjectGenerated;
    public Transform Antibody;
    public GameObject antibodyTarget;

    protected void Start()
    {
        simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
        simulation.SetAllowCollisions(true); //for testing purposes

        // TODO: should trigger spawning animation...
    }

    /*protected void Update()
    {
        // ...
    }

    protected void FixedUpdate()
    {
        // ...
    }*/

    public void Tick()
    {
        // ...
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (simulation.CollisionsAllowed()) { 
            collisionCell = collision.gameObject.GetComponent<Cell>();

            if (collisionCell.type == CellType.Pathogen)
            {
                if (this.type == CellType.WhiteBloodCell)
                {
                    bool applyForDrop = simulation.ApplyForDrop(this);
                    if (applyForDrop == true) {
                        Debug.Log("WhiteCell");
                        SpawnElements(Antibody, sparklesPrefab, collision, sparklesSound, 0.7f, false, true);
                    }
                    else
                    {
                        SpawnElements(sparklesPrefab, collision, sparklesSound, 0.7f, false);
                    }
                }
                else if (this.type == CellType.Antibody)
                {
                    Debug.Log("Antibody");
                    SpawnElements(ObjectGenerated, explosionPrefab, collision, explosionSound, 0.7f, true, false);
                }
                // TODO: do something on other collisions...
            }
        }
    }

    public void SpawnElements(Transform Element, Transform Effect, Collision collision, AudioClip sound, float volume, bool destroyGameObjects, bool spawnAroundObject)
    {
        if (destroyGameObjects == true)
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }

        Vector3 center = collision.transform.position;

        Vector3 RandomCircle(Vector3 center, float radius)
        {
            float ang = Random.value * 360;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            pos.z = center.z + Random.Range(-1.5f, 1.5f);
            return pos;
        }

        Vector3 pos = RandomCircle(center, 2f);
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
        Transform newInstance;

        if (spawnAroundObject == true)
        {
            newInstance = Instantiate(Element, pos, rot);
        }

        else
        {
            newInstance = Instantiate(Element, collision.transform.position, collision.transform.rotation);
        }

        Transform generatedEffect = Instantiate(Effect, collision.transform.position, collision.transform.rotation);
        newInstance.gameObject.GetComponent<AudioSource>().PlayOneShot(sound, volume);
        Destroy(generatedEffect.gameObject, 1);
    }

    public void SpawnElements(Transform Effect, Collision collision, AudioClip sound, float volume, bool destroyGameObjects)
    {
        if (destroyGameObjects == true)
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }

        Transform generatedEffect = Instantiate(Effect, collision.transform.position, collision.transform.rotation);
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(sound, volume);
        Destroy(generatedEffect.gameObject, 1);
    }

    public void Despawn()
    {
        // ...
    }
}