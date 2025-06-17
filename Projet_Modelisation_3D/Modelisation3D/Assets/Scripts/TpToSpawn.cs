using UnityEngine;
using UnityEngine.AI;

public class TpToSpawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    private bool triggered = false;

    private void Start()
    {
        GameObject agent = GameObject.FindGameObjectWithTag("AI");
        if (agent != null)
        {
            spawnPoint = agent.transform.position;
        }
        else
        {
            Debug.LogWarning("Aucun objet avec le tag 'AI' trouvé.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("AI"))
        {
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                triggered = true;
                agent.Warp(spawnPoint); // téléportation propre pour NavMeshAgent
                Debug.Log("AI téléportée au point de spawn.");
            }
        }
    }
}
