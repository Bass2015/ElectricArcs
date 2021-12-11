using UnityEngine;

public class FollowWPAUX : MonoBehaviour
{
	public GameObject[] waypoints;
	protected int currentWP = 0;

	float speed;

	public bool moving;

	void Start()
	{
	}

	void Update()
	{
		speed = DistanciaTotalARecorrer() / GameManager.instance.levelConfig.TargetTravelTime;

		if (moving && waypoints.Length > 0)
		{
			if (Vector3.Distance(this.transform.position, waypoints[currentWP].transform.position) < float.Epsilon)
				currentWP++;

			if (currentWP >= waypoints.Length)
				currentWP = 0;

			Vector3 moveTo = Vector3.MoveTowards(transform.position, waypoints[currentWP].transform.position, speed * Time.deltaTime);
			transform.position = moveTo;
		}
	}

	float DistanciaTotalARecorrer()
	{
		float distance = 0;
		for (int i = 0; i < waypoints.Length; i++)
		{
			if (i < waypoints.Length - 1)
			{
				distance += Vector3.Distance(waypoints[i].transform.position, waypoints[i + 1].transform.position);
			}
		}
		return distance * 2;
	}
}
