using System.Collections;
using UnityEngine;

public class randomMovement : MonoBehaviour
{
    public float movementDuration = 7.0f;
    public float waitBeforeMoving = 0f;
    private bool hasArrived = false;
    public float rangelowerX = (-50f);
    public float rangeupperX = (50f);
    public float rangelowerY = (-50f);
    public float rangeupperY = (50f);
    public float rangelowerZ = (-50f);
    public float rangeupperZ = (50f);

    private void Update()
    {
        if (!hasArrived)
        {
            hasArrived = true;
            float randX = Random.Range(rangelowerX, rangeupperX);
            float randY = Random.Range(rangelowerY, rangeupperY);
            float randZ = Random.Range(rangelowerZ, rangeupperZ);
            StartCoroutine(MoveToPoint(new Vector3(randX, randY, randZ)));
            Debug.Log(new Vector3(randX, randY, randZ));
        }
    }

    private IEnumerator MoveToPoint(Vector3 targetPos)
    {
        float timer = 0.0f;
        Vector3 startPos = transform.position;

        while (timer < movementDuration)
        {
            timer += Time.deltaTime;
            float t = timer / movementDuration;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        yield return new WaitForSeconds(waitBeforeMoving);
        hasArrived = false;
    }
}