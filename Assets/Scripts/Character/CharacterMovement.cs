using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    protected CharacterController controller;
    private NavMeshAgent agent;
    private List<Waypoint> waypoints;
    private Waypoint currentWaypoint;

    public void Initialize(CharacterController controller)
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
                currentWaypoint = waypoints[NextPositionIndex(currentWaypoint)];
            }
        }
    }

    internal int NextPositionIndex(Waypoint current)
    {
        if (waypoints.IndexOf(current) + 1 >= waypoints.Count)
        {
            return 0;
        }
        else
        {
            return waypoints.IndexOf(currentWaypoint) + 1;
        }
    }
}
