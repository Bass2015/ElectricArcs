using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTutorial : MonoBehaviour
{
    GameObject body;
    Collider mCollider;
    [SerializeField]
    private TutorialEvent tutorialEvent;
    public float fadingTime;
    public float fadingSpeed;
    Material mMaterial;

    [SerializeField]
    FloatReference alphaForMouse;

    private void Start()
    {
        body = transform.GetChild(0).gameObject;
        mCollider = GetComponent<Collider>();
        mCollider.enabled = false;
        mMaterial = body.GetComponent<MeshRenderer>().material;
        mMaterial.SetFloat("_AlphaValue", 0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            StartCoroutine("ActivateCoroutine");
    }
    protected void OnEnable()
    {
        tutorialEvent.tutorialEvent += OnTutorialEvent;
    }


    private void OnTutorialEvent(TutorialEvent.TutorialEventType eventType)
    {
        if(eventType == TutorialEvent.TutorialEventType.End)
        {
            print("TARGEEET");
            StartCoroutine("ActivateCoroutine");
        }
    }

    private IEnumerator ActivateCoroutine()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadingTime)
        {
            float value = elapsedTime / fadingTime;

           
            mMaterial.SetFloat("_AlphaValue", EaseInCubic(value));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mCollider.enabled = true;
    }
    protected void OnDisable()
    {
        tutorialEvent.tutorialEvent -= OnTutorialEvent;
    }

    private float EaseInCubic(float x){
        return  Mathf.Pow(x, 3);
    }
}
