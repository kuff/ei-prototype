using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Grabable : MonoBehaviour
{
    private bool grabbed = false;
    private SteamVR_Input_Sources isource;
    private Simulation simulation;
    public SteamVR_Action_Boolean input;
    public SteamVR_Action_Boolean input2;
    public bool grabbable = false;
    // Start is called before the first frame update
    void Start()
    {
        simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
    }

    // Update is called once per frame
    void Update()
    {
        Hand localHand = gameObject.GetComponent<Interactable>().hoveringHand;
        if (localHand == null) return;

        if (!grabbed)
            isource = gameObject.GetComponent<Interactable>().hoveringHand.handType;

        if (input.GetStateDown(isource) && !grabbed)
        {
            attachToHand();
        }
    }

    void attachToHand()
    {
        this.simulation.OnPickup.Invoke(new Scenario());

        gameObject.transform.parent = GetComponent<Interactable>().hoveringHand.transform;
        gameObject.transform.localPosition = new Vector3(0f, -0f, 0f);  // 0f, -0.15f, 0.15f
        gameObject.transform.localRotation = Quaternion.Euler(0f, -0f, -0f);
        grabbed = true;
    }

}
