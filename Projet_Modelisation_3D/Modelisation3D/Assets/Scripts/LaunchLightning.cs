using System;
using UnityEngine;

public class LaunchLightning : MonoBehaviour
{
    public ParticleSystem effectToPlay;
    public float pressDepth = 0.1f;         // How far the button moves down
    public float pressDuration = 0.1f;      // How fast it moves
    private bool playerInRange = false;
    private bool isPressed = false;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !isPressed)
        {
            if (effectToPlay != null)
                effectToPlay.Play();

            StartCoroutine(AnimateButtonPress());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    System.Collections.IEnumerator AnimateButtonPress()
    {
        isPressed = true;

        Vector3 pressedPosition = originalPosition + Vector3.down * pressDepth;

        // Move down
        float elapsed = 0;
        while (elapsed < pressDuration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, pressedPosition, elapsed / pressDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = pressedPosition;

        // Wait briefly
        yield return new WaitForSeconds(0.05f);

        // Move back up
        elapsed = 0;
        while (elapsed < pressDuration)
        {
            transform.localPosition = Vector3.Lerp(pressedPosition, originalPosition, elapsed / pressDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;

        isPressed = false;
    }
}
