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
            case TutorialEvent.TutorialEventType.StartDragging:
                aimEnabled = true;
                break;
            case TutorialEvent.TutorialEventType.EnableShoot:
                shootEnabled = true;
                break;
            case TutorialEvent.TutorialEventType.EnableFastShot:
                shootingForce.useConstant = false;
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        cannonPivot = transform.GetChild(0);
        shootingForce.useConstant = true;
        shootingForce.constantValue = 1;
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

    protected override void Shoot()
    {
        if (Input.GetMouseButtonUp(0))
            {
                Shoot(shootingForce.Value);
            }
    }
}
