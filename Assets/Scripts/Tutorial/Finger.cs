
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

    private void Start()
    {
        InitVariables()
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
        switch (currentWP)
        {
            case 0:
                travelTime = 0;
                break;
            case 1:
                travelTime = startTravelTime;
                break;
            case 2:
                travelTime = dragTravelTime;
                TappingAction();
                break;
            case 6:
                tutorialEvent.RaiseEvent(TutorialEvent.TutorialEventType.EnableAim);
                break;
            case 7:
                TappingAction();
                travelTime = shortShotTravelTime;
                break;
            case 8:
                tutorialEvent.RaiseEvent(TutorialEvent.TutorialEventType.EnableShoot);
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
            case TutorialEvent.TutorialEventType.EnableAim:
                UntappingAction();
                waiting = true;
                StartCoroutine("WaitingCoroutine", 2f);
                break;
            case TutorialEvent.TutorialEventType.EnableShoot:
                UntappingAction();
                waiting = true;
                StartCoroutine("WaitingCoroutine", 2f);
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
