using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform targetToFollow;
    public Transform ikTarget;

    void Update()
    {
        ikTarget.position = targetToFollow.position;
        Debug.DrawLine(ikTarget.position, ikTarget.position + Vector3.up, Color.red);
    }
}
