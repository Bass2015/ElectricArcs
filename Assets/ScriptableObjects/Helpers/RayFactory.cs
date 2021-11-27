using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(menuName = "Helpers/Ray Factory")]
public class RayFactory : ScriptableObject
{
    [SerializeField]
    ElectricRay rayPrefab;

    [SerializeField]
    TargetHitEvent targetHitEvent;

    [SerializeField]
    GameEvent resetElementsEvent;

    [SerializeField]
    GameEvent winEvent;

    Stack<ElectricRay> sleepingRays;
    Stack<ElectricRay> activeRays;

    Scene rayPoolScene;

    public void OnTargetHit(Transform origin, GameObject destination)
    {
        CreateScenePool();
        CreateRayStacks();
        ElectricRay newRay = CreateRay();


        //newRay.Flicker(origin.position, destination.transform.position);
        newRay.FinalConnection(origin.position, destination.transform.position);
        activeRays.Push(newRay);
    }

  
    private ElectricRay CreateRay()
    {
        ElectricRay newRay;
        if (sleepingRays.Count > 0)
        {
            newRay = sleepingRays.Pop();
            newRay.gameObject.SetActive(true);
        }
        else
        {
            newRay = Instantiate(rayPrefab);
            SceneManager.MoveGameObjectToScene(newRay.gameObject, rayPoolScene);
        }
        return newRay;
    }

    private void CreateRayStacks()
    {
        if (sleepingRays == null)
        {
            sleepingRays = new Stack<ElectricRay>();
        }
        if (activeRays == null) 
        {
            activeRays = new Stack<ElectricRay>();
        }
    }

    private void CreateScenePool()
    {
        rayPoolScene = SceneManager.GetSceneByName(name);
        if (rayPoolScene.isLoaded)
        {
            return;
        }
        rayPoolScene = SceneManager.CreateScene(name);
        
    }

    void RemoveRays()
    {
        if (activeRays != null)
        {
            while (activeRays.Count > 0)
            {
                ElectricRay rayToRemove = activeRays.Pop();
                rayToRemove.gameObject.SetActive(false);
                sleepingRays.Push(rayToRemove);
            } 
        }
    }

    public  void OnResetElementsEvent()
    {
        RemoveRays();
    }

    public void OnWinEvent()
    {

    }

    private void OnEnable()
    {
        targetHitEvent.targetHit += OnTargetHit;
        resetElementsEvent.BaseEvent += OnResetElementsEvent;
    }

    private void OnDisable()
    {
        targetHitEvent.targetHit -= OnTargetHit;
        resetElementsEvent.BaseEvent -= OnResetElementsEvent;
    }
    
}
