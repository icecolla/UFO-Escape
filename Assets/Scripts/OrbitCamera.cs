using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    [SerializeField, Min(0f)]
    float focusRadius = 1f;

    Vector3 focusPoint;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = .5f;

    [SerializeField, Range(1f, 20f)]
    float distance = 5f;

    private void Awake()
    {
        focusPoint = focus.position;
    }

    private void LateUpdate()
    {
        //Vector3 focusPoint = focus.position;
        UpdateFocusPoint();
        Vector3 lookDirection = transform.forward;
        transform.localPosition = focusPoint - lookDirection * distance;
    }

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = focus.position;
        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                //focusPoint = Vector3.Lerp(targetPoint, focusPoint, focusRadius / distance);
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        //else
        //{
        //    focusPoint = targetPoint;
        //}
    }
}
