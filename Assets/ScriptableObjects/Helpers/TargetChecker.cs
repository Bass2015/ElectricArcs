using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Helpers/TargetChecker")]
public class TargetChecker : ScriptableObject
{

    public List<Target> targets;

    [SerializeField]
    GameEvent winEvent;
    [SerializeField]
    GameEvent resetElementsEvent;

    public void RegisterTarget(Target newTarget)
    {
        if (!targets.Contains(newTarget))
        {
            targets.Add(newTarget);
        }
    }
    public void UnRegisterTarget(Target oldTarget) 
    {
        if (targets.Contains(oldTarget)) 
        {
            targets.Remove(oldTarget);
        }
    }

    public void CheckIfAllHit()
    {
        int targetsHit = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].Hit)
            {
                targetsHit++;
            }
        }
        if (targetsHit == targets.Count)
        {
            winEvent.RaiseEvent();
        }
        else
        {
            resetElementsEvent.RaiseEvent();
        }
    }
}