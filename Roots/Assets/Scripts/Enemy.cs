using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isAggred;
    private bool isDead;
    [SerializeField] private int maxHealth, health;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        RestartHealth();
    }

    private void Update()
    {
        if (isDead) return;

        if(!isAggred)
            FoolingAround();
    }

    private void Vision()
    {

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

        Debug.Log("DAMAGE IS " + damage + "ENEMY HEALTH IS " + health);
    }

    private void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    private void FoolingAround()
    {
        if (Vector3.Distance(agent.transform.position, agent.destination) < 1)
        {
            Debug.Log("agent has arrived");
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
}
