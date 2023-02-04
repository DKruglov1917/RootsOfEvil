using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTree : MonoBehaviour
{
    public float rotateSpeed;
    public static bool toRotate = true;

    private void Update()
    {
        if(toRotate) transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);
    }
}
