using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEyesDirectionAnimator : MonoBehaviour
{
    [SerializeField] private Movement ghostMovement;
    private Animator eyesAnimator;

    private void Start()
    {
        eyesAnimator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {

        eyesAnimator.SetFloat("x", ghostMovement.direction.x);
        eyesAnimator.SetFloat("y", ghostMovement.direction.y);
    }
}
