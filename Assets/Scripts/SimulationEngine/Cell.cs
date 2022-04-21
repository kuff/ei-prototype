using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    public Transform destructionPrefab;
    public AudioClip explosionSound;
    public AudioClip sparklesSound;
    public AudioClip destructionSound;
    public Transform ObjectGenerated;
    public Transform Antibody;
    public GameObject antibodyTarget;

    protected void Start()
    {
        simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
        simulation.SetAllowCollisions(true);  // for testing purposes

        // self destruct logic for Neutralized Pathogen Cells
        IEnumerator DestructionEffect()
        {
            yield return new WaitForSeconds(3);
            Transform generatedEffect = Instantiate(destructionPrefab, this.transform.position, this.transform.rotation);
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(destructionSound, 0.7f);
            Destroy(this.gameObject.transform.GetChild(0).gameObject);
            Destroy(generatedEffect.gameObject, 1);  // destroy effect after 1 second
        }

        IEnumerator Destruction()
        {
            yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);
        }
        
        if (this.type == CellType.PathogenNeutralized)
        {
            StartCoroutine(DestructionEffect());
            StartCoroutine(Destruction());
        }
        else
        {
            this.SpawnElements(this.transform, sparklesPrefab, null, explosionSound, 0.2f); // trigger spawning animation
        }
    }
    
    public void Tick()
    {
        // ...
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (simulation.CollisionsAllowed()) { 
            collisionCell = collision.gameObject.GetComponent<Cell>();

            if (collisionCell?.type == CellType.Pathogen)
            {
                if (this.type == CellType.WhiteBloodCell)
                {
                    bool applyForDrop = simulation.ApplyForDrop(this);
                    if (applyForDrop == true) {
                        Debug.Log("WhiteCell");
                        SpawnElements(sparklesPrefab, collision, sparklesSound, 0.7f, true);
                    }
                    else
                    {
                        SpawnElements(this.transform, sparklesPrefab, collision, sparklesSound, 0.7f);
                    }
                }
                else if (this.type == CellType.Antibody)
                {
                    Debug.Log("Antibody");
                    SpawnElements(explosionPrefab, collision, explosionSound, 0.7f, false);
                    
                    this.simulation.DespawnCell(this);
                    this.simulation.DespawnCell(collision.gameObject.GetComponent<Cell>() ?? null);
                }
                
                this.simulation.OnCollision.Invoke(new Scenario());  // TODO: define the API for this...
            }
        }
    }

    /*
     * Spawn Cells and play audio and particle effects
     */
    public void SpawnElements(Transform effect, Collision collision, AudioClip sound, float volume, bool spawnAroundObject)
    {
        // define point of collision, where we want particles to spawn
        Vector3 center = collision.transform.position;
        Vector3 player = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 direction = center - player;
        float vectMagnitude = direction.magnitude;

        Debug.Log(vectMagnitude);

        float spacing = vectMagnitude / 50f;
        Vector3 position = center - (direction * spacing);
        position.y += Random.Range(-0.5f, 0.5f);
        position.x += Random.Range(-0.5f, 0.5f);
        position.z += Random.Range(-0.5f, 0.5f);


        // find a random area around the center to avoid spawning inside a Cell and look more organic
        /*Vector3 RandomCircle(Vector3 center, float radius)
        {
            float ang = Random.value * 360;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            pos.z = center.z + Random.Range(-1.5f, 1.5f);
            return pos;
        }
        Vector3 pos = RandomCircle(center, 1f);
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);*/

        // spawn the new Cell differently depending on what we are colliding with
        // either replaces the object or spawns the new Cell around it
        Transform newInstance;
        if (spawnAroundObject == true)
            newInstance = this.simulation.SpawnCell(CellType.Antibody, position)?.transform;
        else
            newInstance = this.simulation.SpawnCell(CellType.PathogenNeutralized, collision.transform.position)?.transform;

        // play the effects
        this.SpawnElements(newInstance, effect, collision, sound, volume);
    }

    /*
     * Spawn particle effect and play audio, but no Cell spawning
     */
    private void SpawnElements([CanBeNull] Transform instance, Transform effect, [CanBeNull] Collision collision, AudioClip sound, float volume)
    {
        // play the effects
        var position = collision != null ? collision.transform.position : this.gameObject.transform.position;
        var rotation = collision != null ? collision.transform.rotation : this.gameObject.transform.rotation;
        Transform generatedEffect = Instantiate(effect, position, rotation);
        instance?.gameObject.GetComponent<AudioSource>().PlayOneShot(sound, volume);
        Destroy(generatedEffect.gameObject, 1);
    }

    public void PlayDespawnAnimation()
    {
        Transform generatedEffect = Instantiate(destructionPrefab, this.transform.position, this.transform.rotation);
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(destructionSound, 0.7f);
        Destroy(generatedEffect.gameObject, 1);  // destroy effect after 1 second
    }
}