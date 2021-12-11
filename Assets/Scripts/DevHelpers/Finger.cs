
using UnityEngine;
using System.Collections;
using System;

public class Finger : FollowWP
{
    const float ScaleMultiplier = 0.8f;
    Vector3 initFingerScale;
    Transform tapping;
    Transform finger;

    public float elapsedTime;
    public float travelTime;

    Vector3 start;
    Vector3 end;
    public bool waiting;

    private void Start()
    {
        InitVariables();
        
    }

    private void InitVariables()
    {
        tapping = transform.GetChild(1);
        finger = transform.GetChild(0);
        initFingerScale = finger.localScale;
        finger.localScale = initFingerScale * ScaleMultiplier;
        start = transform.position;
        end = waypoints[currentWP].transform.position;
        UntappingAction();

    }



    private void Update()
    {
        if (!waiting)
        {
            Move();

        }
    }

    private void RefreshTravelTime()
    {
        int plusOne = waypoints.Length + 1;
        switch (currentWP)
        {
            case 0:
                travelTime = 0;
                break;
            case 1:
                travelTime = 5;
                break;
            case 2:
                travelTime = 1.8f;
                TappingAction();
                break;
            case 6:
                UntappingAction();
                waiting = true;
                break;
            case 7:
                TappingAction();
                break;
            case 8:
                UntappingAction();
                break;
        }
    }

    protected override void MoveToNextWP()
    {
        elapsedTime += Time.deltaTime;
        float percentage = elapsedTime / travelTime;
        transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, percentage));
        
    }

    protected override void RefreshCurrentWaypoint()
    {
        if (ArrivedAtDestination())
        {
            start = waypoints[currentWP].transform.position;
            base.RefreshCurrentWaypoint();
            if(currentWP < waypoints.Length)
                end = waypoints[currentWP].transform.position;
            elapsedTime = 0;
            RefreshTravelTime();

        }
    }

    private void UntappingAction()
    {
        finger.localScale = initFingerScale;
        tapping.gameObject.SetActive(false);
    }

    private void TappingAction()
    {
        finger.localScale = initFingerScale * ScaleMultiplier;
        tapping.gameObject.SetActive(true);
    }
}
