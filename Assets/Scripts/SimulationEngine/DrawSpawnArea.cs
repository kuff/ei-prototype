using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpawnArea : MonoBehaviour
{
    //Add this code to the scene when editing spawn areas for cells and input your values to draw the area
    public Vector3 origin = Vector3.zero;
    [Range(0.1f, 1f)]
    public float ceiling;
    public float min, max;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < 500; i++)
        {
            Vector3 randpoint = UnityEngine.Random.insideUnitSphere.normalized;
            Vector3 spawnPos = origin + new Vector3(randpoint.x, Mathf.Abs(randpoint.y * ceiling),randpoint.z) * UnityEngine.Random.Range(min, max);
            Gizmos.DrawSphere(spawnPos, 0.1f);

        }
    }
}
