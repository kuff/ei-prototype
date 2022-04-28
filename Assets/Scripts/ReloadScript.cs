using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScript : MonoBehaviour
{
    public bool isLoaded = false;
    [SerializeField]
    private Animator anim;
    private Simulation simulation;

    protected void Start()
    {
        this.simulation = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        Debug.Log(other.name);
        if(other.tag != "Projectile" || isLoaded)return;
        else
        {
            isLoaded = true;
            //Destroy(other.gameObject);
            this.simulation.DespawnCell(other.GetComponentInChildren<Cell>(), false);
            this.simulation.OnReload.Invoke(new Scenario());

            this.GetComponentInParent<AudioSource>().PlayOneShot(this.GetComponentInParent<GunScript>().reloadSound);
            anim.SetTrigger("Load");
        }

    }

}
