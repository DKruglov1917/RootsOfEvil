using UnityEngine;
using System.Collections;

public class RayScan : MonoBehaviour
{
    public int rays = 8;
    public int distance = 30;
    public float angle = 20;
    public Vector3 offset;
    public static Transform target;

    public GameObject player;
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        player = GameObject.Find("Player");
    }

    bool GetRaycast(Vector3 dir)
    {
        bool result = false;
        RaycastHit hit = new RaycastHit();
        Vector3 pos = transform.position + offset;
        if (Physics.Raycast(pos, dir, out hit, distance))
        {
            if (hit.transform.tag == "Player")
            {
                result = true;
                Debug.DrawLine(pos, hit.point, Color.green);
            }
            else
            {
                Debug.DrawLine(pos, hit.point, Color.blue);
            }
        }
        else
        {
            Debug.DrawRay(pos, dir * distance, Color.red);
        }
        return result;
    }

    bool RayToScan()
    {
        bool result = false;
        bool a = false;
        bool b = false;
        float j = 0;
        for (int i = 0; i < rays; i++)
        {
            var x = Mathf.Sin(j);
            var y = Mathf.Cos(j);

            j += angle * Mathf.Deg2Rad / rays;

            Vector3 dir = transform.TransformDirection(new Vector3(x, 0, y));

            if (GetRaycast(dir)) a = true;

            if (x != 0)
            {
                dir = transform.TransformDirection(new Vector3(-x, 0, y));
                if (GetRaycast(dir)) b = true;
            }
        }

        if (a || b) result = true;

        return result;
    }

    void Update()
    {
        if (enemy.isDead) return;

        target = player.transform;

        if (Vector3.Distance(transform.position, target.position) < distance)
        {
            if (RayToScan())
            {
                var direction = (player.transform.position - transform.position).normalized;                
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 5f);

                enemy.target = player;
                enemy.isAggred = true;
                
            }
            else
            {
                enemy.isAggred = false;
            }
        }
    }
}