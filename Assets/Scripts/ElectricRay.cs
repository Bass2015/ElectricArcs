﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricRay : MonoBehaviour {
	Transform nodeAtOrigin;
	Transform nodeAtDest;
	ParticleSystem[] particles;
	public float minNoiseStrength;
	public float maxNoiseStrength;
	public float smoothSpeed;

    private void Awake()
    {
		nodeAtOrigin = transform.GetChild(0);
		nodeAtDest = transform.GetChild(1);
		particles = GetComponentsInChildren<ParticleSystem>();
	}
    // Use this for initialization
    void Start () 
	{
		
	}
	
	public void Reset(float timeToReset){
		if(gameObject.activeSelf)
			StartCoroutine(DeactivateCoroutine(timeToReset));
	}

	IEnumerator DeactivateCoroutine(float timeToReset)
    {
		yield return new WaitForSeconds(timeToReset);
		gameObject.SetActive(false);
	}

	public void ConnectTwoPoints(Vector3 origin, Vector3 destination)
    {
		SetNodePosition(nodeAtOrigin, origin);
		if (destination != null)
		{
			SetNodePosition(nodeAtDest, destination);
			TurnOnBothParticles(destination);
		}
        else
        {
			nodeAtDest.gameObject.SetActive(false);
			TurnOnOneParticle();
		}
	}

	private void TurnOnBothParticles(Vector3 target)
    {
        float distanceBetweenNodes = Vector3.Distance(nodeAtOrigin.position, target);
		MakeNodesLookAtEachOther();
        foreach (ParticleSystem particle in particles)
        {
            var main = particle.main;
            main.startSpeed = distanceBetweenNodes / 1.5f;
			StartCoroutine("BirthAnimation", particle);
        }
    }

	void MakeNodesLookAtEachOther()
    {
		Vector3 originToDest = (nodeAtDest.position - nodeAtOrigin.position).normalized;
		Vector3 destToOrigin = (nodeAtOrigin.position - nodeAtDest.position).normalized;

		nodeAtOrigin.right = originToDest;
		nodeAtDest.right = destToOrigin;
    }
	private void TurnOnOneParticle()
    {
		ParticleSystem particle = nodeAtOrigin.GetComponent<ParticleSystem>();
		var main = particle.main;
		main.startSpeed = 10;
		StartCoroutine("BirthAnimation", particle);

	}

	IEnumerator BirthAnimation(ParticleSystem particle)
    {
		var noiseModule = particle.noise;
		float actualStrength = maxNoiseStrength;
		while(actualStrength > minNoiseStrength)
        {
			actualStrength -= smoothSpeed * Time.deltaTime;
			noiseModule.strength = actualStrength;
			yield return new WaitForEndOfFrame();
        }
    }

    private void SetNodePosition(Transform node, Vector3 position)
    {
		node.gameObject.SetActive(true);
		node.position = position;
	}
    void OnEnable()
    {
	//	ResetEventDispatcher.ResetEvent += Reset;
    }

	void OnDisable()
    {
	//	ResetEventDispatcher.ResetEvent -= Reset;
    }
	
}