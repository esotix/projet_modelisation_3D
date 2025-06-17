using UnityEngine;

public class Target : MonoBehaviour
{
    private Transform targetToFollow;
    public Transform ikTarget;
    private void Start()
    {
    }
    void Update()
    {
        if (targetToFollow == null) targetToFollow = GameObject.FindGameObjectWithTag("Target").transform;
        ikTarget.position = targetToFollow.position;
    }
}
