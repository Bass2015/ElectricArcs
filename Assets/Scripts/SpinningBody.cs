using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBody : MonoBehaviour
{

    Vector3 orbitAngles = new Vector3(0, 0, 0);


    [SerializeField]
    FloatReference rotatingSpeed;

    float maxRotatingSpeed = 500;
    
    public Vector3 axis;

    // Update is called once per frame
    void Update()
    {
        SpinBody();
    }

    protected void SpinBody()
    {
        float finalSpeed = DecideRotationSpeed();
        Target targetScript = transform.parent.GetComponent<Target>();
        if (targetScript != null && targetScript.Hit)
        {
            finalSpeed = maxRotatingSpeed;
        }
        SpinBody(finalSpeed);
    }

    private void SpinBody(float speed)
    {
        orbitAngles = speed * Time.deltaTime * axis;
        transform.Rotate(orbitAngles);
    }

    private float DecideRotationSpeed()
    {
        float finalSpeed;
        if (rotatingSpeed.Value > rotatingSpeed.constantValue)
        {
            finalSpeed = rotatingSpeed.Value;
        }
        else
        {
            finalSpeed = rotatingSpeed.constantValue;
        }
        return finalSpeed;
    }
  }
