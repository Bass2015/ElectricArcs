using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
	public float aimingSpeed;
	public float autoRotationSpeed;
	public float rotationRange;
	public float impulseMod;
	protected bool waitingForResult;
    #endregion

    #region References

    [SerializeField][HideInInspector]
	GameObject bulletPrefab;
	
	[SerializeField][HideInInspector]
	GameObject bulletSpawnPoint;

	[SerializeField]
	protected FloatReference shootingForce;

	protected Transform cannonPivot;
    #endregion

    private void Start()
    {
		cannonPivot = transform.GetChild(0);
    }

    #region Behaviour

    // Update is called once per frame
    void Update()
	{
        if (!waitingForResult)
        {
            Aim();
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Shoot(shootingForce.Value);
        }
    }

    protected void Aim()
    {
        if (Input.GetMouseButton(0))
        {
            OrientateTurretTowards(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void OrientateTurretTowards(Vector3 position)
	{
		Vector3 direction = new Vector3(position.x, position.y, 0) - cannonPivot.position;
		Vector3 lookToEuler = (Quaternion.LookRotation(cannonPivot.forward, direction)).eulerAngles;
		Quaternion lookRotation = Quaternion.Euler(lookToEuler.x, lookToEuler.y, lookToEuler.z + 90);
        cannonPivot.rotation = Quaternion.Slerp(cannonPivot.rotation, lookRotation, aimingSpeed * Time.deltaTime);
	}

    protected void Shoot(float impulse)
    {
        Rigidbody bulletRb = GameObject.Instantiate(bulletPrefab,
            bulletSpawnPoint.transform.position,
            cannonPivot.rotation).GetComponent<Rigidbody>();
        float finalImpulse = impulse / impulseMod;
        finalImpulse = Mathf.Clamp(finalImpulse, shootingForce.constantValue, finalImpulse);

        bulletRb.AddForce(bulletRb.gameObject.transform.right * finalImpulse, ForceMode.Impulse);

    }

    #endregion

    #region Events
    [Header("Events")]
	[SerializeField]
	TargetHitEvent targetHitEvent;

	[SerializeField]
	GameEvent resetElementsEvent;

	Coroutine autoRotationCo;

    private void OnResetEvent()
	{
		waitingForResult = false;
		StopAllCoroutines();
	}

	private void OnTargetHit(Transform shooter, GameObject hitObject)
	{
		waitingForResult = true;
		if(shooter.CompareTag("Bullet"))
        {
			if (autoRotationCo != null)
			{
				StopCoroutine(autoRotationCo);
			}

			autoRotationCo = StartCoroutine(OrientateTurretCoroutine(hitObject.transform.position));
		}
		
	}

    private IEnumerator OrientateTurretCoroutine(Vector3 position)
    {
		Vector3 direction = new Vector3(position.x, position.y, 0) - cannonPivot.position;
		Vector3 lookToEuler = (Quaternion.LookRotation(cannonPivot.forward, direction)).eulerAngles;
		Quaternion lookRotation = Quaternion.Euler(lookToEuler.x, lookToEuler.y, lookToEuler.z + 90);
		while (cannonPivot.rotation != lookRotation)
        {
			cannonPivot.rotation = Quaternion.Slerp(cannonPivot.rotation, lookRotation, autoRotationSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

    protected virtual void OnEnable()
	{
		targetHitEvent.targetHit += OnTargetHit;
		resetElementsEvent.BaseEvent += OnResetEvent;
	}


    protected  virtual void OnDisable()
	{
		targetHitEvent.targetHit -= OnTargetHit;
		resetElementsEvent.BaseEvent -= OnResetEvent;
	}
    #endregion
}
