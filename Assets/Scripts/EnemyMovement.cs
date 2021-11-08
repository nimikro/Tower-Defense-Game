using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// list required components for the script
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int waypointIndex = 0;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
    }
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
        // we update the speed back up to start speed
        // if the laser is still slowing then the enemy speed will be slowed and the enemy will move with the slow speed
        // then the speed will go back up to starting speed
        // as long as the laser is slowing the enemy will move with the slowed speed because the movement happens on line 22, before the speed gets reset
        // there might be a frame where the speed is the starting speed istead of the slowed speed but it will not be noticeable
        // one way to fix that would be to use the script execution order of unity making the enemyMovement script execute last
        // or we could write different code -> use a list of debuffs that trigger on the enemy and then stop when the turret is out of range
        enemy.speed = enemy.startSpeed;
    }

    void GetNextWaypoint()
    {
        // if enemy reaches the last waypoint -> destroy enemy
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }
        // else get next waypoint
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    // if enemy reaches the final waypoint (END) deal damage to player
    void EndPath()
    {
        PlayerStats.Lives--;
        WaveSpawnDifferentNew.EnemiesAlive--;
        Destroy(gameObject);
    }
}
