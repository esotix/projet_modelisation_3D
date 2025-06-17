using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class FireTrapZone : MonoBehaviour
{
    [Header("Prefabs & References")]
    public GameObject firePrefab;
    public GameObject lightningEffectPrefab;
    public AudioClip lightningSound;
    public NavMeshSurface navSurface;

    [Header("Settings")]
    public float fireDelay = 0.5f; // delay before fire spawns

    private bool triggered = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (navSurface == null)
            navSurface = Object.FindFirstObjectByType<NavMeshSurface>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("AI") || other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(TriggerLightningAndFire());
        }
    }

    private IEnumerator TriggerLightningAndFire()
    {
        Vector3 trapCenter = transform.position + Vector3.up * 10f;

        // Raycast down to find ground surface
        if (Physics.Raycast(trapCenter, Vector3.down, out RaycastHit hit, 20f))
        {
            Vector3 groundPosition = hit.point;

            // 1. Spawn lightning effect slightly above hit point
            GameObject lightning = Instantiate(lightningEffectPrefab, groundPosition + Vector3.up * 3f, Quaternion.identity);
            lightning.transform.LookAt(groundPosition);

            // 2. Play lightning sound
            if (lightningSound)
                audioSource.PlayOneShot(lightningSound);

            // 3. Wait for lightning to "strike"
            yield return new WaitForSeconds(fireDelay);

            // 4. Spawn fire at ground point
            Quaternion fireRotation = Quaternion.Euler(-90f, 0f, 0f);
            GameObject fire = Instantiate(firePrefab, groundPosition, fireRotation);

            // 5. Block navmesh
            var mod = fire.AddComponent<NavMeshModifier>();
            mod.overrideArea = true;
            mod.area = 1; // Not Walkable

            navSurface.BuildNavMesh();

        }
    }
}
