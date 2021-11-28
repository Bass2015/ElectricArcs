using System;
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

   

    private Stack<Connection> connections = new Stack<Connection>();

    public void OnTargetHit(Transform origin, GameObject destination)
    {
        CreateScenePool();
        CreateRayStacks();
        ElectricRay newRay = CreateRay();

        addConnection(origin, destination);
        newRay.Flicker(origin.position, destination.transform.position);
       // newRay.FinalConnection(origin.position, destination.transform.position);
        activeRays.Push(newRay);
    }

    private void addConnection(Transform origin, GameObject destination)
    {
        Connection connection = new Connection(origin.position, destination.transform.position);
        connections.Push(connection);
        Debug.Log("connection added");
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
        connections.Clear();
        Debug.Log("Connections Cleared");
    }

    public void OnWinEvent()
    {
        Debug.Log("winevent");
        Connection connection;
        foreach (var ray in activeRays)
        {
         ray.gameObject.SetActive(true);
            if (connections.Count > 0)
            {
                connection = connections.Pop();
                ray.FinalConnection(connection.Origin,
                                connection.Destination);
            }
        }
    }

    private void OnEnable()
    {
        targetHitEvent.targetHit += OnTargetHit;
        resetElementsEvent.BaseEvent += OnResetElementsEvent;
        winEvent.BaseEvent += OnWinEvent;
    }

    private void OnDisable()
    {
        targetHitEvent.targetHit -= OnTargetHit;
        resetElementsEvent.BaseEvent -= OnResetElementsEvent;
        winEvent.BaseEvent -= OnWinEvent;

    }
    private struct Connection
    {
        Vector3 origin;
        Vector3 destination;

        public Connection (Vector3 origin, Vector3 destination)
        {
            this.origin = origin;
            this.destination = destination;
        }

        public Vector3 Origin { get => origin;}
        public Vector3 Destination { get => destination; }
    }
}
