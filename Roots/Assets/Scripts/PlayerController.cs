using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public RoomsPlacer roomsPlacer;
    public Animator animator;
    public float playerSpeed = 2.0f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] private int damage;
    [SerializeField] private Animator swordAnimator;   
    private float gravityValue = -9.81f;

    private bool isAiming;
    private Vector2 aimStart, aimEnd;
    public Vector3 lookDot;
    private Vector3 velocity;

    private string state;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        MovementControls();
        Attack();
        Aim();
        if (!isAiming) Look();
        Animate();
    }

    private void MovementControls()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        velocity = controller.velocity.normalized;

        if (!controller.isGrounded)
            playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Animate()
    {
        var heading = lookDot - new Vector3(transform.position.x, 0, transform.position.z);

        var angle = Vector3.SignedAngle(velocity, heading, Vector3.up);

        if (velocity == Vector3.zero || angle == 90)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            Debug.Log(angle);
            if (angle > -45 && angle < 45) animator.SetTrigger("Fwd");
            if (angle > -120 && angle < -45) animator.SetTrigger("Right");
            if (angle > 45 && angle < 120) animator.SetTrigger("Left");
            if (angle > 120 || angle < -120) animator.SetTrigger("Bwd");
        }
    }

    private void Aim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isAiming = false;
        }
    }

    private void Look()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, 7))
        {
            var direction = (hit.point - transform.position).normalized;
            direction.y = 0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 5f);
            lookDot = new Vector3(hit.point.x, 0, hit.point.z);
        }        
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            swordAnimator.SetTrigger("Attack");

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.TryGetComponent<Enemy>(out Enemy hitEnemy))
                {
                    hitEnemy.TakeDamage(damage);
                }

                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }        
    }
}
