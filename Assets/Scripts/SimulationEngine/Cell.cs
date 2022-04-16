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


    protected void Start()
    {
        //simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
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

    void OnCollisionEnter(Collision collision)
    {
        collisionCell = collision.gameObject.GetComponent<Cell>();
        if (collisionCell.type == CellType.Pathogen)
        {
            if (this.type == CellType.WhiteBloodCell)
            {
                //bool application = simulation.ApplyForDrop(this);
                Instantiate(sparklesPrefab, collision.transform.position, collision.transform.rotation);
                /*if (application == true)
                {
                    Instantiate(Antibody, collision.transform.position, collision.transform.rotation);
                }*/
            }

            else if (this.type == CellType.Antibody)
            {
                Destroy(this.gameObject);
                Destroy(collision.gameObject);
                Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);
                Instantiate(ObjectGenerated, collision.transform.position, collision.transform.rotation);
            }
            //else 
            // do something on other collisions
        }
    }

    public void Despawn()
    {
        // ...
    }
}
        