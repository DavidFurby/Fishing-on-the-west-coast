using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    protected CharacterController controller;
    private NavMeshAgent agent;
    private GameObject waypoint;

    public void Initialize(CharacterController controller)
    {
        this.controller = controller;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Waypoint"))
            {
                waypoint = child.gameObject;
                print(waypoint);
                break;
            }
        }
    }
    internal void Move()
    {
        if (waypoint != null)
        {
            agent.destination = waypoint.transform.position;
            print(agent.destination);
        }
    }
}
