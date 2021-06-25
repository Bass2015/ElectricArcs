﻿#define Testing
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
	public static GameManager instance;
	[SerializeField]
	float secondsBeforeReset;

	[Header("Levels")]
	[SerializeField]
	private List<LevelSettings> levels;
	public LevelSettings levelConfig;
	private int currentLevel = 0;
	
	[Header("Scriptable Objects")]
	[SerializeField]
	RayFactory rayFactory;
	[SerializeField]
	FloatVariable mouseTravel;

	#region Initialization
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
		DontDestroyOnLoad(gameObject);
	}


	private void Start()
	{
        if (Application.isEditor)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
				Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name.Contains("Level"))
                {
					currentLevel = loadedScene.buildIndex;
					SceneManager.SetActiveScene(loadedScene);
					return;
                }
            }
        }
		LoadNextLevel();
	}

	#endregion

	#region Scene Management

	public void LoadNextLevel()
    {
		StartCoroutine(LoadNextLevelCoroutine());
		rayFactory.OnResetElementsEvent();
	}
	IEnumerator LoadNextLevelCoroutine()
	{
		enabled = false;
		if(currentLevel > 0)
        {
			yield return SceneManager.UnloadSceneAsync(currentLevel);
        }
		if (currentLevel < SceneManager.sceneCountInBuildSettings - 1)
		{
			currentLevel++;
			yield return SceneManager.LoadSceneAsync(currentLevel, LoadSceneMode.Additive);
			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentLevel));
		}
		else
		{
			print("There are no more levels");
		}
		enabled = true;
	}


	#endregion

	#region Events
	[SerializeField] 
	TargetChecker targetChecker;

	[Header("Events")]
	[SerializeField]
	TargetHitEvent targetHitEvent;

	[SerializeField]
	GameEvent resetEvent;
	Coroutine waitForReset;

	[SerializeField]
	GameEvent winEvent;
    

    void OnTargetHit(Transform shooter, GameObject hitObject)
	{
		if (waitForReset == null)
		{
			waitForReset = StartCoroutine("WaitAndCheckTargets");
		}
	}
	IEnumerator WaitAndCheckTargets() 
	{
		yield return new WaitForSeconds(secondsBeforeReset);
		targetChecker.CheckIfAllHit();
		waitForReset = null;
	}
	private void OnWinEvent()
	{
		
	}

	private void OnEnable()
    {
        targetHitEvent.targetHit += OnTargetHit;
        //winEvent.BaseEvent += OnWinEvent;
        EnableRayFactory();
    }

    

    private void OnDisable()
    {
        targetHitEvent.targetHit -= OnTargetHit;
        //winEvent.BaseEvent -= OnWinEvent;
        DisableRayFactory();
    }
	private void EnableRayFactory()
	{
		targetHitEvent.targetHit += rayFactory.OnTargetHit;
		resetEvent.BaseEvent += rayFactory.OnResetElementsEvent;
	}
	private void DisableRayFactory()
    {
        targetHitEvent.targetHit -= rayFactory.OnTargetHit;
        resetEvent.BaseEvent -= rayFactory.OnResetElementsEvent;
    }
	#endregion

	#region Updating

	
	private Vector3 initialMousePos;
	private void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			initialMousePos = Input.mousePosition;
		}
        if (Input.GetMouseButton(0))
        {
			mouseTravel.Value = SetMouseTravel();
		}
		
	}
    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
			mouseTravel.Value = 0;
        }
    }

    float SetMouseTravel()
    {
		Vector3 finalMousePos = Input.mousePosition;
		float distanceMoved = finalMousePos.x - initialMousePos.x;
		return distanceMoved;
	}
    #endregion
}