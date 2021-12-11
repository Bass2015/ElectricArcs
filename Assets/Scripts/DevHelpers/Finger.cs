
using UnityEngine;
using System.Collections;

public class Finger : FollowWP
{
    const float ScaleMultiplier = 0.8f;

    Vector3 initFingerScale;
    Transform tapping;
    Transform finger;

    private void Start()
    {
        InitVariables();
        InitSpeed();
    }

    private void InitVariables()
    {
        tapping = transform.GetChild(1);
        finger = transform.GetChild(0);
        initFingerScale = finger.localScale;
        finger.localScale = initFingerScale * ScaleMultiplier;
    }

    private void Update()
    {
        Move();
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
