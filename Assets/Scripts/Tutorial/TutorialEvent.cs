using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Tutorial Event")]
public class TutorialEvent : ScriptableObject
{
    public enum TutorialEventType
    {
        Start, EnableAim, EnableShoot, ShowTarget, End
    }


    public virtual event Action<TutorialEventType> tutorialEvent;

    public virtual void RaiseEvent(TutorialEventType tutorialCase)
    {
        if (tutorialEvent != null)
        {
            Debug.Log(tutorialCase.ToString());
            tutorialEvent(tutorialCase);
        }
    }
}
