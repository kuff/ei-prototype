using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class GunScript : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform bulletSource = null;
    [SerializeField]
    private float projectileSpeed = 5.0f;
    private bool grabbed = false;
    private SteamVR_Input_Sources isource;

    public int gunType = 1;
    public SteamVR_Action_Boolean input;
    public SteamVR_Action_Boolean input2;
    public bool grabbable = false;

    private void Update()
    {
        Hand localHand = gameObject.GetComponent<Interactable>().hoveringHand;
        if (localHand == null) return;

        if (!grabbed) 
            isource = gameObject.GetComponent<Interactable>().hoveringHand.handType;

        if(input.GetStateDown(isource) && !grabbed)
        {
            attachToHand();
        }
        else
        {
            if (input2.GetStateDown(isource)&& grabbed)
                shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shoot();
        }
    }
    void shoot()
    {
        Debug.Log("Shoot!");

        GameObject Projectile;
        Projectile = Instantiate(projectilePrefab, bulletSource.position, bulletSource.rotation);
        Projectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * projectileSpeed, ForceMode.Impulse);

    }

    void attachToHand()
    {
        gameObject.transform.parent = GetComponent<Interactable>().hoveringHand.transform;
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);  // 0f, -0.15f, 0.15f
        gameObject.transform.localRotation = Quaternion.Euler(135f, 0f, 0f);
        grabbed = true;
    }

}
