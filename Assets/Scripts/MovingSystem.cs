using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSystem : MonoBehaviour
{
    public Transform[] points;
    public Transform obj;
    public float speed;
    public bool cycle;

    private Transform targetPoint;
    private int currentPoint;
    private bool forward;

    private void Start()
    {
        forward = true;
        currentPoint = 0;
        targetPoint = points[currentPoint]; 
    }

    private void Update()
    {
        if (obj.position == targetPoint.position)
        {
            if (forward)
                currentPoint++;
            else
                currentPoint--;

            if (currentPoint >= points.Length && cycle)
            {
                currentPoint = 0;
            }
            else if (currentPoint >= points.Length && !cycle)
            {
                forward = false;
                currentPoint = points.Length - 2;
            }
            else if (currentPoint < 0)
            {
                forward = true;
                currentPoint = 1;
            }

            targetPoint = points[currentPoint];
        }

        obj.position = Vector3.MoveTowards(obj.position, targetPoint.position, speed * Time.deltaTime);
    }
}
