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
            Debug.LogWarning("Aucun objet avec le tag 'AI' trouv�.");
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
                agent.Warp(spawnPoint); // t�l�portation propre pour NavMeshAgent
                Debug.Log("AI t�l�port�e au point de spawn.");
            }
        }
    }
}
