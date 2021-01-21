using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{

    public enum WanderType { Random, Waypoint };
    public WanderType wanderType = WanderType.Random;
    public int health = 100;
    public int wanderDistance = 10;
    private Vector3 wanderPoint;
    private bool isDeadDestenationSeted = false;
    public Transform[] waypoints;
    private int wayPointIndex = 0;
    public float wanderSpeed = 0.5f;
    public float chaseSpeed = 1f;

    public float viewDistance = 5f;
    private bool isAware = false;
    private bool isDetecting = true;
    private float detectingDistance = 7f;
    private Vector3 detectingPosition;

    private NavMeshAgent agent;
    private Renderer renderer;
    public GameObject player;
    private Animator animator;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        animator = GetComponentInChildren<Animator>();
        wanderPoint = getWanderPoint(wanderDistance);
    }

    public void Update()
    {
        if(health <= 0)
        {
            if(!isDeadDestenationSeted)
            {
            animator.SetBool("Dead", true);
            isDeadDestenationSeted = true;
            }
            else
            {
            }
        }
        else if (isAware)
        {
            agent.speed = chaseSpeed;
            animator.SetBool("Aware", true);
            agent.SetDestination(player.transform.position);
            if(isAware && isDetecting)
            {
                detectingPosition = transform.position;
                isDetecting = false;
            }
            if(Vector3.Distance(detectingPosition, transform.position) < detectingDistance)
            {
                offAware();
                isDetecting = true;
            }
        }
        else
        {
            agent.speed = wanderSpeed;
            animator.SetBool("Aware", false);
            Wander();
            
        }
        SearchForPlayer();
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
                wanderPoint = getWanderPoint(wanderDistance);
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

    public void onHit(int damage)
    {
        health -= damage;
    }

    public Vector3 getWanderPoint(int wanderDistance)
    {
        return new Vector3(transform.position.x + wanderDistance, transform.position.y, transform.position.z);
    }
}
