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
public class FirstAntibodyInfusionEvent : UnityEvent<Scenario> { }
[Serializable]
public class PathogensReducedToFiftyPercentEvent : UnityEvent<Scenario> { }

public class Scenario : Level
{
    public FirstCollisionEvent                  OnFirstCollision                    = new FirstCollisionEvent();
    public FirstPlayerPickupEvent               OnFirstPlayerPickup                 = new FirstPlayerPickupEvent();
    public FirstPlayerReloadEvent               OnFirstPlayerReload                 = new FirstPlayerReloadEvent();
    public FirstAntibodyInfusionEvent           OnFirstAntibodyInfusion             = new FirstAntibodyInfusionEvent();
    public PathogensReducedToFiftyPercentEvent  OnPathogensReducedToFiftyPercent    = new PathogensReducedToFiftyPercentEvent();

    private Simulation simulation;
    private bool hasReachFiftyPercent;

    protected new void Start()
    {
        this.simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
        this.hasReachFiftyPercent = false;

        this.simulation.OnCollision.AddListener(this.HandleFirstCollision);
        /* TODO: implement the following event handlers
         * 
         * OnFirstPlayerPickup
         * OnFirstPlayerReload
         */
        this.simulation.OnPathgenDespawn.AddListener(HandleFirstAntibodyInfusion);
        this.simulation.OnPathgenDespawn.AddListener(HandlePathogensReducedToFiftyPercent);

        base.Start();
    }

    private void HandleFirstCollision(Scenario s)
    {
        if (this.simulation.collisionCount == 1)
            this.OnFirstCollision.Invoke(s);
    }

    private void HandleFirstAntibodyInfusion(Scenario s)
    {
        if (this.simulation.PathogensDestroyed == 1)
            this.OnFirstAntibodyInfusion.Invoke(s);
    }

    private void HandlePathogensReducedToFiftyPercent(Scenario s)
    {
        if (!this.hasReachFiftyPercent && simulation.PathogensDestroyed >= simulation.PathogenCount)
        {
            this.hasReachFiftyPercent = true;
            this.OnPathogensReducedToFiftyPercent.Invoke(s);
        }
    }

    /*protected void Update()
    {
        // ...
    }*/

    protected void FixedUpdate()
    {
        if (this.isActive) simulation.Tick();

        // ...
    }

    public new void Activate()
    {
        base.Activate();

        // TODO: set up simulation
    }

    public new void Complete()
    {
        // TODO: tear down simulation

        base.Complete();
    }


}
