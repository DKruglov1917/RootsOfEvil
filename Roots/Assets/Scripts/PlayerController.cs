using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip swordSwing;

    private void Awake()

    {
        audioSource = GetComponent<AudioSource>();
    } 

    public void PlaySword()
    {
        audioSource.clip = swordSwing;
        audioSource.Play();
    }

    public static int damage = 50;

    public RoomsPlacer roomsPlacer;
    public Animator animator;
    public float playerSpeed = 2.0f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] private Animator swordAnimator;   
    private float gravityValue = -9.81f;

    private bool isAiming;
    private Vector2 aimStart, aimEnd;
    public Vector3 lookDot;
    private Vector3 velocity;

    public GameObject attackCursor;

    public TrailRenderer swordTrail;

    public static bool isPlayerAttack;

    public int health;

    public Animator damageAnimator;

    public Slider slider;

    private bool dead;

    public GameObject DeathScreen;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        health = 100;
        slider.maxValue = health;
        slider.value = health;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (dead) return;

        Aim();

        if (AttackAnimCheck())
        {
            isPlayerAttack = true;
            swordTrail.enabled = true;
            return;
        }
        else
        {
            isPlayerAttack = false;
            swordTrail.enabled = false;
            if (!isAiming) MovementControls();
            if (isAiming) TrailTheAttack();
            if (!isAiming) Look();
            Animate();
        }
    }

    private void Die()
    {
        dead = true;
        DeathScreen.SetActive(true);
        Time.timeScale = 0.2f;
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

    public void TakeDamage()
    {
        health -= 5;
        damageAnimator.SetTrigger("takeDamage");
        slider.value = health;

        if (health <= 0) Die();

        Debug.Log(health);
    }

    private void Animate()
    {
        if (isAiming) return;

        var heading = lookDot - new Vector3(transform.position.x, 0, transform.position.z);

        var angle = Vector3.SignedAngle(velocity, heading, Vector3.up);

        if (velocity == Vector3.zero || angle == 90)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            if (angle > -45 && angle < 45) animator.SetTrigger("Fwd");
            else if (angle > -120 && angle < -45) animator.SetTrigger("Right");
            else if (angle > 45 && angle < 120) animator.SetTrigger("Left");
            else if (angle > 120 || angle < -120) animator.SetTrigger("Bwd");
        }
    }

    private void TrailTheAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            attackCursor.transform.position = hit.point;
        }
    }

    private void Aim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            controller.Move(Vector3.zero);
            aimStart = Input.mousePosition;
            attackCursor.SetActive(true);            
        }
        if (Input.GetMouseButtonUp(0))
        {
            aimEnd = Input.mousePosition;
            attackCursor.SetActive(false);


            var attackHeading = aimEnd - aimStart;
            float x, y;
            x = Math.Abs(attackHeading.x);
            y = Math.Abs(attackHeading.y);

            float max3 = Math.Max(x, y);

            if(max3 == x)
            {
                animator.SetTrigger("slashAttack");
            }
            if(max3 == y)
            {
                if(attackHeading.y >= 0)
                {
                    animator.SetTrigger("downAttack");
                }
                else
                {
                    animator.SetTrigger("upAttack");
                }
            }

            animator.SetTrigger("Attack");

            StartCoroutine("StopAim");
        }
    }

    private bool AttackAnimCheck()
    {
        if (AnimatorIsPlaying("upAttack")) return true;
        else if (AnimatorIsPlaying("downAttack")) return true;
        else if (AnimatorIsPlaying("slashAttack")) return true;
        else return false;
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

    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    IEnumerator StopAim()
    {        
        yield return new WaitForSeconds(.5f);
        isAiming = false;
    }
}
