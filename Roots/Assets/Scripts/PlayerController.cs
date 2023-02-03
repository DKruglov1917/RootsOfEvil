using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;

    [SerializeField]private Animator swordAnimator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovementControls();
        Attack();
    }

    private void MovementControls()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
            Look(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Look(Vector3 dot)
    {
        transform.LookAt(dot);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            swordAnimator.SetTrigger("Attack");
            Debug.Log("MEH");
        }
    }
}
