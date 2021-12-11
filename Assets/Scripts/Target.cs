using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    protected bool hit;

    [SerializeField][HideInInspector]
    protected TargetHitEvent targetHitEvent;

    [SerializeField]
    [HideInInspector]
    protected GameEvent resetEvent;

    [SerializeField]
    [HideInInspector]
    protected TargetChecker targetChecker;

    public bool Hit { get { return hit; } }

    protected virtual void OnTargetHit(Transform shooter, GameObject hitObject)
    {
        if (hitObject == this.gameObject)
        {
            hit = true;
        }
        var followWPScript = GetComponent<FollowWPLoop>();
        if (followWPScript != null)
        {
            followWPScript.moving = false;
        }
    }
    protected virtual void OnResetEvent()
    {
        hit = false;
        var followWPScript = GetComponent<FollowWPLoop>();
        if (followWPScript != null)
        {
            followWPScript.moving = true;
        }
    }

    protected void OnEnable()
    {
        targetHitEvent.targetHit += OnTargetHit;
        resetEvent.BaseEvent += OnResetEvent;
        targetChecker.RegisterTarget(this);
    }

   

    protected void OnDisable()
    {
        targetHitEvent.targetHit -= OnTargetHit;
        targetChecker.UnRegisterTarget(this);
        resetEvent.BaseEvent -= OnResetEvent;
    }
}
