using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Transform cible; // Target

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (cible != null)
        {
            agent.SetDestination(cible.position);
        }
    }
}
