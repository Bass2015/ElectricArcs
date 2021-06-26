using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct Cannon
{
	public GameObject originSpawn;
	public GameObject destSpawn;
}

public class ShootingTarget : Target
{

    [SerializeField]
    int numberOfCannons;

    [SerializeField]
    GameObject cannonPrefab;
    [SerializeField]
	List<Cannon> cannons;
    private float newCannonRotation;

    public int NumberOfCannons { get => numberOfCannons;}

    // Start is called before the first frame update
    void Start()
    {
        if (numberOfCannons > 0)
        {
            InitializeCannons();
        }
    }

    private void InitializeCannons()
    {
        cannons = new List<Cannon>();
        for (int i = 0; i < numberOfCannons; i++)
        {
            Transform cannonObject = transform.GetChild(i);
            AddCannon(cannonObject);
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

   
    public  void CreateCannon()
    {
        UnpackPrefab();
        GameObject newCannon = Instantiate(cannonPrefab);
        newCannon.transform.parent = this.transform;
        newCannon.transform.SetSiblingIndex(numberOfCannons);
        newCannon.transform.localPosition = Vector3.zero;
        newCannon.transform.localRotation = Quaternion.Euler(0, 0, newCannonRotation);
        newCannonRotation += 10;
        numberOfCannons++;
        newCannon.name = "Cannon_" + numberOfCannons;
    }
    private void AddCannon(Transform cannonObject)
    {
        var newCannon = new Cannon();
        newCannon.originSpawn = cannonObject.GetChild(0).gameObject;
        newCannon.destSpawn = cannonObject.GetChild(1).gameObject;
        cannons.Add(newCannon);
    }

    private void UnpackPrefab()
    {
        if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
            PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
    }

    public void RemoveCannon()
    {
        if (numberOfCannons > 0)
        {
            GameObject cannonToRemove = transform.GetChild(transform.childCount - 2).gameObject;
            DestroyImmediate(cannonToRemove);
            newCannonRotation -= 10;
            numberOfCannons--;
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
