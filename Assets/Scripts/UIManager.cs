using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    private Button nextLevelButton;
    public float alphaIncreasePerFrame;

    [SerializeField]
    GameEvent winEvent;

    private void Start()
    {
        nextLevelButton = transform.GetComponentInChildren<Button>();
        nextLevelButton.gameObject.SetActive(false);
    }

    [ContextMenu("IncreaseAlpha")]
    private void OnWinEvent()
    {
        nextLevelButton.gameObject.SetActive(true);
        StartCoroutine(IncreaseButtonAlpha());
    }

    private IEnumerator IncreaseButtonAlpha()
    {
        Color buttonColor = nextLevelButton.image.color;
        buttonColor.a = 0;
        float newAlpha = 0;
        nextLevelButton.image.color = buttonColor;
        while (buttonColor.a != 1)
        {
            newAlpha += alphaIncreasePerFrame;
            buttonColor.a = Mathf.Clamp(newAlpha, 0, 1);
            nextLevelButton.image.color = buttonColor;
            yield return new WaitForEndOfFrame(); 
        }
    }

    private void OnLoadedScene(Scene scene, LoadSceneMode loadMode)
    {
        if(nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(false);    
    }


    private void OnEnable()
    {
        winEvent.BaseEvent += OnWinEvent;
        SceneManager.sceneLoaded += OnLoadedScene;
    }

   
    private void OnDisable()
    {
        winEvent.BaseEvent -= OnWinEvent;
        SceneManager.sceneLoaded -= OnLoadedScene;
    }
}
