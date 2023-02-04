using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public RoomsPlacer roomsPlacer;

    private CharacterController controller;
    private Vector3 playerVelocity;
    public float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;

    [SerializeField] private int damage;

    [SerializeField] private Animator swordAnimator;

    private Vector3 lookDot;

    public GameObject testSphere;

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
        Look();
    }

    private void MovementControls()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if(!controller.isGrounded)
            playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Look()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, 7))
        {
            Debug.Log(hit.transform.name);
            Debug.Log("hit");

            var direction = (hit.point - transform.position).normalized;
            direction.y = 0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 5f);

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
