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
    public CellType type;
    public double creationTimestamp;
    public Cell targetCell;

    protected void Start()
    {
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

    public void Despawn()
    {
        // ...
    }
}
