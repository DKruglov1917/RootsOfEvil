using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    public Animator animator;
    public bool isAggred;
    public GameObject target;
    private bool isDead;
    [SerializeField] private int maxHealth, health;

    private Outline outline;

    private bool isAttacking;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        outline = GetComponent<Outline>();
        target = GameObject.Find("Player");
        RestartHealth();
    }

    private void Update()
    {
        if (isDead) return;
        if (isAttacking) return;

        if (isAggred) agent.speed = 2;
        else agent.speed = 1;

        if (!isAggred)
            FoolingAround();
        else Chasing();

        Animate();
    }

    private void RestartHealth()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    private void Animate()
    {
        if (isAggred) animator.SetTrigger("Run");
        else animator.SetTrigger("Idke");
    }

    private void Chasing()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 1)
            agent.destination = new Vector3(target.transform.position.x, 0.5f, target.transform.position.z);
        else Attack();
    }

    private void Attack()
    {
        StartCoroutine("AttackCourotine");
    }

    private void FoolingAround()
    {
        if (Vector3.Distance(agent.transform.position, agent.destination) < 1)
        {
            agent.destination = GetRandomLocation();
        }
    }

    private Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Sword" && PlayerController.isPlayerAttack)
        {
            TakeDamage(PlayerController.damage);
            Debug.Log("Attack");
            isAggred = true;
            if(gameObject.activeSelf)
                StartCoroutine("DamageOutline");
        }
    }

    IEnumerator DamageOutline()
    {
        outline.enabled = true;
        yield return new WaitForSeconds(.3f);
        outline.enabled = false;
    }

    IEnumerator AttackCourotine()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            yield return new WaitForSeconds(1f);
            target.GetComponent<PlayerController>().TakeDamage();
            isAttacking = false;
        }
    }

}
