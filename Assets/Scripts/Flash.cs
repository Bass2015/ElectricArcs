using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{

    MeshRenderer mRenderer;
    Light mLight;

    const float minValue = 0.55f;
    const float maxValue = 0.8f;
    Color baseColor;
    

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        mLight = GetComponent<Light>();
        baseColor = mRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        float intensity = Random.Range(minValue, maxValue);
     
        Color newColor = baseColor;
        newColor.a = intensity;

        mRenderer.material.color = newColor;
        mLight.intensity = intensity;
    }
}
