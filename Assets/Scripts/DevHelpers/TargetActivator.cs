using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetActivator : MonoBehaviour
{

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target.SetActive(false);
        print("ACTIVATOR");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (target.activeSelf)
            {
                target.SetActive(false);

            }
            else
            {
                target.SetActive(true);

            }
        }
    }
}
