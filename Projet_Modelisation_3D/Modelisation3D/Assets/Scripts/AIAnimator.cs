using UnityEngine;
using UnityEngine.AI;

public class AIMovementState : MonoBehaviour
{
    public bool walkForward = false;
    public Animator animator;

    public NavMeshAgent agent;

    void Start()
    {
    }

    void Update()
    {
        // Check if agent is moving and not at its destination
        if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("WalkForward", true);
        }
        else
        {
            animator.SetBool("WalkForward", false);
        }
    }
}
