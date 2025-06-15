using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MovementBoolAnimator : MonoBehaviour
{
    public Animator animator;

    void Start()
    {

    }

    void Update()
    {
        setAnimation("WalkForward", Input.GetKey(KeyCode.W));
        setAnimation("WalkBack", Input.GetKey(KeyCode.S));
        setAnimation("WalkLeft", Input.GetKey(KeyCode.A));
        setAnimation("WalkRight", Input.GetKey(KeyCode.D));

    }

    void setAnimation(string animationName, bool keyCode)
    {
        animator.SetBool(animationName, keyCode);

    }
}
