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
    Filler,  // one filler type despite several different models, because the system does not need to distinguish between different models of filler Cells
    Vaccine,
}
[RequireComponent(typeof(AudioSource))]
public class Cell : MonoBehaviour
{
    private Simulation simulation;
    private Cell collisionCell;
    private AudioSource audioSource;
    public CellType type;
    public double creationTimestamp;
    public Transform targetTransform;
    public Transform sparklesPrefab;
    public Transform explosionPrefab;
    public Transform destructionPrefab;
    public Transform spawnPrefab;
    public Transform despawnPrefab;
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
            this.gameObject.transform.localScale = new Vector3(0, 0, 0);
            PlayDestructionAnimation();
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(destructionSound, 0.7f);
        }

        IEnumerator Destruction()
        {
            yield return new WaitForSeconds(6);
            simulation.DespawnCell(this.gameObject, false);
        }
        
        if (this.type == CellType.PathogenNeutralized)
        {
            StartCoroutine(DestructionEffect());
            StartCoroutine(Destruction());
        }
        else if (this.type != CellType.Antibody)
        {
            this.SpawnElements(this.transform, spawnPrefab, null, explosionSound, 0f); // trigger spawning animation
        }
    }
    
    public void Tick(Vector3 gravityVector)
    {
        // apply inter-Cell gravitational force
        this.gameObject.GetComponentInChildren<Rigidbody>()?.AddForce(gravityVector);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (this.simulation == null) return;
        if (simulation.CollisionsAllowed()) { 
            collisionCell = collision.gameObject.GetComponentInChildren<Cell>() ?? collision.gameObject.GetComponent<Cell>();

            if (collisionCell?.type == CellType.Pathogen)
            {
                if (this.type == CellType.WhiteBloodCell)
                {
                    bool applyForDrop = simulation.ApplyForDrop(this);
                    if (applyForDrop == true) {
                        SpawnElements(sparklesPrefab, collision, sparklesSound, 0.7f, true);
                    }
                    else
                    {
                        SpawnElements(this.transform, sparklesPrefab, collision, sparklesSound, 0.7f);
                    }
                }
                else if (this.type == CellType.Antibody)
                {
                    SpawnElements(explosionPrefab, collision, explosionSound, 0.7f, false);
                    
                    CellType ct = collision.gameObject.GetComponentInChildren<Cell>().type;
                    this.simulation.DespawnCell(this, true);
                    this.simulation.DespawnCell(collision.gameObject.GetComponentInChildren<Cell>() ?? null, true);
                }
            }
            
            else if (collisionCell?.type == CellType.Vaccine)
            {
                if (this.type == CellType.WhiteBloodCell)
                {
                    bool applyForDrop = simulation.ApplyForDrop(this);
                    if (applyForDrop == true) {
                        for (int i = 0; i < 4; i++)
                        {
                            SpawnElements(sparklesPrefab, collision, sparklesSound, 0.5f, true);
                        }
                        simulation.DespawnCell(collisionCell.gameObject, false);
                    }
                    else
                    {
                        SpawnElements(this.transform, sparklesPrefab, collision, sparklesSound, 0.7f);
                    }
                }
            }
            
            this.simulation.OnCollision.Invoke(new Scenario());  // TODO: define the API for this...
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

        //Debug.Log(vectMagnitude);

        float spacing = vectMagnitude / 50f;
        Vector3 position = center - (direction * spacing);
        position.y += Random.Range(-0.5f, 0.5f);
        position.x += Random.Range(-0.5f, 0.5f);
        position.z += Random.Range(-0.5f, 0.5f);

        // spawn the new Cell differently depending on what we are colliding with
        // either replaces the object or spawns the new Cell around it
        Transform newInstance;
        if (spawnAroundObject == true)
            newInstance = this.simulation.SpawnCell(CellType.Antibody, position, Quaternion.identity)?.transform;
        else
            newInstance = this.simulation.SpawnCell(CellType.PathogenNeutralized, collision.transform.position, Quaternion.identity)?.transform;

        // play the effects
        //if (newInstance.GetComponentInChildren<Cell>().type != CellType.Antibody)
        this.SpawnElements(newInstance, effect, collision, sound, volume);
    }

    /*
     * Spawn particle effect and play audio, but no Cell spawning
     */
    private void SpawnElements(Transform instance, Transform effect, [CanBeNull] Collision collision, AudioClip sound, float volume)
    {
        // play the effects
        var position = collision != null ? collision.transform.position : this.gameObject.transform.position;
        var rotation = collision != null ? collision.transform.rotation : this.gameObject.transform.rotation;
        Transform generatedEffect = Instantiate(effect, position, rotation);
        instance.gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(sound, volume);
        Destroy(generatedEffect.gameObject, 1);
    }

    public void PlayDespawnAnimation()
    {
        Transform generatedEffect = Instantiate(despawnPrefab, this.transform.position, this.transform.rotation);
        Destroy(generatedEffect.gameObject, 1);  // destroy effect after 1 second
    }

    public void PlayDestructionAnimation()
    {
        Transform generatedEffect = Instantiate(destructionPrefab, this.transform.position, this.transform.rotation);
        Destroy(generatedEffect.gameObject, 1);  // destroy effect after 1 second
    }
}