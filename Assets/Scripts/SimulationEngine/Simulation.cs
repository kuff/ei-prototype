using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class WBCSpawnEvent : UnityEvent<Scenario> { }
[Serializable]
public class PathogenSpawnEvent : UnityEvent<Scenario> { }
[Serializable]
public class AntibodySpawnEvent : UnityEvent<Scenario> { }
[Serializable]
public class WBCDespawnEvent : UnityEvent<Scenario> { }
[Serializable]
public class PathogenDespawnEvent : UnityEvent<Scenario> { }
[Serializable]
public class AntibodyDespawnEvent : UnityEvent<Scenario> { }
[Serializable]
public class CollisionEvent : UnityEvent<Scenario> { }
[Serializable]
public class PlayerPickupEvent : UnityEvent<Scenario> { }
[Serializable]
public class PlayerReloadEvent : UnityEvent<Scenario> { }
[Serializable]
public class PlayerShootEvent : UnityEvent<Scenario> { }

public class Simulation : MonoBehaviour
{
    [HideInInspector]
    public int WBCsSpawned;
    [HideInInspector]
    public int PathogensSpawned;
    [HideInInspector]
    public int AntibodiesSpawned;
    [HideInInspector]
    public int WBCsDestroyed;
    [HideInInspector]
    public int PathogensDestroyed;
    [HideInInspector]
    public int AntibodiesDestroyed;
    [HideInInspector]
    public int WBCCount;
    [HideInInspector]
    public int PathogenCount;
    [HideInInspector]
    public int AntibodyCount;
    [HideInInspector]
    public int collisionCount;
    [HideInInspector]
    public int AntibodyPickupCount;
    [HideInInspector]
    public int gunReloadCount;
    [HideInInspector]
    public int gunShotCount;

    public WBCSpawnEvent        OnWBCSpawn          = new WBCSpawnEvent();
    public PathogenSpawnEvent   OnPathogenSpawn     = new PathogenSpawnEvent();
    public AntibodySpawnEvent   OnAntibodySpawn     = new AntibodySpawnEvent();
    public WBCDespawnEvent      OnWBCDespawn        = new WBCDespawnEvent();
    public PathogenDespawnEvent OnPathgenDespawn    = new PathogenDespawnEvent();
    public AntibodyDespawnEvent OnAntibodyDespawn   = new AntibodyDespawnEvent();
    public CollisionEvent       OnCollision         = new CollisionEvent();
    public PlayerPickupEvent    OnPickup            = new PlayerPickupEvent();
    public PlayerReloadEvent    OnReload            = new PlayerReloadEvent();
    public PlayerShootEvent     OnShot              = new PlayerShootEvent();

    private List<Cell> cells = new List<Cell>();
    private bool allowCollisions = true;  // TODO: implement the logic for this...


    //Spawn/Despawn variables
    public float WBCRadiusMax, WBCRadiusMin;
    public float PathogenRadiusMax, PathogenRadiusMin;
    public float AntibodyRadiusMax, AntibodyRadiusMin;
    [Range(0.1f, 1f)]
    public float ceiling;

    protected void Start()
    {
        this.OnPickup   .AddListener(s => this.AntibodyPickupCount++);
        this.OnReload   .AddListener(s => this.gunReloadCount++);
        this.OnShot     .AddListener(s => this.gunShotCount++);
    }


    public void Tick()
    {
        foreach (Cell c in this.cells)
            c.Tick();

        // ...
    }

    public void ClearCount()
    {
        this.WBCsSpawned = 0;
        this.PathogensSpawned = 0;
        this.AntibodiesSpawned = 0;
        this.WBCsDestroyed = 0;
        this.PathogensDestroyed = 0;
        this.AntibodiesDestroyed = 0;
        this.WBCCount = 0;
        this.PathogenCount = 0;
        this.AntibodyCount = 0;
        this.collisionCount = 0;
    }

    public bool CollisionsAllowed()
    {
        return this.allowCollisions;
    }

    public void SetAllowCollisions(bool isAllowed)
    {
        this.allowCollisions = isAllowed;
    }

    public bool ApplyForDrop(Cell applicant)
    {
        // ...

        return false;
    }

    public void SpawnWBCells(int amount)
    {
       Vector3 playerPos = GameObject.Find("Player").transform.position;
       for (int i = 0; i < amount; i++)
        {
            Vector3 randpoint = UnityEngine.Random.insideUnitSphere.normalized;
            Vector3 spawnPos = playerPos + new Vector3(randpoint.x, Mathf.Abs(randpoint.y), randpoint.z) * UnityEngine.Random.Range(WBCRadiusMin, WBCRadiusMax);
            SpawnCell(CellType.WhiteBloodCell,spawnPos);
        }
    }

    public void SpawnPathogenCells(int amount, bool neutralized = false)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randpoint = UnityEngine.Random.insideUnitSphere.normalized;
            Vector3 spawnPos = Vector3.zero + new Vector3(randpoint.x, Mathf.Abs(randpoint.y), randpoint.z) * UnityEngine.Random.Range(PathogenRadiusMin, PathogenRadiusMax);
            SpawnCell(CellType.Pathogen, spawnPos);
        }
    }

    public void SpawnAntibodyCells(int amount)
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        for (int i = 0; i < amount; i++)
        {
            Vector3 randpoint = UnityEngine.Random.insideUnitSphere.normalized;
            Vector3 spawnPos = playerPos + new Vector3(randpoint.x, Mathf.Abs(randpoint.y * 0.4f), randpoint.z) * UnityEngine.Random.Range(AntibodyRadiusMin, AntibodyRadiusMax);
            SpawnCell(CellType.Antibody, spawnPos);
        }
    }

    public void SpawnFillerCells(int amount)
    {
        // ...
    }

    public void DespawnWBCells(int amount = 0)
    {
        // ...
    }

    public void DespawnPathogenCells(int amount = 0)
    {
        // ...
    }

    public void DespawnAntibodyCells(int amount = 0)
    {
        // ...
    }

    public void DespawnFillerCells(int amount = 0)
    {
        // ...
    }

    public void DespawnWBCells(Cell[] cells)
    {
        // ...
    }

    public void DespawnPathogenCells(Cell[] cells)
    {
        // ...
    }

    public void DespawnAntibodyCells(Cell[] cells)
    {
        // ...
    }

    public void DespawnFillerCells(Cell[] cells)
    {
        // ...
    }

    private void SpawnCell(CellType type, Vector3 position)
    {
        GameObject newCellObject;

        switch(type) {

            case CellType.WhiteBloodCell:
                // TODO: spawn White Blood Cell prefab
                newCellObject = new GameObject();

                this.WBCsSpawned++;
                this.WBCCount++;
                this.OnWBCSpawn.Invoke(new Scenario());  // TODO: replace Scenario placeholder
                break;                                   // API will probably change here...

            case CellType.Pathogen:
                // TODO: spawn Pathogen Cell prefab
                newCellObject = new GameObject();

                this.PathogensSpawned++;
                this.PathogenCount++;
                this.OnPathogenSpawn.Invoke(new Scenario());
                break;

            case CellType.PathogenNeutralized:
                // TODO: spawn NeutralizedPathogen Cell prefab
                newCellObject = new GameObject();

                this.PathogensSpawned++;
                this.PathogenCount++;
                this.OnPathogenSpawn.Invoke(new Scenario());
                break;

            case CellType.Antibody:
                // TODO: spawn Antibody Cell prefab
                newCellObject = new GameObject();

                this.AntibodiesSpawned++;
                this.AntibodyCount++;
                this.OnAntibodySpawn.Invoke(new Scenario());
                break;

            default:  // CellType.Filler
                // TODO: randomly select and spawn Filler Cell prefab
                newCellObject = new GameObject();
                break;
        }

        cells.Add(newCellObject.GetComponent<Cell>());
    }

    private void DespawnCell(Cell cell)
    {
        cell.Despawn();
        this.cells.Remove(cell);

        switch (cell.type)
        {
            case CellType.WhiteBloodCell:
                this.WBCCount--;
                this.WBCsDestroyed++;
                this.OnWBCDespawn.Invoke(new Scenario());
                break;

            case CellType.Pathogen:
            case CellType.PathogenNeutralized:
                this.PathogenCount--;
                this.PathogensDestroyed++;
                this.OnPathgenDespawn.Invoke(new Scenario());
                break;

            case CellType.Antibody:
                this.AntibodyCount--;
                this.AntibodiesDestroyed++;
                this.OnAntibodyDespawn.Invoke(new Scenario());
                break;

            // Filler Cells don't have these events so we don't need to catch them here
        }
    }
}
