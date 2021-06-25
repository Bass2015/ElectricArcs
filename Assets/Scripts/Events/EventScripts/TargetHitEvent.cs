using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Target Hit Event")]
public class TargetHitEvent : ScriptableObject
{

    public event Action<Transform, GameObject> targetHit;
   
    public void OnTargetHit(Transform shooter, GameObject hitObject)
    {
        if(targetHit != null)
        {
            targetHit(shooter, hitObject);
        } 
    }
}
