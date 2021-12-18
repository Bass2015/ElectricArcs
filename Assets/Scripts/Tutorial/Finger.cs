
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
    public float travelTime = 0;

    Vector3 start;
    Vector3 end;
    public bool waiting;

    [SerializeField]
    TutorialEvent tutorialEvent;

    public float startTravelTime = 2.5f;
    public float dragTravelTime = 1f;
    public float shortShotTravelTime;
    public float longShotTravelTime;
    public float exitTravelTime;


    
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
    const int StartDraggingIndex = 2;
    const int EndDraggingIndex = 6;
    const int StartSlowShotIndex = 7;
    const int EndSlowShotIndex = 8;
    const int StartFastShotIndex = 9;
    const int EndFastShotIndex = 10;
    
    
    private void RefreshTravelTime()
    {
        switch (currentWP)
        {
            case 1:
                travelTime = startTravelTime;
                break;
            case StartDraggingIndex:
                tutorialEvent.RaiseEvent(TutorialEvent.TutorialEventType.StartDragging);
                break;
            case EndDraggingIndex:
                UntappingAction();
                waiting = true;
                StartCoroutine("WaitingCoroutine", 2f);
                break;
            case StartSlowShotIndex:
                TappingAction();
                travelTime = shortShotTravelTime;
                break;
            case EndSlowShotIndex:
                tutorialEvent.RaiseEvent(TutorialEvent.TutorialEventType.EnableShoot);
                break;
            case StartFastShotIndex:
                tutorialEvent.RaiseEvent(TutorialEvent.TutorialEventType.EnableFastShot);
                break;
            case EndFastShotIndex:
                UntappingAction();
                waiting = true;
                StartCoroutine("WaitingCoroutine", 2f);
                travelTime = exitTravelTime;
                tutorialEvent.RaiseEvent(TutorialEvent.TutorialEventType.End);
                break;

        }
    }

    private IEnumerator WaitingCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        waiting = false;
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
            if (currentWP < waypoints.Length)
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

    void OnTutorialEvent(TutorialEvent.TutorialEventType eType)
    {
        switch (eType)
        {
            case TutorialEvent.TutorialEventType.StartDragging:
                TappingAction();
                travelTime = dragTravelTime;
                break;
            case TutorialEvent.TutorialEventType.EnableShoot:
                UntappingAction();
                waiting = true;
                StartCoroutine("WaitingCoroutine", 2f);
                break;
            case TutorialEvent.TutorialEventType.EnableFastShot:
                TappingAction();
                break;
        }
    }

    protected void OnEnable()
    {
        tutorialEvent.tutorialEvent += OnTutorialEvent;
    }

    protected void OnDisable()
    {
        tutorialEvent.tutorialEvent -= OnTutorialEvent;
    }

}
