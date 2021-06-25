using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game Event")]
public class GameEvent : ScriptableObject
{

    public virtual event Action BaseEvent;

    public virtual void RaiseEvent()
    {
        if (BaseEvent != null)
        {
            BaseEvent();
        }
    }
}