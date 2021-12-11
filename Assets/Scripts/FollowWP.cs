using UnityEngine;

public class FollowWP : MonoBehaviour
{
	public GameObject[] waypoints;
	protected int currentWP = 0;

	float speed;

	public bool moving;

	void Start()
	{
		InitSpeed();
	}

	void Update()
    {
        Move();
    }

    protected void Move()
    {
        RefreshCurrentWaypoint();
        MoveToNextWP();
    }

    protected void InitSpeed()
	{
		float distance = 0;
		for (int i = 0; i < waypoints.Length; i++)
		{
			if (i < waypoints.Length - 1)
			{
				distance += Vector3.Distance(waypoints[i].transform.position, waypoints[i + 1].transform.position);
			}
		}
		speed = (distance * 2) / GameManager.instance.levelConfig.TargetTravelTime; ;
	}

	protected void MoveToNextWP()
    {
        if (CanMove())
        {
			Vector3 nextPoint = waypoints[currentWP].transform.position;
			Vector3 moveTo = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
            transform.position = moveTo; 
        }
    }

	private bool CanMove()
	{
		return moving && waypoints.Length > 0 && currentWP < waypoints.Length;
	}

	protected virtual void RefreshCurrentWaypoint()
    {
		if (currentWP < waypoints.Length && 
			Vector3.Distance(this.transform.position, waypoints[currentWP].transform.position) < float.Epsilon)
			currentWP++;
	}
}
