using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LaunchLightning : MonoBehaviour
{
    public ParticleSystem [] effectToPlay;
    public GameObject Target;
    public float pressDepth = 0.1f;         // How far the button moves down
    public float pressDuration = 0.1f;      // How fast it moves
    public AudioSource lightningAudio;
    public float delay;

    private bool playerInRange = false;
    private bool isPressed = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        targetPosition = new Vector3(1.02600002f, 9.62199974f, 19.1399994f);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !isPressed)
        {
            if (effectToPlay != null)
                for (int i = 0; i < effectToPlay.Length; i++)
                    effectToPlay[i].Play();
            StartCoroutine(setTarget(0f, targetPosition));
            StartCoroutine(setTarget(4f, new Vector3(0f,0f,0f)));
            StartCoroutine(AnimateButtonPress());
            StartCoroutine(PlayWithDelay(delay));
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

    IEnumerator AnimateButtonPress()
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

    IEnumerator setTarget(float delay, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(delay);
        Target.transform.localPosition = targetPosition;
    }

    IEnumerator PlayWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lightningAudio.Play();
    }
}
