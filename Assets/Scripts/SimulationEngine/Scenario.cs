using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : Level
{
    private Simulation simulation;

    protected new void Start()
    {
        simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();

        base.Start();
    }

    protected void Update()
    {
        // ...
    }

    protected void FixedUpdate()
    {
        simulation.Tick();

        // ...
    }

    // TODO: the rest of this should be Level class field overrides (if any are needed)...
}
