using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkEnemy : MonoBehaviour
{
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField]
    private List<Transform> goal = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        StartCoroutine(GoToNextPoint());
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            
        }

        if(!agent.isStopped)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    IEnumerator GoToNextPoint()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.destination = goal[destPoint].position;

            destPoint = (destPoint + 1) % goal.Count;
        }
        
        yield return new WaitForSeconds(3.0f);

        StartCoroutine(GoToNextPoint());
    }
}
