using System;
using UnityEngine;
using UnityEngine.Events;

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
public class PathogensReducedToFiftyPercentEvent : UnityEvent<Scenario> { }

public class Scenario : Level
{
    public FirstCollisionEvent                  OnFirstCollision                    = new FirstCollisionEvent();
    public FirstPlayerPickupEvent               OnFirstPlayerPickup                 = new FirstPlayerPickupEvent();
    public FirstPlayerReloadEvent               OnFirstPlayerReload                 = new FirstPlayerReloadEvent();
    public FirstPlayerShootEvent                OnFirstPlayerShot                   = new FirstPlayerShootEvent();
    public FirstAntibodyInfusionEvent           OnFirstAntibodyInfusion             = new FirstAntibodyInfusionEvent();
    public PathogensReducedToFiftyPercentEvent  OnPathogensReducedToFiftyPercent    = new PathogensReducedToFiftyPercentEvent();

    public bool allowUpdateTicks;  // TODO: probably set this to false eventually...
    public float tickRate;
    
    private Simulation simulation;
    private bool hasReachFiftyPercent;
    private double lastUpdateTimestamp;

    protected new void Start()
    {
        this.simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
        this.hasReachFiftyPercent = false;
        this.lastUpdateTimestamp = 0;

        this.simulation.OnCollision     .AddListener(this.HandleFirstCollision);
        this.simulation.OnPickup        .AddListener(this.HandleFirstPickup);
        this.simulation.OnReload        .AddListener(this.HandleFirstReload);
        this.simulation.OnShot          .AddListener(this.HandleFirstShot);
        this.simulation.OnPathgenDespawn.AddListener(HandleFirstAntibodyInfusion);
        this.simulation.OnPathgenDespawn.AddListener(HandlePathogensReducedToFiftyPercent);

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

    private void HandlePathogensReducedToFiftyPercent(Scenario s)
    {
        if (this.isActive && !this.hasReachFiftyPercent && simulation.PathogensDestroyed >= simulation.PathogenCount)
        {
            this.hasReachFiftyPercent = true;
            this.OnPathogensReducedToFiftyPercent.Invoke(s);
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
}
