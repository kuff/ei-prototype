using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class FirstCollisionEvent : UnityEvent<Scenario> { }
[Serializable]
public class FirstPlayerPickupEvent : UnityEvent<Scenario> { }
[Serializable]
public class FirstPlayerReloadEvent : UnityEvent<Scenario> { }
[Serializable]
public class FirstPlayerShootEvent : UnityEvent<Scenario> { }
[Serializable]
public class FirstAntibodyInfusionEvent : UnityEvent<Scenario> { }
[Serializable]
public class PathogensReducedToEvent : UnityEvent<Scenario> { }
[Serializable]
public class AllPathogensDestroyedEvent : UnityEvent<Scenario> { }
[Serializable]
public class FirstPlayerPathogenKillEvent : UnityEvent<Scenario> { }
[Serializable]
public class PathogenDestroyedEvent : UnityEvent<Scenario> { }
[Serializable]
public class VaccinesDestroyedEvent : UnityEvent<Scenario> { }

public class Scenario : Level
{
    public FirstCollisionEvent                  OnFirstCollision                    = new FirstCollisionEvent();
    public FirstPlayerPickupEvent               OnFirstPlayerPickup                 = new FirstPlayerPickupEvent();
    public FirstPlayerReloadEvent               OnFirstPlayerReload                 = new FirstPlayerReloadEvent();
    public FirstPlayerShootEvent                OnFirstPlayerShot                   = new FirstPlayerShootEvent();
    public FirstAntibodyInfusionEvent           OnFirstAntibodyInfusion             = new FirstAntibodyInfusionEvent();
    [FormerlySerializedAs("OnPathogensReducedToFiftyPercent")] public PathogensReducedToEvent  onPathogensReducedEvent    = new PathogensReducedToEvent();
    public AllPathogensDestroyedEvent           OnAllPathogensDestroyed             = new AllPathogensDestroyedEvent();
    public FirstPlayerPathogenKillEvent         OnFirstPlayerPathogenKill           = new FirstPlayerPathogenKillEvent();
    public PathogenDestroyedEvent               OnPathogenDestroyed                 = new PathogenDestroyedEvent();
    public VaccinesDestroyedEvent               OnVaccinesDestroyed                 = new VaccinesDestroyedEvent();

    public bool allowUpdateTicks;  // TODO: probably set this to false eventually...
    public float tickRate;
    
    private Simulation simulation;
    private bool hasReachFiftyPercent;
    private double lastUpdateTimestamp;
    private bool hasShot;
    private bool hasInfused;
    private bool vaccinesDestroyedCalled;

    protected new void Start()
    {
        this.simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
        this.hasReachFiftyPercent = false;
        this.lastUpdateTimestamp = 0;
        this.vaccinesDestroyedCalled = false;

        // hook in to Simulation
        this.simulation.OnCollision     .AddListener(this.HandleFirstCollision);
        this.simulation.OnPickup        .AddListener(this.HandleFirstPickup);
        this.simulation.OnReload        .AddListener(this.HandleFirstReload);
        this.simulation.OnShot          .AddListener(this.HandleFirstShot);
        this.simulation.OnPathgenDespawn.AddListener(this.HandleFirstAntibodyInfusion);
        this.simulation.OnPathgenDespawn.AddListener(this.HandlePathogensReduced);
        this.simulation.OnCollision     .AddListener(this.HandleAntibodyRespawn);

        // define logic for first Player Pathogen kill event
        this.hasShot = false;
        this.hasInfused = false;
        this.OnFirstAntibodyInfusion.AddListener(s => this.hasInfused = true);
        this.OnFirstPlayerShot.AddListener(s => this.hasShot = true);
        this.simulation.OnPathgenDespawn.AddListener(s =>
        {
            if (this.isActive && this.hasInfused && this.hasShot && this.simulation.PathogensDestroyed == 1) 
                this.OnFirstPlayerPathogenKill.Invoke(this);
        });
        
        // all pathogens destroyed logic
        this.simulation.OnCollision.AddListener(s =>
        {
            Debug.Log(this.simulation.PathogensSpawned);
            if (this.isActive && this.hasReachFiftyPercent && this.simulation.PathogensSpawned > 0 && this.simulation.PathogenCount == 0)
            {
                Debug.Log("Pathogens destroyed in " + this.gameObject.name);
                this.OnAllPathogensDestroyed.Invoke(this);
            }
        });
        
        // all vaccines destroyed logic
        this.simulation.OnCollision.AddListener(s =>
        {
            if (!this.vaccinesDestroyedCalled && this.simulation.VaccineCount == 0 && this.simulation.VaccinesDestroyed > 0)
            {
                this.OnVaccinesDestroyed.Invoke(this);
                this.vaccinesDestroyedCalled = true;
            }
        });
        
        base.Start();
    }

    private void HandleFirstCollision(Scenario s)
    {
        if (this.isActive && this.simulation.collisionCount == 1)
            this.OnFirstCollision.Invoke(s);
    }

    private void HandleFirstAntibodyInfusion(Scenario s)
    {
        if (this.isActive && this.simulation.PathogensDestroyed == 1)
            this.OnFirstAntibodyInfusion.Invoke(s);
    }

    private void HandlePathogensReduced(Scenario s)
    {
        //Debug.Log(this.simulation.PathogensDestroyed + ", " + this.simulation.PathogenCount);
        if (this.isActive && !this.hasReachFiftyPercent && simulation.PathogensDestroyed >= simulation.PathogenCount)
        {
            this.hasReachFiftyPercent = true;
            this.onPathogensReducedEvent.Invoke(s);
        }
    }

    private void HandleFirstPickup(Scenario s)
    {
        if (this.isActive && this.simulation.AntibodyPickupCount == 1)
            this.OnFirstPlayerPickup.Invoke(s);
    }

    private void HandleFirstReload(Scenario s)
    {
        if (this.isActive && this.simulation.gunReloadCount == 1)
            this.OnFirstPlayerReload.Invoke(s);
    }

    private void HandleFirstShot(Scenario s)
    {
        if (this.isActive && this.simulation.gunShotCount == 1)
            this.OnFirstPlayerShot.Invoke(s);
    }

    private void HandleAntibodyRespawn(Scenario s)
    {
        if (this.isActive)
            this.OnPathogenDestroyed.Invoke(this);
    }

    /*protected void Update()
    {
        // ...
    }*/

    protected void FixedUpdate()
    {
        var newTimestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        //Debug.Log(newTimestamp - this.lastUpdateTimestamp);
        if (newTimestamp - this.lastUpdateTimestamp >= this.tickRate)
        {
            this.lastUpdateTimestamp = newTimestamp;
            this.Tick();
        }
    }

    protected void Tick()
    {
        //Debug.Log(this.isActive + ", " + this.allowUpdateTicks);
        if (this.isActive && this.allowUpdateTicks) 
            simulation.Tick();
    }

    public new void Activate()
    {
        base.Activate();

        // TODO: set up simulation
    }

    public new void Complete()
    {
        this.simulation.ClearCount();

        base.Complete();
    }

    public void StartTicks()
    {
        this.allowUpdateTicks = true;
    }
    
    public void StopTicks()
    {
        this.allowUpdateTicks = false;
    }

    public void SpawnTutorialPathogen()
    {
        this.simulation.SpawnCell(CellType.Pathogen, new Vector3(-8f, 1f, 0f), Quaternion.identity);
    }

    public void SpawnTutorialAntibody()
    {
        this.simulation.SpawnCell(CellType.Antibody, new Vector3(-8f, 2f, 0f), Quaternion.identity);
    }

    public void Reset()
    {
        this.simulation.ClearCount();
        
        this.hasReachFiftyPercent = false;
        this.hasShot = false;
        this.hasInfused = false;
    }
}
