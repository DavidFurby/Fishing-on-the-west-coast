using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    protected CharacterManager manager;
    private NavMeshAgent agent;
    private List<Waypoint> waypoints;
    private Waypoint currentWaypoint;
    private int currentWaypointIndex = 0;

    public void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        GetWayPoints();
    }

    private void GetWayPoints()
    {
        waypoints = new List<Waypoint>();
        Waypoint[] waypointsInScene = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypointsInScene)
        {
            if (waypoint.character == manager.character.name)
            {
                waypoints.Add(waypoint);
            }
        }
        waypoints.Sort((a, b) => a.orderPosition.CompareTo(b.orderPosition));
        if (waypoints.Count > 0)
        {
            currentWaypoint = waypoints[0];
        }
    }

    internal void Move()
    {
        if (currentWaypoint != null)
        {
            EnabledMovement();
            agent.destination = currentWaypoint.transform.position;
            if (transform.position == agent.destination)
            {
                currentWaypointIndex = NextPositionIndex();
                currentWaypoint = waypoints[currentWaypointIndex];
            }
        }
    }
    internal void PauseMovement()
    {
        if (agent != null && agent.enabled)
        {
            agent.enabled = false;
            manager.animationController.gesture.TriggerIdleAnimation();
        }
    }
    internal void EnabledMovement()
    {
        if (agent != null && !agent.enabled)
        {
            agent.enabled = true;
            manager.animationController.gesture.TriggerWalkAnimation();
        }
    }

    internal int NextPositionIndex()
    {
        // Use modulo operation to loop back to the start of the list
        return (currentWaypointIndex + 1) % waypoints.Count;
    }
}
