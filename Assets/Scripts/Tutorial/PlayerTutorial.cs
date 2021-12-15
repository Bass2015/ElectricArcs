using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : PlayerController
{
    bool aimEnabled;
    bool shootEnabled;

    [SerializeField]
    TutorialEvent tutorialManager;

    private void OnTutorialEvent(TutorialEvent.TutorialEventType eType)
    {
        switch (eType)
        {
            case TutorialEvent.TutorialEventType.EnableAim:
                aimEnabled = true;
                break;
            case TutorialEvent.TutorialEventType.EnableShoot:
                shootEnabled = true;
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        cannonPivot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitingForResult)
        {
            if (aimEnabled)
            {
                Aim();
            }
            if (shootEnabled)
            {
                Shoot();
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        tutorialManager.tutorialEvent += OnTutorialEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        tutorialManager.tutorialEvent -= OnTutorialEvent;
    }
}
