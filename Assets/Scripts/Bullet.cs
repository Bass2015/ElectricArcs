using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPoint;
    [SerializeField]
    TargetHitEvent targetHit;

    private void Start()
    {
        startPoint = transform.position;
    }

    private void Update()
    {
        if (!IsInScene())
        {
            Destroy(this.gameObject);
        }
    }

    bool IsInScene()
    {
        return (transform.position.x < 10 && transform.position.x > -10 &&
                transform.position.y < 6 && transform.position.y > -6);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {

            transform.position = startPoint;

            targetHit.OnTargetHit(transform, other.gameObject); 
            Destroy(this.gameObject);
            //Just testing
        }
    }

}
