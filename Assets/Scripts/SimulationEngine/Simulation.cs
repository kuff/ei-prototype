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

public class Simulation : MonoBehaviour
{
    public int WBCsSpawned;
    public int PathogensSpawned;
    public int AntibodiesSpawned;
    public int WBCsDestroyed;
    public int PathogensDestroyed;
    public int AntibodiesDestroyed;
    public int WBCCount;
    public int PathogenCount;
    public int AntibodyCount;
    public int collisionCount;

    public WBCSpawnEvent        OnWBCSpawn          = new WBCSpawnEvent();
    public PathogenSpawnEvent   OnPathogenSpawn     = new PathogenSpawnEvent();
    public AntibodySpawnEvent   OnAntibodySpawn     = new AntibodySpawnEvent();
    public WBCDespawnEvent      OnWBCDespawn        = new WBCDespawnEvent();
    public PathogenDespawnEvent OnPathgenDespawn    = new PathogenDespawnEvent();
    public AntibodyDespawnEvent OnAntibodyDespawn   = new AntibodyDespawnEvent();
    public CollisionEvent       OnCollision         = new CollisionEvent();

    private List<Cell> cells;
    private bool allowCollisions;

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

    public void SpawnWBCCells(int amount)
    {
        // ...
    }

    public void SpawnPathogenCells(int amount)
    {
        // ...
    }

    public void SpawnAntibodyCells(int amount)
    {
        // ...
    }

    public void SpawnFillerCells(int amount)
    {
        // ...
    }

    public void DespawnWBCCells(int amount = 0)
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

    public void DespawnWBCCells(Cell[] cells)
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
                break;
            case CellType.Pathogen:
                // TODO: spawn Pathogen Cell prefab
                newCellObject = new GameObject();
                break;
            case CellType.Antibody:
                // TODO: spawn Antibody Cell prefab
                newCellObject = new GameObject();
                break;
            default:
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
    }
}
