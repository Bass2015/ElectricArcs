using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBody : MonoBehaviour
{

    Vector3 orbitAngles = new Vector3(0, 0, 0);


    [SerializeField]
    FloatReference rotatingSpeed;
    
    public Vector3 axis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float finalSpeed;
        if(rotatingSpeed.Value > rotatingSpeed.constantValue)
        {
            finalSpeed = rotatingSpeed.Value;
        }
        else
        {
            finalSpeed = rotatingSpeed.constantValue;
        }
        orbitAngles = finalSpeed * Time.deltaTime * axis;
      //  transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(orbitAngles), rotatinSpeed);
        transform.Rotate(orbitAngles);
    }
}
