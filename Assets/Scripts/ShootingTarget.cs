using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public struct Cannon
{
    public Transform transform;
	public GameObject originSpawn;
	public GameObject destSpawn;
}

public class ShootingTarget : Target
{

    [SerializeField][HideInInspector]
    int numberOfCannons;

    [SerializeField]
    GameObject cannonPrefab;
    [SerializeField]
	List<Cannon> cannons;
    [SerializeField][HideInInspector]
    private float newCannonRotation;

    public int NumberOfCannons { get => numberOfCannons;}
    public List<Cannon> Cannons { get => cannons;}

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
        if(cannons == null)
        {
            cannons = new List<Cannon>();
        }
        for (int i = 0; i < numberOfCannons; i++)
        {
            Transform cannonObject = transform.GetChild(i);
           
            AddCannon(cannonObject);
        }
    }
    private void AddCannon(Transform cannonObject)
    {
        if (cannons == null)
        {
            cannons = new List<Cannon>();
        }
        var newCannon = new Cannon();
        newCannon.transform = cannonObject;
        newCannon.originSpawn = cannonObject.GetChild(0).gameObject;
        newCannon.destSpawn = cannonObject.GetChild(1).gameObject;
        cannons.Add(newCannon);
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
    #region Custom Editor
#if UNITY_EDITOR
    public void CreateCannon()
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
#endif
    #endregion
    protected override void OnTargetHit(Transform shooter, GameObject hitObject)
    {
        base.OnTargetHit(shooter, hitObject);
        if (hitObject == this.gameObject)
        {
            ShootRays();
        }
    }
}
