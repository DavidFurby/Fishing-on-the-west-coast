using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    protected CharacterManager controller;
    private NavMeshAgent agent;
    private List<Waypoint> waypoints;
    private Waypoint currentWaypoint;
    private int currentWaypointIndex = 0;

    public void Initialize(CharacterManager controller)
    {
        this.controller = controller;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetWayPoints();
    }

    private void GetWayPoints()
    {
        waypoints = new List<Waypoint>();
        Waypoint[] waypointsInScene = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypointsInScene)
        {
            if (waypoint.character == controller.character.name)
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
            agent.destination = currentWaypoint.transform.position;
            if (transform.position == agent.destination)
            {
                currentWaypointIndex = NextPositionIndex();
                currentWaypoint = waypoints[NextPositionIndex()];
            }
        }
    }
    internal void PauseMovement()
    {
        if (agent != null)
        {
            agent.enabled = false;
        }
    }
    internal void EnabledMovement()
    {
        if (agent != null)
        {
            agent.enabled = true;

        }
    }

    internal int NextPositionIndex()
    {
        // Use modulo operation to loop back to the start of the list
        return (currentWaypointIndex + 1) % waypoints.Count;
    }
}
