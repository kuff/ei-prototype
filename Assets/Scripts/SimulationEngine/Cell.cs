using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    WhiteBloodCell,
    Pathogen,
    Antibody,
    Filler,         // one filler type despite several different models, because the system does not need to distinguish between different models of filler Cells
}
[RequireComponent(typeof(AudioSource))]
public class Cell : MonoBehaviour
{
    private Simulation simulation;
    public CellType type;
    public double creationTimestamp;
    public Cell targetCell;
    private Cell collisionCell;
    public Transform sparklesPrefab;
    public Transform explosionPrefab;
    public Transform ObjectGenerated;
    public Transform Antibody;
    private AudioSource audioSource;
    public AudioClip explosionSound;
    public AudioClip sparklesSound;

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
                    //bool application = simulation.ApplyForDrop(this);
                    //if (application == true)
                    //{
                    Debug.Log("WhiteCell");
                        SpawnElements(Antibody, sparklesPrefab, collision, sparklesSound, 0.7f, false, false);
                    //}
                }

                else if (this.type == CellType.Antibody)
                {
                    Debug.Log("Antibody");
                    SpawnElements(ObjectGenerated, explosionPrefab, collision, explosionSound, 0.7f, true, true);
                }
                //else 
                // do something on other collisions
            }
        }
    }

    public void SpawnElements(Transform Element, Transform Effect, Collision collision, AudioClip sound, float volume, bool destroySelf, bool destroyOther)
    {
        if (destroySelf == true)
        {
            Destroy(this.gameObject);
        }

        if (destroyOther == true)
        {
            Destroy(collision.gameObject);
        }

        Transform newInstance = Instantiate(Element, collision.transform.position, collision.transform.rotation);
        Transform generatedEffect = Instantiate(Effect, collision.transform.position, collision.transform.rotation);
        newInstance.gameObject.GetComponent<AudioSource>().PlayOneShot(sound, volume);
    }

    public void Despawn()
    {
        // ...
    }
}
        