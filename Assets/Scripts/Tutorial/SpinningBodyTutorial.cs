using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBodyTutorial : SpinningBody
{
    bool spinEnabled;

    [SerializeField]
    TutorialEvent tutorialEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (spinEnabled)
        {
            SpinBody();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnEnable()
    {
        tutorialEvent.tutorialEvent += OnTutorialEvent;
    }

    private void OnTutorialEvent(TutorialEvent.TutorialEventType eType)
    {
        if (eType == TutorialEvent.TutorialEventType.EnableShoot)
        {
           spinEnabled = true;
        }
    }

    protected void OnDisable()
    {
        tutorialEvent.tutorialEvent -= OnTutorialEvent;
    }
}
