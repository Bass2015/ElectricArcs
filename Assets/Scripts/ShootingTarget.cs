using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cannon
{
	public GameObject originSpawn;
	public GameObject destSpawn;
}

public class ShootingTarget : Target
{
	[SerializeField]
    int numberOfCannons;
   
	
	Cannon[] cannons;

    public int NumberOfCannons { get => numberOfCannons;}

    // Start is called before the first frame update
    void Start()
    {
        InitializeCannons();
    }

    private void InitializeCannons()
    {
        cannons = new Cannon[numberOfCannons];
        for (int i = 0; i < numberOfCannons; i++)
        {
            var newCannon = new Cannon();
            Transform cannonObject = transform.GetChild(i);
            newCannon.originSpawn = cannonObject.GetChild(0).gameObject;
            newCannon.destSpawn = cannonObject.GetChild(1).gameObject;
            cannons[i] = newCannon;
        }
    }

	public void ShootRays()
	{
        for (int i = 0; i < numberOfCannons; i++)
        {
            RaycastHit rayHitInfo = new RaycastHit();
            Transform origin = cannons[i].originSpawn.transform;
            Vector3 rayDirection = (cannons[i].destSpawn.transform.position - origin.position).normalized;
            bool rayHit = Physics.Raycast(origin.position, rayDirection, out rayHitInfo);
            if (rayHit && rayHitInfo.collider.CompareTag("Target"))
            {
                Target targetScript = rayHitInfo.collider.gameObject.GetComponent<Target>();
                targetHitEvent.OnTargetHit(origin, targetScript.gameObject);
            }
            else
            {
                targetHitEvent.OnTargetHit(origin, cannons[i].destSpawn);
            }
        }
    }

    

    protected override void OnTargetHit(Transform shooter, GameObject hitObject)
    {
        base.OnTargetHit(shooter, hitObject);
        if (hitObject == this.gameObject)
        {
            ShootRays();
        }
        
    }
}
