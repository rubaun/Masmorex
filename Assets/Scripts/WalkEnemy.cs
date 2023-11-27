using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkEnemy : MonoBehaviour
{
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private BoxCollider b_collider;
    [SerializeField]
    private List<Transform> goal = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        b_collider = GetComponent<BoxCollider>();
        agent.autoBraking = false;
        StartCoroutine(GoToNextPoint());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GoToNextPoint()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            animator.SetBool("Moving", true);
            animator.SetFloat("velx", 0.244f);
            animator.SetFloat("vely", -0.326f);

            agent.destination = goal[destPoint].position;

            destPoint = (destPoint + 1) % goal.Count;
        }

        yield return new WaitForSeconds(3.0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(GoToNextPoint());
            Debug.Log("Trigger Inimigo");
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Moving", true);
            animator.SetFloat("velx", -0.166f);
            animator.SetFloat("vely", -0.331f);
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            animator.SetBool("Moving", false);
            Debug.Log("Trigger Destination");
            StartCoroutine(GoToNextPoint());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GoToNextPoint());
        }
    }
}
