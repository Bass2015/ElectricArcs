using UnityEngine;

public class FollowWPLoop : FollowWP
{
  	void Start(){
        InitSpeed();
    }

	void Update(){
		Move();
	}

    protected override void RefreshCurrentWaypoint()
	{
		if (currentWP >= waypoints.Length)
			currentWP = 0;
		base.RefreshCurrentWaypoint();
	}
}
