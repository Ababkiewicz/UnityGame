using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{

    public enum WanderType { Random, Waypoint };
    public WanderType wanderType = WanderType.Random;
    public int wanderDistance = 10;
    private Vector3 wanderPoint;
    public Transform[] waypoints;
    private int wayPointIndex = 0;

    public float viewDistance = 10f;
    private bool isAware = false;

    private NavMeshAgent agent;
    private Renderer renderer;
    public GameObject player;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        wanderPoint = getWanderPoint(wanderDistance);
    }

    public void Update()
    {
        if (isAware)
        {
            agent.SetDestination(player.transform.position);
            renderer.material.color = Color.red;
        }
        else
        {
            SearchForPlayer();
            Wander();
            renderer.material.color = Color.blue;
        }
    }

    public void SearchForPlayer()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < viewDistance)
        {
            onAware();
        }
    }

    public void onAware()
    {
        isAware = true;
    }

    public void offAware()
    {
        isAware = false;
    }

    public void Wander()
    {
        if(wanderType == WanderType.Random)
        {
            if (transform.position.x == wanderPoint.x)
            {
                wanderDistance = wanderDistance * (-1);
                this.wanderPoint = getWanderPoint(wanderDistance);
            }
            else
            {
                agent.SetDestination(wanderPoint);
            }
        }
        else 
        {
            if (waypoints.Length >= 2)
            {
                if (waypoints[wayPointIndex].position.x == transform.position.x)
                {
                    if (wayPointIndex == waypoints.Length - 1)
                    {
                        wayPointIndex = 0;
                    }
                    else
                    {
                        wayPointIndex++;
                    }
                }
                else
                {
                    agent.SetDestination(waypoints[wayPointIndex].position);
                }
            }
            else
            {
                Debug.LogWarning("Please assing more than 1 waypoint to the AI:" + gameObject.name);
            }
        }
    }

    public Vector3 getWanderPoint(int wanderDistance)
    {
        return new Vector3(transform.position.x + wanderDistance, transform.position.y, transform.position.z);
    }
}
